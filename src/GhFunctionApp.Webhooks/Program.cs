using GhFunctionApp.Webhooks;
using GhFunctionApp.Webhooks.Factories;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Octokit.Webhooks;
using Octokit.Webhooks.AzureFunctions;

var host = new HostBuilder()
    .ConfigureAppConfiguration((context, config) =>
    {
        config.AddJsonFile("local.settings.json", optional: false, reloadOnChange: true)
            .Build();
    })
    .ConfigureFunctionsWebApplication()
    .ConfigureServices(services =>
    {
        services.AddApplicationInsightsTelemetryWorkerService();
        services.ConfigureFunctionsApplicationInsights();

        services.AddSingleton<GitHubClientFactory, GitHubClientFactory>();
        services.AddSingleton<WebhookEventProcessor, PushWebhookEventProcessor>();
    })
    .ConfigureGitHubWebhooks()
    .Build();

host.Run();
