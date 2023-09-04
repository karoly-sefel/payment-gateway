using System.Reflection;
using Swashbuckle.AspNetCore.Filters;

namespace Checkout.PaymentGateway.Api.OpenApi;

public static class ConfigureOpenApi
{
    public static void AddOpenApi(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options => options.ExampleFilters());
        services.AddSwaggerExamplesFromAssemblies(Assembly.GetExecutingAssembly());
    }

    public static void UseOpenApi(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}
