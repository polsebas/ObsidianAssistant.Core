using System;
using System.Text.Json;
using System.Threading.Tasks;
using System.Linq;
using ObsidianAssistant.Core.Models;
using System.Collections.Generic;

namespace ObsidianAssistant.Core.Services;

public class OpenAIService
{
    private readonly MarkdownService _markdownService;
    private readonly OpenAIClient _openAIclient;
    private List<Message> _conversationHistory;

    public OpenAIService(MarkdownService markdownService, OpenAIClient openAIClient)
    {            
        _markdownService = markdownService;
        _openAIclient = openAIClient;
        _conversationHistory = [
            new() {
                    Role = "system",
                    Content = @$"You are an expert in using Obsidian for note-taking and knowledge management.             
You are an expert in using Retrieval-Augmented Generation (RAG) within Obsidian for creating and managing notes. Your task is to help the user create detailed and well-formatted notes. For each note, you will:
1. **Create a well-structured note**: Ensure the note is organized with headings, bullet points, and any other necessary markdown formatting.
2. **Format the note using Markdown**: Include headings, subheadings, bullet points, numbered lists, bold and italic text, code blocks, and any other relevant markdown syntax to make the note clear and easy to read.
3. **Propose relevant tags**: Suggest a list of tags based on the content of the note to help the user categorize and find their notes easily.
4. **Suggest associations with other notes**: Identify and recommend possible links to other notes in the user's Obsidian vault that are related to the current note, helping to build a network of interconnected notes.

Here's the format to follow:
**User Input**: 
- Note content provided by the user.

**Output**:
- Well-structured note with appropriate markdown formatting.
- A list of proposed tags.
- Suggestions for associations with other notes.

### Example
**User Input**:
Today I learned about the basics of machine learning. Machine learning is a subset of artificial intelligence that involves training algorithms to make predictions or decisions based on data. The key concepts include supervised learning, unsupervised learning, and reinforcement learning.

**Output**:
**Markdown Formatted Note**:
##Machine Learning Basics
Today I learned about the basics of machine learning. Machine learning is a subset of artificial intelligence that involves training algorithms to make predictions or decisions based on data.

###Key Concepts
Supervised Learning: Training algorithms on labeled data.
Unsupervised Learning: Training algorithms on unlabeled data.
Reinforcement Learning: Training algorithms through rewards and punishments.

**Proposed Tags**:
- #machinelearning
- #artificialintelligence
- #supervisedlearning
- #unsupervisedlearning
- #reinforcementlearning

**Suggested Associations with Other Notes**:
- [Introduction to Artificial Intelligence](obsidian://vault/AI/Introduction%20to%20Artificial%20Intelligence)
- [Data Science Fundamentals](obsidian://vault/DataScience/Data%20Science%20Fundamentals)
- [Supervised vs Unsupervised Learning](obsidian://vault/MachineLearning/Supervised%20vs%20Unsupervised%20Learning)

### Instructions
1. **Receive Note Content**: Accept the raw note content from the user.
2. **Create Structured Note**: Organize the content into a well-structured format using markdown.
3. **Format with Markdown**: Apply appropriate markdown syntax to enhance readability.
4. **Propose Tags**: Analyze the content and suggest relevant tags.
5. **Suggest Associations**: Identify and recommend possible links to other related notes in the user's vault.
Remember to keep the formatting clean and ensure the tags and associations are relevant to the content of the note."}];
    }

    public async Task<string> GetNoteSuggestionsAsync(string note)
    {
        if (string.IsNullOrWhiteSpace(note)) 
            throw new ArgumentException("Note content cannot be null or empty", nameof(note));

        var context = await RetrieveContextAsync(note);

        _conversationHistory.Add(
            new() {
                    Role = "user",
                    Content = $"I need your help to create a well-structured note, format it using Markdown, and propose relevant tags and possible associations with other notes. Here are the details of my note:\n{note}"
                    //content = $"I need your help to create a well-structured note, format it using Markdown, and propose relevant tags and possible associations with other notes. Here are the details of my note based on this context:\n\nContext: {context}\nNote: {note}"
                });

        //si la conversacion es demasiado extensa voy eliminando los primeros mensajes pero la conversacion completa persiste en _conversationHistory
        var conversationHistoryRequest = ChatHistoryService.TruncateHistory(_conversationHistory);

        //ToDo: segun la nota enviada calcular el numero maximo de tokens
        var requestBody = new RequestOpenAI
        {
            messages = conversationHistoryRequest,
            max_tokens = 1000,
            temperature = 0.7M,
            n = 1,
            model = "gpt-3.5-turbo"
        };

        try
        {
            var response = await _openAIclient.GetChatResponseAsync(requestBody);
            var result = JsonSerializer.Deserialize<ResponseOpenAI>(response);

            if (result == null || result.Choices == null || result.Choices.Count == 0)
                throw new InvalidOperationException("Invalid response received from OpenAI API");

            return result.Choices[0].Message.Content;
        }
        catch (Exception ex)
        {
            // Handle or log the exception as needed
            Console.Error.WriteLine($"Error during API call: {ex.Message}");
            throw;
        }
    }

    private async Task<string> RetrieveContextAsync(string note)
    {
        //de todas las notas debo extraer los titulos, los links y los tags, ordenar por los mas frecuentes y los mas nuevos 
        var allNotes = await _markdownService.GetAllNotesAsync();
        var relevantNotes = allNotes.Where(n => note.Contains(n, StringComparison.OrdinalIgnoreCase)).Take(5);
        return string.Join("\n", relevantNotes);
    }

    public async Task<float[]> GetEmbeddingAsync(string text)
    {
        var request = new RequestEmdeddingOpenAI
        {
            text = text,
            model = "text-embedding-ada-002"
        };
        var responseContent = await _openAIclient.GetEmbeddingResponseAsync(request);
        var result = JsonSerializer.Deserialize<ResponseEmdeddingOpenAI>(responseContent);

        return result.Embedding;
    }
}