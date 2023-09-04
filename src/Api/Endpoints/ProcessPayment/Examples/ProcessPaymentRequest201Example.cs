using Checkout.PaymentGateway.Application.Payments.Commands;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.PaymentGateway.Api.Endpoints.Examples;

public class ProcessPaymentRequest201Example : IExamplesProvider<PaymentRequest>
{
    public PaymentRequest GetExamples()
    {
        return new PaymentRequest(
            "4242424242424242",
            8,
            2025,
            102400,
            "GBP",
            "123"
        );
    }
}
