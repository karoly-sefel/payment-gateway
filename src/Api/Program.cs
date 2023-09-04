using Checkout.PaymentGateway.Api.Endpoints;
using Checkout.PaymentGateway.Application;
using Checkout.PaymentGateway.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddApplicationLayer();
builder.Services.AddInfrastructureLayer();

builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();

app.MapGet("/", () => "Hello World!");

app.MapRetrievePaymentEndpoint();

app.Run();
