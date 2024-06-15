using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using ObsidianAssistant.Core.Models;

namespace ObsidianAssistant.Core.Services;

public class OpenAIClient
{
    private static readonly HttpClient client = new();
    private readonly IConfiguration _configuration;

    public OpenAIClient(IConfiguration configuration)
    {
        _configuration = configuration;
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _configuration["OpenAI:ApiKey"]);
    }

    public async Task<string> GetResponseAsync(RequestOpenAI requestOpenAI)
    {
        var jsonRequestBody = JsonConvert.SerializeObject(requestOpenAI);
        var request = new HttpRequestMessage(HttpMethod.Post, _configuration["OpenAI:UrlChat"])
        {
            Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json")
        };
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
