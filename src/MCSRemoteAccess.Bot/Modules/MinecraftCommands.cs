namespace MCSRemoteAccess.Bot.Modules;

using MCSRemoteAccess.Bot.Attributes;
using MCSRemoteAccess.Bot.Services;
using NetCord.Rest;
using NetCord.Services.ApplicationCommands;

public class MinecraftCommands : ApplicationCommandModule<ApplicationCommandContext>
{
    private readonly MinecraftServerService _mcService;

    public MinecraftCommands(MinecraftServerService mcService)
    {
        _mcService = mcService;
    }

    [SlashCommand("startserver", "Launches the Minecraft server process")]
    [RequireAdmin]
    public async Task StartServerAsync()
    {
        bool started = _mcService.StartServer();

        if (started)
        {
            await RespondAsync(InteractionCallback.Message("Minecraft server process has been started!"));
        }
        else
        {
            await RespondAsync(InteractionCallback.Message("Server is already running or configuration path is invalid."));
        }
    }

    [SlashCommand("cmd", "Sends a console command to the Minecraft server via RCON")]
    [RequireAdmin]
    public async Task ExecuteConsoleCommandAsync(
        [SlashCommandParameter(Name = "command", Description = "Console command to run (e.g. list, op <player>)")] 
        string command)
    {
        // Отправка отложенного ответа
        await RespondAsync(InteractionCallback.DeferredMessage());

        string response = await _mcService.SendRconCommandAsync(command);

        await FollowupAsync($"```text\n{response}\n```");
    }
}