using System;
using IGCLWrapper;

namespace MemoryInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IGCLWrapper - Memory Information Sample");
            Console.WriteLine("========================================\n");

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

                    GetMemoryInfo(adapters[0]);
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

        static unsafe void GetMemoryInfo(IntPtr adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumMemoryModules((_ctl_device_adapter_handle_t*)adapter, &count, null);

            if (result != _ctl_result_t.CTL_RESULT_SUCCESS || count == 0)
            {
                Console.WriteLine("Memory information not available.");
                return;
            }

            Console.WriteLine($"Found {count} memory module(s)\n");

            var mems = new _ctl_mem_handle_t*[count];
            fixed (_ctl_mem_handle_t** pMems = mems)
            {
                IGCL.ctlEnumMemoryModules((_ctl_device_adapter_handle_t*)adapter, &count, pMems);

                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"Memory Module #{i + 1}:");

                    // Get properties
                    var props = new _ctl_mem_properties_t
                    {
                        Size = (uint)sizeof(_ctl_mem_properties_t),
                        Version = 0
                    };

                    if (IGCL.ctlMemoryGetProperties(mems[i], &props) == _ctl_result_t.CTL_RESULT_SUCCESS)
                    {
                        Console.WriteLine($"  Type        : {props.type}");
                        Console.WriteLine($"  Location    : {props.location}");
                        Console.WriteLine($"  Physical Size: {props.physicalSize / (1024.0 * 1024.0 * 1024.0):F2} GB");
                        Console.WriteLine($"  Bus Width   : {props.busWidth} bits");
                        Console.WriteLine($"  Channels    : {props.numChannels}");
                    }

                    // Get current state
                    var state = new _ctl_mem_state_t
                    {
                        Size = (uint)sizeof(_ctl_mem_state_t),
                        Version = 0
                    };

                    if (IGCL.ctlMemoryGetState(mems[i], &state) == _ctl_result_t.CTL_RESULT_SUCCESS)
                    {
                        Console.WriteLine($"  Free        : {state.free / (1024.0 * 1024.0 * 1024.0):F2} GB");
                        Console.WriteLine($"  Used        : {(state.size - state.free) / (1024.0 * 1024.0 * 1024.0):F2} GB");
                    }

                    Console.WriteLine();
                }
            }

            Console.WriteLine("? Memory information retrieved!");
        }
    }
}
