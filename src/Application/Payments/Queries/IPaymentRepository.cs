using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Application.Payments.Queries;

public interface IPaymentRepository
{
    Task<Maybe<Transaction>> GetById(PaymentId paymentId, MerchantId merchantId, CancellationToken cancellationToken);
    Task RecordTransaction(Transaction transaction, CancellationToken cancellationToken);
}
