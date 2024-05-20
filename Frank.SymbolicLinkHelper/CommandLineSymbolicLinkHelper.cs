using CliWrap;
using Microsoft.Extensions.Logging;

namespace Frank.SymbolicLinkHelper;

public class CommandLineSymbolicLinkHelper(ILogger<CommandLineSymbolicLinkHelper> logger) : ISymbolicLinkHelper
{
    /// <inheritdoc />
    public void CreateSymbolicLink(FileInfo targetFile, FileInfo linkFile)
    {
        RunCreateSymbolicFileLinkCommand(targetFile.FullName, linkFile.FullName);
    }

    /// <inheritdoc />
    public void CreateSymbolicLink(DirectoryInfo targetDirectory, DirectoryInfo linkDirectory)
    {
        RunCreateSymbolicDirectoryLinkCommand(targetDirectory.FullName, linkDirectory.FullName);
    }
    
    private void RunCreateSymbolicFileLinkCommand(string target, string link)
    {
        var output = new StringBuilder();
        var createSymbolicFileLinkCommand = Cli.Wrap("cmd")
            .WithArguments($"/c mklink {link} {target}")
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(output))
            .WithStandardErrorPipe(PipeTarget.ToStringBuilder(output))
            .WithValidation(CommandResultValidation.None)
            ;
        
        var result = createSymbolicFileLinkCommand.ExecuteAsync().GetAwaiter().GetResult();
        
        if (result.ExitCode != 0)
        {
            logger.LogError("Unable to create symbolic link. Exit code: {ResultExitCode}. Output: {Output}", result.ExitCode, output);
        }
        
        logger.LogInformation("Created symbolic link '{Link}' for file '{Target}'", link, target);
    }
    
    private void RunCreateSymbolicDirectoryLinkCommand(string target, string link)
    {
        var output = new StringBuilder();
        var createSymbolicDirectoryLinkCommand = Cli.Wrap("cmd")
            .WithArguments($"/c mklink /d {link} {target}")
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(output))
            .WithStandardErrorPipe(PipeTarget.ToStringBuilder(output))
            .WithValidation(CommandResultValidation.None)
            ;
        
        var result = createSymbolicDirectoryLinkCommand.ExecuteAsync().GetAwaiter().GetResult();
        
        if (result.ExitCode != 0)
        {
            logger.LogError("Unable to create symbolic link. Exit code: {ResultExitCode}. Output: {Output}", result.ExitCode, output);
        }
        
        logger.LogInformation("Created symbolic link '{Link}' for directory '{Target}'", link, target);
    }
}