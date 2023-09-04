namespace Checkout.PaymentGateway.Domain.Entities;

public record PaymentError(PaymentErrorReason Reason);

public enum PaymentErrorReason
{
    Unknown,
    InsufficientFunds,
    ExpiredCard,
}
