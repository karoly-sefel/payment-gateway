namespace Checkout.PaymentGateway.BankSimulator.Endpoints.Dtos;

public class PaymentRequest
{
    public string MerchantId { get; set; }
    public string TransactionId { get; set; }
    public int Amount { get; set; }
    public string Currency { get; set; }
    public PaymentCard PaymentCard { get; set; }
}

public class PaymentCard
{
    public string CardNumber { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public int CVV { get; set; }
}
