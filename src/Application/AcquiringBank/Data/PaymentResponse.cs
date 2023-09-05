namespace Checkout.PaymentGateway.Application.AcquiringBank.Data;

public record PaymentResponse(PaymentStatus Status, PaymentResult Result);

public enum PaymentStatus
{
    Approved,
    Declined,
    Pending
}

public enum PaymentResult
{
    Success,
    InsufficientFunds,
    ExpiredCard,
    SecurityViolation,
    LostCardPickUp,
    AuthenticationRequired3DS,
    ActivityAmountLimitExceeded,
}
