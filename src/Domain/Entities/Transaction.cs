using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Domain.Entities;

public record Transaction(
    MerchantId MerchantId,
    PaymentId PaymentId,
    PaymentCard PaymentCard,
    PaymentAmount Amount,
    TransactionStatus Status,
    DateTime CreatedOn,
    DateTime UpdatedOn,
    PaymentErrorReason? Error = null
);
