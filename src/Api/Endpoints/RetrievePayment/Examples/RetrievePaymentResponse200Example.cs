using Checkout.PaymentGateway.Application.Payments.Queries;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.PaymentGateway.Api.Endpoints.Examples;

public class RetrievePaymentResponseExample  : IExamplesProvider<PaymentDto>
{
    public PaymentDto GetExamples() => new("abc1234", "**** **** **** 1234");
}
