using System.Reflection;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Application;

public static class RegisterDependencies
{
    public static void AddApplicationLayer(this IServiceCollection services)
    {
        services.AddMediatR(config =>
            config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly())
        );
    }
}
