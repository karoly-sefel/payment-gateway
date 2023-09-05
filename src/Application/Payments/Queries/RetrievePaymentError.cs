using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Application.Payments.Queries;

public record RetrievePaymentError(string Message, ErrorCategory Category);

public record RetrievePaymentRequestValidationError(string PaymentId, string Message) : RetrievePaymentError(Message, ErrorCategory.ValidationError);

public record PaymentNotFoundError(PaymentId PaymentId) : RetrievePaymentError($"Payment not found with id: {PaymentId}", ErrorCategory.ValidationError);

public enum ErrorCategory
{
    Unknown,
    ValidationError,
}
