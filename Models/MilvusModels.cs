using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace ObsidianAssistant.Core.Models
{
    public class CreateCollectionPayload
    {
        [JsonPropertyName("collection_name")]
        public string CollectionName { get; set; }

        [JsonPropertyName("fields")]
        public List<Field> Fields { get; set; }
    }

    public class Field
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("data_type")]
        public string DataType { get; set; }

        [JsonPropertyName("is_primary_key")]
        public bool IsPrimaryKey { get; set; }

        [JsonPropertyName("auto_id")]
        public bool AutoId { get; set; }

        [JsonPropertyName("params")]
        public Dictionary<string, object> Params { get; set; }
    }

    public class InsertEmbeddingPayload
    {
        [JsonPropertyName("collection_name")]
        public string CollectionName { get; set; }

        [JsonPropertyName("entities")]
        public List<Entity> Entities { get; set; }
    }

    public class Entity
    {
        [JsonPropertyName("embedding")]
        public List<float> Embedding { get; set; }
    }

    public class SearchEmbeddingPayload
    {
        [JsonPropertyName("collection_name")]
        public string CollectionName { get; set; }

        [JsonPropertyName("query_vectors")]
        public List<List<float>> QueryVectors { get; set; }

        [JsonPropertyName("top_k")]
        public int TopK { get; set; }

        [JsonPropertyName("params")]
        public Dictionary<string, object> Params { get; set; }
    }

    public class SearchResult
    {
        [JsonPropertyName("results")]
        public List<List<float>> Results { get; set; }
    }
}
