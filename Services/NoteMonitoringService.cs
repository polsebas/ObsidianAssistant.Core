using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace ObsidianAssistant.Core.Services
{
    public class NoteMonitoringService : BackgroundService
    {
        private readonly ILogger<NoteMonitoringService> _logger;
        private readonly OpenAIService _openAIService;
        private readonly MilvusService _milvusService;
        private readonly FileSystemWatcher _fileSystemWatcher;

        public NoteMonitoringService(ILogger<NoteMonitoringService> logger, IConfiguration configuration, OpenAIService openAIService, MilvusService milvusService)
        {
            _logger = logger;
            _openAIService = openAIService;
            _milvusService = milvusService;

            var notesDirectory = configuration["MarkdownDirectory"];
            _fileSystemWatcher = new FileSystemWatcher(notesDirectory)
            {
                NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName,
                Filter = "*.md"
            };

            _fileSystemWatcher.Changed += OnChanged;
            _fileSystemWatcher.Created += OnChanged;
            _fileSystemWatcher.Deleted += OnDeleted;
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _fileSystemWatcher.EnableRaisingEvents = true;
            return Task.CompletedTask;
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation($"File {e.ChangeType}: {e.FullPath}");
            Task.Run(async () => await UpdateEmbeddingAsync(e.FullPath));
        }

        private void OnDeleted(object sender, FileSystemEventArgs e)
        {
            _logger.LogInformation($"File Deleted: {e.FullPath}");
            Task.Run(async () => await DeleteEmbeddingAsync(e.FullPath));
        }

        private async Task UpdateEmbeddingAsync(string filePath)
        {
            if (File.Exists(filePath))
            {
                var content = await File.ReadAllTextAsync(filePath);
                var embedding = await _openAIService.GetEmbeddingAsync(content);

                await _milvusService.InsertEmbeddingAsync("notes_collection", embedding);
            }
        }

        private async Task DeleteEmbeddingAsync(string filePath)
        {
            // Milvus does not have a direct delete by path, but you can handle logic here if you have a mechanism to track embeddings by path
            // Example: You might store the file path in an additional field in Milvus to identify and delete it
            // Implement your delete logic here if applicable
        }

        public override void Dispose()
        {
            _fileSystemWatcher.Dispose();
            base.Dispose();
        }
    }
}
