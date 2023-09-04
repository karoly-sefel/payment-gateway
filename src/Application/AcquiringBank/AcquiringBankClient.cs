using System.Net.Http.Json;
using Checkout.PaymentGateway.Application.AcquiringBank.Data;

namespace Checkout.PaymentGateway.Application.AcquiringBank;

public class AcquiringBankClient : IAcquiringBankClient
{
    private readonly HttpClient _httpClient;

    public AcquiringBankClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<BankPaymentResult> SendPayment(PaymentRequest paymentRequest)
    {
        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/take-payment", paymentRequest);

        if (response.IsSuccessStatusCode)
            return BankPaymentResult.Success;

        return BankPaymentResult.Failure;
    }
}
