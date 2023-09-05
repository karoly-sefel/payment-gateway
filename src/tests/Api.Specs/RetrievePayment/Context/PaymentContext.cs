using Checkout.PaymentGateway.Api.Specs.Fakes;
using Checkout.PaymentGateway.Application.Payments.Queries;
using TechTalk.SpecFlow;

namespace Checkout.PaymentGateway.Api.Specs.Context;

[Binding]
public class PaymentContext
{
    public List<MerchantData> Merchants = new();

    public string MerchantId { get; set; } = "";
    public PaymentDto? CurrentPayment { get; set; }

}
