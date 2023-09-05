using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Application.AcquiringBank;

public interface IAcquiringBankClient
{
    Task<(TransactionStatus, PaymentErrorReason?)> ProcessPayment(Transaction paymentRequest, MerchantId merchantId, CancellationToken cancellationToken);
}

