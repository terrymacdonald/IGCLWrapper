# IGCLWrapper Samples Structure Proposal

## ?? Recommended Directory Structure

```
IGCLWrapper/
??? Samples/
?   ??? Samples.sln                          # Samples-only solution
?   ??? README.md                            # Samples overview and index
?   ?
?   ??? 1-GettingStarted/
?   ?   ??? GettingStarted.csproj
?   ?   ??? Program.cs                       # Basic initialization & enumeration
?   ?   ??? README.md                        # Sample documentation
?   ?
?   ??? 2-DisplayInformation/
?   ?   ??? DisplayInformation.csproj
?   ?   ??? Program.cs                       # Display properties, resolution, refresh rate
?   ?   ??? README.md
?   ?
?   ??? 3-GpuMonitoring/
?   ?   ??? GpuMonitoring.csproj
?   ?   ??? Program.cs                       # Power, temperature, frequency monitoring
?   ?   ??? README.md
?   ?
?   ??? 4-FanControl/
?   ?   ??? FanControl.csproj
?   ?   ??? Program.cs                       # Fan speed control (if supported)
?   ?   ??? README.md
?   ?
?   ??? 5-MemoryInfo/
?   ?   ??? MemoryInfo.csproj
?   ?   ??? Program.cs                       # GPU memory information
?   ?   ??? README.md
?   ?
?   ??? 6-RealTimeMonitor/
?   ?   ??? RealTimeMonitor.csproj
?   ?   ??? Program.cs                       # Console-based real-time monitoring app
?   ?   ??? MonitoringService.cs
?   ?   ??? README.md
?   ?
?   ??? 7-AdvancedFeatures/
?       ??? AdvancedFeatures.csproj
?       ??? Program.cs                       # Overclocking, 3D settings, advanced APIs
?       ??? README.md
?
??? README.md                                # Main README (updated)
```

## ?? Sample Descriptions

### 1. Getting Started
**Purpose**: First sample every developer should run  
**Demonstrates**:
- Initializing IGCL API
- Enumerating adapters
- Getting basic GPU information
- Proper disposal pattern
- Error handling

**Code Preview**:
```csharp
using IGCLWrapper;

using (var igcl = IGCLApi.Initialize())
{
    var adapters = igcl.EnumerateAdapters();
    Console.WriteLine($"Found {adapters.Length} GPU(s)");
    
    foreach (var adapter in adapters)
    {
        var props = IGCLHelpers.GetProperties(adapter);
        Console.WriteLine($"GPU: {new string(props.name)}");
    }
}
```

### 2. Display Information
**Purpose**: Working with displays  
**Demonstrates**:
- Enumerating displays
- Getting display properties
- Resolution and refresh rate
- Display timing information
- Helper methods usage

**Code Preview**:
```csharp
var displays = igcl.EnumerateDisplays(adapter);
foreach (var display in displays)
{
    var (width, height) = IGCLHelpers.GetResolution(display);
    var refreshRate = IGCLHelpers.GetRefreshRate(display);
    Console.WriteLine($"{width}x{height} @ {refreshRate:F2} Hz");
}
```

### 3. GPU Monitoring
**Purpose**: Real-time GPU metrics  
**Demonstrates**:
- Power telemetry
- Temperature monitoring
- Frequency states
- Engine utilization
- Structured data output

**Code Preview**:
```csharp
unsafe
{
    var telemetry = new _ctl_power_telemetry_t
    {
        Size = (uint)sizeof(_ctl_power_telemetry_t),
        Version = 0
    };
    
    IGCL.ctlPowerTelemetryGet(
        (_ctl_device_adapter_handle_t*)adapter, 
        &telemetry
    );
    
    Console.WriteLine($"Power: {telemetry.gpuEnergyCounter} mJ");
    Console.WriteLine($"Temperature: {telemetry.gpuCurrentTemperature}°C");
}
```

### 4. Fan Control
**Purpose**: Fan management (if supported)  
**Demonstrates**:
- Enumerating fans
- Getting fan properties
- Reading fan speed
- Setting fan curves (if supported)
- Safe feature detection

### 5. Memory Info
**Purpose**: GPU memory monitoring  
**Demonstrates**:
- Enumerating memory modules
- Memory properties
- Memory state and usage
- Bandwidth information

### 6. Real-Time Monitor
**Purpose**: Complete monitoring application  
**Demonstrates**:
- Multi-threaded monitoring
- Console UI with updates
- Data aggregation
- Performance best practices
- Production-ready code structure

