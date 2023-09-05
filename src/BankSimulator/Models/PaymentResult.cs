namespace Checkout.PaymentGateway.BankSimulator.Models;

public enum PaymentResult
{
    Success,
    InsufficientFunds,
    ExpiredCard,
    SecurityViolation,
    LostCardPickUp,
    AuthenticationRequired3DS,
    ActivityAmountLimitExceeded
}
