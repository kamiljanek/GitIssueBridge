using GitIssueBridge.Options;
using Microsoft.Extensions.DependencyInjection;

namespace GitIssueBridge;

/// <summary>
/// Extension methods to register Git Issue Manager services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds Git issue services to the IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection instance.</param>
    /// <param name="configure">The action used to configure Git issue manager options.</param>
    /// <returns>The IServiceCollection instance.</returns>
    public static IServiceCollection AddGitIssueManager(this IServiceCollection services, Action<GitIssueManagerOptions> configure)
    {
        var options = new GitIssueManagerOptions();
        configure(options);

        AddGitHub(services, options);
        AddGitLab(services, options);

        services.AddSingleton<GitIssueServiceFactory>();

        return services;
    }
    
    private static void AddGitHub(IServiceCollection services, GitIssueManagerOptions options)
    {
        services.Configure<GitHubOptions>(opts =>
        {
            opts.Owner = options.GitHubConfig.Owner;
            opts.Repo = options.GitHubConfig.Repo;
            opts.AccessToken = options.GitHubConfig.AccessToken;
        });

        services.AddHttpClient(nameof(EGitServiceType.GitHub),
            client =>
            {
                client.BaseAddress = new Uri("https://api.github.com");
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", options.GitHubConfig.AccessToken);
            });
    }
    
    private static void AddGitLab(IServiceCollection services, GitIssueManagerOptions options)
    {
        services.Configure<GitLabOptions>(opts =>
        {
            opts.ProjectId = options.GitLabConfig.ProjectId;
            opts.AccessToken = options.GitLabConfig.AccessToken;
        });

        services.AddHttpClient(nameof(EGitServiceType.GitLab),
            client =>
            {
                client.BaseAddress = new Uri("https://gitlab.com/api/v4");
                client.DefaultRequestHeaders.Add("PRIVATE-TOKEN", options.GitLabConfig.AccessToken);
            });
    }
}