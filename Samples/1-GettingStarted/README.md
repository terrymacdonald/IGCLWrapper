# Getting Started with IGCLWrapper

## Description
This sample demonstrates the basic usage of IGCLWrapper, including initialization, adapter enumeration, and retrieving GPU information.

## What You'll Learn
- How to initialize the IGCL API
- How to enumerate Intel GPU adapters
- How to get basic GPU properties
- Proper resource disposal using the `using` statement
- Basic error handling for IGCL operations

## Prerequisites
- Intel GPU with IGCL support
- .NET 10.0 SDK or later
- Intel Graphics drivers (version 25.20.100.6618 or higher)

## How to Run

### Using .NET CLI:
```powershell
cd Samples/1-GettingStarted
dotnet run
```

### Using Visual Studio:
1. Open `Samples/Samples.sln`
2. Right-click on `1-GettingStarted` project
3. Select "Set as Startup Project"
4. Press F5 or click Run

## Key Concepts

### Initialization Pattern
```csharp
using var igcl = IGCLApiHelper.Initialize();
```

### Using Helper Methods
```csharp
using var api = IGCLApiHelper.Initialize();
var adapter = api.EnumerateAdapters().First();
var props = adapter.GetProperties();
var gpuName = adapter.Name;
```

## Related Samples
- [Display Information](../2-DisplayInformation/) - Learn about working with displays
- [GPU Monitoring](../3-GpuMonitoring/) - Monitor power, temperature, and frequency
