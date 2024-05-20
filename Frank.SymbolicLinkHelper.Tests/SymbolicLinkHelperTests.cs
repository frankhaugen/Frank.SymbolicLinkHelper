using Frank.Testing.Logging;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;

namespace Frank.SymbolicLinkHelper.Tests;

public class SymbolicLinkHelperTests
{
    private readonly ITestOutputHelper _outputHelper;

    public SymbolicLinkHelperTests(ITestOutputHelper outputHelper)
    {
        _outputHelper = outputHelper;
    }

    [Fact]
    public void GetSymbolicLinkHelperFromServiceProvider()
    {
        var serviceProvider = CreateServiceProvider();
        var symbolicLinkHelper = serviceProvider.GetService<ISymbolicLinkHelper>();
        Assert.NotNull(symbolicLinkHelper);
    }

    [Fact]
    public void CreateSymbolicLinkToDirectory()
    {
        var serviceProvider = CreateServiceProvider();
        var symbolicLinkHelper = serviceProvider.GetRequiredService<ISymbolicLinkHelper>();
        var currentDirectory = GetCurrentDirectory();
        var symbolsDirectory = new DirectoryInfo(Path.Combine(currentDirectory.FullName, Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));
        symbolsDirectory.Create();
        var tempDirectory = GetTempDirectory();
        var linkDirectory = new DirectoryInfo(Path.Combine(symbolsDirectory.FullName, Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));

        try
        {
            symbolicLinkHelper.CreateSymbolicLink(tempDirectory, linkDirectory);
            _outputHelper.WriteLine($"Symbolic link target: {tempDirectory.FullName} -> {linkDirectory.FullName}");

            Assert.True(tempDirectory.Exists, "Temp directory does not exist.");
            Assert.True(symbolsDirectory.Exists, "Symbols directory does not exist.");
            Assert.NotEmpty(symbolsDirectory.GetDirectories());
            
            _outputHelper.WriteLine("Directories in symbols directory:");
            foreach (var directory in symbolsDirectory.GetDirectories())
            {
                _outputHelper.WriteLine($"Name: {directory.Name}, CreationTime: {directory.CreationTime}, Exists: {directory.Exists}, FullName: {directory.FullName}, SymbolicLink: {directory.LinkTarget}");
            }
        }
        catch (Exception ex)
        {
            _outputHelper.WriteLine($"Exception: {ex}");
            throw;
        }
        finally
        {
            symbolsDirectory.Delete(recursive: true);
            tempDirectory.Delete(recursive: true);
        }
    }

    [Fact]
    public void CreateSymbolicLinkToFile()
    {
        var serviceProvider = CreateServiceProvider();
        var symbolicLinkHelper = serviceProvider.GetRequiredService<ISymbolicLinkHelper>();
        var currentDirectory = GetCurrentDirectory();
        var symbolsDirectory = new DirectoryInfo(Path.Combine(currentDirectory.FullName, Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));
        symbolsDirectory.Create();
        var tempFile = GetTempFile();
        var linkFile = new FileInfo(Path.Combine(symbolsDirectory.FullName, Path.GetRandomFileName()));

        try
        {
            symbolicLinkHelper.CreateSymbolicLink(tempFile, linkFile);
            _outputHelper.WriteLine($"Symbolic link target: {tempFile.FullName} -> {linkFile.FullName}");

            Assert.True(tempFile.Exists, "Temp file does not exist.");
            Assert.True(symbolsDirectory.Exists, "Symbols directory does not exist.");
            Assert.NotEmpty(symbolsDirectory.GetFiles());

            _outputHelper.WriteLine("Files in symbols directory:");
            foreach (var file in symbolsDirectory.GetFiles())
            {
                _outputHelper.WriteLine($"Name: {file.Name}, Length: {file.Length}, CreationTime: {file.CreationTime}, Exists: {file.Exists}, FullName: {file.FullName}, SymbolicLink: {file.LinkTarget}");
            }
        }
        catch (Exception ex)
        {
            _outputHelper.WriteLine($"Exception: {ex}");
            throw;
        }
        finally
        {
            tempFile.Delete();
            symbolsDirectory.Delete(recursive: true);
        }
    }

    private static DirectoryInfo GetTempDirectory()
    {
        var tempDirectory = new DirectoryInfo(Path.Combine(Path.GetTempPath(), Path.GetFileNameWithoutExtension(Path.GetRandomFileName())));
        tempDirectory.Create();
        return tempDirectory;
    }

    private static FileInfo GetTempFile()
    {
        var tempFile = new FileInfo(Path.Combine(GetTempDirectory().FullName, Path.GetRandomFileName()));
        tempFile.Create().Dispose();
        return tempFile;
    }

    private static DirectoryInfo GetCurrentDirectory() => new DirectoryInfo(AppContext.BaseDirectory);

    private ServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSymbolicLinkHelper();
        services.AddTestLogging(_outputHelper);
        return services.BuildServiceProvider(new ServiceProviderOptions() { ValidateOnBuild = true, ValidateScopes = true });
    }
}
