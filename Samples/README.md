# IGCLWrapper Samples

This directory contains sample applications demonstrating various features of IGCLWrapper.

## Prerequisites

- Intel GPU with IGCL support
- .NET 8.0 SDK
- Intel Graphics drivers (version 25.20.100.6618+)

## Running Samples

### Using Visual Studio
1. Open `Samples/Samples.sln`
2. Set desired sample as startup project
3. Press F5 to run

### Using .NET CLI
```bash
cd Samples/1-GettingStarted
dotnet run
```

## Sample Index

| # | Sample | Difficulty | Description |
|---|--------|------------|-------------|
| 1 | [Getting Started](1-GettingStarted/) | ? Beginner | Basic initialization and enumeration |
| 2 | [Display Information](2-DisplayInformation/) | ? Beginner | Working with displays |
| 3 | [GPU Monitoring](3-GpuMonitoring/) | ?? Intermediate | Power, temperature, frequency monitoring |
| 4 | [Fan Control](4-FanControl/) | ?? Intermediate | Fan speed and control |
| 5 | [Memory Info](5-MemoryInfo/) | ?? Intermediate | GPU memory information |
| 6 | [Real-Time Monitor](6-RealTimeMonitor/) | ??? Advanced | Complete monitoring application |
| 7 | [Advanced Features](7-AdvancedFeatures/) | ???? Expert | Overclocking and advanced APIs |

## Learning Path

**Beginners**: Start with samples 1-2  
**Intermediate**: Samples 3-5  
**Advanced**: Samples 6-7

## Notes

- All samples include error handling for missing hardware
- Samples gracefully degrade if features aren't supported
- Advanced samples include safety warnings
- Each sample is self-contained and can run independently

## Common Patterns

All samples follow these patterns:

### Initialization
```csharp
using (var igcl = IGCLApi.Initialize())
{
    // Your code here
} // Automatic cleanup
```

### Error Handling
```csharp
try
{
    using (var igcl = IGCLApi.Initialize())
    {
        // Your code
    }
}
catch (IGCLException ex)
{
    Console.WriteLine($"IGCL Error: {ex.Message}");
}
catch (DllNotFoundException)
{
    Console.WriteLine("Intel drivers not installed");
}
```

### Using Helpers
```csharp
// Prefer helper methods for common operations
var props = IGCLHelpers.GetProperties(adapter);
var (width, height) = IGCLHelpers.GetResolution(display);
```

## Building All Samples

```bash
cd Samples
dotnet build Samples.sln
```

## Support

For issues with samples, see [main documentation](../README.md) or open an issue on GitHub.
