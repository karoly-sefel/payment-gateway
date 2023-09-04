using BoDi;
using Checkout.PaymentGateway.Api.Specs.Context;
using Checkout.PaymentGateway.Api.Specs.Fakes;
using Checkout.PaymentGateway.Application.Payments.Queries;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Checkout.PaymentGateway.Api.Specs;

public class Application : WebApplicationFactory<Program>
{
    private readonly IObjectContainer _objectContainer;

    public Application(IObjectContainer objectContainer) =>
        _objectContainer = objectContainer;

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Replace(ServiceDescriptor.Singleton<IPaymentRepository, FakePaymentRepository>());
            services.AddTransient(_ => _objectContainer.Resolve<PaymentContext>());
        });

        base.ConfigureWebHost(builder);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        return base.CreateHost(builder);
    }
}
