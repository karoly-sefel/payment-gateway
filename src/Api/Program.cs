using Asp.Versioning;
using Asp.Versioning.Builder;
using Checkout.PaymentGateway.Api.Authorization;
using Checkout.PaymentGateway.Api.Endpoints;
using Checkout.PaymentGateway.Api.Merchants;
using Checkout.PaymentGateway.Api.OpenApi;
using Checkout.PaymentGateway.Application;
using Checkout.PaymentGateway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();

builder.Services.AddScoped<MerchantIdAccessor>();

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1);
    options.ReportApiVersions = true;
}).AddApiExplorer(options =>
{
    options.SubstituteApiVersionInUrl = true;
    options.GroupNameFormat = "'v'V";
});

builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddMerchantAuthorization();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseAuthentication();
app.UseAuthorization();

IVersionedEndpointRouteBuilder versionedApi = app
    .NewVersionedApi()
    .HasApiVersion(1);

app.UseOpenApi();

versionedApi.MapGet("/", () => "Payment Gateway API")
    .IsApiVersionNeutral()
    .ExcludeFromDescription();

versionedApi.MapRetrievePaymentEndpoint();
versionedApi.MapProcessPaymentEndpoint();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
