using Checkout.PaymentGateway.Application.Payments.Queries;
using Checkout.PaymentGateway.Domain.ValueObjects;
using CSharpFunctionalExtensions;

namespace Checkout.PaymentGateway.Infrastructure.Persistence;

public class InMemoryPaymentRepository : IPaymentRepository
{
    private readonly Dictionary<(PaymentId, MerchantId), PaymentDto> _storedPayments = new()
    {
        { (PaymentId.From("abc1234"), MerchantId.From("merchantA")), new PaymentDto("abc1234", "**** **** **** 1234") }
    };

    private static readonly Task<Maybe<PaymentDto>> NotFoundResult = Task.FromResult(Maybe<PaymentDto>.None);

    public Task<Maybe<PaymentDto>> GetById(PaymentId paymentId, MerchantId merchantId, CancellationToken cancellationToken)
    {
        if (paymentId.Value == "boom")
            throw new ApplicationException("Database is unavailable");

        if (!_storedPayments.TryGetValue((paymentId, merchantId), out PaymentDto? payment))
            return NotFoundResult;

        return Task.FromResult(Maybe<PaymentDto>.From(payment));
    }
}
