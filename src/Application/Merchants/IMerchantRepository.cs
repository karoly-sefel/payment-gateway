using Checkout.PaymentGateway.Domain.Entities;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Application.Merchants;

public interface IMerchantRepository
{
    public Task<Maybe<Merchant>> FindById(MerchantId merchantId, CancellationToken cancellationToken);
}
