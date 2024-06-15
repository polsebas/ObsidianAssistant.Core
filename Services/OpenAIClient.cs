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

    public OpenAIClient(IConfiguration configuration)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", configuration["OpenAI:ApiKey"]);
    }

    public async Task<string> GetResponseAsync(RequestOpenAI requestOpenAI)
    {
        var jsonRequestBody = JsonConvert.SerializeObject(requestOpenAI);
        var request = new HttpRequestMessage(HttpMethod.Post, "https://api.openai.com/v1/chat/completions")
        {
            Content = new StringContent(jsonRequestBody, Encoding.UTF8, "application/json")
        };
        var response = await client.SendAsync(request);
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadAsStringAsync();
    }
}
