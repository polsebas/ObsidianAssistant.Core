using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ObsidianAssistant.Core.Services
{
    public class MarkdownService
    {
        private readonly string _directory;

        public MarkdownService(string directory)
        {
            _directory = directory;

            if (!Directory.Exists(_directory))
            {
                Directory.CreateDirectory(_directory);
            }
        }

        public async Task WriteNoteAsync(string fileName, string content)
        {
            var filePath = Path.Combine(_directory, $"{fileName}.md");
            await File.WriteAllTextAsync(filePath, content);
        }

        public async Task<string> ReadNoteAsync(string fileName)
        {
            var filePath = Path.Combine(_directory, $"{fileName}.md");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified note does not exist.");
            }

            return await File.ReadAllTextAsync(filePath);
        }

        public async Task UpdateNoteAsync(string fileName, string content)
        {
            var filePath = Path.Combine(_directory, $"{fileName}.md");

            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("The specified note does not exist.");
            }

            await File.WriteAllTextAsync(filePath, content);
        }

        public async Task<IEnumerable<string>> GetAllNotesAsync()
        {
            var noteFiles = Directory.GetFiles(_directory, "*.md");
            var notes = new List<string>();

            foreach (var file in noteFiles)
            {
                notes.Add(await File.ReadAllTextAsync(file));
            }

            return notes;
        }
    }
}
