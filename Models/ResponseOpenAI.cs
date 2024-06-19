using System.Text.Json.Serialization;
using System.Collections.Generic;

namespace ObsidianAssistant.Core.Models;

public class ResponseOpenAI
{
    [JsonPropertyName("id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("object")]
    public string Object { get; set; } = string.Empty;

    [JsonPropertyName("created")]
    public long Created { get; set; }

    [JsonPropertyName("model")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("system_fingerprint")]
    public string SystemFingerprint { get; set; } = string.Empty;

    [JsonPropertyName("choices")]
    public List<Choice> Choices { get; set; } = new List<Choice>();

    [JsonPropertyName("usage")]
    public Usage Usage { get; set; } = new Usage();
}

public class Choice
{
    [JsonPropertyName("index")]
    public int Index { get; set; }

    [JsonPropertyName("message")]
    public Message Message { get; set; } = new Message();

    [JsonPropertyName("logprobs")]
    public object Logprobs { get; set; } = null;

    [JsonPropertyName("finish_reason")]
    public string FinishReason { get; set; } = string.Empty;
}

public class Message
{
    [JsonPropertyName("role")]
    public string Role { get; set; } = string.Empty;

    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;
}

public class Usage
{
    [JsonPropertyName("prompt_tokens")]
    public int PromptTokens { get; set; }

    [JsonPropertyName("completion_tokens")]
    public int CompletionTokens { get; set; }

    [JsonPropertyName("total_tokens")]
    public int TotalTokens { get; set; }
}

// public class ResponseEmdeddingOpenAI
// {
//     [JsonPropertyName("object")]
//     public string Tipo { get; set; }

//     [JsonPropertyName("embedding")]
//     public List<float> Embedding { get; set; } = new List<float>();

//     [JsonPropertyName("index")]
//     public int Index { get; set; }
// }