using Checkout.PaymentGateway.Application.Merchants;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;
using CSharpFunctionalExtensions;

namespace Checkout.PaymentGateway.Infrastructure.Persistence;

public class InMemoryMerchantRepository : IMerchantRepository
{
    private static readonly Task<Maybe<Merchant>> NotFoundResult = Task.FromResult(Maybe<Merchant>.None);

    private readonly Dictionary<MerchantId, Merchant> _merchants = new()
    {
        { new MerchantId("merchantA"), new Merchant(new MerchantId("merchantA"), "Merchant A Ltd.") },
        { new MerchantId("merchantB"), new Merchant(new MerchantId("merchantB"), "Merchant B Plc.") },
        { new MerchantId("merchantC"), new Merchant(new MerchantId("merchantC"), "Merchant C Ltd.") },
    };


    public InMemoryMerchantRepository()
    {
        var merchantC = _merchants[MerchantId.From("merchantC")];
        merchantC.CloseAccount();
    }

    public Task<Maybe<Merchant>> FindById(MerchantId merchantId, CancellationToken cancellationToken)
    {
        if (!_merchants.TryGetValue(merchantId, out Merchant? merchant))
            return NotFoundResult;

        return Task.FromResult(Maybe<Merchant>.From(merchant));
    }
}
