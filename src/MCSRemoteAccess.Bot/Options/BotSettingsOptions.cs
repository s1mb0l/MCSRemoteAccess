namespace MCSRemoteAccess.Bot.Options;

public class BotSettingsOptions
{
    public List<ulong> AdminIds { get; set; } = new();
    public List<ulong> GuildIds { get; set; } = new();
}