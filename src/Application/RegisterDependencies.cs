using System.Reflection;
using Checkout.PaymentGateway.Application.AcquiringBank;
using Checkout.PaymentGateway.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Application;

public static class RegisterDependencies
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );

        services.AddScoped<IPaymentIdGenerator, PaymentIdGenerator>();

        services.AddScoped<IAcquiringBankClient, AcquiringBankClient>();
    }
}
