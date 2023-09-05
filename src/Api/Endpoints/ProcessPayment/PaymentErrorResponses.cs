using Checkout.PaymentGateway.Domain.Entities;
using IResult = Microsoft.AspNetCore.Http.IResult;

namespace Checkout.PaymentGateway.Api.Endpoints;

public class PaymentErrorResponses
{
    internal static IResult MapToProblemDetailsResponse(PaymentError paymentError, HttpContext httpContext) =>
        paymentError switch
        {
            PaymentValidationError validationError => MapToProblemDetails(validationError, httpContext),
            AcquiringBankPaymentError bankPaymentError => MapToProblemDetails(bankPaymentError, httpContext),
            PaymentError error => MapToProblemDetails(error, httpContext),
            _ => Results.Problem("Internal Server Error", statusCode: StatusCodes.Status500InternalServerError)
        };

    private static IResult MapToProblemDetails(PaymentError paymentError, HttpContext context) =>
        Results.Problem(
            detail: "Transaction failed",
            instance: context.Request.Path,
            title: "Payment error",
            type: "https://httpstatuses.com/422",
            statusCode: StatusCodes.Status422UnprocessableEntity,
            extensions: new Dictionary<string, object?>
            {
                { "errorCode", paymentError.Reason },
                { "paymentId", (paymentError as AcquiringBankPaymentError)?.PaymentId.Value }
            }
        );

    private static IResult MapToProblemDetails(PaymentValidationError error, HttpContext context) =>
        Results.ValidationProblem(
            new Dictionary<string, string[]>
            {
                { error.Field, new[] { error.Message } }
            },
            detail: "Payment validation error",
            instance: context.Request.Path,
            title: "Bad Request",
            type: "https://httpstatuses.com/404",
            statusCode: StatusCodes.Status400BadRequest
        );
}
