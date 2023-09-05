using System.Text.Json.Serialization;
using Checkout.PaymentGateway.Application.Merchants;
using Checkout.PaymentGateway.Application.Payments.Queries;
using Checkout.PaymentGateway.Domain.Services;
using Checkout.PaymentGateway.Infrastructure.AcquiringBank;
using Checkout.PaymentGateway.Infrastructure.Persistence;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Json;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Infrastructure;

public static class RegisterDependencies
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>();
        services.AddSingleton<IMerchantRepository, InMemoryMerchantRepository>();
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.RegisterAcquiringBankClient();
        services.AddSingleton<Clock>(() => DateTime.UtcNow);

        services.Configure<JsonOptions>(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
            options.SerializerOptions.PropertyNameCaseInsensitive = true;
        });
    }
}
