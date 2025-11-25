# Display Information Sample

## Description
Learn how to work with displays connected to Intel GPUs, including resolution, refresh rate, and display properties.

## What You'll Learn
- Enumerating displays on an adapter
- Getting display properties
- Using helper methods for common tasks
- Display timing information
- Checking if a display is active

## How to Run
```bash
cd Samples/2-DisplayInformation
dotnet run
```

## Key Code
```csharp
var displays = igcl.EnumerateDisplays(adapter);
var (width, height) = IGCLHelpers.GetResolution(display);
var refreshRate = IGCLHelpers.GetRefreshRate(display);
```
