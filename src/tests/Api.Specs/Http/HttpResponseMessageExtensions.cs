using System.Net.Http.Json;
using System.Text.Json;
using Ardalis.GuardClauses;

namespace Checkout.PaymentGateway.Api.Specs.Http;

public static class HttpResponseMessageExtensions
{
    private static readonly JsonSerializerOptions JsonSerializerOptions = new(JsonSerializerDefaults.Web);

    public static Task<string> AsString(this HttpResponseMessage? response)
    {
        HttpContent httpContent = response.GetContentOrThrow();
        return httpContent.ReadAsStringAsync();
    }

    public static async Task<T> AsJson<T>(this HttpResponseMessage? response)
    {
        HttpContent httpContent = response.GetContentOrThrow();

        string json = await httpContent.ReadAsStringAsync();
        StringContent clone = new StringContent(json);
        T? obj = await clone.ReadFromJsonAsync<T>(JsonSerializerOptions);

        Guard.Against.Null(obj, "Cannot convert response body to {typeof(T)}");

        return obj;
    }

    private static HttpContent GetContentOrThrow(this HttpResponseMessage? response)
    {
        HttpContent? httpContent = response?.Content;
        Guard.Against.Null(httpContent, "Response message doesn't have any content");
        return httpContent;
    }
}
