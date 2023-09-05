using Checkout.PaymentGateway.Application.AcquiringBank;
using Checkout.PaymentGateway.Application.Merchants;
using Checkout.PaymentGateway.Application.Payments.Queries;
using Checkout.PaymentGateway.Application.Payments.Services;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.Services;
using Checkout.PaymentGateway.Domain.ValueObjects;
using Microsoft.Extensions.Logging;

namespace Checkout.PaymentGateway.Application.Payments.Commands;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Result<ProcessPaymentResponse, PaymentError>>
{
    private readonly IAcquiringBankClient _acquiringBank;
    private readonly PaymentRequestToTransactionMapper _mapper;
    private readonly IPaymentRepository _paymentRepository;
    private readonly IMerchantRepository _merchants;
    private readonly Clock _clock;
    private readonly ILogger<ProcessPaymentCommandHandler> _logger;

    public ProcessPaymentCommandHandler(IAcquiringBankClient acquiringBank, PaymentRequestToTransactionMapper mapper, IPaymentRepository paymentRepository, IMerchantRepository merchants, Clock clock, ILogger<ProcessPaymentCommandHandler> logger)
    {
        _acquiringBank = acquiringBank;
        _mapper = mapper;
        _paymentRepository = paymentRepository;
        _merchants = merchants;
        _clock = clock;
        _logger = logger;
    }

    public async Task<Result<ProcessPaymentResponse, PaymentError>> Handle(ProcessPaymentCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing payment for merchant: {MerchantId}", command.MerchantId);

        var transactionResult = _mapper.TryCreateTransaction(command.PaymentRequest, command.MerchantId);

        Result<ProcessPaymentResponse, PaymentError> result = await transactionResult.Bind(async transaction =>
        {
            await _paymentRepository.RecordTransaction(transaction, cancellationToken);

            return await GetMerchant(command.MerchantId, cancellationToken)
                .Ensure(merchant => merchant.IsActive, AccountClosedError())
                .Bind(merchant => ProcessPayment(merchant, transaction, cancellationToken));
        });

        return result;
    }

    private async Task<Result<ProcessPaymentResponse, PaymentError>> ProcessPayment(Merchant merchant, Transaction transaction, CancellationToken cancellationToken)
    {
        var (transactionStatus, paymentErrorReason) = await _acquiringBank.ProcessPayment(transaction, merchant.Id, cancellationToken);

        var updatedTransaction = transaction with { UpdatedOn = _clock(), Status = transactionStatus, Error = paymentErrorReason };
        await _paymentRepository.RecordTransaction(updatedTransaction, cancellationToken);

        if (transactionStatus == TransactionStatus.Failed)
            return new AcquiringBankPaymentError(paymentErrorReason ?? PaymentErrorReason.Unknown, transaction.PaymentId);

        return new ProcessPaymentResponse(
            transaction.PaymentId.Value,
            updatedTransaction.Status.ToString("G"),
            updatedTransaction.Error?.ToString("G") ?? "Processed"
        );
    }

    private static PaymentError AccountClosedError() =>
        new(PaymentErrorReason.MerchantAccountClosed);

    private Task<Result<Merchant, PaymentError>> GetMerchant(MerchantId id, CancellationToken cancellationToken) =>
        _merchants.FindById(id, cancellationToken)
            .ToResult(new PaymentError(PaymentErrorReason.MerchantNotFound));
}
