using System;
using IGCLWrapper;

namespace AdvancedFeatures
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IGCLWrapper - Advanced Features Sample");
            Console.WriteLine("=======================================\n");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("WARNING: This sample demonstrates advanced/expert-level APIs");
            Console.WriteLine("Some features may modify GPU settings - use with caution!");
            Console.ResetColor();
            Console.WriteLine();

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

                    CheckOverclockingSupport(igcl, adapters[0]);
                    Check3DCapabilities(igcl, adapters[0]);
                    CheckVideoProcessing(igcl, adapters[0]);
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

        static void CheckOverclockingSupport(IGCLApiHelper api, IGCLAdapterHelper adapter)
        {
            Console.WriteLine("Overclocking Support:");

            var overclockHelper = api.GetOverclockHelper(adapter);
            var props = overclockHelper.GetProperties();
            if (!props.HasValue)
            {
                Console.WriteLine("  Not supported on this hardware.");
                Console.WriteLine();
                return;
            }
            Console.WriteLine($"  Supported        : {props.Value.IsSupported}");
            Console.WriteLine($"  GPU OC Supported : {props.Value.GpuFrequencyOffset.IsSupported}");
            Console.WriteLine($"  VRAM OC Supported: {props.Value.VramFrequencyOffset.IsSupported}");
            Console.WriteLine();
        }

        static void Check3DCapabilities(IGCLApiHelper api, IGCLAdapterHelper adapter)
        {
            Console.WriteLine("3D Graphics Capabilities:");

            var helper = api.Get3DHelper(adapter);
            var caps = helper.GetSupported3DCapabilities();
            if (caps.HasValue)
                Console.WriteLine($"  Supported Features: {caps.Value.NumSupportedFeatures}");
            else
                Console.WriteLine("  Not supported on this hardware.");
            Console.WriteLine();
        }

        static void CheckVideoProcessing(IGCLApiHelper api, IGCLAdapterHelper adapter)
        {
            Console.WriteLine("Video Processing Capabilities:");

            var helper = api.GetMediaHelper(adapter);
            var caps = helper.GetSupportedVideoProcessingCapabilities();
            if (caps.HasValue)
                Console.WriteLine($"  Supported Features: {caps.Value.NumSupportedFeatures}");
            else
                Console.WriteLine("  Not supported on this hardware.");
            Console.WriteLine();
        }
    }
}
