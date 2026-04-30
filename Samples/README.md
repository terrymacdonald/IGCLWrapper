# IGCLWrapper Samples

This directory contains sample applications demonstrating various features of IGCLWrapper.

## Prerequisites

- Intel GPU with IGCL support
- .NET 10.0 SDK
- Intel Graphics drivers (version 25.20.100.6618+)

## Running Samples

### Using Visual Studio
1. Open `Samples/Samples.sln`
2. Set desired sample as startup project
3. Press F5 to run

### Using .NET CLI
```powershell
cd Samples/1-GettingStarted
dotnet run
```

## Sample Index

| # | Sample | Difficulty | Description |
|---|--------|------------|-------------|
| 1 | [Getting Started](1-GettingStarted/) | Beginner | Basic initialization and enumeration |
| 2 | [Display Information](2-DisplayInformation/) | Beginner | Working with displays |
| 3 | [GPU Monitoring](3-GpuMonitoring/) | Intermediate | Power, temperature, frequency monitoring |
| 4 | [Fan Control](4-FanControl/) | Intermediate | Fan speed and control |
| 5 | [Memory Info](5-MemoryInfo/) | Intermediate | GPU memory information |
| 6 | [Real-Time Monitor](6-RealTimeMonitor/) | Advanced | Continuous monitoring application |
| 7 | [Advanced Features](7-AdvancedFeatures/) | Expert | Overclocking and advanced APIs |

## Common Patterns

Initialization:
```csharp
using var igcl = IGCLApiHelper.Initialize();
```

Error handling:
```csharp
try
{
    using var igcl = IGCLApiHelper.Initialize();
}
catch (IGCLException ex)
{
    Console.WriteLine($"IGCL Error: {ex.Message}");
}
```

Helpers:
```csharp
using System.Linq;
using IGCLWrapper;

using var api = IGCLApiHelper.Initialize();
var adapter = api.EnumerateAdapters().First();
var display = adapter.EnumerateDisplayOutputs().First();

var encoder = display.GetAdapterDisplayEncoderProperties();
var combined = adapter.GetCombinedDisplay();
```
Get/Set operations are split into separate `Get*()` and `Set*()` helpers. For advanced use cases needing raw IGCL structs, use `IGCLApi` directly.

## Building All Samples

```powershell
cd Samples
dotnet build Samples.sln
```

For issues with samples, see the main documentation or open an issue on GitHub.
