## MCSRemoteAccess (Minecraft Server Remote Access)
A portfolio project: a remote Minecraft server management tool powered by a .NET bot with RCON integration and automated game process lifecycle control. The utility allows launching the server on the required Java version, monitoring its status, and securely executing commands.

Requirements
* Java 21 (required for modern Minecraft versions like Paper 1.21).

* .NET 10.0 SDK (if building from source code).

## Installation & Setup
1. Clone the Repository
Download or clone the project using Git:
```bash
git clone https://github.com/your_username/MCSRemoteAccess.git
cd MCSRemoteAccess
```
2. Configuration
Create an `appsettings.json` file in the root directory based on the provided template `appsettings.example.json`.
```json
{
  "MinecraftServer": {
    "ServerDirectory": "/path/to/server/folder",
    "JarName": "server.jar",
    "JavaExecutable": "/path/to/jdk-21/bin/java",
    "JvmArguments": "-Xmx2G -Xms1G",
    "RconHost": "127.0.0.1",
    "RconPort": 25575,
    "RconPassword": "your_secure_password"
  }
}
```
  Important:
  * Specify the absolute path to the *Java 21* `java` executable in `JavaExecutable`.
  * Set `ServerDirectory` to the folder containing your `server.jar`.

3. Server RCON Configuration
To allow the bot to send commands, ensure RCON is enabled in your Minecraft server's `server.properties` file:
```properties
enable-rcon=true
rcon.port=25575
rcon.password=your_secure_password
```
*(The password and port must completely match those specified in the bot's configuration)*.

## Download / Releases
Ready-to-use builds are available in the Releases section.
Each release includes a `.zip` archive containing:

* The compiled self-contained executable (`.exe`).

* The configuration template (`appsettings.example.json`).

## Building & Running
**Publishing a Self-Contained .exe for Windows**
You can compile the application into a single executable that does not require .NET installed on the target machine:
```bash
dotnet publish -c Release -r win-x64 --self-contained true -p:PublishSingleFile=true
```
The resulting file will be located at: `bin/Release/net10.0/win-x64/publish/`.

**Running from Source**
To run the project in development mode, execute:
```bash
dotnet run
```

## How the Project Works (Under the Hood)
The utility operates as a bridge between administrative commands and the operating system's process management:
  * **Process Control**: The .NET backend uses the `System.Diagnostics.Process` class to spawn the Minecraft server as a managed child process. It passes custom JVM flags (such as memory allocation `-Xmx` and   `-Xms`), working directories, and exact paths to the required Java 21 runtime.
  * **Environment Isolation**: By isolating execution parameters in the configuration file, the application prevents version conflicts (such as accidentally running modern server cores on incompatible Java releases).
  * **RCON Protocol Integration**: Once the server finishes booting up and opens its local RCON socket, the bot establishes an authenticated TCP connection. It translates administrative actions into standard Minecraft console commands, securely handles responses, and closes the socket stream.

## Future Roadmap & Architecture Note
You might notice a C++ native module and a placeholder for Python scripts in the repository. While the current version of MCSRemoteAccess runs fully on .NET 10.0, these components are intentionally included as a foundation for future scalability:
  * **C++ Module**: Designed for low-level OS-process monitoring and native system interactions via P/Invoke.
  * **Python Integration**: Planned for auxiliary tasks such as real-time Minecraft log parsing, error analysis, and metric processing.
