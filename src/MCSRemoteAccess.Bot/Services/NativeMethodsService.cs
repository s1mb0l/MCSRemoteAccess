namespace MCSRemoteAccess.Bot.Services;

using System.Runtime.InteropServices;

public class NativeMethodsService
{
    private const string LibraryName = "MCSRemoteAccess.Native";

    [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
    private static extern int GetServerProcessStatus(int processId);

    public bool IsProcessAlive(int processId)
    {
        if (!OperatingSystem.IsWindows())
        {
            return false;
        }

        try
        {
            return GetServerProcessStatus(processId) == 1;
        }
        catch (DllNotFoundException)
        {
            return false;
        }
    }
}