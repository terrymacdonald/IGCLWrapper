using System;
using IGCLWrapper;

namespace MemoryInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IGCLWrapper - Memory Info Sample\n");

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

                    QueryMemory(igcl, adapters[0]);
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
        }

        static void QueryMemory(IGCLApiHelper api, IGCLAdapterHelper adapter)
        {
            var memoryHelper = api.GetMemoryHelper(adapter);
            var modules = memoryHelper.EnumMemoryModules();
            if (modules.Count == 0)
            {
                Console.WriteLine("Memory enumeration failed or none found.");
                return;
            }

            for (int i = 0; i < modules.Count; i++)
            {
                var props = memoryHelper.MemoryGetProperties(modules[i]);
                if (!props.HasValue)
                {
                    Console.WriteLine($"Module {i + 1}: properties not supported on this hardware.\n");
                    continue;
                }

                Console.WriteLine($"Module {i + 1}:");
                Console.WriteLine($"  Type       : {props.Value.Type}");
                Console.WriteLine($"  Bus Width  : {props.Value.BusWidth} bits");
                Console.WriteLine($"  Location   : {props.Value.Location}");

                var state = memoryHelper.MemoryGetState(modules[i]);
                if (state.HasValue)
                {
                    Console.WriteLine($"  Free       : {state.Value.Free / (1024 * 1024)} MB");
                    Console.WriteLine($"  Total      : {state.Value.TotalSize / (1024 * 1024)} MB\n");
                }
                else
                {
                    Console.WriteLine($"  State      : not supported\n");
                }
            }
        }
    }
}
