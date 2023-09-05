using BoDi;
using Checkout.PaymentGateway.Api.Specs.Authentication;
using Checkout.PaymentGateway.Api.Specs.Context;
using Checkout.PaymentGateway.Api.Specs.Fakes;
using Checkout.PaymentGateway.Api.Specs.Http;
using Checkout.PaymentGateway.Application.Merchants;
using Checkout.PaymentGateway.Application.Payments.Queries;
using Checkout.PaymentGateway.Application.Payments.Services;
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

        builder.UseSetting("Authentication:DefaultScheme", "Bearer");
        builder.UseSetting("Authentication:Schemes:Bearer:ValidAudiences:0", JwtTokenGenerator.Audience);
        builder.UseSetting("Authentication:Schemes:Bearer:ValidIssuer", JwtTokenGenerator.Issuer);
        builder.UseSetting("Authentication:Schemes:Bearer:ValidateIssuer", "false");

        builder.ConfigureTestServices(services =>
        {
            services.Replace(ServiceDescriptor.Singleton<IPaymentRepository, FakePaymentRepository>());
            services.Replace(ServiceDescriptor.Singleton<IMerchantRepository, FakeMerchantRepository>());
            services.Replace(ServiceDescriptor.Singleton<IPaymentIdGenerator>(_ => _objectContainer.Resolve<SequentialPaymentIdGenerator>()));
            services.AddTransient(_ => _objectContainer.Resolve<PaymentContext>());

            RegisterMockHttpClient(services);
        });

        base.ConfigureWebHost(builder);
    }

    private void RegisterMockHttpClient(IServiceCollection services)
    {
        var mockHttpMessageHandler = new MockHttpMessageHandler();
        _objectContainer.RegisterFactoryAs(_ => mockHttpMessageHandler).InstancePerContext();

        var client = mockHttpMessageHandler.ToHttpClient();
        client.BaseAddress = new Uri("https://test-bank.com");

        services.RemoveAll<HttpClient>();
        services.Add(ServiceDescriptor.Transient<HttpClient>(_ => client));

        services.Replace(ServiceDescriptor.Singleton<IHttpClientFactory>(_ => new HttpClientFactoryStub(client)));
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.UseEnvironment("Testing");
        return base.CreateHost(builder);
    }
}
