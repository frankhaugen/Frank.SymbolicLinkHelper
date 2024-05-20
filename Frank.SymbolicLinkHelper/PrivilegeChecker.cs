namespace Frank.SymbolicLinkHelper;

using System.Security.Principal;

public static class PrivilegeChecker
{
    public static bool IsRunningAsAdministrator()
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            using WindowsIdentity identity = WindowsIdentity.GetCurrent();
            WindowsPrincipal principal = new(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux) || RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            return Environment.GetEnvironmentVariable("USER") == "root";
        
        throw new PlatformNotSupportedException();
    }

    public static void EnsureIsRunningAsAdministrator()
    {
        if (!IsRunningAsAdministrator())
            throw new InvalidOperationException("This operation requires elevated privileges.");
    }
}
