using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using ObsidianAssistant.Core.Models;

namespace ObsidianAssistant.Core.Services
{
    public class MilvusService
    {
        private readonly HttpClient _httpClient;
        private readonly string _milvusBaseUrl;

        public MilvusService(IConfiguration configuration)
        {
            _httpClient = new HttpClient();
            _milvusBaseUrl = configuration["Milvus:BaseUrl"];
        }

        public async Task CreateCollectionAsync(string collectionName)
        {
            var payload = new CreateCollectionPayload
            {
                CollectionName = collectionName,
                Fields = new List<Field>
                {
                    new Field
                    {
                        Name = "id",
                        DataType = "INT64",
                        IsPrimaryKey = true,
                        AutoId = true,
                        Params = new Dictionary<string, object>()
                    },
                    new Field
                    {
                        Name = "embedding",
                        DataType = "FLOAT_VECTOR",
                        IsPrimaryKey = false,
                        AutoId = false,
                        Params = new Dictionary<string, object> { { "dim", 128 } } // Assuming 128-dim vectors
                    }
                }
            };
            var response = await _httpClient.PostAsync(
                $"{_milvusBaseUrl}/collections",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
        }

        public async Task InsertEmbeddingAsync(string collectionName, float[] embedding)
        {
            var payload = new InsertEmbeddingPayload
            {
                CollectionName = collectionName,
                Entities = new List<Entity>
                {
                    new Entity
                    {
                        Embedding = new List<float>(embedding)
                    }
                }
            };
            var response = await _httpClient.PostAsync(
                $"{_milvusBaseUrl}/entities",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();
        }

        public async Task<List<float[]>> SearchEmbeddingAsync(string collectionName, float[] queryEmbedding, int topK)
        {
            var payload = new SearchEmbeddingPayload
            {
                CollectionName = collectionName,
                QueryVectors = new List<List<float>> { new List<float>(queryEmbedding) },
                TopK = topK,
                Params = new Dictionary<string, object> { { "nprobe", 10 } }
            };
            var response = await _httpClient.PostAsync(
                $"{_milvusBaseUrl}/search",
                new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json"));

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var result = JsonSerializer.Deserialize<SearchResult>(responseContent);

            return result.Results.Select(r => r.ToArray()).ToList();
        }
    }
}
