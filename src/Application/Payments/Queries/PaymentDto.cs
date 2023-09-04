namespace Checkout.PaymentGateway.Application.Payments.Queries;

public record PaymentDto(
    string PaymentId,
    string MaskedCardNumber
);
