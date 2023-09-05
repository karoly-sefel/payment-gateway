using System.Security.Claims;
using Checkout.PaymentGateway.Domain.ValueObjects;

namespace Checkout.PaymentGateway.Api.Merchants;

public class MerchantIdAccessor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MerchantIdAccessor(IHttpContextAccessor httpContextAccessor) => _httpContextAccessor = httpContextAccessor;

    public MerchantId CurrentMerchant => new(_httpContextAccessor.HttpContext?.User.FindFirstValue(WellKnownClaims.MerchantId)!);
}
