namespace Checkout.PaymentGateway.Application.AcquiringBank.Data;

public record PaymentRequest(
    string MerchantId,
    string TransactionId,
    int Amount,
    string Currency,
    PaymentCard PaymentCard
);

public record PaymentCard(
    string CardNumber,
    int ExpiryYear,
    int ExpiryMonth,
    string CVV
);
