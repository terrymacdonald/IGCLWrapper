# IGCLWrapper

A modern C# wrapper for Intel Graphics Control Library (IGCL), providing easy access to Intel GPU features and settings.

## Features

- IntPtr-based API surface; no custom handle types to manage
- Automatic cleanup via `IDisposable` (SafeHandle-backed)
- Strongly typed structs/enums matching IGCL headers
- Helper methods for common adapter/display queries
- Facade DTOs for bool-friendly helper results
- ClangSharp-generated bindings kept in sync with the SDK
- Tests skip gracefully when hardware is absent
- Split test suites:
  - `IGCLWrapper.NativeTests` exercises only ClangSharp-generated APIs.
  - `IGCLWrapper.FacadeTests` exercises the new facade helpers.

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

### Install / consume the wrapper
- Local build artifacts: after `./build_igcl.ps1`, reference `IGCLWrapper\bin\Debug\net10.0\IGCLWrapper.dll` (or `Release` if you build release) from your project.
- Add as a project reference: add `IGCLWrapper/IGCLWrapper.csproj` to your solution and reference it.
- Requirements at runtime: Windows x64, Intel GPU/driver, and IGCL DLLs available (the wrapper dynamically loads `ControlLib` from the installed Intel drivers).

### Basic usage (facade)
```csharp
using IGCLWrapper;

using var api = IGCLApiHelper.Initialize();
var adapters = api.EnumerateAdapters();
Console.WriteLine($"Found {adapters.Count} Intel GPU(s)");

foreach (var adapter in adapters)
{
    var props = adapter.GetProperties();
    Console.WriteLine($"\nGPU: {adapter.Name}");
    Console.WriteLine($"Device ID: 0x{props.pci_device_id:X}");

    foreach (var display in adapter.EnumerateDisplayOutputs())
    {
        if (!display.IsActive()) continue;
        var (width, height) = display.GetResolution();
        var refresh = display.GetRefreshRateHz();
        Console.WriteLine($"  {width}x{height} @ {refresh:F2} Hz");
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

## Working with the facade helpers (IGCLApiHelper)
Use the facade helpers to avoid manual struct sizing/handle management.
DTO-returning helpers use `bool` properties instead of native `byte` fields.
Get/Set operations are split into separate `Get*()` and `Set*()` helpers. For advanced use cases requiring raw IGCL structs (e.g. pixel transformation with pointer fields), use the `IGCLApi` native layer directly.

### Nullable returns and unsupported features
Facade getter methods return `T?` (nullable struct) instead of throwing when the hardware or driver does not support a feature. Setter methods return `bool` — `true` on success, `false` when the feature is not supported. Only genuinely unexpected errors throw `IGCLException`.

The four IGCL result codes that map to `null`/`false` rather than an exception are:
- `CTL_RESULT_ERROR_UNSUPPORTED_FEATURE`
- `CTL_RESULT_ERROR_UNSUPPORTED_VERSION`
- `CTL_RESULT_ERROR_INVALID_OPERATION_TYPE`
- `CTL_RESULT_ERROR_INVALID_ARGUMENT`

Example:
```csharp
using var api = IGCLApiHelper.Initialize();
var tempHelper = api.GetTemperatureHelper(adapter);
var sensor = tempHelper.EnumTemperatureSensors().FirstOrDefault();
if (sensor != IntPtr.Zero)
{
    var props = tempHelper.TemperatureGetProperties(sensor);
    if (props.HasValue)
        Console.WriteLine($"Max temperature: {props.Value.MaxTemperature:F1} C");
    else
        Console.WriteLine("Temperature properties not supported on this hardware.");

    var tempC = tempHelper.TemperatureGetState(sensor);
    if (tempC.HasValue)
        Console.WriteLine($"Current: {tempC.Value:F1} C");
}
```

### List active display resolutions
```csharp
using IGCLWrapper;

using var api = IGCLApiHelper.Initialize();
foreach (var adapter in api.EnumerateAdapters())
{
    foreach (var display in adapter.EnumerateDisplayOutputs())
    {
        if (!display.IsActive()) continue;
        var (w, h) = display.GetResolution();
        var hz = display.GetRefreshRateHz();
        Console.WriteLine($"{adapter.Name}: {w}x{h} @ {hz:F2} Hz");
    }
}
```

### List only combined displays
```csharp
using IGCLWrapper;

