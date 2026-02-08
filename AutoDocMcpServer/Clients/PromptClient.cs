using AutoDocMcpServer.Models;
using System.Text;

namespace AutoDocMcpServer.Clients
{
    public class PromptClient
    {
        public static string Send(IEnumerable<MyCommit> commits, Variable variable)
        {
            if (commits is null || !commits.Any() || variable is null)
                return "No datas found.";

            var builder = new StringBuilder();

            // 1. Mission & Role
            builder.AppendLine("# MISSION");
            builder.AppendLine("You are a Senior Software Engineer and Technical Writer. Your task is to analyze Git commits and transform them into a high-quality, professional daily CSV report.");
            builder.AppendLine();

            // 2. Project Context
            builder.AppendLine("# PROJECT CONTEXT");
            builder.AppendLine($"- Project Name: {variable.ProjectName}");
            builder.AppendLine($"- Project Metadata: {variable.ProjectMetadata}");
            builder.AppendLine($"- Technical Owner: {variable.OwnerName} ({variable.OwnerEmail})");
            builder.AppendLine($"- Target Culture/Locale: {variable.Culture}");
            builder.AppendLine();

            // 3. Output Specifications (Ajustado para remover aspas)
            builder.AppendLine("# OUTPUT REQUIREMENTS");
            builder.AppendLine($"- CSV Column Titles: {variable.ReportColumnsTitles}");
            builder.AppendLine("- CSV Formatting: Do NOT wrap values in double quotes (\") unless the value itself contains a comma. Keep it clean and readable.");
            builder.AppendLine("- Language: The entire report must be written in the specified Target Culture.");
            builder.AppendLine("- Format: Ensure the output is a valid CSV code block.");
            builder.AppendLine();

            // 4. Custom Rules & Constraints
            builder.AppendLine("# OPERATIONAL RULES");
            builder.AppendLine(variable.Rules);
            builder.AppendLine("- Use the Project Metadata to give context-aware summaries. Explain WHY, not just WHAT.");
            builder.AppendLine();

            // 5. The Data
            builder.AppendLine("# RAW COMMIT DATA");
            if (!commits.Any())
            {
                builder.AppendLine("No commits found for the specified period.");
            }
            else
            {
                foreach (var c in commits)
                {
                    builder.AppendLine($"[Timestamp: {c.CreatedAt}] [Title: {c.Tittle}] [Message: {c.Message}]");
                }
            }

            builder.AppendLine("\nBased on the rules and data above, please generate the CSV report now.");

            // 6. The Next Steps
            builder.AppendLine("\n### NEXT STEP INSTRUCTION ###");
            builder.AppendLine("1. Show this clean CSV to the user in a code block.");
            builder.AppendLine($"2. Use the 'save_report' tool to save this RAW content as 'report_{variable.ProjectName}_{DateTime.Now:yyyyMMdd}_{Guid.CreateVersion7().ToString("N")}.csv' in the pre-configured directory.");

            return builder.ToString();
        }
    }
}