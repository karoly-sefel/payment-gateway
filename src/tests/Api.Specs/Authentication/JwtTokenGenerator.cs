using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Checkout.PaymentGateway.Api.Merchants;
using Microsoft.IdentityModel.Tokens;

namespace Checkout.PaymentGateway.Api.Specs.Authentication;

public class JwtTokenGenerator
{
    internal const string Issuer = "http://payment-gateway.local";
    internal const string Audience = "http://localhost";

    public static string GenerateToken(string merchantId, string[] scopes)
    {
        string signingSecret = new String('*', 25);
        SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(signingSecret));

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, merchantId),
                new Claim(WellKnownClaims.MerchantId, merchantId),
            }.Concat(scopes.Select(scope => new Claim("scope", scope)))),
            Expires = DateTime.UtcNow.AddDays(7),
            Issuer = Issuer,
            Audience = Audience,
            SigningCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
