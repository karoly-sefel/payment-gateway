using Checkout.PaymentGateway.Domain.ValueObjects;
using NanoidDotNet;

namespace Checkout.PaymentGateway.Application.Services;

public class PaymentIdGenerator : IPaymentIdGenerator
{
    public async Task<PaymentId> Generate()
    {
        string id = await Nanoid.GenerateAsync(size: 12);
        return PaymentId.From(id);
    }
}
