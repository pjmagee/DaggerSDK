using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Dagger;

public class GraphQLClient
{
    readonly HttpClient _httpClient;

    public GraphQLClient() : this(Environment.GetEnvironmentVariable("DAGGER_SESSION_PORT")!, Environment.GetEnvironmentVariable("DAGGER_SESSION_TOKEN")!)
    {
        
    }

    public GraphQLClient(string port, string token, string scheme = "http", string host = "localhost")
    {
        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", GetTokenHeaderValue(token));
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        _httpClient.BaseAddress = new Uri($"{scheme}://{host}:{port}");
    }

    private static string GetTokenHeaderValue(string token) => Convert.ToBase64String(Encoding.UTF8.GetBytes($"{token}:"));
    
    public async Task<HttpResponseMessage> RequestAsync(string query, CancellationToken cancellationToken = default)
    {
        using StringContent content = new StringContent(JsonSerializer.Serialize(new { query }), Encoding.UTF8, "application/json");
        return await _httpClient.PostAsync("/query", content, cancellationToken);
    }
}