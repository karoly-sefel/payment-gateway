using Checkout.PaymentGateway.Domain.ValueObjects;
using NanoidDotNet;

namespace Checkout.PaymentGateway.Application.Payments.Services;

public class PaymentIdGenerator : IPaymentIdGenerator
{
    public PaymentId Generate()
    {
        string id = Nanoid.Generate(size: 12);
        return PaymentId.From(id);
    }
}
