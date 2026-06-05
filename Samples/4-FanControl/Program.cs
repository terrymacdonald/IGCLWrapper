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
                using (var igcl = IGCLApiHelper.Initialize())
                {
                    var adapters = igcl.EnumerateAdapters();

                    if (adapters.Count == 0)
                    {
                        Console.WriteLine("No Intel GPU found.");
                        return;
                    }

                    ControlFans(igcl, adapters[0]);
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

        static void ControlFans(IGCLApiHelper api, IGCLAdapterHelper adapter)
        {
            var fanHelper = api.GetFanHelper(adapter);
            var fans = fanHelper.EnumFans();
            if (fans.Count == 0)
            {
                Console.WriteLine("No controllable fans found.");
                return;
            }

            for (int i = 0; i < fans.Count; i++)
            {
                var props = fanHelper.FanGetProperties(fans[i]);
                if (!props.HasValue)
                {
                    Console.WriteLine($"Fan {i + 1}: properties not supported on this hardware.");
                    continue;
                }

                Console.WriteLine($"Fan {i + 1}:");
                Console.WriteLine($"  Max RPM    : {props.Value.MaxRpm}");
                Console.WriteLine($"  Can Control: {props.Value.CanControl}");

                var speed = fanHelper.FanGetState(fans[i], ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM);
                if (speed.HasValue)
                    Console.WriteLine($"  Current RPM: {speed.Value:F0}\n");
                else
                    Console.WriteLine($"  Current RPM: not supported\n");
            }
        }
    }
}
