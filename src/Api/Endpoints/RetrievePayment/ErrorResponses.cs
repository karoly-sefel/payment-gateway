using Checkout.PaymentGateway.Application.Payments.Queries;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Checkout.PaymentGateway.Api.Endpoints;

public class ErrorResponses
{
    internal static IResult MapToProblemDetailsResponse(RetrievePaymentError error, HttpContext context) =>
        error switch
        {
            PaymentNotFoundError notFound => PaymentNotFound(notFound, context),
            RetrievePaymentRequestValidationError badRequest => InvalidPaymentId(badRequest, context),
            _ => Results.Problem("Internal Server Error", statusCode: StatusCodes.Status500InternalServerError)
        };

    private static IResult PaymentNotFound(PaymentNotFoundError error, HttpContext context) =>
        Results.Problem(
            detail: "No payment record can be found with the given id",
            instance: context.Request.Path,
            title: "Payment not found",
            type: "https://httpstatuses.com/404",
            statusCode: StatusCodes.Status404NotFound,
            extensions: new Dictionary<string, object?>
            {
                { "paymentId", error.PaymentId }
            }
        );

    private static IResult InvalidPaymentId(RetrievePaymentRequestValidationError error, HttpContext context) =>
        Results.ValidationProblem(
            new Dictionary<string, string[]>
            {
                { "paymentId", new[] { error.PaymentId } }
            },
            detail: "Invalid payment id format",
            instance: context.Request.Path,
            title: "Bad Request",
            type: "https://httpstatuses.com/400",
            statusCode: StatusCodes.Status400BadRequest
        );
}
