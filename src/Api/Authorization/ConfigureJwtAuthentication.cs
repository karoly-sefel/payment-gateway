using System.IdentityModel.Tokens.Jwt;

namespace Checkout.PaymentGateway.Api.Authorization;

public static class ConfigureAuthorization
{
    public static void AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters.ClockSkew = TimeSpan.FromMinutes(5);

                if (configuration.GetValue<bool?>("Authentication:Schemes:Bearer:ValidateIssuer") == false)
                {
                    // NOTE: Issuer validation has been disabled to make it easier to share local development tokens
                    // Don't do this in production as anyone can forge JWT tokens to access your API
                    options.TokenValidationParameters.ValidateIssuer = false;
                    options.TokenValidationParameters.ValidateIssuerSigningKey = false;
                    options.TokenValidationParameters.SignatureValidator = (token, _) =>
                    {
                        var jwt = new JwtSecurityToken(token);
                        return jwt;
                    };
                }
            });
    }
}
