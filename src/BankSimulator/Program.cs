using System.Text.Json.Serialization;
using Checkout.PaymentGateway.BankSimulator.Endpoints;
using Checkout.PaymentGateway.BankSimulator.Services;
using Microsoft.AspNetCore.Http.Json;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<PaymentService>();

builder.Services.Configure<JsonOptions>(options =>
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter())
);

var app = builder.Build();

app.MapGet("/", () => "Bank Simulator API");

app.MapTakePayment();

app.Run();
