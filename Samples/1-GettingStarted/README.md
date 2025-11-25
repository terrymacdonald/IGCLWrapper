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
- .NET 8.0 SDK or later
- Intel Graphics drivers (version 25.20.100.6618 or higher)

## How to Run

### Using .NET CLI:
```bash
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
using (var igcl = IGCLApi.Initialize())
{
    // All IGCL operations go here
    // Resources are automatically cleaned up
}
```

### Using Helper Methods
```csharp
var props = IGCLHelpers.GetProperties(adapter);
string gpuName = new string(props.name);
```

## Related Samples
- [Display Information](../2-DisplayInformation/) - Learn about working with displays
- [GPU Monitoring](../3-GpuMonitoring/) - Monitor power, temperature, and frequency
