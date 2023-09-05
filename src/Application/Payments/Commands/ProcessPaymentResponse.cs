namespace Checkout.PaymentGateway.Application.Payments.Commands;

public record ProcessPaymentResponse(string PaymentId, string Status, string StatusDescription);
