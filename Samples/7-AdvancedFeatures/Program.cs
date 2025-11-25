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
            Console.WriteLine("? WARNING: This sample demonstrates advanced/expert-level APIs");
            Console.WriteLine("? Some features may modify GPU settings - use with caution!");
            Console.ResetColor();
            Console.WriteLine();

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

                    CheckOverclockingSupport(adapters[0]);
                    Check3DCapabilities(adapters[0]);
                    CheckVideoProcessing(adapters[0]);
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

        static unsafe void CheckOverclockingSupport(IntPtr adapter)
        {
            Console.WriteLine("Overclocking Support:");
            
            var props = new _ctl_oc_properties_t
            {
                Size = (uint)sizeof(_ctl_oc_properties_t),
                Version = 0
            };

            var result = IGCL.ctlOverclockGetProperties((_ctl_device_adapter_handle_t*)adapter, &props);

            if (result == _ctl_result_t.CTL_RESULT_SUCCESS)
            {
                Console.WriteLine($"  Supported        : Yes");
                Console.WriteLine($"  GPU OC Supported : {props.bSupported != 0}");
                Console.WriteLine($"  VRAM OC Supported: {props.bVRAMOverclockSupported != 0}");
            }
            else
            {
                Console.WriteLine($"  Supported        : No ({result})");
            }
            Console.WriteLine();
        }

        static unsafe void Check3DCapabilities(IntPtr adapter)
        {
            Console.WriteLine("3D Graphics Capabilities:");

            var caps = new _ctl_3d_feature_caps_t
            {
                Size = (uint)sizeof(_ctl_3d_feature_caps_t),
                Version = 0,
                NumSupportedFeatures = 0,
                pFeatureDetails = null
            };

            var result = IGCL.ctlGetSupported3DCapabilities((_ctl_device_adapter_handle_t*)adapter, &caps);

            if (result == _ctl_result_t.CTL_RESULT_SUCCESS)
            {
                Console.WriteLine($"  Supported Features: {caps.NumSupportedFeatures}");
            }
            else
            {
                Console.WriteLine($"  Not available ({result})");
            }
            Console.WriteLine();
        }

        static unsafe void CheckVideoProcessing(IntPtr adapter)
        {
            Console.WriteLine("Video Processing Capabilities:");

            var caps = new _ctl_video_processing_feature_caps_t
            {
                Size = (uint)sizeof(_ctl_video_processing_feature_caps_t),
                Version = 0,
                NumSupportedFeatures = 0,
                pFeatureDetails = null
            };

            var result = IGCL.ctlGetSupportedVideoProcessingCapabilities((_ctl_device_adapter_handle_t*)adapter, &caps);

            if (result == _ctl_result_t.CTL_RESULT_SUCCESS)
            {
                Console.WriteLine($"  Supported Features: {caps.NumSupportedFeatures}");
            }
            else
            {
                Console.WriteLine($"  Not available ({result})");
            }
            Console.WriteLine();
        }
    }
}
