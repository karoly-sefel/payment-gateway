using Checkout.PaymentGateway.BankSimulator.Models;

namespace Checkout.PaymentGateway.BankSimulator.Endpoints.Dtos;

public record PaymentResponse(PaymentStatus Status, PaymentResult Result)
{
    public static PaymentResponse Approved() => new(PaymentStatus.Approved, PaymentResult.Success);
    public static PaymentResponse ExpiredCard() => new(PaymentStatus.Declined, PaymentResult.ExpiredCard);
    public static PaymentResponse InsufficientFunds() => new(PaymentStatus.Declined, PaymentResult.InsufficientFunds);
    public static PaymentResponse SecurityViolation() => new(PaymentStatus.Declined, PaymentResult.SecurityViolation);
    public static PaymentResponse LostCardPickUp() => new(PaymentStatus.Declined, PaymentResult.LostCardPickUp);
    public static PaymentResponse ActivityAmountLimitExceeded() => new(PaymentStatus.Declined, PaymentResult.ActivityAmountLimitExceeded);
    public static PaymentResponse AuthenticationRequired3DSecure() => new(PaymentStatus.Pending, PaymentResult.AuthenticationRequired3DS);
}
