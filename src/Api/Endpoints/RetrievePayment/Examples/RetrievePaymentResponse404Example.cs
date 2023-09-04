using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.PaymentGateway.Api.Endpoints.Examples;

public class RetrievePaymentResponse404Example  : IExamplesProvider<ProblemDetails>
{
    public ProblemDetails GetExamples() =>
        new()
        {
            Title = "Payment not found",
            Type = "https://httpstatuses.com/404",
            Detail = "No payment record can be found with the given id",
            Instance = "/payments/xyz1234",
            Status = StatusCodes.Status404NotFound,
            Extensions =
            {
                { "paymentId", "xyz1234"}
            }
        };
}
