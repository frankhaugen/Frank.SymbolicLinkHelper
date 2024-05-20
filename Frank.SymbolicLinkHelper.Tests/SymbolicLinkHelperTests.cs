using Microsoft.Extensions.DependencyInjection;

namespace Frank.SymbolicLinkHelper.Tests;

public class SymbolicLinkHelperTests
{
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
        // Arrange
        var serviceProvider = CreateServiceProvider();
        var symbolicLinkHelper = serviceProvider.GetRequiredService<ISymbolicLinkHelper>();
        var currentDirectory = GetCurrentDirectory();
        var tempDirectory = GetTempDirectory();
        
        // Act
        symbolicLinkHelper.CreateSymbolicLink(tempDirectory, currentDirectory);
        
        // Assert
        Assert.True(tempDirectory.Exists);
        Assert.True(currentDirectory.Exists);
        Assert.NotEmpty(currentDirectory.GetDirectories());
        
        // Cleanup
        tempDirectory.Delete(recursive: true);
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
    
    private static ServiceProvider CreateServiceProvider()
    {
        var services = new ServiceCollection();
        services.AddSymbolicLinkHelper();
        return services.BuildServiceProvider(new ServiceProviderOptions() { ValidateOnBuild = true, ValidateScopes = true });
    }
}