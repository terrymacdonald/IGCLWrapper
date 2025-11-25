using System;
using IGCLWrapper;

namespace GpuMonitoring
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IGCLWrapper - GPU Monitoring Sample");
            Console.WriteLine("====================================\n");

            try
            {
                using (var igcl = IGCLApi.Initialize())
                {
                    var adapters = igcl.EnumerateAdapters();
                    
                    if (adapters.Length == 0)
                    {
                        Console.WriteLine("No Intel GPU found.");
                        return;
                    }

                    var adapter = adapters[0];
                    var props = IGCLHelpers.GetProperties(adapter);
                    Console.WriteLine($"Monitoring: {new string(props.name).TrimEnd('\0')}\n");

                    // Power Telemetry
                    unsafe
                    {
                        var telemetry = new _ctl_power_telemetry_t
                        {
                            Size = (uint)sizeof(_ctl_power_telemetry_t),
                            Version = 0
                        };

                        var result = IGCL.ctlPowerTelemetryGet((_ctl_device_adapter_handle_t*)adapter, &telemetry);

                        if (result == _ctl_result_t.CTL_RESULT_SUCCESS)
                        {
                            Console.WriteLine("Power & Thermal:");
                            Console.WriteLine($"  GPU Power      : {telemetry.gpuEnergyCounter.value} mJ");
                            Console.WriteLine($"  Temperature    : {telemetry.gpuCurrentTemperature}°C");
                            Console.WriteLine($"  Current Freq   : {telemetry.gpuCurrentClockFrequency} MHz\n");
                        }
                        else
                        {
                            Console.WriteLine($"Power telemetry not available (Result: {result})\n");
                        }
                    }

                    // Temperature Sensors
                    MonitorTemperature(adapter);

                    // Frequency Domains
                    MonitorFrequency(adapter);

                    Console.WriteLine("\n? Monitoring completed!");
                }
            }
            catch (IGCLException ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
            catch (DllNotFoundException)
            {
                Console.WriteLine("Intel Graphics drivers not found.");
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static unsafe void MonitorTemperature(IntPtr adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumTemperatureSensors((_ctl_device_adapter_handle_t*)adapter, &count, null);

            if (result == _ctl_result_t.CTL_RESULT_SUCCESS && count > 0)
            {
                Console.WriteLine("Temperature Sensors:");
                
                var temps = new _ctl_temp_handle_t*[count];
                fixed (_ctl_temp_handle_t** pTemps = temps)
                {
                    IGCL.ctlEnumTemperatureSensors((_ctl_device_adapter_handle_t*)adapter, &count, pTemps);
                    
                    for (int i = 0; i < count; i++)
                    {
                        double temperature;
                        if (IGCL.ctlTemperatureGetState(temps[i], &temperature) == _ctl_result_t.CTL_RESULT_SUCCESS)
                        {
                            Console.WriteLine($"  Sensor {i + 1}      : {temperature:F1}°C");
                        }
                    }
                }
                Console.WriteLine();
            }
        }

        static unsafe void MonitorFrequency(IntPtr adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumFrequencyDomains((_ctl_device_adapter_handle_t*)adapter, &count, null);

            if (result == _ctl_result_t.CTL_RESULT_SUCCESS && count > 0)
            {
                Console.WriteLine("Frequency Domains:");
                
                var freqs = new _ctl_freq_handle_t*[count];
                fixed (_ctl_freq_handle_t** pFreqs = freqs)
                {
                    IGCL.ctlEnumFrequencyDomains((_ctl_device_adapter_handle_t*)adapter, &count, pFreqs);
                    
                    for (int i = 0; i < count; i++)
                    {
                        var state = new _ctl_freq_state_t
                        {
                            Size = (uint)sizeof(_ctl_freq_state_t),
                            Version = 0
                        };

                        if (IGCL.ctlFrequencyGetState(freqs[i], &state) == _ctl_result_t.CTL_RESULT_SUCCESS)
                        {
                            Console.WriteLine($"  Domain {i + 1}      : {state.actual:F0} MHz (Request: {state.request:F0} MHz)");
                        }
                    }
                }
            }
        }
    }
}
