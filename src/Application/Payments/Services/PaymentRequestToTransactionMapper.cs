using Ardalis.GuardClauses;
using Checkout.PaymentGateway.Application.Payments.Commands;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.Services;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Application.Payments.Services;

public class PaymentRequestToTransactionMapper
{
    private static readonly string[] SupportedCurrencies = { "EUR", "GBP", "USD", "DKK", "JPY" };

    private readonly IPaymentIdGenerator _paymentIdGenerator;
    private readonly Clock _clock;

    public PaymentRequestToTransactionMapper(IPaymentIdGenerator paymentIdGenerator, Clock clock)
    {
        _paymentIdGenerator = paymentIdGenerator;
        _clock = clock;
    }

    public Result<Transaction, PaymentError> TryCreateTransaction(PaymentRequest request, MerchantId merchantId)
    {
        var transaction = from paymentCard in ValidateCardDetails(request, _clock)
            from paymentAmount in ValidateAmountAndCurrency(request.Amount, request.Currency)
            from paymentId in GeneratePaymentId()
            select new Transaction(merchantId, paymentId, paymentCard, paymentAmount, TransactionStatus.Created, _clock(), _clock());

        return transaction;
    }

    private Result<PaymentId, PaymentError> GeneratePaymentId() => _paymentIdGenerator.Generate();

    private Result<PaymentCard, PaymentError> ValidateCardDetails(PaymentRequest paymentRequest, Clock clock)
    {
        Guard.Against.Null(paymentRequest);

        if (!PaymentCardValidator.IsValidCardNumber(paymentRequest.CardNumber))
            return new PaymentValidationError(nameof(paymentRequest.CardNumber), "Invalid Card Number");

        if (!PaymentCardValidator.IsValidCvv(paymentRequest.CVV))
            return new PaymentValidationError(nameof(paymentRequest.CVV), "Invalid CVV number");

        if (!PaymentCardValidator.IsValidYear(paymentRequest.ExpiryYear))
            return new PaymentValidationError(nameof(paymentRequest.ExpiryYear), "Invalid Expiry Year");

        if (!PaymentCardValidator.IsValidMonth(paymentRequest.ExpiryMonth))
            return new PaymentValidationError(nameof(paymentRequest.ExpiryMonth), "Invalid Expiry Month");

        var paymentCard = new PaymentCard(paymentRequest.CardNumber, paymentRequest.ExpiryYear, paymentRequest.ExpiryMonth, paymentRequest.CVV);

        if (paymentCard.IsExpired(clock))
            return new PaymentError(PaymentErrorReason.ExpiredCard);

        return paymentCard;
    }

    private Result<PaymentAmount, PaymentError> ValidateAmountAndCurrency(int amount, string currency)
    {
        if (amount <= 0)
            return new PaymentValidationError(nameof(PaymentRequest.Amount), "Payment amount must be a positive number");

        if (!SupportedCurrencies.Contains(currency))
            return new PaymentError(PaymentErrorReason.NotSupportedCurrency);

        return new PaymentAmount(amount, currency);
    }
}
