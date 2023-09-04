using Checkout.PaymentGateway.Application.AcquiringBank;
using Checkout.PaymentGateway.Application.AcquiringBank.Data;
using Checkout.PaymentGateway.Application.Services;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Application.Payments.Commands;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Result<ProcessPaymentResponse, PaymentError>>
{
    private readonly IAcquiringBankClient _bankClient;
    private readonly IPaymentIdGenerator _idGenerator;

    public ProcessPaymentCommandHandler(IAcquiringBankClient bankClient, IPaymentIdGenerator idGenerator)
    {
        _bankClient = bankClient;
        _idGenerator = idGenerator;
    }

    public async Task<Result<ProcessPaymentResponse, PaymentError>> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        BankPaymentResult bankResponse = await _bankClient.SendPayment(new AcquiringBank.Data.PaymentRequest());

        if (bankResponse != BankPaymentResult.Success)
            return new PaymentError(PaymentErrorReason.InsufficientFunds);

        PaymentId paymentId = await _idGenerator.Generate();

        return new ProcessPaymentResponse(paymentId.Value);
    }
}
