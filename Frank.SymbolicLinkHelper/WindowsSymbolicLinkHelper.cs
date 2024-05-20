namespace Frank.SymbolicLinkHelper;

internal partial class WindowsSymbolicLinkHelper : ISymbolicLinkHelper
{
    [LibraryImport("kernel32.dll", EntryPoint = "CreateSymbolicLinkW", SetLastError = true, StringMarshalling = StringMarshalling.Utf16)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static partial bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);

    private const int SYMBOLIC_LINK_FLAG_FILE = 0x0;
    private const int SYMBOLIC_LINK_FLAG_DIRECTORY = 0x1;

    public void CreateSymbolicLink(FileInfo targetFile, FileInfo linkFile)
    {
        PrivilegeChecker.EnsureIsRunningAsAdministrator();
        if (!CreateSymbolicLink(linkFile.FullName, targetFile.FullName, SYMBOLIC_LINK_FLAG_FILE))
        {
            int errorCode = Marshal.GetLastWin32Error();
            throw new InvalidOperationException($"Unable to create symbolic link for file. Error code: {errorCode}. Target: {targetFile.FullName}, Link: {linkFile.FullName}");
        }
    }

    public void CreateSymbolicLink(DirectoryInfo targetDirectory, DirectoryInfo linkDirectory)
    {
        PrivilegeChecker.EnsureIsRunningAsAdministrator();
        if (!CreateSymbolicLink(linkDirectory.FullName, targetDirectory.FullName, SYMBOLIC_LINK_FLAG_DIRECTORY))
        {
            int errorCode = Marshal.GetLastWin32Error();
            throw new InvalidOperationException($"Unable to create symbolic link for directory. Error code: {errorCode}. Target: {targetDirectory.FullName}, Link: {linkDirectory.FullName}");
        }
    }
}
