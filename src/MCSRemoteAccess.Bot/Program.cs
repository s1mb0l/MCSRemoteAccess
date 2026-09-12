using MCSRemoteAccess.Bot.Options;
using MCSRemoteAccess.Bot.Services;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.AddDiscordGateway(options =>
{
    var token = builder.Configuration["Discord:Token"] 
                ?? builder.Configuration["DiscordToken"];

    if (string.IsNullOrWhiteSpace(token) || token == "YOUR_DISCORD_BOT_TOKEN")
    {
        throw new InvalidOperationException("Discord token isn't indicated in appsettings.json");
    }

    options.Token = token;
});

builder.Services.AddApplicationCommands();

builder.Services.Configure<BotSettingsOptions>(builder.Configuration.GetSection("BotSettings"));
builder.Services.Configure<MinecraftServerOptions>(builder.Configuration.GetSection("MinecraftServer"));

builder.Services.AddSingleton<NativeMethodsService>();
builder.Services.AddSingleton<PythonRunnerService>();
builder.Services.AddSingleton<MinecraftServerService>();

builder.Services.AddHostedService<BotInitializationService>();

var host = builder.Build();

host.AddModules(typeof(Program).Assembly);

await host.RunAsync();