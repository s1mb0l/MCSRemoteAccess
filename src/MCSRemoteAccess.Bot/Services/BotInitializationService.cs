namespace MCSRemoteAccess.Bot.Services;

using Microsoft.Extensions.Hosting;

public class BotInitializationService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;
    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}