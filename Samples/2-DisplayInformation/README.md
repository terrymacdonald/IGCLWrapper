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
using var api = IGCLApiHelper.Initialize();
var adapter = api.EnumerateAdapters().First();
var displays = adapter.EnumerateDisplayOutputs();
var (width, height) = displays[0].GetResolution();
var refreshRate = displays[0].GetRefreshRateHz();
```
