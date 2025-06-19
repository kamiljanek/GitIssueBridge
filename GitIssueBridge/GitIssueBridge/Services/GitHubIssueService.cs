using System.Text;
using System.Text.Json;
using GitIssueBridge.Options;
using GitIssueBridge.Responses;
using Microsoft.Extensions.Options;

namespace GitIssueBridge.Services;

/// <summary>
/// Implementation of IGitIssueService for GitHub.
/// </summary>
public class GitHubIssueService : IGitIssueService
{
    private readonly HttpClient _httpClient;
    private readonly GitHubOptions _options;

    /// <summary>
    /// Initializes a new instance of the GitHubIssueService class.
    /// </summary>
    /// <param name="httpClient">Configured HttpClient instance.</param>
    /// <param name="optionsSnapshot">Options snapshot for GitHub configuration.</param>
    public GitHubIssueService(HttpClient httpClient, IOptionsSnapshot<GitHubOptions> optionsSnapshot)
    {
        _httpClient = httpClient;
        _options = optionsSnapshot.Get(nameof(EGitServiceType.GitHub));
    }

    public async Task<string> AddIssue(string name, string description)
    {
        var content = new
        {
            title = name,
            body = description
        };
        var response = await _httpClient.PostAsync($"/repos/{_options.Owner}/{_options.Repo}/issues",
            new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        
        var jsonString = await response.Content.ReadAsStringAsync();
        var issue = JsonSerializer.Deserialize<GitHubAddIssueResponse>(jsonString);
        if (issue == null)
        {
            throw new Exception("Deserialization failed: issue is null");
        }
        
        return issue.Url;
    }

    public async Task UpdateIssue(int issueNumber, string name, string description)
    {
        var content = new
        {
            title = name,
            body = description
        };
        var response = await _httpClient.PatchAsync($"/repos/{_options.Owner}/{_options.Repo}/issues/{issueNumber}",
            new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
    }

    public async Task CloseIssue(string issueId)
    {
        var content = new
        {
            state = "closed"
        };
        var response = await _httpClient.PatchAsync($"/repos/{_options.Owner}/{_options.Repo}/issues/{issueId}",
            new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
    }
}