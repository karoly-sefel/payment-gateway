using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Checkout.PaymentGateway.Application.AcquiringBank.Data;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.AcquiringBank;

public class AcquiringBankClient : IAcquiringBankClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<AcquiringBankClient> _logger;

    private static readonly JsonSerializerOptions JsonSerializerOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };

    public AcquiringBankClient(HttpClient httpClient, ILogger<AcquiringBankClient> logger) => (_httpClient, _logger) = (httpClient, logger);

    public async Task<(TransactionStatus, PaymentErrorReason?)> ProcessPayment(Transaction paymentRequest, MerchantId merchantId, CancellationToken cancellationToken)
    {
        PaymentRequest payload = Map(paymentRequest, merchantId);

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync("/api/process-payment", payload, cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            string responseBody = await response.Content.ReadAsStringAsync(cancellationToken);
            _logger.LogError("Error while processing transaction with acquiring bank: {PaymentId} - Status code: {StatusCode} Body: {Response}",
                paymentRequest.PaymentId, response.StatusCode, responseBody);

            return (TransactionStatus.Failed, PaymentErrorReason.Unknown);
        }

        PaymentResponse result = (await response.Content.ReadFromJsonAsync<PaymentResponse>(options: JsonSerializerOptions, cancellationToken))!;

        var transactionStatus = Map(result.Status);
        var paymentErrorReason = Map(result.Result);
        return (transactionStatus, paymentErrorReason);
    }

    private PaymentErrorReason? Map(PaymentResult result) =>
        result switch
        {
            PaymentResult.Success => null,
            PaymentResult.InsufficientFunds => PaymentErrorReason.InsufficientFunds,
            PaymentResult.ExpiredCard => PaymentErrorReason.ExpiredCard,
            PaymentResult.SecurityViolation => PaymentErrorReason.SecurityViolation,
            PaymentResult.LostCardPickUp => PaymentErrorReason.LostCardPickUp,
            PaymentResult.AuthenticationRequired3DS => PaymentErrorReason.AuthenticationRequired3DS,
            PaymentResult.ActivityAmountLimitExceeded => PaymentErrorReason.ActivityAmountLimitExceeded,
            _ => PaymentErrorReason.Unknown
        };

    private TransactionStatus Map(PaymentStatus status) =>
        status switch
        {
            PaymentStatus.Approved => TransactionStatus.Success,
            PaymentStatus.Declined => TransactionStatus.Failed,
            PaymentStatus.Pending => TransactionStatus.Pending,
            _ => throw new ArgumentOutOfRangeException(nameof(status), status, null)
        };

    private PaymentRequest Map(Transaction transaction, MerchantId merchantId)
    {
        var paymentCard = transaction.PaymentCard;

        var card = new Data.PaymentCard(
            paymentCard.CardNumber.Value,
            paymentCard.ExpiryYear,
            paymentCard.ExpiryMonth,
            paymentCard.Cvv);

        var request = new PaymentRequest(
            merchantId.Value,
            transaction.PaymentId.Value,
            transaction.Amount.Amount,
            transaction.Amount.CurrencyCode,
            card
        );

        return request;
    }
}
