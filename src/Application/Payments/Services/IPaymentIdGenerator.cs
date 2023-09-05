using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Application.Payments.Services;

public interface IPaymentIdGenerator
{
    PaymentId Generate();
}
