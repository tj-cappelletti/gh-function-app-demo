using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Octokit.Webhooks;
using Octokit.Webhooks.Events;

namespace GhFunctionApp.Webhooks;

public class PushWebhookEventProcessor : WebhookEventProcessor
{
    private readonly ILogger<PushWebhookEventProcessor> _logger;

    public PushWebhookEventProcessor(ILogger<PushWebhookEventProcessor> logger)
    {
        _logger = logger;
    }

    protected override Task ProcessPushWebhookAsync(WebhookHeaders headers, PushEvent pushEvent)
    {
        _logger.LogInformation("Processing push event for {RepositoryName} with {CommitsCount} commits", pushEvent.Repository?.FullName, pushEvent.Commits.Count());

        return Task.CompletedTask;
    }
}