using var api = IGCLApiHelper.Initialize();
foreach (var adapter in api.EnumerateAdapters())
{
    var result = adapter.GetCombinedDisplay();
    if (result.HasValue && result.Value.IsSupported && result.Value.NumOutputs > 0)
    {
        Console.WriteLine($"Adapter {adapter.Name} has a combined display with {result.Value.NumOutputs} outputs.");
    }
}
```

### Query combined display layout
```csharp
using IGCLWrapper;
using System.Linq;

using var api = IGCLApiHelper.Initialize();
foreach (var adapter in api.EnumerateAdapters())
{
    var combined = adapter.GetCombinedDisplay();
    if (!combined.HasValue || combined.Value.NumOutputs == 0 || combined.Value.ChildInfos == null)
    {
        Console.WriteLine($"Adapter {adapter.Name} has no combined display configured.");
        continue;
    }

    Console.WriteLine($"Combined display: {combined.Value.CombinedDesktopWidth}x{combined.Value.CombinedDesktopHeight}");
    for (var i = 0; i < combined.Value.NumOutputs; i++)
    {
        var child = combined.Value.ChildInfos[i];
        Console.WriteLine(
            $"  Output {i}: handle={child.DisplayOutput}, " +
            $"FbSrc={child.FbSrc.Left},{child.FbSrc.Top},{child.FbSrc.Right},{child.FbSrc.Bottom}, " +
            $"FbPos={child.FbPos.Left},{child.FbPos.Top},{child.FbPos.Right},{child.FbPos.Bottom}, " +
            $"Orientation={child.DisplayOrientation}, " +
            $"Target={child.TargetMode.Width}x{child.TargetMode.Height}@{child.TargetMode.RefreshRate}");
    }
}
```

### Get current temperature
```csharp
using IGCLWrapper;

using var api = IGCLApiHelper.Initialize();
foreach (var adapter in api.EnumerateAdapters())
{
    var tempHelper = api.GetTemperatureHelper(adapter);
    var sensor = tempHelper.EnumTemperatureSensors().FirstOrDefault();
    if (sensor != IntPtr.Zero)
    {
        var tempC = tempHelper.TemperatureGetState(sensor);
        if (tempC.HasValue)
            Console.WriteLine($"{adapter.Name}: {tempC.Value:F1} C");
    }
}
```

### Wait for a property change on an adapter
```csharp
using IGCLWrapper;

using var api = IGCLApiHelper.Initialize();
var adapter = api.EnumerateAdapters().First();
var args = new ctl_wait_property_change_args_t
{
    Size = (uint)sizeof(ctl_wait_property_change_args_t),
    Version = 0,
    PropertyType = ctl_property_type_flags_t.CTL_PROPERTY_TYPE_FLAG_DISPLAY,
    TimeOutMilliSec = 5_000 // 5 seconds
};
adapter.WaitForPropertyChange(args);
Console.WriteLine("Property change observed or timeout reached.");
```

## Doing the same with the native API
If you prefer direct P/Invoke access, use `IGCLApi` and the generated structs/functions.

### List active display resolutions (native)
```csharp
using IGCLWrapper;

using var igcl = IGCLApi.Initialize();
foreach (var adapter in igcl.EnumerateAdapters())
{
    var displays = igcl.EnumerateDisplays(adapter);
    foreach (var display in displays)
    {
        var props = new ctl_display_properties_t { Size = (uint)sizeof(ctl_display_properties_t), Version = 0 };
        if (IGCL.ctlGetDisplayProperties((_ctl_display_output_handle_t*)display, &props) != ctl_result_t.CTL_RESULT_SUCCESS)
            continue;
        var timing = props.Display_Timing_Info;
        if (timing.HActive == 0 || timing.VActive == 0) continue;
        Console.WriteLine($"{timing.HActive}x{timing.VActive} @ {timing.RefreshRate / 1000.0:F2} Hz");
    }
}
```

### List only combined displays (native)
```csharp
using IGCLWrapper;

