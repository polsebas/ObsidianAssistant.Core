using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ObsidianAssistant.Core.Services;
using ObsidianAssistant.Core.Models;

namespace ObsidianAssistant.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly OpenAIService _openAIService;
        private readonly MarkdownService _markdownService;
        private readonly MilvusService _milvusService;

        public NotesController(OpenAIService openAIService, MarkdownService markdownService, MilvusService milvusService)
        {
            _openAIService = openAIService;
            _markdownService = markdownService;
            _milvusService = milvusService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNoteAsync([FromBody] NoteRequest request)
        {
            var embedding = await _openAIService.GetEmbeddingAsync(request.Note);

            var folder = await DetermineFolder(embedding);
            var filePath = $"{folder}/{request.FileName}.md";

            await _markdownService.WriteNoteAsync(filePath, request.Note);
            await _milvusService.InsertEmbeddingAsync("notes_collection", embedding);

            return Ok(new { Message = "Note created successfully", Folder = folder });
        
        }

        private async Task<string> DetermineFolder(float[] embedding)
        {
            var results = await _milvusService.SearchEmbeddingAsync("notes_collection", embedding, 1);
            // Logic to determine folder based on search results
            // For simplicity, return the folder of the closest note
            // In a real scenario, you might need more complex logic
            return results.Count > 0 ? "found_folder" : "default";
        }
    }
}
