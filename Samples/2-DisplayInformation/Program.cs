using System;
using IGCLWrapper;

namespace DisplayInformation
{
    /// <summary>
    /// Display Information sample for IGCLWrapper
    /// Demonstrates working with displays, getting properties, resolution, and refresh rate
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            PrintHeader();

            try
            {
                using (var igcl = IGCLApi.Initialize())
                {
                    Console.WriteLine("? IGCL API initialized\n");

                    var adapters = igcl.EnumerateAdapters();
                    
                    if (adapters.Length == 0)
                    {
                        Console.WriteLine("No Intel GPU adapters found.");
                        return;
                    }

                    foreach (var adapter in adapters)
                    {
                        var adapterProps = IGCLHelpers.GetProperties(adapter);
                        Console.WriteLine($"GPU: {new string(adapterProps.name).TrimEnd('\0')}\n");

                        // Enumerate displays connected to this adapter
                        var displays = igcl.EnumerateDisplays(adapter);
                        Console.WriteLine($"Found {displays.Length} connected display(s)\n");

                        if (displays.Length == 0)
                        {
                            Console.WriteLine("No displays connected to this GPU.");
                            continue;
                        }

                        for (int i = 0; i < displays.Length; i++)
                        {
                            DisplayInfo(displays[i], i + 1);
                        }
                    }

                    Console.WriteLine("\n? Sample completed successfully!");
                }
            }
            catch (IGCLException ex)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"\n? IGCL Error: {ex.Message}");
                Console.ResetColor();
            }
            catch (DllNotFoundException)
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("\n? Intel Graphics drivers not found!");
                Console.ResetColor();
            }

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        static void DisplayInfo(IntPtr display, int index)
        {
            PrintSectionHeader($"Display #{index}");

            // Use helper methods for easy access
            bool isActive = IGCLHelpers.IsActive(display);
            Console.WriteLine($"\nStatus       : {(isActive ? "Active" : "Inactive")}");

            if (isActive)
            {
                // Get resolution using helper method
                var (width, height) = IGCLHelpers.GetResolution(display);
                Console.WriteLine($"Resolution   : {width} x {height}");

                // Get refresh rate using helper method
                var refreshRate = IGCLHelpers.GetRefreshRate(display);
                Console.WriteLine($"Refresh Rate : {refreshRate:F2} Hz");

                // Get detailed properties
                var props = IGCLHelpers.GetDisplayProperties(display);
                Console.WriteLine($"Display Type : {props.Type}");

                // Get timing information
                var timing = IGCLHelpers.GetTiming(display);
                Console.WriteLine($"\nTiming Details:");
                Console.WriteLine($"  H Active   : {timing.HActive}");
                Console.WriteLine($"  H Total    : {timing.HTotal}");
                Console.WriteLine($"  V Active   : {timing.VActive}");
                Console.WriteLine($"  V Total    : {timing.VTotal}");
                Console.WriteLine($"  Pixel Clock: {timing.PixelClock / 1000.0:F2} MHz");
            }
        }

        static void PrintHeader()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("????????????????????????????????????????????????????????????????");
            Console.WriteLine("?          IGCLWrapper - Display Information Sample            ?");
            Console.WriteLine("????????????????????????????????????????????????????????????????");
            Console.ResetColor();
            Console.WriteLine();
        }

        static void PrintSectionHeader(string title)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n????????????????????????????????????????????????????????????????");
            Console.WriteLine($"? {title,-60} ?");
            Console.WriteLine($"????????????????????????????????????????????????????????????????");
            Console.ResetColor();
        }
    }
}
