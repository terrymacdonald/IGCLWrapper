# IGCLWrapper

A modern C# wrapper for Intel Graphics Control Library (IGCL), providing easy access to Intel GPU features and settings.

## Features

- IntPtr-based API surface; no custom handle types to manage
- Automatic cleanup via `IDisposable` (SafeHandle-backed)
- Strongly typed structs/enums matching IGCL headers
- Helper methods for common adapter/display queries
- ClangSharp-generated bindings kept in sync with the SDK
- Tests skip gracefully when hardware is absent

## Quick Start

### Prerequisites
- Intel GPU with IGCL support
- Windows 10/11 x64
- .NET 10.0 SDK
- Intel Graphics drivers (25.20.100.6618+)

### Build the wrapper
```powershell
git clone https://github.com/terrymacdonald/IGCLWrapper.git
cd IGCLWrapper

./prepare_igcl.ps1   # pulls the IGCL SDK
./build_igcl.ps1     # restores, regenerates bindings, builds, tests
```

### Basic usage
```csharp
using System;
using System.Runtime.InteropServices;
using System.Text;
using IGCLWrapper;

using var igcl = IGCLApi.Initialize();
var adapters = igcl.EnumerateAdapters();
Console.WriteLine($"Found {adapters.Length} Intel GPU(s)");

foreach (var adapter in adapters)
{
    var props = IGCLHelpers.GetProperties(adapter);

    ReadOnlySpan<sbyte> nameSpan = MemoryMarshal.CreateReadOnlySpan(ref props.name.e0, 100);
    int term = nameSpan.IndexOf((sbyte)0);
    if (term >= 0) nameSpan = nameSpan[..term];
    var name = Encoding.UTF8.GetString(MemoryMarshal.Cast<sbyte, byte>(nameSpan));

    Console.WriteLine($"\nGPU: {name}");
    Console.WriteLine($"Device ID: 0x{props.pci_device_id:X}");

    var displays = igcl.EnumerateDisplays(adapter);
    Console.WriteLine($"Connected Displays: {displays.Length}");

    foreach (var display in displays)
    {
        if (IGCLHelpers.IsActive(display))
        {
            var (width, height) = IGCLHelpers.GetResolution(display);
            var refresh = IGCLHelpers.GetRefreshRate(display);
            Console.WriteLine($"  {width}x{height} @ {refresh:F2} Hz");
        }
    }
}
```

### Error handling
```csharp
try
{
    using var igcl = IGCLApi.Initialize();
    // use the API
}
catch (IGCLException ex)
{
    Console.WriteLine($"IGCL Error: {ex.Result} - {ex.Message}");
}
catch (DllNotFoundException)
{
    Console.WriteLine("IGCL DLL not found. Install Intel Graphics drivers.");
}
```

## Testing
Tests require Intel GPU hardware and the IGCL DLLs present. They skip gracefully if not available.
```powershell
./test_igcl.ps1
# or
dotnet test IGCLWrapper.Tests/IGCLWrapper.Tests.csproj
```

## Updating bindings
When Intel releases a new IGCL:
```powershell
./prepare_igcl.ps1   # update SDK bits
./build_igcl.ps1     # regenerates bindings via ClangSharp and rebuilds
```

## Project structure
- `IGCLWrapper/` – main wrapper
  - `cs_generated/` – ClangSharp output (auto-generated)
  - `IGCLApi.cs` – high-level API
  - `IGCLExtensions.cs` – helpers for common ops
- `IGCLWrapper.Tests/` – test suite
- `Samples/` – sample apps
- `drivers.gpu.control-library/` – IGCL SDK payload (populated by prepare script)

## Usage notes
- Always dispose `IGCLApi` (use `using`); SafeHandle + finalizer backstops leaks.
- Handles returned from enumerate calls are opaque; pass them back to IGCL or helper methods.
- Struct `Version` fields are bytes; use `(byte)0/1` as shown in helpers.

## Contributing
PRs welcome—please add/keep tests passing and let the generator own `cs_generated`.
