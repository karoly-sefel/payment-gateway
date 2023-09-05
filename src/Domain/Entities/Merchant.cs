using Ardalis.GuardClauses;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Domain.Entities;

public class Merchant
{
    public MerchantId Id { get; }
    public string Name { get; private set; }
    public MerchantStatus Status { get; private set; } = MerchantStatus.Active;

    public bool IsActive => Status == MerchantStatus.Active;

    public Merchant(MerchantId id, string name)
    {
        Guard.Against.Null(id);
        Id = id;
        Name = name;
    }

    public void CloseAccount()
    {
        if (Status == MerchantStatus.Inactive)
            throw new ApplicationException("An inactive account cannot be closed");

        Status = MerchantStatus.Inactive;
    }
};
