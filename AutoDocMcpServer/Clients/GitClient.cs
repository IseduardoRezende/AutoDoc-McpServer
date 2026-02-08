using AutoDocMcpServer.Models;
using LibGit2Sharp;

namespace AutoDocMcpServer.Clients
{
    public static class GitClient
    {
        public static IEnumerable<MyCommit> GetCommits(
            DateTime start,
            DateTime end,
            Variable variable)
        {
            if (start > end || variable is null)
                return [];

            using var repo = new Repository(variable.RepositoryPath);

            List<MyCommit> commits = [];

            foreach (var commit in repo.Commits
                .Where(c => c.Author.Email == variable.OwnerEmail &&
                            c.Author.When.DateTime >= start &&
                            c.Author.When.DateTime <= end)
                .OrderBy(c => c.Author.When.DateTime))
            {
                commits.Add(new MyCommit
                {
                    Message = commit.Message,
                    Tittle = commit.MessageShort,
                    CreatedAt = commit.Author.When.DateTime
                });
            }

            return commits;
        }
    }
}
