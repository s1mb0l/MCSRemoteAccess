## MCSRemoteAccess (Minecraft Server Remote Access)
A portfolio project: a remote Minecraft server management tool powered by a .NET bot with RCON integration and automated game process lifecycle control. The utility allows launching the server on the required Java version, monitoring its status, and securely executing commands.

Requirements
* Java 21 (required for modern Minecraft versions like Paper 1.21).

* .NET 8.0 SDK (if building from source code).

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
