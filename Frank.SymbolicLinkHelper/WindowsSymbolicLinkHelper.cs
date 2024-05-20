namespace Frank.SymbolicLinkHelper;

internal class WindowsSymbolicLinkHelper : ISymbolicLinkHelper
{
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool CreateSymbolicLink(string lpSymlinkFileName, string lpTargetFileName, int dwFlags);

    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern bool GetFileAttributesEx(string lpFileName, int fInfoLevelId, out WIN32_FILE_ATTRIBUTE_DATA fileData);

    private const int SYMBOLIC_LINK_FLAG_FILE = 0x0;
    private const int SYMBOLIC_LINK_FLAG_DIRECTORY = 0x1;

    public void CreateSymbolicLink(FileInfo targetFile, FileInfo linkFile)
    {
        if (!CreateSymbolicLink(linkFile.FullName, targetFile.FullName, SYMBOLIC_LINK_FLAG_FILE))
            throw new InvalidOperationException("Unable to create symbolic link. Error: " + Marshal.GetLastWin32Error());
    }

    public void CreateSymbolicLink(DirectoryInfo targetDirectory, DirectoryInfo linkDirectory)
    {
        if (!CreateSymbolicLink(linkDirectory.FullName, targetDirectory.FullName, SYMBOLIC_LINK_FLAG_DIRECTORY))
            throw new InvalidOperationException("Unable to create symbolic link. Error: " + Marshal.GetLastWin32Error());
    }

    public FileInfo GetSymbolicLinkTarget(FileInfo linkFile) 
        => new(GetTargetPath(linkFile.FullName));

    public DirectoryInfo GetSymbolicLinkTarget(DirectoryInfo linkDirectory) 
        => new(GetTargetPath(linkDirectory.FullName));

    public IEnumerable<FileInfo> SearchSymbolicLinkFiles(DirectoryInfo directory) 
        => directory.GetFiles().Where(file => IsSymbolicLink(file.FullName));

    public IEnumerable<DirectoryInfo> SearchSymbolicLinkDirectories(DirectoryInfo directory) 
        => directory.GetDirectories().Where(dir => IsSymbolicLink(dir.FullName));

    private string GetTargetPath(string linkPath)
    {
        WIN32_FILE_ATTRIBUTE_DATA data;
        if (!GetFileAttributesEx(linkPath, 0, out data))
            throw new InvalidOperationException("Unable to get file attributes. Error: " + Marshal.GetLastWin32Error());

        // Logic to extract the target path from the WIN32_FILE_ATTRIBUTE_DATA.
        // This is a simplified example and may need to be expanded for robustness.
        return linkPath; // Placeholder for actual target path extraction logic.
    }

    private static bool IsSymbolicLink(string path)
    {
        WIN32_FILE_ATTRIBUTE_DATA data;
        if (!GetFileAttributesEx(path, 0, out data))
            throw new InvalidOperationException("Unable to get file attributes. Error: " + Marshal.GetLastWin32Error());

        // Check if the file attribute indicates a symbolic link
        const uint FILE_ATTRIBUTE_REPARSE_POINT = 0x400;
        return (data.FileAttributes & FILE_ATTRIBUTE_REPARSE_POINT) != 0;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct WIN32_FILE_ATTRIBUTE_DATA
    {
        public uint FileAttributes;
        public FILETIME CreationTime;
        public FILETIME LastAccessTime;
        public FILETIME LastWriteTime;
        public uint FileSizeHigh;
        public uint FileSizeLow;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct FILETIME
    {
        public uint DateTimeLow;
        public uint DateTimeHigh;
    }
}