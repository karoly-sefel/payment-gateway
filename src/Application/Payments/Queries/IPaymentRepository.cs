using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Application.Payments.Queries;

public interface IPaymentRepository
{
    Task<Maybe<PaymentDto>> GetById(PaymentId paymentId, CancellationToken cancellationToken);
}
