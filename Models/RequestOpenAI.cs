using System.Collections.Generic;

namespace ObsidianAssistant.Core.Models;

public class RequestOpenAI
{
    public List<Message> messages { get; set; } = new();
    public string model { get; set; } = string.Empty;
    public int max_tokens{ get; set; }
    public int n { get; set; } = 1;
    public decimal temperature { get; set; } = 1;
    public decimal? frequency_penalty { get;set; }
    public bool stream { get; set; } = false;
}
