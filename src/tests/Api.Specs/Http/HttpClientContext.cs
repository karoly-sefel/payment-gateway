using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace Checkout.PaymentGateway.Api.Specs.Http;

[Binding]
public class HttpClientContext
{
    private readonly List<HttpResponseMessage> _responses = new();

    private readonly HttpClient _httpClient;

    public HttpClientContext(Application application)
    {
        _httpClient = application.CreateDefaultClient(
            new Uri("http://localhost")
        );
    }

    public HttpResponseMessage? LatestResponseMessage => _responses.LastOrDefault();
    public HttpStatusCode? LatestStatusCode => LatestResponseMessage?.StatusCode;

    public async Task<HttpResponseMessage> Get(string url, string? bearerToken = null)
    {
        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Get, url);
        SetAuthorizationHeader(httpRequestMessage, bearerToken);

        var response = await _httpClient.SendAsync(httpRequestMessage);
        _responses.Add(response);
        return response;
    }

    private static void SetAuthorizationHeader(HttpRequestMessage httpRequestMessage, string? bearerToken)
    {
        if (bearerToken is not null)
            httpRequestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
    }

    public Task<T> GetLatestResponse<T>() where T : class => LatestResponseMessage.AsJson<T>();

    public async Task<HttpResponseMessage> Post<T>(string url, T payload, string? bearerToken = null)
    {
        var json = new StringContent(JsonSerializer.Serialize(payload), mediaType: new MediaTypeHeaderValue("application/json"));

        var httpRequestMessage = new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = json,
        };

        SetAuthorizationHeader(httpRequestMessage, bearerToken);

        var response = await _httpClient.SendAsync(httpRequestMessage);

        _responses.Add(response);
        return response;
    }
}
