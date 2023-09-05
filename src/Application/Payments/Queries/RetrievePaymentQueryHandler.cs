using Checkout.PaymentGateway.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.Payments.Queries;

public class RetrievePaymentQueryHandler : IRequestHandler<RetrievePaymentQuery, Maybe<PaymentDto>>
{
    private readonly IPaymentRepository _payments;
    private readonly ILogger<RetrievePaymentQueryHandler> _logger;

    public RetrievePaymentQueryHandler(IPaymentRepository payments, ILogger<RetrievePaymentQueryHandler> logger) =>
        (_payments, _logger) = (payments, logger);

    public async Task<Maybe<PaymentDto>> Handle(RetrievePaymentQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Finding payment details for id: {PaymentId} merchant: {MerchantId}", query.PaymentId, query.MerchantId);

        PaymentId paymentId = PaymentId.From(query.PaymentId);
        Maybe<PaymentDto> payment = await _payments.GetById(paymentId, query.MerchantId, cancellationToken);

        return payment;
    }
}
