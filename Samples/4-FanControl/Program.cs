using System;
using IGCLWrapper;

namespace FanControl
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("IGCLWrapper - Fan Control Sample\n");

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

                    ControlFans(adapters[0]);
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

        static unsafe void ControlFans(IntPtr adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumFans((_ctl_device_adapter_handle_t*)adapter, &count, null);

            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
            {
                Console.WriteLine($"Fan enumeration failed: {result}");
                return;
            }

            if (count == 0)
            {
                Console.WriteLine("No controllable fans found.");
                return;
            }

            var fans = new _ctl_fan_handle_t*[count];
            fixed (_ctl_fan_handle_t** pFans = fans)
            {
                IGCL.ctlEnumFans((_ctl_device_adapter_handle_t*)adapter, &count, pFans);

                for (int i = 0; i < count; i++)
                {
                    var props = new ctl_fan_properties_t
                    {
                        Size = (uint)sizeof(ctl_fan_properties_t),
                        Version = (byte)0
                    };

                    if (IGCL.ctlFanGetProperties(fans[i], &props) == ctl_result_t.CTL_RESULT_SUCCESS)
                    {
                        Console.WriteLine($"Fan {i + 1}:");
                        Console.WriteLine($"  Max RPM    : {props.maxRPM}");
                        Console.WriteLine($"  Can Control: {props.canControl != 0}");
                    }

                    var speed = new ctl_fan_speed_t
                    {
                        Size = (uint)sizeof(ctl_fan_speed_t),
                        Version = (byte)0,
                        units = ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM
                    };

                    if (IGCL.ctlFanGetState(fans[i], speed.units, &speed.speed) == ctl_result_t.CTL_RESULT_SUCCESS)
                    {
                        Console.WriteLine($"  Current RPM: {speed.speed:F0}\n");
                    }
                }
            }
        }
    }
}
