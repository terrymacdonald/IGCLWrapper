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

                    MonitorPower(powerHelper);
                    MonitorTemperature(tempHelper);
                    MonitorFrequency(freqHelper);

                    Console.WriteLine("\nMonitoring completed.");
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

        static void MonitorPower(IGCLPowerHelper powerHelper)
        {
            var domains = powerHelper.EnumPowerDomains();
            if (domains.Count == 0)
            {
                Console.WriteLine("Power telemetry not available.\n");
                return;
            }

            Console.WriteLine("Power Domains:");
            for (int i = 0; i < domains.Count; i++)
            {
                var energy = powerHelper.PowerGetEnergyCounter(domains[i]);
                if (energy.HasValue)
                    Console.WriteLine($"  Domain {i + 1}     : {energy.Value.Energy} uJ (timestamp {energy.Value.Timestamp})");
                else
                    Console.WriteLine($"  Domain {i + 1}     : not supported");
            }
            Console.WriteLine();
        }

        static void MonitorTemperature(IGCLTemperatureHelper tempHelper)
        {
            var sensors = tempHelper.EnumTemperatureSensors();
            if (sensors.Count > 0)
            {
                Console.WriteLine("Temperature Sensors:");
                for (int i = 0; i < sensors.Count; i++)
                {
                    var temperature = tempHelper.TemperatureGetState(sensors[i]);
                    if (temperature.HasValue)
                        Console.WriteLine($"  Sensor {i + 1}      : {temperature.Value:F1} C");
                    else
                        Console.WriteLine($"  Sensor {i + 1}      : not supported");
                }
                Console.WriteLine();
            }
        }

        static void MonitorFrequency(IGCLFrequencyHelper freqHelper)
        {
            var domains = freqHelper.EnumFrequencyDomains();
            if (domains.Count > 0)
            {
                Console.WriteLine("Frequency Domains:");
                for (int i = 0; i < domains.Count; i++)
                {
                    var state = freqHelper.FrequencyGetState(domains[i]);
                    if (state.HasValue)
                        Console.WriteLine($"  Domain {i + 1}      : {state.Value.Actual:F0} MHz (Request: {state.Value.Request:F0} MHz)");
                    else
                        Console.WriteLine($"  Domain {i + 1}      : not supported");
                }
            }
        }
    }
}
