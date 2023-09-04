using Checkout.PaymentGateway.Api.Endpoints;
using Checkout.PaymentGateway.Api.OpenApi;
using Checkout.PaymentGateway.Application;
using Checkout.PaymentGateway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();

builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseOpenApi();

app.MapGet("/", () => "Payment Gateway API")
    .ExcludeFromDescription();

app.MapRetrievePaymentEndpoint();

app.Run();

// Make the implicit Program class public so test projects can access it
public partial class Program { }
