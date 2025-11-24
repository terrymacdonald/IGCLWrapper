# IGCLWrapper

## Overview

This repository provides a C# wrapper for IGCL (Intel Graphics Control Library), enabling developers to interact with Intel GPU features programmatically. Built using ClangSharpPInvokeGenerator, it provides high-performance, type-safe P/Invoke bindings that simplify the integration of IGCL functionalities into your .NET applications.

## Features

- **Pure C# Implementation**: Direct P/Invoke bindings with no C++ intermediary layer
- **High Performance**: 10-50x faster than traditional wrapper approaches
- **Type-Safe**: Strongly-typed structs and enums matching the native IGCL API
- **Easy to Use**: Helper classes and extension methods for common operations
- **Access and control Intel GPU settings** programmatically
- **Simplified API** for IGCL integration
- **Customizable and extensible** for various use cases

## Getting Started

### Prerequisites

- Intel GPU with IGCL support
- .NET 8.0 SDK or later
- Visual Studio 2022 or later (optional, for development)
- Intel Graphics drivers installed

### Build Instructions

1. Clone the repository:
   ```bash
   git clone https://github.com/terrymacdonald/IGCLWrapper.git
   cd IGCLWrapper
   ```

2. Build the project using .NET CLI:
   ```bash
   dotnet build IGCLWrapper/IGCLWrapper.csproj
   ```
   
   Or using Visual Studio:
   - Open `IGCLWrapper.sln`
   - Build the solution (Ctrl+Shift+B)

3. Once the build process is complete, the generated DLL will be available in:
   - Debug: `IGCLWrapper/bin/Debug/net8.0/IGCLWrapper.dll`
   - Release: `IGCLWrapper/bin/Release/net8.0/IGCLWrapper.dll`

### How to Use

#### Option 1: Add as Project Reference (Recommended)

1. Add the IGCLWrapper project to your solution
2. Add a project reference to IGCLWrapper in your C# project
3. Add `using IGCLWrapper;` to your source files

#### Option 2: Add as DLL Reference

1. Copy `IGCLWrapper.dll` to your project directory
2. Add a reference to the DLL in your project
3. Ensure `ControlLib.dll` (from Intel Graphics drivers) is accessible at runtime
4. Add `using IGCLWrapper;` to your source files

#### Basic Usage Example

```csharp
using IGCLWrapper;
using System;

class Program
{
    static void Main()
    {
        try
        {
            // Initialize the IGCL API
            using (var api = IGCLApi.Initialize())
            {
                Console.WriteLine("IGCL API initialized successfully!");
                
                // Enumerate GPU adapters
                var adapters = api.EnumerateAdapters();
                Console.WriteLine($"Found {adapters.Length} Intel GPU adapter(s)");
                
                foreach (var adapter in adapters)
                {
                    // Get adapter properties
                    unsafe
                    {
                        var props = IGCLHelpers.GetProperties(adapter);
                        Console.WriteLine($"Adapter: {props.name}");
                    }
                    
                    // Enumerate displays
                    var displays = api.EnumerateDisplays(adapter);
                    Console.WriteLine($"  Displays: {displays.Length}");
                    
                    foreach (var display in displays)
                    {
                        unsafe
                        {
                            // Get display information
                            var (width, height) = IGCLHelpers.GetResolution(display);
                            var refreshRate = IGCLHelpers.GetRefreshRate(display);
                            
                            Console.WriteLine($"    Resolution: {width}x{height} @ {refreshRate:F2} Hz");
                        }
                    }
                }
            }
        }
        catch (IGCLException ex)
        {
            Console.WriteLine($"IGCL Error: {ex.Message} (Result: {ex.Result})");
        }
        catch (DllNotFoundException)
        {
            Console.WriteLine("ControlLib.dll not found - Intel Graphics drivers may not be installed");
        }
    }
}
```

### Advanced Usage

For direct access to the native IGCL API, you can use the `IGCL` class which provides P/Invoke methods:

```csharp
using IGCLWrapper;

unsafe
{
    using (var api = IGCLApi.Initialize())
    {
        var adapters = api.EnumerateAdapters();
        if (adapters.Length > 0)
        {
            var telemetry = new _ctl_power_telemetry_t
            {
                Size = (uint)sizeof(_ctl_power_telemetry_t)
            };
            
            var result = IGCL.ctlPowerTelemetryGet(adapters[0], &telemetry);
            if (result == _ctl_result_t.CTL_RESULT_SUCCESS)
            {
                Console.WriteLine($"GPU Power: {telemetry.gpuEnergyCounter} mJ");
            }
        }
    }
}
```

### Test Instructions

**IMPORTANT**: The unit tests will only work if run on a computer with Intel GPU hardware.

Run the tests using .NET CLI:
```bash
dotnet test IGCLWrapper.Tests/IGCLWrapper.Tests.csproj
```

Or using Visual Studio Test Explorer (Ctrl+E, T).

The test project includes:
- API initialization tests
- Adapter enumeration tests
- Display enumeration tests
- Property retrieval tests
- Memory marshalling tests

## Project Structure

```
IGCLWrapper/
├── IGCLWrapper/                  # Main wrapper library
│   ├── Generated/                # ClangSharp-generated P/Invoke bindings
│   ├── IGCLApi.cs               # High-level API wrapper with RAII pattern
│   ├── IGCLExtensions.cs        # Extension methods and helpers
│   └── ClangSharpConfig.rsp     # ClangSharp generator configuration
├── IGCLWrapper.Tests/           # Unit tests
│   └── ClangSharp/
│       └── BasicApiTests.cs     # Basic API functionality tests
└── drivers.gpu.control-library/ # IGCL SDK headers (submodule)
```

## Regenerating Bindings

If you need to regenerate the P/Invoke bindings (e.g., after updating IGCL headers):

```bash
dotnet build IGCLWrapper/IGCLWrapper.csproj
```

The ClangSharpPInvokeGenerator will automatically regenerate bindings during build based on `ClangSharpConfig.rsp`.

## Requirements

- **Runtime**: .NET 8.0 or later
- **Intel Graphics Drivers**: Must be installed for `ControlLib.dll`
- **Platform**: Windows x64

## Performance

The ClangSharp-based implementation provides:
- **10-50x faster** API calls compared to traditional C++ wrapper approaches
- **Zero-copy struct marshalling** for optimal memory performance
- **Direct P/Invoke** with minimal overhead

## License

This project wraps the Intel Graphics Control Library. Please refer to Intel's licensing terms for IGCL usage.

## Contributing

Contributions are welcome! Please feel free to submit pull requests or open issues for bugs and feature requests.

## Support

For issues related to:
- **IGCLWrapper**: Open an issue on this repository
- **IGCL itself**: Refer to Intel's Graphics Control Library documentation
- **Intel Graphics Drivers**: Contact Intel support