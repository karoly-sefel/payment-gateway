using Asp.Versioning.Builder;
using Checkout.PaymentGateway.Api.Endpoints.Examples;
using Checkout.PaymentGateway.Application.Payments.Commands;
using Checkout.PaymentGateway.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using IResult = Microsoft.AspNetCore.Http.IResult;
using static Checkout.PaymentGateway.Api.Http.ContentTypes;

namespace Checkout.PaymentGateway.Api.Endpoints;

public static class ProcessPaymentEndpoint
{
    public static void MapProcessPaymentEndpoint(this IVersionedEndpointRouteBuilder app) =>
        app.MapPost("/v{version:apiVersion}/payments", Handle)
            .WithSummary("Process payment")
            .WithTags("Payments")
            .Produces(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest, ProblemDetailsJson)
            .ProducesProblem(StatusCodes.Status500InternalServerError, ProblemDetailsJson);

    [SwaggerRequestExample(typeof(PaymentRequest), typeof(ProcessPaymentRequest201Example))]
    private static async Task<IResult> Handle([FromBody]PaymentRequest paymentRequest, [FromServices]IMediator mediator, HttpContext httpContext, CancellationToken cancelToken)
    {
        Result<ProcessPaymentResponse, PaymentError> result = await mediator.Send(new ProcessPaymentCommand(paymentRequest), cancelToken);

        return result.Match(
            response => Results.Created($"/v1/payments/{response.PaymentId}", response),
            error => Results.UnprocessableEntity(error.Reason)
        );
    }
}
