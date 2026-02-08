using AutoDocMcpServer;
using AutoDocMcpServer.Tools;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

// Configure all logs to go to stderr (stdout is used for the MCP protocol messages).
builder.Logging.AddConsole(o => o.LogToStandardErrorThreshold = LogLevel.Trace);

builder.Configuration.AddEnvironmentVariables();
builder.Services.AddScoped<AutoDocTool>();

builder.Services.Configure<Variable>(options =>
{
    options.Culture = builder.Configuration["CULTURE"] ?? "en-US";
    options.Rules = builder.Configuration["RULES"] ?? string.Empty;
    options.OwnerEmail = builder.Configuration["OWNER_EMAIL"] ?? string.Empty;
    options.OwnerName = builder.Configuration["OWNER_NAME"] ?? string.Empty;
    options.OutputPath = builder.Configuration["OUTPUT_PATH"] ?? string.Empty;
    options.RepositoryPath = builder.Configuration["REPOSITORY_PATH"] ?? string.Empty;
    options.ProjectMetadata = builder.Configuration["PROJECT_METADATA"] ?? string.Empty;
    options.ReportColumnsTitles = builder.Configuration["REPORT_COLUMNS_TITLES"] ?? string.Empty;
});

// Add the MCP services: the transport to use (stdio) and the tools to register.
builder.Services
    .AddMcpServer()
    .WithStdioServerTransport()
    .WithTools<AutoDocTool>();

await builder.Build().RunAsync();
