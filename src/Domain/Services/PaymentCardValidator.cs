using Checkout.PaymentGateway.Domain.Extensions;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Domain.Services;

public class PaymentCardValidator
{
    public static bool IsValidYear(int year) => year.IsBetweenInclusive(1950, 3000); // only format check

    public static bool IsValidMonth(int month) => month.IsBetweenInclusive(1, 12);

    public static bool IsValidCvv(string cvv) => cvv.Length == 3 && cvv.All(Char.IsDigit);

    public static bool IsValidCardNumber(string cardNumber) => CardNumber.IsValid(cardNumber);
}
