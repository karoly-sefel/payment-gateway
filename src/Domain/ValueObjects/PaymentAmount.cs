using Ardalis.GuardClauses;

namespace Checkout.PaymentGateway.Domain.ValueObjects;

public record PaymentAmount
{
    public int Amount { get; }
    public string CurrencyCode { get; }

    public PaymentAmount(int amount, string currencyCode)
    {
        Guard.Against.NegativeOrZero(amount);
        Guard.Against.InvalidFormat(currencyCode, nameof(currencyCode), "[A-Z]{3}", "Invalid currency code format");
        Amount = amount;
        CurrencyCode = currencyCode;
    }
};
