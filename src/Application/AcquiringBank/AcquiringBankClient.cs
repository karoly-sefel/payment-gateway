using System.Net.Http.Json;
using Checkout.PaymentGateway.Application.AcquiringBank.Data;
using PaymentRequest = Checkout.PaymentGateway.Application.Payments.Commands.PaymentRequest;

namespace Checkout.PaymentGateway.Application.AcquiringBank;

public class AcquiringBankClient : IAcquiringBankClient
{
    private readonly HttpClient _httpClient;

    public AcquiringBankClient(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<BankPaymentResult> SendPayment(PaymentRequest paymentRequest, CancellationToken cancellationToken)
    {
        Data.PaymentRequest data = Map(paymentRequest);

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/process-payment", data, cancellationToken);

        if (response.IsSuccessStatusCode)
            return BankPaymentResult.Success;

        return BankPaymentResult.Failure;
    }

    private Data.PaymentRequest Map(PaymentRequest request) =>
        new Data.PaymentRequest();
}
