using GitIssueBridge.Options;

namespace GitIssueBridge;

public class GitIssueManagerOptions
{
    public GitLabOptions GitLabConfig { get; private set; } = null!;
    public GitHubOptions GitHubConfig { get; private set; } = null!;

    public void AddGitHubConfig(GitHubOptions options)
    {
        GitHubConfig = options;
    }
    
    public void AddGitLabConfig(GitLabOptions options)
    {
        GitLabConfig = options;
    }
}