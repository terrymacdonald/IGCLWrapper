using System;
using System.Runtime.InteropServices;
using System.Text;
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
                    ReadOnlySpan<sbyte> nameSpan = MemoryMarshal.CreateReadOnlySpan(ref props.name.e0, 100);
                    int term = nameSpan.IndexOf((sbyte)0);
                    if (term >= 0) nameSpan = nameSpan[..term];
                    var name = Encoding.UTF8.GetString(MemoryMarshal.Cast<sbyte, byte>(nameSpan));
                    Console.WriteLine($"Monitoring: {name}\n");

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
            var telemetry = new ctl_power_telemetry_t
            {
                Size = (uint)sizeof(ctl_power_telemetry_t),
                Version = (byte)0
            };

            if (IGCL.ctlPowerTelemetryGet((_ctl_device_adapter_handle_t*)adapter, &telemetry) == ctl_result_t.CTL_RESULT_SUCCESS)
            {
                Console.WriteLine($"GPU Temperature : {telemetry.gpuCurrentTemperature.value.datadouble,6:F1}°C");
                Console.WriteLine($"GPU Power       : {telemetry.gpuEnergyCounter.value.datadouble / 1000.0,6:F2} J  ");
                Console.WriteLine($"GPU Frequency   : {telemetry.gpuCurrentClockFrequency.value.datadouble,6:F0} MHz");
            }
            else
            {
                Console.WriteLine("Telemetry not available");
            }

            Console.WriteLine($"\nLast Update     : {DateTime.Now:HH:mm:ss}    ");
        }
    }
}
