using System.Text.Json;
using System.Text.Json.Serialization;

namespace Checkout.PaymentGateway.Api.Http;

public class DefaultJsonSerializerOptions
{
    public static readonly JsonSerializerOptions Options = new(JsonSerializerDefaults.Web)
    {
        Converters = { new JsonStringEnumConverter() }
    };
}
