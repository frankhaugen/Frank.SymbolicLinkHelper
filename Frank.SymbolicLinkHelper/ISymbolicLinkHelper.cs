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

    /// <summary>
    /// Retrieves the target of a symbolic link file.
    /// </summary>
    /// <param name="linkFile">The symbolic link file.</param>
    /// <returns>The target file of the symbolic link.</returns>
    FileInfo GetSymbolicLinkTarget(FileInfo linkFile);

    /// <summary>
    /// Retrieves the target of a symbolic link directory.
    /// </summary>
    /// <param name="linkDirectory">The symbolic link directory.</param>
    /// <returns>The target directory of the symbolic link.</returns>
    DirectoryInfo GetSymbolicLinkTarget(DirectoryInfo linkDirectory);

    /// <summary>
    /// Searches for symbolic link files within a specified directory.
    /// </summary>
    /// <param name="directory">The directory to search within.</param>
    /// <returns>A collection of symbolic link files found within the specified directory.</returns>
    IEnumerable<FileInfo> SearchSymbolicLinkFiles(DirectoryInfo directory);

    /// <summary>
    /// Searches for symbolic link directories within a specified directory.
    /// </summary>
    /// <param name="directory">The directory to search within.</param>
    /// <returns>A collection of symbolic link directories found within the specified directory.</returns>
    IEnumerable<DirectoryInfo> SearchSymbolicLinkDirectories(DirectoryInfo directory);
}