using Checkout.PaymentGateway.Application.AcquiringBank.Data;

namespace Checkout.PaymentGateway.Application.AcquiringBank;

public interface IAcquiringBankClient
{
    Task<BankPaymentResult> SendPayment(PaymentRequest paymentRequest);
}

