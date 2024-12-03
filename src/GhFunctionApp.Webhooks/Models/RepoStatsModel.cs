namespace GhFunctionApp.Webhooks.Models;

public class RepoStatsModel
{
    public int ContributorCount { get; set; }

    public string? Organization { get; set; }

    public int RepositoryCount { get; set; }
}
