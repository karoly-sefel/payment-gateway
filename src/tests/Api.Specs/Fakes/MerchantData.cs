namespace Checkout.PaymentGateway.Api.Specs.Fakes;

public class MerchantData
{
    public required string MerchantId { get; set; }
    public bool IsActive { get; set; } = true;
}
