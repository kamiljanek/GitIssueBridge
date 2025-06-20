using System.Text;
using System.Text.Json;
using GitIssueBridge.Options;
using GitIssueBridge.Responses;
using Microsoft.Extensions.Options;

namespace GitIssueBridge.Services;

/// <summary>
/// Implementation of IGitIssueService for GitLab.
/// </summary>
public class GitLabIssueService : IGitIssueService
{
    private readonly HttpClient _httpClient;
    private readonly GitLabOptions _options;

    /// <summary>
    /// Initializes a new instance of the GitHubIssueService class.
    /// </summary>
    /// <param name="httpClient">Configured HttpClient instance.</param>
    /// <param name="optionsSnapshot">Options snapshot for GitHub configuration.</param>
    public GitLabIssueService(HttpClient httpClient, IOptionsMonitor<GitLabOptions> optionsSnapshot)
    {
        _httpClient = httpClient;
        _options = optionsSnapshot.Get(nameof(EGitServiceType.GitHub));
    }

    public async Task<string> AddIssue(string name, string description)
    {
        var content = new
        {
            title = name,
            description
        };
        var response = await _httpClient.PostAsync($"/projects/{_options.ProjectId}/issues",
            new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
        
        var jsonString = await response.Content.ReadAsStringAsync();
        var issue = JsonSerializer.Deserialize<GitLabAddIssueResponse>(jsonString);
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
            description
        };
        var response = await _httpClient.PutAsync($"/projects/{_options.ProjectId}/issues/{issueNumber}",
            new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
    }

    public async Task CloseIssue(int issueNumber)
    {
        var content = new
        {
            state_event = "close"
        };
        var response = await _httpClient.PutAsync($"/projects/{_options.ProjectId}/issues/{issueNumber}",
            new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json"));

        response.EnsureSuccessStatusCode();
    }
}