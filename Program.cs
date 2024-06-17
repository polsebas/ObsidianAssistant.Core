using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using ObsidianAssistant.Core.Services;
//"UrlChat": "https://api.openai.com/v1/chat/completions"
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "ObsidianAssistant API", Version = "v1" });
});

builder.Services.AddSingleton<OpenAIService>();
builder.Services.AddSingleton(provider =>
    new MarkdownService(builder.Configuration["MarkdownDirectory"]));
builder.Services.AddSingleton<OpenAIClient>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "ObsidianAssistant API v1"));
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
