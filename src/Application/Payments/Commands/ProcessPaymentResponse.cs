using Checkout.PaymentGateway.Domain.Entities;

namespace Checkout.PaymentGateway.Application.Payments.Commands;

public record ProcessPaymentResponse(string PaymentId, TransactionStatus Status, string StatusDescription);
