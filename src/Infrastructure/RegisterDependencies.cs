using Checkout.PaymentGateway.Application.Payments.Queries;
using Checkout.PaymentGateway.Infrastructure.Persistence;
using Microsoft.Extensions.DependencyInjection;

namespace Checkout.PaymentGateway.Infrastructure;

public static class RegisterDependencies
{
    public static void AddInfrastructureLayer(this IServiceCollection services)
    {
        services.AddSingleton<IPaymentRepository, InMemoryPaymentRepository>();
    }
}
