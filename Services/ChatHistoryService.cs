using System;
using System.Collections.Generic;
using System.Linq;
using ObsidianAssistant.Core.Models;

namespace ObsidianAssistant.Core.Services;
public class ChatHistoryService
{
    public static List<Message> TruncateHistory(List<Message> history, int maxTokens = 2048)
    {
        int currentTokens = history.Sum(msg => msg.content.Split(' ').Length);
        while (currentTokens > maxTokens)
        {
            history.RemoveAt(1); // Elimina el mensaje más antiguo (después del 'system')
            currentTokens = history.Sum(msg => msg.content.Split(' ').Length);
        }
        return history;
    }
}
