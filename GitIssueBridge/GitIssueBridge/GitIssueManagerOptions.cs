using GitIssueBridge.Options;

namespace GitIssueBridge;

public class GitIssueManagerOptions
{
    public GitLabOptions? GitLabConfig { get; private set; }
    public GitHubOptions? GitHubConfig { get; private set; }

    public void AddGitHubConfig(GitHubOptions options)
    {
        GitHubConfig = options;
    }
    
    public void AddGitLabConfig(GitLabOptions options)
    {
        GitLabConfig = options;
    }
}