using Checkout.PaymentGateway.BankSimulator.Endpoints.Dtos;

namespace Checkout.PaymentGateway.BankSimulator.Services;

public class PaymentService
{
    public PaymentResponse ProcessPayment(PaymentRequest request)
    {
        ArgumentNullException.ThrowIfNull(request);

        PaymentResponse? response = CardValidationRules.All
            .Select(getResponseIfDeclined => getResponseIfDeclined(request))
            .FirstOrDefault(paymentResponse => paymentResponse is not null);

        return response ?? PaymentResponse.Approved();
    }
}
