using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ObsidianAssistant.Core.Services;

namespace ObsidianAssistant.Core.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotesController : ControllerBase
    {
        private readonly OpenAIService _openAIService;
        private readonly MarkdownService _markdownService;

        public NotesController(OpenAIService openAIService, MarkdownService markdownService)
        {
            _openAIService = openAIService;
            _markdownService = markdownService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateNoteAsync([FromBody] NoteRequest request)
        {
            var suggestions = await _openAIService.GetNoteSuggestionsAsync(request.Note);
            await _markdownService.WriteNoteAsync(request.FileName, suggestions);

            return Ok(new { Message = "Note created successfully", Suggestions = suggestions });
        }

        public class NoteRequest
        {
            public string FileName { get; set; }
            public string Note { get; set; }
        }
    }
}
