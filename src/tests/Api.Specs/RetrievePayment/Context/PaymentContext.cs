using Checkout.PaymentGateway.Application.Payments.Queries;
using TechTalk.SpecFlow;

namespace Checkout.PaymentGateway.Api.Specs.Context;

[Binding]
public class PaymentContext
{
    public PaymentDto? CurrentPayment { get; set; }
}
