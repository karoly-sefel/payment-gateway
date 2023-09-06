using System.Net;
using System.Net.Http.Json;
using Checkout.PaymentGateway.Api.Authorization;
using Checkout.PaymentGateway.Api.Specs.Authentication;
using Checkout.PaymentGateway.Api.Specs.Context;
using Checkout.PaymentGateway.Api.Specs.Fakes;
using Checkout.PaymentGateway.Api.Specs.Http;
using Checkout.PaymentGateway.Application.AcquiringBank.Data;
using Checkout.PaymentGateway.Application.Payments.Commands;
using Checkout.PaymentGateway.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RichardSzalay.MockHttp;
using TechTalk.SpecFlow;
using PaymentRequest = Checkout.PaymentGateway.Application.Payments.Commands.PaymentRequest;

namespace Checkout.PaymentGateway.Api.Specs.Steps;

[Binding]
public class ProcessPaymentSteps
{
    private readonly HttpClientContext _httpClient;

    private readonly SequentialPaymentIdGenerator _idGenerator;
    private readonly MockHttpMessageHandler _http;

    private readonly PaymentContext _paymentContext;
    private PaymentRequest? _paymentRequest;

    public ProcessPaymentSteps(HttpClientContext httpClient, PaymentContext context, SequentialPaymentIdGenerator idGenerator, MockHttpMessageHandler http) =>
        (_httpClient, _paymentContext, _idGenerator, _http) = (httpClient, context, idGenerator, http);

    [Given(@"a customer")]
    public void GivenACustomer()
    {
        // noop
    }

    [Given(@"the customer has insufficient balance on their credit card for the payment")]
    public void GivenTheCustomerHasInsufficientBalanceOnTheirCreditCardForThePayment() =>
        ConfigureBankResponse(new PaymentResponse(PaymentStatus.Declined, PaymentResult.InsufficientFunds));

    [Given(@"the customer uses a stolen credit card for the payment")]
    public void GivenTheCustomerUsesAStolenCreditCardForThePayment() =>
        ConfigureBankResponse(new PaymentResponse(PaymentStatus.Declined, PaymentResult.LostCardPickUp));

    [Given(@"the customer has sufficient balance on their credit card for the payment")]
    public void GivenTheCustomerHasSufficientBalanceOnTheirCreditCardForThePayment() =>
        ConfigureBankResponse(new PaymentResponse(PaymentStatus.Approved, PaymentResult.Success));

    [Given(@"the customer uses a credit cards that's enrolled in 3DS for the payment")]
    public void GivenTheCustomerUsesACreditCardsThatsEnrolledIn3DsForThePayment() =>
        ConfigureBankResponse(new PaymentResponse(PaymentStatus.Pending, PaymentResult.AuthenticationRequired3DS));

    [Given(@"the customer uses an expired credit card for the payment")]
    public void GivenTheCustomerUsesAnExpiredCreditCardForThePayment() =>
        _paymentRequest = new PaymentRequest(
            "1111 1111 1111 1234",
            01,
            2010,
            100000,
            "EUR",
            "123"
        );

    [When(@"a request is made to the ProcessPayment endpoint")]
    public async Task WhenARequestIsMadeToTheProcessPaymentEndpoint() =>
        await ProcessPayment(_paymentRequest ?? new PaymentRequest(
            "1111 1111 1111 1234",
            11,
            2028,
            100000,
            "EUR",
            "123"
        ));

    [When(@"a request is made to the ProcessPayment endpoint without a bearer token")]
    public async Task WhenARequestIsMadeToTheProcessPaymentEndpointWithoutABearerToken() =>
        await ProcessPayment(new PaymentRequest(
            "1111 1111 1111 1234",
            11,
            2028,
            100000,
            "EUR",
            "123"
        ), includeToken: false);

    [Then(@"the response includes the payment id for the current transaction with a Processed status")]
    public Task ThenTheResponseIncludesThePaymentIdForTheCurrentTransactionWithAProcessedStatus() =>
        ExpectPaymentResponse(TransactionStatus.Success, "Processed");

    [Then(@"the response includes the payment id for the current transaction with a AuthenticationRequired3DS status")]
    public Task ThenTheResponseIncludesThePaymentIdForTheCurrentTransactionWithAAuthenticationRequiredDsStatus() =>
        ExpectPaymentResponse(TransactionStatus.Pending, "AuthenticationRequired3DS");

    private async Task ExpectPaymentResponse(TransactionStatus transactionStatus, string statusCode)
    {
        ProcessPaymentResponse response = await _httpClient.GetLatestResponse<ProcessPaymentResponse>();
        response.Should().BeEquivalentTo(new ProcessPaymentResponse(_idGenerator.LastId!.Value, transactionStatus, statusCode));
    }

    [Then(@"the payment is failed with a (.*) error code")]
    public async Task ThenThePaymentIsFailedWithALostCardPickUpErrorCode(string errorCode)
    {
        var expectedResponse = new ProblemDetails
        {
            Title = "Payment error",
            Type = "https://httpstatuses.com/422",
            Instance = "/v1/payments",
            Detail = "Transaction failed",
            Status = StatusCodes.Status422UnprocessableEntity,
        };

        var problemDetails = await _httpClient.GetLatestResponse<ProblemDetails>();

        problemDetails.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(details => details.Extensions));

        ShouldContainKeyWithValue(problemDetails.Extensions, "errorCode", errorCode);

        string[] validationErrors = { "ExpiredCard" };

        if (!validationErrors.Contains(errorCode))
        {
            ShouldContainKeyWithValue(problemDetails.Extensions, "paymentId", "aaaaaaaaaa1");
        }
    }

    private Task ProcessPayment(PaymentRequest request, bool includeToken = true) =>
        _httpClient.Post("/v1/payments", request,
            bearerToken: includeToken ? JwtTokenGenerator.GenerateToken(_paymentContext.MerchantId, new []{ Scopes.PaymentProcess }) : null);

    private void ShouldContainKeyWithValue(IDictionary<string, object?> dictionary, string key, string value)
    {
        dictionary.Should().ContainKey(key);
        dictionary[key]!.ToString().Should().Be(value);
    }

    private void ConfigureBankResponse(PaymentResponse response) =>
        _http.When(HttpMethod.Post, "/api/process-payment")
            .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(response)
            });
}
