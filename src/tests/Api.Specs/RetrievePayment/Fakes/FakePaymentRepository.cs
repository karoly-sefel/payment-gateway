using Checkout.PaymentGateway.Api.Specs.Context;
using Checkout.PaymentGateway.Application.Payments.Queries;
using Checkout.PaymentGateway.Domain.ValueObjects;
using CSharpFunctionalExtensions;
using static CSharpFunctionalExtensions.Maybe;

namespace Checkout.PaymentGateway.Api.Specs.Fakes;

public class FakePaymentRepository : IPaymentRepository
{
    private readonly PaymentContext _context;

    public FakePaymentRepository(PaymentContext context) => _context = context;

    public Task<Maybe<PaymentDto>> GetById(PaymentId paymentId, MerchantId merchantId, CancellationToken cancellationToken)
    {
        Maybe<PaymentDto> payment = None;

        if (_context.CurrentPayment?.PaymentId == paymentId.Value && _context.MerchantId == merchantId.Value)
            payment = _context.CurrentPayment;

        return Task.FromResult(payment);
    }
}
