using Checkout.PaymentGateway.Application.Payments.Queries;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;
using CSharpFunctionalExtensions;

namespace Checkout.PaymentGateway.Infrastructure.Persistence;

public class InMemoryPaymentRepository : IPaymentRepository
{
    private readonly List<Transaction> _storedTransactions = new();

    public InMemoryPaymentRepository()
    {
        RecordTransaction(new Transaction(
            MerchantId.From("merchantA"),
            PaymentId.From("abc1234"),
            new PaymentCard("1111 1111 1111 1111", 2028, 12, "123"),
            new PaymentAmount(50000, "GBP"),
            TransactionStatus.Success,
            new DateTime(2023, 09, 01, 12, 30, 00),
            new DateTime(2023, 09, 01, 12, 35, 00)
        ), CancellationToken.None);
    }

    private static readonly Task<Maybe<Transaction>> NotFoundResult = Task.FromResult(Maybe<Transaction>.None);

    public Task<Maybe<Transaction>> GetById(PaymentId paymentId, MerchantId merchantId, CancellationToken cancellationToken)
    {
        Transaction? transaction = _storedTransactions
            .Where(t => t.PaymentId == paymentId && t.MerchantId == merchantId)
            .MaxBy(t => t.UpdatedOn);

        if (transaction is null)
            return NotFoundResult;

        return Task.FromResult(Maybe<Transaction>.From(transaction));
    }

    public Task RecordTransaction(Transaction transaction, CancellationToken cancellationToken)
    {
        _storedTransactions.Add(transaction);
        return Task.CompletedTask;
    }
}
