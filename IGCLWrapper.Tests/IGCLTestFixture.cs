using System;
using System.Runtime.Versioning;
using Xunit.Abstractions;

namespace IGCLWrapper.Tests
{
    /// <summary>
    /// Shared test fixture for IGCL tests
    /// Performs hardware detection, DLL check, and IGCL initialization once per test class
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class IGCLTestFixture : IDisposable
    {
        /// <summary>
        /// True if Intel GPU hardware was detected via PCI
        /// </summary>
        public bool HasIntelHardware { get; private set; }

        /// <summary>
        /// True if IGCL DLL is available in the search path
        /// </summary>
        public bool HasIGCLDll { get; private set; }

        /// <summary>
        /// The initialized IGCL API instance (null if initialization failed)
        /// </summary>
        public IGCLApi? Api { get; private set; }

        /// <summary>
        /// Enumerated GPUs from IGCL (empty if none found)
        /// </summary>
        public IntPtr[] GPUs { get; private set; }

        /// <summary>
        /// Reason why tests should be skipped (empty if tests can run)
        /// </summary>
        public string SkipReason { get; private set; }

        /// <summary>
        /// Detected Intel GPU names from PCI detection
        /// </summary>
        public string[] DetectedGPUNames { get; private set; }

        public IGCLTestFixture()
        {
            GPUs = Array.Empty<IntPtr>();
            SkipReason = string.Empty;
            DetectedGPUNames = Array.Empty<string>();

            // Step 1: Check for IGCL Hardware via PCI
            if (!HardwareDetection.HasIntelGPU(out string hwError))
            {
                HasIntelHardware = false;
                HasIGCLDll = false;
                SkipReason = hwError;
                return;
            }

            HasIntelHardware = true;
            DetectedGPUNames = HardwareDetection.GetIntelGPUNames();

            // Step 2: Check for IGCL DLL
            if (!IGCLApi.IsIGCLDllAvailable(out string dllError))
            {
                HasIGCLDll = false;
                SkipReason = dllError;
                return;
            }

            HasIGCLDll = true;

            // Step 3: Try to initialize ADLX
            try
            {
                Api = IGCLApi.Initialize();
                GPUs = Api.EnumerateAdapters();

                if (GPUs.Length == 0)
                {
                    SkipReason = "IGCL initialized but no adapters enumerated";
                }
            }
            catch (Exception ex)
            {
                SkipReason = $"IGCL initialization failed: {ex.Message}";
            }
        }

        /// <summary>
        /// True if all prerequisites are met and tests can run
        /// </summary>
        public bool CanRunTests => HasIntelHardware && HasIGCLDll && Api != null && GPUs.Length > 0;

        /// <summary>
        /// Write diagnostic information to test output
        /// </summary>
        public void WriteDiagnostics(ITestOutputHelper output)
        {
            if (!CanRunTests)
            {
                output.WriteLine($"?? Tests will be skipped: {SkipReason}");

                if (!HasIntelHardware)
                {
                    output.WriteLine("   ? No IGCL GPU hardware detected via PCI");
                }
                else if (DetectedGPUNames.Length > 0)
                {
                    output.WriteLine($"   ? IGCL GPU detected: {string.Join(", ", DetectedGPUNames)}");
                }

                if (HasIntelHardware && !HasIGCLDll)
                {
                    output.WriteLine("   ? IGCL DLL not found in search path");
                    output.WriteLine("   ? Please ensure Intel IGCL drivers are installed");
                }
            }
            else
            {
                output.WriteLine($"? Test environment ready: {GPUs.Length} GPU(s) available");
                if (DetectedGPUNames.Length > 0)
                {
                    output.WriteLine($"   Detected GPUs: {string.Join(", ", DetectedGPUNames)}");
                }
                output.WriteLine($"   IGCL Version: {IGCLApi.GetImplVersion()}");
            }
        }

        public void Dispose()
        {
            // Note: GPU adapter handles are managed by the IGCL library internally.
            // They do not require individual release calls - only the API handle cleanup via ctlClose()
            // which is handled by IGCLApi.Dispose()
            
            Api?.Dispose();
        }
    }
}
