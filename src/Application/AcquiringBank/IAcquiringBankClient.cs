using Checkout.PaymentGateway.Application.AcquiringBank.Data;
using PaymentRequest = Checkout.PaymentGateway.Application.Payments.Commands.PaymentRequest;

namespace Checkout.PaymentGateway.Application.AcquiringBank;

public interface IAcquiringBankClient
{
    Task<BankPaymentResult> SendPayment(PaymentRequest paymentRequest, CancellationToken cancellationToken);
}

