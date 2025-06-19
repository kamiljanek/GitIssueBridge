namespace GitIssueBridge.Services;

/// <summary>
/// Interface defining basic issue operations for any Git service provider.
/// </summary>
public interface IGitIssueService
{
    /// <summary>
    /// Creates a new issue in the repository.
    /// </summary>
    /// <param name="name">Issue name.</param>
    /// <param name="description">Issue description.</param>
    /// <returns>Url to new issue.</returns>
    Task<string> AddIssue(string name, string description);

    /// <summary>
    /// Updates an existing issue in the repository.
    /// </summary>
    /// <param name="issueNumber">ID of the issue to update.</param>
    /// <param name="name">Updated name.</param>
    /// <param name="description">Updated description.</param>
    Task UpdateIssue(int issueNumber, string name, string description);

    /// <summary>
    /// Closes an existing issue in the repository.
    /// </summary>
    /// <param name="issueId">ID of the issue to close.</param>
    Task CloseIssue(string issueId);
}