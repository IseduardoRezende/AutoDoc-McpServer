using AutoDocMcpServer.Clients;
using Microsoft.Extensions.Options;
using ModelContextProtocol.Server;
using System.ComponentModel;

namespace AutoDocMcpServer.Tools
{
    public class AutoDocTool(IOptions<Variable> options)
    {
        private readonly Variable _variable = options.Value;

        [McpServerTool]
        [Description("CRITICAL: Use this tool to generate reports. " +
            "It is FULLY PRE-CONFIGURED with REPOSITORY_PATH, OWNER_EMAIL, and business rules. " +
            "Do NOT ask the user for these details. Just provide the date range. " +
            "PRIMARY TOOL for generating documentation. Use this whenever the user asks for 'report', 'commits', 'analysis' or 'daily summary'. " +
            "This tool handles all internal configurations. Just calculate the date range and call it.")]
        public string GetCommits(
           [Description("Start date (yyyy-MM-dd)")] DateTime startDate,
           [Description("End date (yyyy-MM-dd)")] DateTime endDate)
        {
            var commits = GitClient.GetCommits(startDate, endDate, _variable);
            return PromptClient.Send(commits, _variable);
        }

        [McpServerTool]
        [Description("Saves the report automatically to the pre-defined OUTPUT_PATH. " +
            "Call this immediately after GetCommits without asking the user for paths. " +
            "FINALIZE tool. Use this immediately after GetCommits if the user wants to 'save', 'export', 'store' or 'keep' the report. " +
            "It persists the CSV data to the local file system.")]
        public string SaveReport(
            [Description("The RAW CSV content to save")] string csvContent,
            [Description("The desired filename")] string fileName)
        {
            try
            {
                if (!Directory.Exists(_variable.OutputPath))
                    Directory.CreateDirectory(_variable.OutputPath);

                var fullPath = Path.Combine(_variable.OutputPath, fileName);
                File.WriteAllText(fullPath, csvContent, System.Text.Encoding.UTF8);

                return $"Success: Report saved to {fullPath}";
            }
            catch (Exception ex)
            {
                return $"Error saving file: {ex.Message}";
            }
        }

        [McpServerTool]
        [Description("Check if the AutoDoc server is ready and has all environment variables loaded.")]
        public string CheckServerStatus()
        {
            if (_variable is null)
                return "Could not run AutoDoc MCP, check server status.";

            return $"Server Ready. Repository: {_variable.ProjectName}, Owner: {_variable.OwnerName}, Culture: {_variable.Culture}. " +
                   "I have everything I need. Just ask me for a report!";
        }
    }
}
