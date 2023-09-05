using Ardalis.GuardClauses;

namespace Checkout.PaymentGateway.Domain.ValueObjects;

public record MerchantId(string Value)
{
    public static MerchantId From(string id)
    {
        string merchantId = Guard.Against.NullOrEmpty(id, nameof(id), "Invalid merchant id");
        return new MerchantId(merchantId);
    }

    public override string ToString() => Value;
}
