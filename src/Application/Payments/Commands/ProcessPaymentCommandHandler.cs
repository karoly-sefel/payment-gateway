using Checkout.PaymentGateway.Application.AcquiringBank;
using Checkout.PaymentGateway.Application.AcquiringBank.Data;
using Checkout.PaymentGateway.Application.Merchants;
using Checkout.PaymentGateway.Application.Services;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.Payments.Commands;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Result<ProcessPaymentResponse, PaymentError>>
{
    private readonly IAcquiringBankClient _acquiringBank;
    private readonly IPaymentIdGenerator _idGenerator;
    private readonly IMerchantRepository _merchants;
    private readonly ILogger<ProcessPaymentCommandHandler> _logger;

    public ProcessPaymentCommandHandler(IAcquiringBankClient acquiringBank, IPaymentIdGenerator idGenerator, IMerchantRepository merchants, ILogger<ProcessPaymentCommandHandler> logger)
    {
        _acquiringBank = acquiringBank;
        _idGenerator = idGenerator;
        _merchants = merchants;
        _logger = logger;
    }

    public async Task<Result<ProcessPaymentResponse, PaymentError>> Handle(ProcessPaymentCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing payment for merchant: {MerchantId}", command.MerchantId);

        Result<ProcessPaymentResponse, PaymentError> result = await GetMerchant(command.MerchantId, cancellationToken)
            .Ensure(merchant => merchant.IsActive, AccountClosedError())
            .Bind(merchant => ProcessPayment(merchant, command.PaymentRequest, cancellationToken));

        return result;
    }

    private async Task<Result<ProcessPaymentResponse, PaymentError>> ProcessPayment(Merchant merchant, PaymentRequest paymentRequest, CancellationToken cancellationToken)
    {
        BankPaymentResult bankResponse = await _acquiringBank.SendPayment(paymentRequest, cancellationToken);

        if (bankResponse != BankPaymentResult.Success)
            return new PaymentError(PaymentErrorReason.InsufficientFunds);

        PaymentId paymentId = await _idGenerator.Generate();

        return new ProcessPaymentResponse(paymentId.Value);
    }

    private static PaymentError AccountClosedError() =>
        new(PaymentErrorReason.MerchantAccountClosed);

    private Task<Result<Merchant, PaymentError>> GetMerchant(MerchantId id, CancellationToken cancellationToken) =>
        _merchants.FindById(id, cancellationToken)
            .ToResult(new PaymentError(PaymentErrorReason.MerchantNotFound));
}
