namespace MCSRemoteAccess.Bot.Services;

using System.Diagnostics;
using System.Net.Sockets;
using System.Text;
using MCSRemoteAccess.Bot.Options;
using Microsoft.Extensions.Options;

public class MinecraftServerService
{
    private readonly MinecraftServerOptions _options;
    private Process? _serverProcess;

    public MinecraftServerService(IOptions<MinecraftServerOptions> options)
    {
        _options = options.Value;
    }

    public bool StartServer()
    {
        if (_serverProcess != null && !_serverProcess.HasExited)
        {
            return false;
        }

        var startInfo = new ProcessStartInfo
        {
            FileName = _options.JavaExecutable,
            Arguments = $"{_options.JvmArguments} -jar {_options.JarName} nogui",
            WorkingDirectory = _options.ServerDirectory,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        _serverProcess = new Process { StartInfo = startInfo };
        _serverProcess.Start();

        return true;
    }

    public async Task<string> SendRconCommandAsync(string command)
    {
        try
        {
            using var client = new TcpClient();
            await client.ConnectAsync(_options.RconHost, _options.RconPort);

            using var stream = client.GetStream();

            if (!await RconAuthenticateAsync(stream, _options.RconPassword))
            {
                return "Error: RCON Authentication failed.";
            }

            string response = await RconSendCommandAsync(stream, command);
            return string.IsNullOrWhiteSpace(response) ? "Command executed (No output returned)." : response;
        }
        catch (Exception ex)
        {
            return $"RCON Error: {ex.Message}";
        }
    }

    private async Task<bool> RconAuthenticateAsync(NetworkStream stream, string password)
    {
        byte[] packet = CreateRconPacket(3, password);
        await stream.WriteAsync(packet, 0, packet.Length);

        byte[] responseHeader = new byte[12];
        await stream.ReadAsync(responseHeader, 0, 12);
        int requestId = BitConverter.ToInt32(responseHeader, 4);

        return requestId != -1;
    }

    private async Task<string> RconSendCommandAsync(NetworkStream stream, string command)
    {
        byte[] packet = CreateRconPacket(2, command);
        await stream.WriteAsync(packet, 0, packet.Length);

        byte[] buffer = new byte[4096];
        int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

        if (bytesRead < 12) return string.Empty;

        return Encoding.UTF8.GetString(buffer, 12, bytesRead - 14);
    }

    private byte[] CreateRconPacket(int type, string payload)
    {
        byte[] payloadBytes = Encoding.UTF8.GetBytes(payload);
        int packetLength = payloadBytes.Length + 10;

        byte[] packet = new byte[packetLength + 4];
        BitConverter.GetBytes(packetLength).CopyTo(packet, 0);
        BitConverter.GetBytes(1).CopyTo(packet, 4);
        BitConverter.GetBytes(type).CopyTo(packet, 8);
        payloadBytes.CopyTo(packet, 12);

        packet[packet.Length - 2] = 0;
        packet[packet.Length - 1] = 0;

        return packet;
    }
}