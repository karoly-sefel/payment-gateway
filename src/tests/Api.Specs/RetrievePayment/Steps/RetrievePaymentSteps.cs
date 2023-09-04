using System.Net;
using Checkout.PaymentGateway.Api.Specs.Context;
using Checkout.PaymentGateway.Api.Specs.Http;
using Checkout.PaymentGateway.Application.Payments.Queries;
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
        // noop for now
    }

    [Given(@"a payment")]
    public void GivenAPayment()
    {
        _context.CurrentPayment = new PaymentDto("abc1234", "**** **** **** 1234");
    }

    [When(@"the merchant requests the details of the payment")]
    public async Task WhenTheMerchantRequestsTheDetailsOfThePayment() => await GetPaymentDetails(_context.CurrentPayment!.PaymentId);

    [When(@"the merchant requests the details of the payment with a payment id that doesn't exist")]
    public async Task WhenTheMerchantRequestsTheDetailsOfThePaymentWithAPaymentIdThatDoesntExist() => await GetPaymentDetails(NonExistingPaymentId);

    [Then(@"the status code is (.*)")]
    public void ThenTheStatusCodeIsOk(HttpStatusCode httpStatusCode) =>
        _httpClient.LatestStatusCode.Should().Be(httpStatusCode);

    [Then(@"the payment details are returned")]
    public async void ThenThePaymentDetailsAreReturned()
    {
        PaymentDto expectedPayment = _context.CurrentPayment!;
        PaymentDto? response = await _httpClient.GetLatestResponse<PaymentDto>();
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

    private Task GetPaymentDetails(string paymentId) => _httpClient.Get($"/v1/payments/{paymentId}");
}
