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

    public async Task<HttpResponseMessage> Get(string url)
    {
        var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Get, url));
        _responses.Add(response);
        return response;
    }

    public Task<T> GetLatestResponse<T>() where T : class => LatestResponseMessage.AsJson<T>();

    public async Task<HttpResponseMessage> Post<T>(string url, T payload)
    {
        var json = new StringContent(JsonSerializer.Serialize(payload), mediaType: new MediaTypeHeaderValue("application/json"));

        var response = await _httpClient.SendAsync(new HttpRequestMessage(HttpMethod.Post, url)
        {
            Content = json,
        });

        _responses.Add(response);
        return response;
    }
}
