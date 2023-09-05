using Asp.Versioning.Builder;
using Checkout.PaymentGateway.Api.Authorization;
using Checkout.PaymentGateway.Api.Endpoints.Examples;
using Checkout.PaymentGateway.Api.Merchants;
using Checkout.PaymentGateway.Application.Payments.Queries;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Filters;
using IResult = Microsoft.AspNetCore.Http.IResult;
using static Checkout.PaymentGateway.Api.Http.ContentTypes;

namespace Checkout.PaymentGateway.Api.Endpoints;

public static class RetrievePaymentEndpoint
{
    public static void MapRetrievePaymentEndpoint(this IVersionedEndpointRouteBuilder app) =>
        app.MapGet("/v{version:apiVersion}/payments/{id}", Handle)
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
            })
            .RequireAuthorization(Scopes.PaymentRead);

    [SwaggerResponseExample(StatusCodes.Status200OK, typeof(RetrievePaymentResponseExample))]
    [SwaggerResponseExample(StatusCodes.Status404NotFound, typeof(RetrievePaymentResponse404Example))]
    private static async Task<IResult> Handle(string id, [FromServices] MerchantIdAccessor merchant, [FromServices] IMediator mediator, HttpContext httpContext,
        CancellationToken cancelToken)
    {
        Result<PaymentDto, RetrievePaymentError> payment = await mediator.Send(new RetrievePaymentQuery(id, merchant.CurrentMerchant), cancelToken);

        return payment.Match(
            data => Results.Json(data, contentType: "application/json"),
            error => ErrorResponses.MapToProblemDetailsResponse(error, httpContext)
        );
    }
}