using var igcl = IGCLApi.Initialize();
foreach (var adapter in igcl.EnumerateAdapters())
{
    var args = new ctl_combined_display_args_t
    {
        Size = (uint)sizeof(ctl_combined_display_args_t),
        Version = 0,
        OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_QUERY_CONFIG
    };
    var result = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)adapter, &args);
    if (result == ctl_result_t.CTL_RESULT_SUCCESS && args.IsSupported && args.NumOutputs > 0)
    {
        Console.WriteLine("Combined display present.");
    }
}
```

### Get current temperature (native)
```csharp
using IGCLWrapper;

using var igcl = IGCLApi.Initialize();
foreach (var adapter in igcl.EnumerateAdapters())
{
    uint count = 0;
    if (IGCL.ctlEnumTemperatureSensors((_ctl_device_adapter_handle_t*)adapter, &count, null) != ctl_result_t.CTL_RESULT_SUCCESS || count == 0)
        continue;

    var sensors = new IntPtr[count];
    unsafe
    {
        fixed (IntPtr* pSensors = sensors)
        {
            if (IGCL.ctlEnumTemperatureSensors((_ctl_device_adapter_handle_t*)adapter, &count, (_ctl_temp_handle_t**)pSensors) != ctl_result_t.CTL_RESULT_SUCCESS)
                continue;
        }

        double temp = 0;
        if (IGCL.ctlTemperatureGetState((_ctl_temp_handle_t*)sensors[0], &temp) == ctl_result_t.CTL_RESULT_SUCCESS)
        {
            Console.WriteLine($"{temp:F1} C");
        }
    }
}
```

## Testing
Tests require Intel GPU hardware and the IGCL DLLs present. They skip gracefully if not available.
```powershell
./test_igcl.ps1
# or individually:
dotnet test IGCLWrapper.NativeTests/IGCLWrapper.NativeTests.csproj
dotnet test IGCLWrapper.FacadeTests/IGCLWrapper.FacadeTests.csproj
```

`./test_igcl.ps1` runs both suites in sequence (native first, then facade) and reports a combined result.

### Passive tests (read-only)
The majority of tests are **passive** — they read state from the GPU but never modify it. These run safely on any machine with Intel GPU hardware and are always included in the default test run. Both `IGCLWrapper.NativeTests` and `IGCLWrapper.FacadeTests` are passive by default.

### Active tests (apply-and-revert)
A subset of facade tests modify hardware state (e.g. change sharpness, brightness, scaling). These live in `*ActiveTests.cs` files (e.g. `IGCLDisplayHelperActiveTests.cs`, `IGCLAdapterHelperActiveTests.cs`) and are tagged `[Trait("Category", "Active")]`. Each active test applies a change, verifies it, then reverts to the original value — even if the test fails.

Active tests are included in the default `./test_igcl.ps1` run. To run them explicitly or to exclude them:
```powershell
# Run only active tests
dotnet test IGCLWrapper.FacadeTests/IGCLWrapper.FacadeTests.csproj --filter "Category=Active"

# Run everything except active tests
dotnet test IGCLWrapper.FacadeTests/IGCLWrapper.FacadeTests.csproj --filter "Category!=Active"
```

Active tests run sequentially within their collection (`ActiveDisplay`, then `ActiveCombined`) to avoid conflicting hardware state changes.

## Updating bindings
When Intel releases a new IGCL:
```powershell
./prepare_igcl.ps1   # update SDK bits
./build_igcl.ps1     # regenerates bindings via ClangSharp and rebuilds
```

## Project structure
- `IGCLWrapper/` - main wrapper
  - `cs_generated/` - ClangSharp output (auto-generated)
  - `IGCLApi.cs` - high-level API
  - `IGCLExtensions.cs` - helpers for common ops
- `IGCLWrapper.NativeTests/` - native test suite
- `IGCLWrapper.FacadeTests/` - facade test suite
- `Samples/` - sample apps
- `drivers.gpu.control-library/` - IGCL SDK payload (populated by prepare script)

## Usage notes
- Always dispose `IGCLApi` (use `using`); SafeHandle + finalizer backstops leaks.
- Handles returned from enumerate calls are opaque; pass them back to IGCL or helper methods.
- Facade helpers return `T?` (nullable struct) from getter methods — check `.HasValue` before using the result. Setter methods return `bool`. For advanced native struct access, use `IGCLApi` directly.
- Struct `Version` fields are bytes in native structs; use `(byte)0/1` in native code paths.

## Contributing
PRs welcome-please add/keep tests passing and let the generator own `cs_generated`.



