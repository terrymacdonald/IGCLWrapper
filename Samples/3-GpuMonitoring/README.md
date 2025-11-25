# GPU Monitoring Sample

## Description
Monitor GPU power, temperature, and frequency in real-time using IGCL telemetry APIs.

## What You'll Learn
- Power telemetry access
- Temperature sensor enumeration
- Frequency domain monitoring
- Direct IGCL API calls with unsafe code

## How to Run
```bash
cd Samples/3-GpuMonitoring
dotnet run
```

## Demonstrated APIs
- `ctlPowerTelemetryGet`
- `ctlEnumTemperatureSensors`
- `ctlTemperatureGetState`
- `ctlEnumFrequencyDomains`
- `ctlFrequencyGetState`
