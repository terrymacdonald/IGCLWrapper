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

## Expected Output
```
????????????????????????????????????????????????????????????????
?           IGCLWrapper - Getting Started Sample               ?
????????????????????????????????????????????????????????????????

Initializing IGCL API...
? IGCL API initialized successfully!

Enumerating Intel GPU adapters...
Found 1 Intel GPU adapter(s)

????????????????????????????????????????????????????????????????
? Adapter #1                                                   ?
????????????????????????????????????????????????????????????????

GPU Information:
  Name         : Intel(R) Arc(TM) A770 Graphics
  Vendor ID    : 0x8086 (Intel)
  Device ID    : 0x56A5
  Revision     : 8
  Driver Ver   : 31.0.101.5122

Architecture:
  Slices       : 8
  Sub-slices   : 32
  EUs per SS   : 16
  Total EUs    : 512

PCI Information:
  Bus          : 3
  Device       : 0
  Function     : 0

? Sample completed successfully!
Press any key to exit...
```

## Key Code Sections

### 1. Initialization and Cleanup
The most important pattern in IGCLWrapper is proper initialization and disposal:

```csharp
try
{
    using (var igcl = IGCLApi.Initialize())
    {
        // All IGCL operations go here
        // Resources are automatically cleaned up when the using block exits
    }
}
catch (IGCLException ex)
{
    // Handle IGCL-specific errors
    Console.WriteLine($"IGCL Error: {ex.Message} (Code: {ex.Result})");
}
catch (DllNotFoundException)
{
    // Handle missing Intel drivers
    Console.WriteLine("Intel Graphics drivers not found. Please install Intel Graphics drivers.");
}
```

### 2. Enumerating Adapters
Get all Intel GPU adapters in the system:

```csharp
var adapters = igcl.EnumerateAdapters();
Console.WriteLine($"Found {adapters.Length} Intel GPU adapter(s)");
```

The `EnumerateAdapters()` method returns an array of `IntPtr` handles. These handles are opaque - you don't need to understand what they contain, just pass them to other API methods.

### 3. Getting GPU Properties
Use the helper method to retrieve detailed information about each adapter:

```csharp
foreach (var adapter in adapters)
{
    var props = IGCLHelpers.GetProperties(adapter);
    
    // Properties are now available as a struct
    string gpuName = new string(props.name);
    uint vendorId = props.pci_vendor_id;  // 0x8086 = Intel
    uint deviceId = props.pci_device_id;
    ulong driverVersion = props.driver_version;
}
```

### 4. Accessing Architecture Details
```csharp
uint slices = props.num_slices;
uint subSlices = props.num_sub_slices_per_slice;
uint eusPerSubSlice = props.num_eus_per_sub_slice;
uint totalEUs = slices * subSlices * eusPerSubSlice;

Console.WriteLine($"Total EUs: {totalEUs}");
```

## Notes
- The `using` statement ensures proper cleanup even if exceptions occur
- All handles (like adapter handles) are `IntPtr` - easy to work with, no complex types
- Helper methods like `IGCLHelpers.GetProperties()` do the heavy lifting for you
- If no Intel GPU is found, `adapters.Length` will be 0 (not an error)
- The sample gracefully handles missing hardware or drivers

## Common Issues

### "DllNotFoundException: ControlLib"
**Solution**: Install Intel Graphics drivers (version 25.20.100.6618 or higher)

### "Found 0 Intel GPU adapters"
**Possible causes**:
- Running on a system without Intel GPU
- Integrated GPU disabled in BIOS
- Using non-Intel GPU as primary

### "IGCLException: CTL_RESULT_ERROR_UNINITIALIZED"
**Solution**: Make sure to initialize the API before calling any methods

## Related Samples
- [Display Information](../2-DisplayInformation/) - Learn about working with displays
- [GPU Monitoring](../3-GpuMonitoring/) - Monitor power, temperature, and frequency

## Next Steps
Once you're comfortable with this sample, try:
1. Modifying the output format
2. Adding more property displays
3. Moving on to the Display Information sample
