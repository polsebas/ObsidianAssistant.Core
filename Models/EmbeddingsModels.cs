using System.Text.Json.Serialization;

namespace ObsidianAssistant.Core.Models;

public class RequestEmdeddingOpenAI
{
    public string text { get; set; } = string.Empty;
    public string model { get; set; } = string.Empty;
}

public class ResponseEmdeddingOpenAI
{
    [JsonPropertyName("object")]
    public string Tipo { get; set; }

    [JsonPropertyName("embedding")]
    public float[] Embedding { get; set; } = [];

    [JsonPropertyName("index")]
    public int Index { get; set; }
}