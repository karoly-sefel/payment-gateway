using Ardalis.GuardClauses;
using Checkout.PaymentGateway.Domain.Extensions;

namespace Checkout.PaymentGateway.Domain.ValueObjects;

public record PaymentId
{
    public string Value { get; }

    private const int MinLength = 5;
    private const int MaxLength = 12;

    private PaymentId(string value) => Value = value;

    public static PaymentId From(string paymentId)
    {
        Guard.Against.InvalidInput(paymentId, nameof(paymentId), IsValid, $"Invalid payment id: {paymentId}");
        return new PaymentId(paymentId);
    }

    public static bool IsValid(string paymentId) => !string.IsNullOrEmpty(paymentId) && paymentId.Length.IsBetweenInclusive(MinLength, MaxLength);

    public override string ToString() => Value;
};
