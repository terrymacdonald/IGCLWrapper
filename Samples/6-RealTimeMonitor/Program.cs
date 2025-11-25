using System;
using System.Threading;
using IGCLWrapper;

namespace RealTimeMonitor
{
    class Program
    {
        private static bool _running = true;

        static void Main(string[] args)
        {
            Console.WriteLine("IGCLWrapper - Real-Time GPU Monitor");
            Console.WriteLine("====================================");
            Console.WriteLine("Press ESC to exit\n");

            Console.CancelKeyPress += (s, e) => { _running = false; e.Cancel = true; };

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

                    var props = IGCLHelpers.GetProperties(adapters[0]);
                    Console.WriteLine($"Monitoring: {new string(props.name).TrimEnd('\0')}\n");

                    while (_running)
                    {
                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                            break;

                        Console.SetCursorPosition(0, 5);
                        DisplayMetrics(adapters[0]);
                        Thread.Sleep(1000);
                    }
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

            Console.WriteLine("\n\nMonitoring stopped.");
        }

        static unsafe void DisplayMetrics(IntPtr adapter)
        {
            var telemetry = new _ctl_power_telemetry_t
            {
                Size = (uint)sizeof(_ctl_power_telemetry_t),
                Version = 0
            };

            if (IGCL.ctlPowerTelemetryGet((_ctl_device_adapter_handle_t*)adapter, &telemetry) == _ctl_result_t.CTL_RESULT_SUCCESS)
            {
                Console.WriteLine($"GPU Temperature : {telemetry.gpuCurrentTemperature,6:F1}°C");
                Console.WriteLine($"GPU Power       : {telemetry.gpuEnergyCounter.value / 1000.0,6:F2} J  ");
                Console.WriteLine($"GPU Frequency   : {telemetry.gpuCurrentClockFrequency,6:F0} MHz");
            }
            else
            {
                Console.WriteLine("Telemetry not available");
            }

            Console.WriteLine($"\nLast Update     : {DateTime.Now:HH:mm:ss}    ");
        }
    }
}
