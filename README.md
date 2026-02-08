# AutoDoc MCP Server — AutoDoc

AutoDoc is an MCP: https://modelcontextprotocol.io/docs/getting-started/intro server that runs as a stdio service and automatically analyzes Git commits to generate structured, daily CSV reports. It’s designed to produce concise activity reports from repository history, suitable for progress tracking, status reports, and automated documentation workflows.

**Core idea**: convert Git commit history into human-readable, business-oriented CSV reports by applying configurable rules and prompts. AutoDoc watches a repository location, extracts relevant commits, summarizes custom columns and saves reports to disk.

**Key features**
- Generates any date interval of CSV reports from Git commit history.
- Runs as an MCP stdio server (suitable for integrations in editors and automation pipelines).
- Configurable via environment variables (repository path, owner, culture, output path, rules, column titles).
- Designed for custom datas.

Getting started
----------------
1. Build and run using dotnet (example uses the project path):

```bash
dotnet run --project AutoDocMcpServer/AutoDocMcpServer.csproj
```

2. Or run the MCP server via an MCP client configuration (.mcp.json):

```json
"inputs": [],
"servers": {
	"AutoDocMCPServer": {
		"type": "stdio",
		"command": "dotnet",
		"args": [
			"run",
			"--project",
			"C:/path/to/AutoDocMcpServer/AutoDocMcpServer.csproj"
		],
		"env": {
			"REPOSITORY_PATH": "C:\\path\\to\\repo",
			"OWNER_EMAIL": "owner@example.com",
			"OWNER_NAME": "Project Owner",
			"CULTURE": "en-US",
			"OUTPUT_PATH": "C:\\Reports",
			"REPORT_COLUMNS_TITLES": "Period,Phase/Stage,Activity (Title),Activity Description,Motivation,Process,Outcome,Participants",
			"RULES": "Analyze only relevant commits. Ignore merges and noisy commits. Use phases: Development, Tests, Infrastructure, Integration. Summarize titles using the configured culture. Use OWNER_NAME for Participants. Be brief and objective.",
			"PROJECT_METADATA": "Short project summary to include in report context."
		}
	}
}
```

Environment variables explained
- `REPOSITORY_PATH`: Full path to the local Git repository to analyze.
- `OWNER_EMAIL` / `OWNER_NAME`: Contact and name used for Participants column.
- `CULTURE`: Language/culture used to format summaries (e.g., `en-US`, `pt-BR`).
- `OUTPUT_PATH`: Directory where CSV reports will be written.
- `REPORT_COLUMNS_TITLES`: Comma-separated header used in generated CSV files.
- `RULES`: Natural-language rules that steer the summarization and filtering of commits.
- `PROJECT_METADATA`: Short description of the project for context in reports.

Usage example (prompt + expected output)
---------------------------------------

Prompt (example sent to the MCP service):

"Generate the report for my commits this week and save it."

Example result printed by AutoDoc:

```
Generating report: weekly (2026-02-02 – 2026-02-08)
Processed 18 commits (filtered 5 noisy commits)
Generated and saved: report_project-api_20260208_repeat.csv to C:\Users\eduar\AutoDoc-NiceAcesso-Reports\
```

Sample CSV row (columns follow `REPORT_COLUMNS_TITLES`):

"2026-02-04","Development","Add JWT auth","Implemented JWT authentication for API endpoints","To secure API access","Updated middleware and token validation, added tests","Auth now requires JWT tokens","Eduardo Rezende"

Best practices
--------------
- Improve commits titles and descriptions for good context
- Keep the local repository up to date and run AutoDoc against a clean working tree for accurate results.
- Tune `RULES` to match your team's commit style and what you consider "relevant".
- Use `REPORT_COLUMNS_TITLES` to adapt output to your internal report templates.

Contributing
------------
Contributions welcome — open issues and pull requests. Keep changes focused: add configuration options, improve commit filtering, or enhance summarization rules.

License
-------
Distributed under the terms in the repository `LICENSE.txt`.

For details, see the MCP configuration example in the project and `AutoDocMcpServer/README.md` for service-specific notes.
