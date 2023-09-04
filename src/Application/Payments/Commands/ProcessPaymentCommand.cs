namespace Checkout.PaymentGateway.Application.Payments.Commands;

public record ProcessPaymentCommand(PaymentRequest PaymentRequest) : IRequest<Result<ProcessPaymentResponse, Exception>>;
