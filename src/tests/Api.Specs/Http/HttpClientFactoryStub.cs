namespace Checkout.PaymentGateway.Api.Specs.Http;

internal class HttpClientFactoryStub : IHttpClientFactory
{
    private readonly HttpClient _httpClient;

    public HttpClientFactoryStub(HttpClient httpClient) => _httpClient = httpClient;

    public HttpClient CreateClient(string name) => _httpClient;
}
