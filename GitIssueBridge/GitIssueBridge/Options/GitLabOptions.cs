namespace GitIssueBridge.Options;

/// <summary>
/// Options for configuring GitLab-specific settings.
/// </summary>
public class GitLabOptions
{
    /// <summary>
    /// Gets or sets the GitLab project identifier.
    /// </summary>
    public required string ProjectId { get; set; }
    
    /// <summary>
    /// The GitLab access token (Bearer).
    /// </summary>
    public required string AccessToken { get; set; }
}