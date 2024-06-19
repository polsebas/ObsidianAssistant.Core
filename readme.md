```markdown
# Obsidian Assistant

Obsidian Assistant is an AI-powered virtual assistant application designed to help you organize your notes in Obsidian. It suggests improvements to your notes and automatically generates markdown files. The assistant uses OpenAI for natural language processing and Milvus for vector storage and similarity search.

## Features

- **Note Organization**: Automatically organizes notes into appropriate folders based on their content.
- **Improvement Suggestions**: Provides suggestions to improve your notes using OpenAI.
- **Automatic Embedding**: Generates and stores embeddings of your notes using Milvus.
- **Real-time Monitoring**: Monitors the notes directory for changes and updates embeddings in real-time.

## Project Structure

```
ObsidianAssistant.Core/
│
├── appsettings.Development.json
├── appsettings.json
├── docker-compose.yml
├── ObsidianAssistant.Core.csproj
├── ObsidianAssistant.Core.sln
├── Program.cs
│
├── Controllers/
│   └── NotesController.cs
│
├── Models/
│   ├── EmbeddingsModels.cs
│   ├── MilvusModels.cs
│   ├── NoteRequest.cs
│   ├── RequestOpenAI.cs
│   └── ResponseOpenAI.cs
│
└── Services/
    ├── ChatHistoryService.cs
    ├── MarkdownService.cs
    ├── MilvusService.cs
    ├── NoteMonitoringService.cs
    ├── OpenAIClient.cs
    └── OpenAIService.cs
```
ObsidianAssistant.Core/
│
├── appsettings.Development.json
├── appsettings.json
├── docker-compose.yml
├── ObsidianAssistant.Core.csproj
├── ObsidianAssistant.Core.sln
├── Program.cs
│
├── Controllers/
│   └── NotesController.cs
│
├── Models/
│   ├── EmbeddingsModels.cs
│   ├── MilvusModels.cs
│   ├── NoteRequest.cs
│   ├── RequestOpenAI.cs
│   └── ResponseOpenAI.cs
│
└── Services/
    ├── ChatHistoryService.cs
    ├── MarkdownService.cs
    ├── MilvusService.cs
    ├── NoteMonitoringService.cs
    ├── OpenAIClient.cs
    └── OpenAIService.cs


## Prerequisites

- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker](https://www.docker.com/get-started)
- [Milvus](https://milvus.io/)

## Setup Instructions

### 1. Clone the Repository

```bash
git clone https://github.com/yourusername/ObsidianAssistant.git
cd ObsidianAssistant
```

### 2. Configure Environment Variables

Create and configure the `appsettings.json` and `appsettings.Development.json` files with your OpenAI API key and Milvus settings.

**appsettings.json**

```json
{
  "OpenAI": {
    "ApiKey": "your-openai-api-key"
  },
  "MarkdownDirectory": "notes",
  "Milvus": {
    "BaseUrl": "http://localhost:19530"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=notes.db"
  }
}
```

### 3. Start Milvus Using Docker Compose

Ensure you have Docker installed, then run:

```bash
docker-compose up -d
```

This will start Milvus and its dependencies.

### 4. Build and Run the .NET Core Application

Navigate to the project directory and run:

```bash
dotnet build
dotnet run
```

The application will start and be accessible at `http://localhost:5000`.

## API Endpoints

### Create a Note

**Endpoint**: `POST /api/notes`

**Request Body**:

```json
{
  "FileName": "TestNote",
  "Note": "This is a test note. Please provide suggestions."
}
```

**Response**:

```json
{
  "Message": "Note created successfully",
  "Folder": "determined_folder"
}
```

## Development

### Adding New Services

To add a new service, create a new file in the `Services` directory and register it in `Program.cs`.

### Adding New Models

To add a new model, create a new file in the `Models` directory.

## Contributing

Feel free to submit issues or pull requests. For major changes, please open an issue first to discuss what you would like to change.

## License

This project is licensed under the MIT License. See the `LICENSE` file for details.

---

This `README.md` file includes an overview of the project, setup instructions, API endpoints, and contributing guidelines. Adjust the repository URL and any specific details as needed for your project.
```
