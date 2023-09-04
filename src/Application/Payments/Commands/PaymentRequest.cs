namespace Checkout.PaymentGateway.Application.Payments.Commands;

public record PaymentRequest(
    string CardNumber,
    int ExpiryMonth,
    int ExpiryYear,
    int Amount,
    string Currency,
    string CVV
);
