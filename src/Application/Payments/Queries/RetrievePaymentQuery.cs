using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Application.Payments.Queries;

public record RetrievePaymentQuery(string PaymentId, MerchantId MerchantId) : IRequest<Maybe<PaymentDto>>;
