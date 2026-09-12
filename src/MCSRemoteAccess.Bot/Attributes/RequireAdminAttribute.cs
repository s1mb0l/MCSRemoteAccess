namespace MCSRemoteAccess.Bot.Attributes;

using MCSRemoteAccess.Bot.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using NetCord.Services;
using NetCord.Services.ApplicationCommands;

public class RequireAdminAttribute : PreconditionAttribute<ApplicationCommandContext>
{
    public override ValueTask<PreconditionResult> EnsureCanExecuteAsync(
        ApplicationCommandContext context, 
        IServiceProvider? services)
    {
        if (services == null)
        {
            return new ValueTask<PreconditionResult>(PreconditionResult.Fail("Service provider unavailable."));
        }

        var options = services.GetRequiredService<IOptions<BotSettingsOptions>>().Value;

        if (options.AdminIds.Contains(context.User.Id))
        {
            return new ValueTask<PreconditionResult>((PreconditionResult)PreconditionResult.Success);
        }

        return new ValueTask<PreconditionResult>(PreconditionResult.Fail("Access denied: You are not authorized to use this command."));
    }
}