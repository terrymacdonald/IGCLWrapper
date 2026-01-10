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
                using (var igcl = IGCLApiHelper.Initialize())
                {
                    var adapters = igcl.EnumerateAdapters();

                    if (adapters.Count == 0)
                    {
                        Console.WriteLine("No Intel GPU found.");
                        return;
                    }

                    var adapter = adapters[0];
                    Console.WriteLine($"Monitoring: {adapter.Name}\n");

                    var powerHelper = igcl.GetPowerHelper(adapter);
                    var tempHelper = igcl.GetTemperatureHelper(adapter);
                    var freqHelper = igcl.GetFrequencyHelper(adapter);

                    var powerDomains = powerHelper.EnumPowerDomains();
                    var tempSensors = tempHelper.EnumTemperatureSensors();
                    var freqDomains = freqHelper.EnumFrequencyDomains();

                    while (_running)
                    {
                        if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                            break;

                        Console.SetCursorPosition(0, 5);
                        DisplayMetrics(powerHelper, tempHelper, freqHelper, powerDomains, tempSensors, freqDomains);
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

        static void DisplayMetrics(
            IGCLPowerHelper powerHelper,
            IGCLTemperatureHelper tempHelper,
            IGCLFrequencyHelper freqHelper,
            System.Collections.Generic.IReadOnlyList<IntPtr> powerDomains,
            System.Collections.Generic.IReadOnlyList<IntPtr> tempSensors,
            System.Collections.Generic.IReadOnlyList<IntPtr> freqDomains)
        {
            string temperature = "n/a";
            if (tempSensors.Count > 0)
            {
                var temp = tempHelper.TemperatureGetState(tempSensors[0]);
                temperature = $"{temp:F1} C";
            }

            string energy = "n/a";
            if (powerDomains.Count > 0)
            {
                var counter = powerHelper.PowerGetEnergyCounter(powerDomains[0]);
                energy = $"{counter.energy} uJ";
            }

            string frequency = "n/a";
            if (freqDomains.Count > 0)
            {
                var state = freqHelper.FrequencyGetState(freqDomains[0]);
                frequency = $"{state.actual:F0} MHz";
            }

            Console.WriteLine($"GPU Temperature : {temperature,-10}");
            Console.WriteLine($"GPU Energy      : {energy,-10}");
            Console.WriteLine($"GPU Frequency   : {frequency,-10}");
            Console.WriteLine($"\nLast Update     : {DateTime.Now:HH:mm:ss}    ");
        }
    }
}
