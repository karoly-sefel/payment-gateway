namespace Checkout.PaymentGateway.Domain.Extensions;

public static class IntegerExtensions
{
    public static bool IsBetweenInclusive(this int value, int min, int max) =>
        value >= min && value <= max;
}
