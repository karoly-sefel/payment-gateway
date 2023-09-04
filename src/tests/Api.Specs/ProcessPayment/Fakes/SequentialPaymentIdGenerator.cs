using Checkout.PaymentGateway.Application.Services;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Api.Specs.Fakes;

public class SequentialPaymentIdGenerator : IPaymentIdGenerator
{
    private int _nextId = 1;
    private const int IdLength = 12;

    public PaymentId? LastId { get; private set; }

    public Task<PaymentId> Generate()
    {
        string id = _nextId.ToString();
        LastId = PaymentId.From(id.PadLeft(IdLength - id.Length, 'a'));
        _nextId++;
        return Task.FromResult(LastId);
    }
}
