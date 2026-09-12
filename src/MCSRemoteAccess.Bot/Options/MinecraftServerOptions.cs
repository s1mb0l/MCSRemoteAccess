namespace MCSRemoteAccess.Bot.Options;

public class MinecraftServerOptions
{
    public string ServerDirectory { get; set; } = string.Empty;
    public string JarName { get; set; } = "server.jar";
    public string JavaExecutable { get; set; } = "java";
    public string JvmArguments { get; set; } = "-Xmx2G -Xms1G";
    public string RconHost { get; set; } = "127.0.0.1";
    public int RconPort { get; set; } = 25575;
    public string RconPassword { get; set; } = string.Empty;
}