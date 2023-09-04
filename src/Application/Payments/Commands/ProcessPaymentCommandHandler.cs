using Checkout.PaymentGateway.Application.Services;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Application.Payments.Commands;

public class ProcessPaymentCommandHandler : IRequestHandler<ProcessPaymentCommand, Result<ProcessPaymentResponse, Exception>>
{
    private readonly IPaymentIdGenerator _idGenerator;

    public ProcessPaymentCommandHandler(IPaymentIdGenerator idGenerator) => _idGenerator = idGenerator;

    public async Task<Result<ProcessPaymentResponse, Exception>> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        PaymentId paymentId = await _idGenerator.Generate();

        return new ProcessPaymentResponse(paymentId.Value);
    }
}
