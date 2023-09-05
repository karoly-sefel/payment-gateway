using Checkout.PaymentGateway.Api.Merchants;

namespace Checkout.PaymentGateway.Api.Authorization;

public static class ConfigureMerchantAuthorization
{
    public static void AddMerchantAuthorization(this IServiceCollection services)
    {
        services.AddAuthorization();

        var authBuilder = services.AddAuthorizationBuilder();

        foreach (string scope in new[]{ Scopes.PaymentRead, Scopes.PaymentProcess})
        {
            authBuilder.AddPolicy(scope, policy =>
                policy.RequireClaim("scope", scope)
                    .RequireClaim(WellKnownClaims.MerchantId)
            );
        }
    }
}
