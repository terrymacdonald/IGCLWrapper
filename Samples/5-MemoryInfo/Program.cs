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
                using (var igcl = IGCLApi.Initialize())
                {
                    var adapters = igcl.EnumerateAdapters();

                    if (adapters.Length == 0)
                    {
                        Console.WriteLine("No Intel GPU found.");
                        return;
                    }

                    QueryMemory(adapters[0]);
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

        static unsafe void QueryMemory(IntPtr adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumMemoryModules((_ctl_device_adapter_handle_t*)adapter, &count, null);

            if (result != ctl_result_t.CTL_RESULT_SUCCESS || count == 0)
            {
                Console.WriteLine($"Memory enumeration failed or none found (result: {result})");
                return;
            }

            var mems = new _ctl_mem_handle_t*[count];
            fixed (_ctl_mem_handle_t** pMems = mems)
            {
                IGCL.ctlEnumMemoryModules((_ctl_device_adapter_handle_t*)adapter, &count, pMems);

                for (int i = 0; i < count; i++)
                {
                    var props = new ctl_mem_properties_t
                    {
                        Size = (uint)sizeof(ctl_mem_properties_t),
                        Version = (byte)0
                    };

                    if (IGCL.ctlMemoryGetProperties(mems[i], &props) == ctl_result_t.CTL_RESULT_SUCCESS)
                    {
                        Console.WriteLine($"Module {i + 1}:");
                        Console.WriteLine($"  Type       : {props.type}");
                        Console.WriteLine($"  Bus Width  : {props.busWidth} bits");
                        Console.WriteLine($"  Location   : {props.location}");
                    }

                    var state = new ctl_mem_state_t
                    {
                        Size = (uint)sizeof(ctl_mem_state_t),
                        Version = (byte)0
                    };

                    if (IGCL.ctlMemoryGetState(mems[i], &state) == ctl_result_t.CTL_RESULT_SUCCESS)
                    {
                        Console.WriteLine($"  Free       : {state.free / (1024 * 1024)} MB");
                        Console.WriteLine($"  Total      : {state.size / (1024 * 1024)} MB\n");
                    }
                }
            }
        }
    }
}
