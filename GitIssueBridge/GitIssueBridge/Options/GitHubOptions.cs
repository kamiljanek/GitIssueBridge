namespace GitIssueBridge.Options;

/// <summary>
/// Options for configuring GitHub-specific settings.
/// </summary>
public class GitHubOptions
{
    /// <summary>
    /// Gets or sets the owner (organization or user) of the GitHub repository.
    /// </summary>
    public required string Owner { get; set; }

    /// <summary>
    /// Gets or sets the name of the GitHub repository.
    /// </summary>
    public required string Repo { get; set; }
    
    /// <summary>
    /// The GitHub access token (Bearer).
    /// </summary>
    public required string AccessToken { get; set; }
}