using System.Net;
using System.Net.Http.Json;
using Checkout.PaymentGateway.Api.Authorization;
using Checkout.PaymentGateway.Api.Specs.Authentication;
using Checkout.PaymentGateway.Api.Specs.Context;
using Checkout.PaymentGateway.Api.Specs.Fakes;
using Checkout.PaymentGateway.Api.Specs.Http;
using Checkout.PaymentGateway.Application.AcquiringBank.Data;
using Checkout.PaymentGateway.Application.Payments.Commands;
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

    public ProcessPaymentSteps(HttpClientContext httpClient, PaymentContext context, SequentialPaymentIdGenerator idGenerator, MockHttpMessageHandler http) =>
        (_httpClient, _paymentContext, _idGenerator, _http) = (httpClient, context, idGenerator, http);

    [Given(@"a customer")]
    public void GivenACustomer()
    {
    }

    [Given(@"the customer has insufficient balance on their credit card for the payment")]
    public void GivenTheCustomerHasInsufficientBalanceOnTheirCreditCardForThePayment() =>
        ConfigureBankResponse(new PaymentResponse(PaymentStatus.Declined, PaymentResult.InsufficientFunds));

    [Given(@"the customer has sufficient balance on their credit card for the payment")]
    public void GivenTheCustomerHasSufficientBalanceOnTheirCreditCardForThePayment() =>
        ConfigureBankResponse(new PaymentResponse(PaymentStatus.Approved, PaymentResult.Success));

    private void ConfigureBankResponse(PaymentResponse response) =>
        _http.When(HttpMethod.Post, "/api/process-payment")
            .Respond(_ => new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = JsonContent.Create(response)
            });

    [When(@"a request is made to the ProcessPayment endpoint")]
    public async Task WhenARequestIsMadeToTheProcessPaymentEndpoint() =>
        await ProcessPayment(new PaymentRequest(
            "1111 1111 1111 1234",
            11,
            2028,
            100000,
            "EUR",
            "123"
        ));

    [Then(@"the response includes the payment id for the current transaction")]
    public async Task ThenTheResponseIncludesThePaymentIdForTheCurrentTransaction()
    {
        ProcessPaymentResponse response = await _httpClient.GetLatestResponse<ProcessPaymentResponse>();
        response.Should().BeEquivalentTo(new ProcessPaymentResponse(_idGenerator.LastId!.Value, "Success", "Processed"));
    }

    private Task ProcessPayment(PaymentRequest request) =>
        _httpClient.Post("/v1/payments", request,
            bearerToken: JwtTokenGenerator.GenerateToken(_paymentContext.MerchantId, new []{ Scopes.PaymentProcess }));
}
