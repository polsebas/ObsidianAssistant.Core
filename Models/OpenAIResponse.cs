namespace ObsidianAssistant.Core.Models;
public class OpenAIResponse
{
    public Choice[] Choices { get; set; }
}

public class Choice
{
    public string Text { get; set; }
}