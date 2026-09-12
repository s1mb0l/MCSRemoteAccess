#include <iostream>

#ifdef _WIN32
    #include <windows.h>
    #define EXPORT __declspec(dllexport)
#else
    #include <unistd.h>
    #include <signal.h>
    #define EXPORT __attribute__((visibility("default")))
#endif

extern "C" {
    EXPORT int GetServerProcessStatus(int processId) {
#ifdef _WIN32
        HANDLE hProcess = OpenProcess(PROCESS_QUERY_INFORMATION, FALSE, processId);
        if (hProcess == NULL) return 0;
        
        DWORD exitCode;
        if (GetExitCodeProcess(hProcess, &exitCode)) {
            CloseHandle(hProcess);
            return (exitCode == STILL_ACTIVE) ? 1 : 0;
        }

        CloseHandle(hProcess);
        return 0;
#else
        if (kill(processId, 0) == 0) return 1;
        return 0;
#endif
    }
}