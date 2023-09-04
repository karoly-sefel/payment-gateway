using Checkout.PaymentGateway.Application.Payments.Queries;
using Microsoft.AspNetCore.Mvc;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Checkout.PaymentGateway.Api.Endpoints;

public static class RetrievePaymentEndpoint
{
    public static void MapRetrievePaymentEndpoint(this WebApplication app) =>
        app.MapGet("/payments/{id}", Handle);

    private static async Task<IResult> Handle(string id, [FromServices]IMediator mediator, CancellationToken cancelToken)
    {
        Maybe<PaymentDto> payment = await mediator.Send(new RetrievePaymentQuery(id), cancelToken);

        return payment.Match(
            dto => TypedResults.Json(dto, contentType: "application/json"),
            () => Results.NotFound($"No payment found with id: {id}")
        );
    }
}
