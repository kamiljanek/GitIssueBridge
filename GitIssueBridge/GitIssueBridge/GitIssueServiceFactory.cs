using GitIssueBridge.Options;
using GitIssueBridge.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace GitIssueBridge;

/// <summary>
/// Factory responsible for providing Git issue services based on the Git service type.
/// </summary>
public class GitIssueServiceFactory
{
    private readonly Dictionary<EGitServiceType, Func<IGitIssueService>> _services;

    /// <summary>
    /// Initializes a new instance of the GitIssueServiceFactory class.
    /// </summary>
    /// <param name="httpClientFactory">IHttpClientFactory for creating HttpClient instances.</param>
    /// <param name="serviceProvider">Service provider used to resolve options and other dependencies dynamically.</param>
    public GitIssueServiceFactory(IHttpClientFactory httpClientFactory, IServiceProvider serviceProvider)
    {
        _services = new Dictionary<EGitServiceType, Func<IGitIssueService>>
        {
            { EGitServiceType.GitHub, () => new GitHubIssueService(httpClientFactory.CreateClient(nameof(EGitServiceType.GitHub)), serviceProvider.GetRequiredService<IOptionsSnapshot<GitHubOptions>>()) },
            { EGitServiceType.GitLab, () => new GitLabIssueService(httpClientFactory.CreateClient(nameof(EGitServiceType.GitLab)), serviceProvider.GetRequiredService<IOptionsSnapshot<GitLabOptions>>()) }
        };
    }

    /// <summary>
    /// Creates the Git issue service for the specified Git service type.
    /// </summary>
    /// <param name="type">The type of Git service.</param>
    /// <returns>Concrete IGitIssueService implementation.</returns>
    public IGitIssueService Create(EGitServiceType type)
    {
        if (_services.TryGetValue(type, out var serviceFactory))
        {
            return serviceFactory();
        }

        throw new NotImplementedException($"Service for {type} is not implemented.");
    }
}