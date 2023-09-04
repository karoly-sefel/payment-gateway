using Checkout.PaymentGateway.Api.Specs.Fakes;
using Checkout.PaymentGateway.Api.Specs.Http;
using Checkout.PaymentGateway.Application.Payments.Commands;
using TechTalk.SpecFlow;

namespace Checkout.PaymentGateway.Api.Specs.Steps;

[Binding]
public class ProcessPaymentSteps
{
    private readonly HttpClientContext _httpClient;
    private readonly SequentialPaymentIdGenerator _idGenerator;

    public ProcessPaymentSteps(HttpClientContext httpClient, SequentialPaymentIdGenerator idGenerator) =>
        (_httpClient, _idGenerator) = (httpClient, idGenerator);

    [When(@"a request is made to the ProcessPayment endpoint")]
    public async Task WhenARequestIsMadeToTheProcessPaymentEndpoint() =>
        await ProcessPayment(new PaymentRequest(
            "123",
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
        response.Should().BeEquivalentTo(new ProcessPaymentResponse(_idGenerator.LastId!.Value));
    }

    private Task ProcessPayment(PaymentRequest request) =>
        _httpClient.Post("/v1/payments", request);
}
