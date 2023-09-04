using Ardalis.GuardClauses;
using Checkout.PaymentGateway.Application.AcquiringBank;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Infrastructure.AcquiringBank;

public static class HttpClientConfigurator
{
    public static void RegisterAcquiringBankClient(this IServiceCollection services)
    {
        services.AddSingleton<AcquiringBankConfig>(provider =>
        {
            string configKey = "AcquiringBank:BaseUrl";
            var config = provider.GetRequiredService<IConfiguration>();
            var baseUrl = config.GetValue<string>(configKey);
            Guard.Against.NullOrEmpty(baseUrl, "baseUrl", configKey);
            return new AcquiringBankConfig { BaseUrl = baseUrl };
        });

        services.AddHttpClient<AcquiringBankClient>((serviceProvider, client) =>
        {
            AcquiringBankConfig config = serviceProvider.GetRequiredService<AcquiringBankConfig>();
            client.BaseAddress = new Uri(config.BaseUrl);
        });
    }
}
