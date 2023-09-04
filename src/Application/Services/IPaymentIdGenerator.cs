using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Application.Services;

public interface IPaymentIdGenerator
{
    Task<PaymentId> Generate();
}
