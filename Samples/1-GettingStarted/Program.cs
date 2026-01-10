using System;
using IGCLWrapper;

namespace GettingStarted
{
    /// <summary>
    /// Getting Started sample for IGCLWrapper
    /// Demonstrates basic API initialization, adapter enumeration, and property retrieval
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            PrintHeader();

            try
            {
                // Initialize IGCL API with automatic resource management
                Console.WriteLine("Initializing IGCL API...");
                using (var igcl = IGCLApiHelper.Initialize())
                {
                    Console.WriteLine("IGCL API initialized successfully.\n");

                    // Enumerate all Intel GPU adapters in the system
                    Console.WriteLine("Enumerating Intel GPU adapters...");
                    var adapters = igcl.EnumerateAdapters();
                    Console.WriteLine($"Found {adapters.Count} Intel GPU adapter(s)\n");

                    if (adapters.Count == 0)
                    {
                        Console.WriteLine("No Intel GPU adapters found on this system.");
                        Console.WriteLine("This sample requires an Intel GPU with IGCL support.");
                        return;
                    }

                    // Display information for each adapter
                    for (int i = 0; i < adapters.Count; i++)
                    {
                        DisplayAdapterInfo(adapters[i], i + 1);
                    }

                    Console.WriteLine("\nSample completed successfully.");
                }
            }
            catch (IGCLException ex)
            {
                // Handle IGCL-specific errors
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nIGCL Error: {ex.Message}");
                Console.WriteLine($"  Error Code: {ex.Result}");
                Console.ResetColor();
            }
            catch (DllNotFoundException)
            {
                // Handle missing Intel Graphics drivers
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\nIntel Graphics drivers not found.");
                Console.WriteLine("  Please install Intel Graphics drivers (version 25.20.100.6618 or higher)");
                Console.WriteLine("  Download from: https://www.intel.com/content/www/us/en/download-center/home.html");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                // Handle unexpected errors
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\nUnexpected Error: {ex.Message}");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Display detailed information about a GPU adapter
        /// </summary>
        static void DisplayAdapterInfo(IGCLAdapterHelper adapter, int index)
        {
            PrintSectionHeader($"Adapter #{index}");

            // Get adapter properties using the helper method
            // This is the recommended way to retrieve GPU information
            var props = adapter.GetProperties();
            var name = adapter.Name;

            // Basic GPU Information
            Console.WriteLine("\nGPU Information:");
            Console.WriteLine($"  Name         : {name}");
            Console.WriteLine($"  Vendor ID    : 0x{props.pci_vendor_id:X} (Intel)");
            Console.WriteLine($"  Device ID    : 0x{props.pci_device_id:X}");
            Console.WriteLine($"  Revision     : {props.rev_id}");
            Console.WriteLine($"  Driver Ver   : {FormatDriverVersion(props.driver_version)}");

            // GPU Architecture Details
            Console.WriteLine("\nArchitecture:");
            Console.WriteLine($"  Slices       : {props.num_slices}");
            Console.WriteLine($"  Sub-slices   : {props.num_sub_slices_per_slice} per slice");
            Console.WriteLine($"  EUs per SS   : {props.num_eus_per_sub_slice}");
            
            uint totalSubSlices = props.num_slices * props.num_sub_slices_per_slice;
            uint totalEUs = totalSubSlices * props.num_eus_per_sub_slice;
            Console.WriteLine($"  Total EUs    : {totalEUs}");

            // Memory Information (if available)
            if (props.Frequency > 0)
            {
                Console.WriteLine("\nFrequency:");
                Console.WriteLine($"  Base Clock   : {props.Frequency} MHz");
            }
        }

        /// <summary>
        /// Format driver version into readable string
        /// </summary>
        static string FormatDriverVersion(ulong version)
        {
            // Driver version is typically stored as a 64-bit value
            // Format varies by vendor, this is a common interpretation
            uint major = (uint)((version >> 48) & 0xFFFF);
            uint minor = (uint)((version >> 32) & 0xFFFF);
            uint build = (uint)((version >> 16) & 0xFFFF);
            uint revision = (uint)(version & 0xFFFF);

            return $"{major}.{minor}.{build}.{revision}";
        }

        /// <summary>
        /// Print a formatted header for the sample
        /// </summary>
        static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("------------------------------------------------------------");
            Console.WriteLine("  IGCLWrapper - Getting Started Sample");
            Console.WriteLine("------------------------------------------------------------");
            Console.ResetColor();
            Console.WriteLine();
        }

        /// <summary>
        /// Print a formatted section header
        /// </summary>
        static void PrintSectionHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n------------------------------------------------------------");
            Console.WriteLine($"  {title}");
            Console.WriteLine($"------------------------------------------------------------");
            Console.ResetColor();
        }
    }
}
