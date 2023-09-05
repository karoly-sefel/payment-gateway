using Checkout.PaymentGateway.Domain.Services;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Domain.Entities;

public class PaymentCard
{
    public CardNumber CardNumber { get; }
    public int ExpiryYear { get; }
    public int ExpiryMonth { get; }
    public string Cvv { get; }

    public PaymentCard(string cardNumber, int expiryYear, int expiryMonth, string cvv)
    {
        CardNumber = new CardNumber(cardNumber);
        ExpiryYear = expiryYear;
        ExpiryMonth = expiryMonth;
        Cvv = cvv;
    }

    public bool IsExpired(Clock clock)
    {
        var today = clock().Date;
        return ExpiryYear < today.Year || (ExpiryYear == today.Year && ExpiryMonth < today.Month);
    }

    public string MaskedCardNumber => CardNumber.Mask();
}
