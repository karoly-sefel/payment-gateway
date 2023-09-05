using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Application.Payments.Commands;

public record ProcessPaymentCommand(PaymentRequest PaymentRequest, MerchantId MerchantId) : IRequest<Result<ProcessPaymentResponse, PaymentError>>;
