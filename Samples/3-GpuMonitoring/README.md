# GPU Monitoring Sample

## Description
Monitor GPU power, temperature, and frequency in real-time using IGCL telemetry APIs.

## What You'll Learn
- Power domain energy counters
- Temperature sensor enumeration
- Frequency domain monitoring
- Facade helper usage

## How to Run
```bash
cd Samples/3-GpuMonitoring
dotnet run
```

## Demonstrated APIs
- `IGCLPowerHelper.PowerGetEnergyCounter`
- `IGCLTemperatureHelper.EnumTemperatureSensors`
- `IGCLTemperatureHelper.TemperatureGetState`
- `IGCLFrequencyHelper.EnumFrequencyDomains`
- `IGCLFrequencyHelper.FrequencyGetState`
