namespace AutoDocMcpServer
{
    public class Variable
    {
        public Variable()
        {
            Rules = string.Empty;
            Culture = string.Empty;
            OwnerName = string.Empty;
            OwnerEmail = string.Empty;
            OutputPath = string.Empty;
            RepositoryPath = string.Empty;
            ProjectMetadata = string.Empty;
            ReportColumnsTitles = string.Empty;
        }

        public string RepositoryPath { get; set; }

        public string ProjectName { get { return Path.GetFileNameWithoutExtension(RepositoryPath); } }

        public string OwnerEmail { get; set; }

        public string OwnerName { get; set; }

        public string Culture { get; set; }
        
        public string OutputPath { get; set; }

        public string ReportColumnsTitles { get; set; }

        public string Rules { get; set; }

        public string ProjectMetadata { get; set; }
    }
}
