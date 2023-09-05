using Checkout.PaymentGateway.Api.Specs.Context;
using Checkout.PaymentGateway.Application.Merchants;
using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;
using CSharpFunctionalExtensions;

namespace Checkout.PaymentGateway.Api.Specs.Fakes;

public class FakeMerchantRepository : IMerchantRepository
{
    private readonly PaymentContext _context;

    public FakeMerchantRepository(PaymentContext context) => _context = context;

    public Task<Maybe<Merchant>> FindById(MerchantId merchantId, CancellationToken cancellationToken)
    {
        if (_context.MerchantId == merchantId.Value)
        {
            var merchantData = _context.Merchants.FirstOrDefault(m => m.MerchantId == merchantId.Value);
            if (merchantData is not null)
            {
                var merchant = new Merchant(merchantId, "Test merchant");

                if(!merchantData.IsActive)
                    merchant.CloseAccount();

                return Task.FromResult(Maybe.From(merchant));
            }
        }

        return Task.FromResult(Maybe<Merchant>.None);
    }
}
