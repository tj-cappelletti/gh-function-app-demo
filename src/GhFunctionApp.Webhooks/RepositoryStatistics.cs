using GhFunctionApp.Webhooks.Factories;
using GhFunctionApp.Webhooks.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Octokit.Webhooks.Events;

namespace GhFunctionApp.Webhooks
{
    public class RepositoryStatistics
    {
        private readonly GitHubClientFactory _gitHubClientFactory;
        private readonly ILogger<RepositoryStatistics> _logger;

        public RepositoryStatistics(GitHubClientFactory gitHubClientFactory, ILogger<RepositoryStatistics> logger)
        {
            _gitHubClientFactory = gitHubClientFactory;
            _logger = logger;
        }

        [Function("RepositoryStatistics")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get")] HttpRequest req)
        {
            var gitHubOrganization = "tjc-actions-demo";

            var gitHubClient = await _gitHubClientFactory.GetClient(gitHubOrganization);

            var repositories = await gitHubClient.Repository.GetAllForOrg(gitHubOrganization);

            var repoStats = new RepoStatsModel
            {
                Organization = gitHubOrganization
            };

            foreach (var repository in repositories)
            {
                repoStats.RepositoryCount++;

                var contributors = await gitHubClient.Repository.Statistics.GetContributors(repository.Id);

                repoStats.ContributorCount += contributors.Count;

                _logger.LogInformation("Repository {RepositoryName} has {ContributorsCount} contributors", repository.FullName, contributors.Count);
            }

            return new OkObjectResult(repoStats);
        }
    }
}
