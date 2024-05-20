namespace Frank.SymbolicLinkHelper;

public class SymbolicLinkHelper : ISymbolicLinkHelper
{
    /// <inheritdoc />
    public void CreateSymbolicLink(FileInfo targetFile, FileInfo linkFile)
    {
        linkFile.CreateAsSymbolicLink(targetFile.FullName);
    }

    /// <inheritdoc />
    public void CreateSymbolicLink(DirectoryInfo targetDirectory, DirectoryInfo linkDirectory)
    {
        linkDirectory.CreateAsSymbolicLink(targetDirectory.FullName);
    }
}