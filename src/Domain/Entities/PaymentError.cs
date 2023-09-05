using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Domain.Entities;

public record PaymentError(PaymentErrorReason Reason);

public record PaymentValidationError(string Field, string Message) : PaymentError(PaymentErrorReason.InvalidOrMissingCardDetails);

public record AcquiringBankPaymentError(PaymentErrorReason Reason, PaymentId PaymentId) : PaymentError(Reason);

public enum PaymentErrorReason
{
    Unknown,
    InsufficientFunds,
    ExpiredCard,
    MerchantNotFound,
    MerchantAccountClosed,
    InvalidOrMissingCardDetails,
    NotSupportedCurrency,
    SecurityViolation,
    LostCardPickUp,
    AuthenticationRequired3DS,
    ActivityAmountLimitExceeded
}


