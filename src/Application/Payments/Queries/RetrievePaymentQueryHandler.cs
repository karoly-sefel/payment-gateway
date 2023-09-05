using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.Payments.Queries;

public class RetrievePaymentQueryHandler : IRequestHandler<RetrievePaymentQuery, Result<PaymentDto, RetrievePaymentError>>
{
    private readonly IPaymentRepository _payments;
    private readonly ILogger<RetrievePaymentQueryHandler> _logger;

    public RetrievePaymentQueryHandler(IPaymentRepository payments, ILogger<RetrievePaymentQueryHandler> logger) =>
        (_payments, _logger) = (payments, logger);

    public async Task<Result<PaymentDto, RetrievePaymentError>> Handle(RetrievePaymentQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Finding payment details for id: {PaymentId} merchant: {MerchantId}", query.PaymentId, query.MerchantId);

        Result<PaymentDto, RetrievePaymentError> payment = await TryParsePaymentId(query.PaymentId)
            .Bind(paymentId => GetPayment(paymentId, query.MerchantId, cancellationToken))
            .Map(ToPaymentDto);

        return payment;
    }

    private static PaymentDto ToPaymentDto(Transaction transaction) =>
        new(transaction.PaymentId.Value,
            transaction.PaymentCard.MaskedCardNumber,
            transaction.Amount.Amount,
            transaction.Amount.CurrencyCode,
            transaction.Status.ToString("G"));

    private Task<Result<Transaction, RetrievePaymentError>> GetPayment(PaymentId id, MerchantId merchantId, CancellationToken cancellationToken) =>
        _payments.GetById(id, merchantId, cancellationToken)
            .ToResult(PaymentNotFound(id));

    private Result<PaymentId, RetrievePaymentError> TryParsePaymentId(string paymentId) =>
        PaymentId.IsValid(paymentId) ? PaymentId.From(paymentId) : new RetrievePaymentRequestValidationError(paymentId, "Invalid paymentId format");

    private RetrievePaymentError PaymentNotFound(PaymentId id) => new PaymentNotFoundError(id);
}
