using System.Text.Json;

namespace Checkout.PaymentGateway.Api.Specs.Http;

internal class DefaultJsonSerializerOptions
{
    internal static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web);
}
