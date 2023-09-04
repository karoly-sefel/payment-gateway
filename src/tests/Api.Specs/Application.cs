using BoDi;
using Checkout.PaymentGateway.Api.Specs.Context;
using Checkout.PaymentGateway.Api.Specs.Fakes;
using Checkout.PaymentGateway.Application.Payments.Queries;
using Checkout.PaymentGateway.Application.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using RichardSzalay.MockHttp;
using TechTalk.SpecFlow;

namespace Checkout.PaymentGateway.Api.Specs;

public class Application : WebApplicationFactory<Program>
{
    private readonly IObjectContainer _objectContainer;

    public Application(IObjectContainer objectContainer) =>
        _objectContainer = objectContainer;

    [BeforeScenario]
    public void BeforeTestRunInjection(ITestRunnerManager testRunnerManager, ITestRunner testRunner)
    {

    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseSetting("AcquiringBank:BaseUrl", "https://test-bank.com");

        builder.ConfigureTestServices(services =>
        {
            services.Replace(ServiceDescriptor.Singleton<IPaymentRepository, FakePaymentRepository>());
            services.Replace(ServiceDescriptor.Singleton<IPaymentIdGenerator>(_ => _objectContainer.Resolve<SequentialPaymentIdGenerator>()));
            services.AddTransient(_ => _objectContainer.Resolve<PaymentContext>());

            var mockHttpMessageHandler = new MockHttpMessageHandler();
            _objectContainer.RegisterFactoryAs(_ => mockHttpMessageHandler).InstancePerContext();

            services.Replace(ServiceDescriptor.Transient<HttpClient>(_ =>
            {
                var client = mockHttpMessageHandler.ToHttpClient();
                client.BaseAddress = new Uri("https://test-bank.com");
                return client;
            }));
        });

        base.ConfigureWebHost(builder);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        return base.CreateHost(builder);
    }
}
