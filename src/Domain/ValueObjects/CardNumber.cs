using Ardalis.GuardClauses;
using Checkout.PaymentGateway.Domain.Extensions;

namespace Checkout.PaymentGateway.Domain.ValueObjects;

public record CardNumber
{
    public string Value { get; }

    public CardNumber(string value)
    {
        Guard.Against.InvalidInput(value, nameof(value), IsValid);
        Value = RemoveSpaces(value);
    }

    public static bool IsValid(string number)
    {
        if (string.IsNullOrEmpty(number))
            return false;

        // https://en.wikipedia.org/wiki/Payment_card_number
        return RemoveSpaces(number).Length.IsBetweenInclusive(8, 19);
    }

    private static string RemoveSpaces(string number) => number.Replace(" ", "");

    public string Mask()
    {
        string last4Digits = Value[^4..];
        return new String('*', Value.Length - 4) + last4Digits;
    }
};
