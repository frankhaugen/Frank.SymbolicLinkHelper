// ReSharper disable InconsistentNaming
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
namespace Frank.SymbolicLinkHelper;

internal class LinuxSymbolicLinkHelper : ISymbolicLinkHelper
{
    [DllImport("libc", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern int symlink(string target, string linkpath);

    [DllImport("libc", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern int readlink(string pathname, StringBuilder buf, int bufsiz);

    [DllImport("libc", CharSet = CharSet.Ansi, SetLastError = true)]
    private static extern int lstat(string path, out stat statBuf);

    public void CreateSymbolicLink(FileInfo targetFile, FileInfo linkFile)
    {
        if (symlink(targetFile.FullName, linkFile.FullName) != 0)
            throw new InvalidOperationException("Unable to create symbolic link. Error: " + Marshal.GetLastWin32Error());
    }

    public void CreateSymbolicLink(DirectoryInfo targetDirectory, DirectoryInfo linkDirectory)
    {
        if (symlink(targetDirectory.FullName, linkDirectory.FullName) != 0)
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

    private static string GetTargetPath(string linkPath)
    {
        const int bufferSize = 1024;
        var buffer = new StringBuilder(bufferSize);
        var result = readlink(linkPath, buffer, bufferSize);
        if (result == -1)
        {
            throw new InvalidOperationException("Unable to read symbolic link. Error: " + Marshal.GetLastWin32Error());
        }

        return buffer.ToString();
    }

    private static bool IsSymbolicLink(string path)
    {
        stat statBuf;
        if (lstat(path, out statBuf) != 0)
            throw new InvalidOperationException("Unable to get file attributes. Error: " + Marshal.GetLastWin32Error());

        // Check if the file mode indicates a symbolic link
        const int S_IFLNK = 0xA000;
        return (statBuf.st_mode & S_IFLNK) == S_IFLNK;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct stat
    {
        public ulong st_dev;
        public ulong st_ino;
        public uint st_mode;
        public uint st_nlink;
        public uint st_uid;
        public uint st_gid;
        public ulong st_rdev;
        public long st_size;
        public long st_blksize;
        public long st_blocks;
        public long st_atime;
        public long st_mtime;
        public long st_ctime;
    }
}