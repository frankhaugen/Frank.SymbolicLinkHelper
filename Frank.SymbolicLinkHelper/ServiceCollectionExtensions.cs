namespace Frank.SymbolicLinkHelper;

/// <summary>
/// Provides extension methods for IServiceCollection to add symbolic link helpers.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the appropriate implementation of ISymbolicLinkHelper based on the operating system.
    /// </summary>
    /// <param name="services">The IServiceCollection to add the service to.</param>
    /// <returns>The updated IServiceCollection.</returns>
    public static IServiceCollection AddSymbolicLinkHelper(this IServiceCollection services)
    {
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            services.AddSingleton<ISymbolicLinkHelper, SymbolicLinkHelper>();
        }
        else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
        {
            services.AddSingleton<ISymbolicLinkHelper, LinuxSymbolicLinkHelper>();
        }
        else
        {
            throw new PlatformNotSupportedException("The current operating system is not supported.");
        }

        return services;
    }
}