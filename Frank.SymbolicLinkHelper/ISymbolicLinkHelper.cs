namespace Frank.SymbolicLinkHelper;

/// <summary>
/// Provides methods to create, retrieve, and search for symbolic links.
/// </summary>
public interface ISymbolicLinkHelper
{
    /// <summary>
    /// Creates a symbolic link for a file.
    /// </summary>
    /// <param name="targetFile">The target file for the symbolic link.</param>
    /// <param name="linkFile">The symbolic link file to be created.</param>
    void CreateSymbolicLink(FileInfo targetFile, FileInfo linkFile);

    /// <summary>
    /// Creates a symbolic link for a directory.
    /// </summary>
    /// <param name="targetDirectory">The target directory for the symbolic link.</param>
    /// <param name="linkDirectory">The symbolic link directory to be created.</param>
    void CreateSymbolicLink(DirectoryInfo targetDirectory, DirectoryInfo linkDirectory);
}