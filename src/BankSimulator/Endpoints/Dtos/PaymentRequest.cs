namespace Checkout.PaymentGateway.BankSimulator.Endpoints.Dtos;

public class PaymentRequest
{
    public required string MerchantId { get; set; }
    public required string TransactionId { get; set; }
    public required int Amount { get; set; }
    public required string Currency { get; set; }
    public required PaymentCard PaymentCard { get; set; }
}

public class PaymentCard
{
    public required string CardNumber { get; set; }
    public required int ExpiryMonth { get; set; }
    public required int ExpiryYear { get; set; }
    public required int CVV { get; set; }
}
