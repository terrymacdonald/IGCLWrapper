using System;
using IGCLWrapper;

namespace FanControl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IGCLWrapper - Fan Control Sample");
            Console.WriteLine("=================================\n");
            Console.WriteLine("? NOTE: Fan control may not be available on all Intel GPUs\n");

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

                    MonitorFans(adapters[0]);
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

        static unsafe void MonitorFans(IntPtr adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumFans((_ctl_device_adapter_handle_t*)adapter, &count, null);

            if (result != _ctl_result_t.CTL_RESULT_SUCCESS)
            {
                Console.WriteLine($"Fan enumeration not supported (Result: {result})");
                return;
            }

            if (count == 0)
            {
                Console.WriteLine("No fans found on this GPU.");
                return;
            }

            Console.WriteLine($"Found {count} fan(s)\n");

            var fans = new _ctl_fan_handle_t*[count];
            fixed (_ctl_fan_handle_t** pFans = fans)
            {
                IGCL.ctlEnumFans((_ctl_device_adapter_handle_t*)adapter, &count, pFans);

                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"Fan #{i + 1}:");

                    // Get fan properties
                    var props = new _ctl_fan_properties_t
                    {
                        Size = (uint)sizeof(_ctl_fan_properties_t),
                        Version = 0
                    };

                    if (IGCL.ctlFanGetProperties(fans[i], &props) == _ctl_result_t.CTL_RESULT_SUCCESS)
                    {
                        Console.WriteLine($"  Max RPM: {props.maxRPM}");
                        Console.WriteLine($"  Max Points: {props.maxPoints}");
                    }

                    // Get current fan speed
                    int speed;
                    if (IGCL.ctlFanGetState(fans[i], _ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM, &speed) == _ctl_result_t.CTL_RESULT_SUCCESS)
                    {
                        Console.WriteLine($"  Current Speed: {speed} RPM");
                    }

                    Console.WriteLine();
                }
            }

            Console.WriteLine("? Fan monitoring completed!");
        }
    }
}
