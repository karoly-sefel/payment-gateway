using Checkout.PaymentGateway.BankSimulator.Endpoints.Dtos;
using Checkout.PaymentGateway.BankSimulator.Models;

namespace Checkout.PaymentGateway.BankSimulator.Services;

public static class CardValidationRules
{
    internal static readonly IReadOnlyList<Func<PaymentRequest, PaymentResponse?>> All = new List<Func<PaymentRequest, PaymentResponse?>>
    {
        CheckExpiry,
        CheckIfEnrolledIn3DSecure,
        CheckFunds,
        CheckIfCardLost,
        CheckIfExceededDailyLimits,
        CheckSecurityViolations
    };

    private static PaymentResponse? CheckExpiry(PaymentRequest request)
    {
        var currentMonth = new DateOnly(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
        var cardExpiry = new DateOnly(request.PaymentCard.ExpiryYear, request.PaymentCard.ExpiryMonth, 1);

        return cardExpiry < currentMonth ? PaymentResponse.ExpiredCard() : null;
    }

    private static PaymentResponse? CheckFunds(PaymentRequest request)
    {
        static int ConvertToSmallestDenomination(int amount) => amount * 100;

        var limits = new Dictionary<string, int>
        {
            { Currencies.DanishKrone, ConvertToSmallestDenomination(10_000) },
            { Currencies.Euro, ConvertToSmallestDenomination(100_000) },
            { Currencies.BritishPounds, ConvertToSmallestDenomination(1_000_000) },
        };

        if (limits.TryGetValue(request.Currency, out int balanceOnAccount) && request.Amount > balanceOnAccount)
            return PaymentResponse.InsufficientFunds();

        return null;
    }

    private static PaymentResponse? CheckIfEnrolledIn3DSecure(PaymentRequest request)
    {
        return TestCards.CardsThatRequire3dsAuthentication.Contains(request.PaymentCard.CardNumber) ?
            PaymentResponse.AuthenticationRequired3DSecure() : null;
    }

    private static PaymentResponse? CheckIfCardLost(PaymentRequest request)
    {
        return TestCards.LostCards.Contains(request.PaymentCard.CardNumber) ?
            PaymentResponse.LostCardPickUp() : null;
    }

    private static PaymentResponse? CheckIfExceededDailyLimits(PaymentRequest request)
    {
        return TestCards.CardsWithActivityAmountExceeded.Contains(request.PaymentCard.CardNumber) ?
            PaymentResponse.ActivityAmountLimitExceeded() : null;
    }

    private static PaymentResponse? CheckSecurityViolations(PaymentRequest request)
    {
        return TestCards.CardsWithSecurityViolation.Contains(request.PaymentCard.CardNumber) ?
            PaymentResponse.SecurityViolation() : null;
    }
}
