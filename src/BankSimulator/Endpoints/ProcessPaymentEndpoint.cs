using Checkout.PaymentGateway.BankSimulator.Endpoints.Dtos;
using Checkout.PaymentGateway.BankSimulator.Services;
using Microsoft.AspNetCore.Mvc;

namespace Checkout.PaymentGateway.BankSimulator.Endpoints;

public static class TakePaymentEndpoint
{
    public static void MapTakePayment(this WebApplication app) =>
        app.MapPost("/api/process-payment", Handle);

    private static Task<IResult> Handle([FromBody]PaymentRequest paymentRequest, [FromServices]PaymentService paymentService, CancellationToken cancelToken)
    {
        var result = paymentService.ProcessPayment(paymentRequest);
        return Task.FromResult(Results.Json(result));
    }
}
