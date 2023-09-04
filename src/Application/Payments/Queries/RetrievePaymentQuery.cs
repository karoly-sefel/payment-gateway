namespace Checkout.PaymentGateway.Application.Payments.Queries;

public record RetrievePaymentQuery(string PaymentId) : IRequest<Maybe<PaymentDto>>;
