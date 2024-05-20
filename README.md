# Frank.SymbolicLinkHelper

[![GitHub License](https://img.shields.io/github/license/frankhaugen/Frank.SymbolicLinkHelper)](LICENSE)
[![NuGet](https://img.shields.io/nuget/v/Frank.SymbolicLinkHelper.svg)](https://www.nuget.org/packages/Frank.SymbolicLinkHelper)
[![NuGet](https://img.shields.io/nuget/dt/Frank.SymbolicLinkHelper.svg)](https://www.nuget.org/packages/Frank.SymbolicLinkHelper)

This is a helper library for creating symbolic links in .NET. It is a wrapper around the Windows API function 
`CreateSymbolicLink` or the Linux/Unix equivalent `symlink` function. It uses dependency injection to allow for swapping the 
implementation of the helper implementation. This allows for platform specific implementations to be used in a 
cross-platform and user-friendly way.

This does not do any error handling, it is up to the caller to handle any exceptions that may be thrown such as 
access/permission errors due to the file system protections or user permissions.

## Installation

This library is available on NuGet. You can install it using the following command:

```bash
dotnet add package Frank.SymbolicLinkHelper
```

## Usage

To use this library, you need to add the following code to your `Startup.cs` file:

```csharp
services.AddSymbolicLinkHelper();
```

Then you can inject the `ISymbolicLinkHelper` interface into your classes and use it to create symbolic links:

```csharp
public class MyService
{
    private readonly ISymbolicLinkHelper _symbolicLinkHelper;

    public MyService(ISymbolicLinkHelper symbolicLinkHelper)
    {
        _symbolicLinkHelper = symbolicLinkHelper;
    }

    public void CreateSymbolicLink(string source, string target)
    {
        _symbolicLinkHelper.CreateSymbolicLink(new File/DirectoryInfo(source), new File/DirectoryInfo(target));
    }
}
```

## License

This library is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.

## Contributing

Contributions are welcome! Please read the [CONTRIBUTING.md](CONTRIBUTING.md) file for more information.

## Code of Conduct

Please read the [CODE_OF_CONDUCT.md](CODE_OF_CONDUCT.md) file for more information.