**Features**:
- Live GPU stats dashboard
- Configurable refresh rate
- CSV export option
- Color-coded output

### 7. Advanced Features
**Purpose**: Advanced/expert-level usage  
**Demonstrates**:
- Overclocking APIs (with warnings!)
- 3D feature capabilities
- Video processing features
- Direct IGCL API calls
- Safety checks and validation

## ?? Samples/README.md Content

```markdown
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
| 1 | [Getting Started](1-GettingStarted/) | Beginner | Basic initialization and enumeration |
| 2 | [Display Information](2-DisplayInformation/) | Beginner | Working with displays |
| 3 | [GPU Monitoring](3-GpuMonitoring/) | Intermediate | Power, temperature, frequency monitoring |
| 4 | [Fan Control](4-FanControl/) | Intermediate | Fan speed and control |
| 5 | [Memory Info](5-MemoryInfo/) | Intermediate | GPU memory information |
| 6 | [Real-Time Monitor](6-RealTimeMonitor/) | Advanced | Complete monitoring application |
| 7 | [Advanced Features](7-AdvancedFeatures/) | Expert | Overclocking and advanced APIs |

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

## Support

For issues with samples, see [main documentation](../README.md) or open an issue.
```

## ?? Sample Code Templates

### Sample Project File Template
```xml
<Project Sdk="Microsoft.NET.Sdk">
  <PropertyGroup>
    <OutputType>Exe</OutputType>
    <TargetFramework>net8.0</TargetFramework>
    <AllowUnsafeBlocks>true</AllowUnsafeBlocks>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\IGCLWrapper\IGCLWrapper.csproj" />
  </ItemGroup>
</Project>
```

### Sample README Template
```markdown
# Sample Name

## Description
Brief description of what this sample demonstrates.

## What You'll Learn
- Bullet point 1
- Bullet point 2
- Bullet point 3

## Prerequisites
- Intel GPU with IGCL support
- [Any specific requirements]

## How to Run

Using .NET CLI:
```bash
dotnet run
```

Using Visual Studio:
1. Set as startup project
2. Press F5

## Expected Output
```
Example output here
```

## Key Code Sections

### Section 1: Description
```csharp
// Code snippet
```

### Section 2: Description
```csharp
// Code snippet
```

## Notes
- Important notes
- Limitations
- Safety considerations (for advanced samples)

## Related Samples
- [Sample X](../X-SampleName/)
- [Sample Y](../Y-SampleName/)
```

## ?? Implementation Steps

### Phase 1: Basic Samples (High Priority)
1. Create `Samples/` directory
2. Create `Samples.sln`
3. Implement Getting Started sample
4. Implement Display Information sample
5. Create Samples/README.md

### Phase 2: Intermediate Samples
1. GPU Monitoring sample
2. Fan Control sample  
3. Memory Info sample

### Phase 3: Advanced Samples
1. Real-Time Monitor (with console UI)
2. Advanced Features (with safety warnings)

### Phase 4: Polish
1. Add XML documentation to sample code
2. Create sample-specific README files
3. Test all samples on hardware
4. Add screenshots/output examples

## ?? Deliverables

### For Each Sample:
- ? Self-contained .csproj file
- ? Well-commented Program.cs
- ? README.md with explanation
- ? Error handling for missing hardware
- ? Proper disposal pattern

### For Samples Directory:
- ? Samples.sln (all samples in one solution)
- ? Master README.md with index
- ? Common project reference to IGCLWrapper
- ? Consistent coding style

## ?? Benefits

### For Users:
- **Quick Start**: Copy-paste working code
- **Learning Path**: Progression from basic to advanced
- **Best Practices**: Production-ready patterns
- **Self-Contained**: Each sample runs independently

### For Maintenance:
- **Testable**: Samples serve as integration tests
- **Documentation**: Living examples of API usage
- **Discoverable**: Easy to find relevant examples
- **Extendable**: Easy to add new samples

## ?? Success Metrics

- [ ] New users can run first sample in < 5 minutes
- [ ] Each sample has clear learning objective
- [ ] All samples handle missing hardware gracefully
- [ ] Code follows consistent style
- [ ] README files are clear and helpful
- [ ] Samples compile without warnings
- [ ] Advanced samples include safety warnings

## ?? Next Steps

1. **Review this proposal** - Approve overall structure
2. **Create directory structure** - Set up folders
3. **Implement Getting Started** - Validate approach
4. **Iterate** - Add remaining samples
5. **Test on hardware** - Verify all samples work
6. **Document** - Complete all README files

Would you like me to start implementing these samples?
