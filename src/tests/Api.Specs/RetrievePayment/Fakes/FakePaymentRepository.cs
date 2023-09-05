using Checkout.PaymentGateway.Api.Specs.Context;
using Checkout.PaymentGateway.Application.Payments.Queries;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;
using CSharpFunctionalExtensions;

namespace Checkout.PaymentGateway.Api.Specs.Fakes;

public class FakePaymentRepository : IPaymentRepository
{
    private readonly PaymentContext _context;

    public FakePaymentRepository(PaymentContext context) => _context = context;

    public Task<Maybe<Transaction>> GetById(PaymentId paymentId, MerchantId merchantId, CancellationToken cancellationToken)
    {
        Transaction? transaction = _context.Transactions
            .Where(t => t.PaymentId == paymentId && t.MerchantId == merchantId)
            .MaxBy(t => t.UpdatedOn);

        return Task.FromResult(Maybe<Transaction>.From(transaction!));
    }

    public Task RecordTransaction(Transaction transaction, CancellationToken cancellationToken)
    {
        _context.Transactions.Add(transaction);
        return Task.CompletedTask;
    }
}
