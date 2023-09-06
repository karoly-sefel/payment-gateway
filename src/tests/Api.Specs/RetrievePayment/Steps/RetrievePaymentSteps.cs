using System.Net;
using Checkout.PaymentGateway.Api.Authorization;
using Checkout.PaymentGateway.Api.Specs.Authentication;
using Checkout.PaymentGateway.Api.Specs.Context;
using Checkout.PaymentGateway.Api.Specs.Fakes;
using Checkout.PaymentGateway.Api.Specs.Http;
using Checkout.PaymentGateway.Application.Payments.Queries;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TechTalk.SpecFlow;

namespace Checkout.PaymentGateway.Api.Specs.Steps;

[Binding]
public class RetrievePaymentSteps
{
    private readonly HttpClientContext _httpClient;
    private readonly PaymentContext _context;

    private const string NonExistingPaymentId = "zzzzzz0001";

    public RetrievePaymentSteps(HttpClientContext httpClient, PaymentContext context)
    {
        _httpClient = httpClient;
        _context = context;
    }

    [Given(@"a merchant")]
    public void GivenAMerchant()
    {
        _context.Merchants.Add(new MerchantData{ MerchantId = "merchantA"});
        _context.MerchantId = "merchantA";
    }

    [Given(@"a payment")]
    public void GivenAPayment()
    {
        _context.Transactions.Add(CreateTestTransaction());
        _context.CurrentPayment = new PaymentDto("abc1234", "************1234", 50000, "EUR", "Success");
    }

    [When(@"the merchant requests the details of the payment")]
    public async Task WhenTheMerchantRequestsTheDetailsOfThePayment() => await GetPaymentDetails(_context.CurrentPayment!.PaymentId);

    [When(@"the merchant requests the details of the payment of the other merchant")]
    public async Task WhenTheMerchantRequestsTheDetailsOfThePaymentOfTheOtherMerchant()
    {
        _context.CurrentPayment = new PaymentDto("zzzzzz0001", "************1234", 50000, "EUR", "Success");
        await WhenTheMerchantRequestsTheDetailsOfThePayment();
    }

    [When(@"the merchant requests the details of the payment with a payment id that doesn't exist")]
    public async Task WhenTheMerchantRequestsTheDetailsOfThePaymentWithAPaymentIdThatDoesntExist() => await GetPaymentDetails(NonExistingPaymentId);

    [When(@"the merchant requests the details of the payment with an invalid payment id")]
    public async Task WhenTheMerchantRequestsTheDetailsOfThePaymentWithAnInvalidPaymentId() => await GetPaymentDetails("aaa");

    [Then(@"the status code is (.*)")]
    public void ThenTheStatusCodeIsOk(HttpStatusCode httpStatusCode) =>
        _httpClient.LatestStatusCode.Should().Be(httpStatusCode);

    [Then(@"the payment details are returned")]
    public async void ThenThePaymentDetailsAreReturned()
    {
        PaymentDto expectedPayment = _context.CurrentPayment!;
        PaymentDto response = await _httpClient.GetLatestResponse<PaymentDto>();

        response
            .Should().NotBeNull()
            .And.Subject
            .Should().BeEquivalentTo(expectedPayment);
    }

    [Then(@"a NotFound problem details response is returned")]
    public async Task ThenANotFoundProblemDetailsResponseIsReturned()
    {
        var expectedResponse = new ProblemDetails
        {
            Title = "Payment not found",
            Type = "https://httpstatuses.com/404",
            Instance = $"/v1/payments/{NonExistingPaymentId}",
            Detail = "No payment record can be found with the given id",
            Status = StatusCodes.Status404NotFound,
        };

        var problemDetails = await _httpClient.GetLatestResponse<ProblemDetails>();

        problemDetails.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(details => details.Extensions));
    }

    [Then(@"a BadRequest problem details response is returned")]
    public async Task ThenABadRequestProblemDetailsResponseIsReturned()
    {
        var expectedResponse = new ProblemDetails
        {
            Title = "Bad Request",
            Type = "https://httpstatuses.com/400",
            Instance = $"/v1/payments/aaa",
            Detail = "Invalid payment id format",
            Status = StatusCodes.Status400BadRequest,
        };

        var problemDetails = await _httpClient.GetLatestResponse<ProblemDetails>();

        problemDetails.Should().BeEquivalentTo(expectedResponse, opt => opt.Excluding(details => details.Extensions));
    }

    private Task GetPaymentDetails(string paymentId) => _httpClient.Get($"/v1/payments/{paymentId}",
        bearerToken: JwtTokenGenerator.GenerateToken(_context.MerchantId, new []{ Scopes.PaymentRead }));

    [Given(@"another merchant who processed a payment")]
    public void GivenAnotherMerchantWhoProcessedAPayment()
    {
        var merchantId = MerchantId.From("merchantB");
        _context.Merchants.Add(new MerchantData{ MerchantId = merchantId.Value });

        var transaction = CreateTestTransaction() with { MerchantId = merchantId, PaymentId = PaymentId.From("zzzzzz0001")};
        _context.Transactions.Add(transaction);
    }

    private Transaction CreateTestTransaction() =>
        new(
            MerchantId.From("merchantA"),
            PaymentId.From("abc1234"),
            new PaymentCard("1111 1111 1111 1234", 2028, 12, "123"),
            new PaymentAmount(50000, "EUR"),
            TransactionStatus.Success,
            new DateTime(2023, 09, 01, 12, 30, 00),
            new DateTime(2023, 09, 01, 12, 35, 00)
        );
}
