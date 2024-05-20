// ReSharper disable InconsistentNaming
#pragma warning disable CS8981 // The type name only contains lower-cased ascii characters. Such names may become reserved for the language.
namespace Frank.SymbolicLinkHelper;

internal partial class LinuxSymbolicLinkHelper : ISymbolicLinkHelper
{
    [LibraryImport("libc", SetLastError = true, StringMarshalling = StringMarshalling.Custom, StringMarshallingCustomType = typeof(System.Runtime.InteropServices.Marshalling.AnsiStringMarshaller))]
    private static partial int symlink(string target, string linkpath);

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
}