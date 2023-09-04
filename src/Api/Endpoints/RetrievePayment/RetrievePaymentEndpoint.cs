using Checkout.PaymentGateway.Api.Endpoints.Examples;
using Checkout.PaymentGateway.Application.Payments.Queries;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using IResult = Microsoft.AspNetCore.Http.IResult;
using static Checkout.PaymentGateway.Api.Http.ContentTypes;

namespace Checkout.PaymentGateway.Api.Endpoints;

public static class RetrievePaymentEndpoint
{
    public static void MapRetrievePaymentEndpoint(this WebApplication app) =>
        app.MapGet("/payments/{id}", Handle)
            .WithSummary("Retrieve a payment by id")
            .WithTags("Payments")
            .Produces<PaymentDto>()
            .ProducesProblem(StatusCodes.Status400BadRequest, ProblemDetailsJson)
            .ProducesProblem(StatusCodes.Status404NotFound, ProblemDetailsJson)
            .ProducesProblem(StatusCodes.Status500InternalServerError, ProblemDetailsJson)
            .WithOpenApi(generatedOperation =>
            {
                var parameter = generatedOperation.Parameters[0];
                parameter.Description = "Payment ID";
                return generatedOperation;
            });

    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RetrievePaymentResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(RetrievePaymentResponse404Example))]
    private static async Task<IResult> Handle(string id, [FromServices]IMediator mediator, HttpContext httpContext, CancellationToken cancelToken)
    {
        Maybe<PaymentDto> payment = await mediator.Send(new RetrievePaymentQuery(id), cancelToken);

        return payment.Match(
            dto => Results.Json(dto, contentType: "application/json"),
            () => PaymentNotFound(id, httpContext)
        );
    }

    private static IResult PaymentNotFound(string paymentId, HttpContext context) =>
        Results.Problem(
            detail: "No payment record can be found with the given id",
            instance: context.Request.Path,
            title: "Payment not found",
            type: "https://httpstatuses.com/404",
            statusCode: StatusCodes.Status404NotFound,
            extensions: new Dictionary<string, object?>
            {
                { "paymentId", paymentId }
            }
        );
}
