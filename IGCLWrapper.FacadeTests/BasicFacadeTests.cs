using System;
using System.Linq;
using Xunit;

using System.Runtime.Versioning;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class BasicFacadeTests
    {
        private static void SkipIfNoHardwareOrDll()
        {
            if (!IGCLApiHelper.IsIGCLDllAvailable(out var dllError))
            {
                throw new Xunit.SkipException($"IGCL DLL unavailable: {dllError}");
            }

            if (!IGCLHardwareDetection.HasIntelGPU(out var hwError))
            {
                throw new Xunit.SkipException($"Intel GPU not detected: {hwError}");
            }
        }

        [SkippableFact]
        public void EnumerateAdaptersAndDisplays_UsesFacade()
        {
            SkipIfNoHardwareOrDll();

            using var api = IGCLApiHelper.Initialize();
            var adapters = api.EnumerateAdapters();

            Skip.If(adapters.Count == 0, "No adapters returned from IGCL.");

            var firstAdapter = adapters[0];
            Assert.False(string.IsNullOrWhiteSpace(firstAdapter.Name));

            var displays = firstAdapter.EnumerateDisplayOutputs();
            Assert.NotNull(displays);
        }

        [SkippableFact]
        public void FeatureHelperFactories_ReturnHelpers()
        {
            SkipIfNoHardwareOrDll();

            using var api = IGCLApiHelper.Initialize();
            var adapter = api.EnumerateAdapters().FirstOrDefault();
            Skip.If(adapter == null, "No adapters returned from IGCL.");

            // Factories should produce helpers without throwing; deeper calls may be unsupported per device.
            Assert.NotNull(api.Get3DHelper(adapter!));
            Assert.NotNull(api.GetEccHelper(adapter!));
            Assert.NotNull(api.GetEngineHelper(adapter!));
            Assert.NotNull(api.GetFanHelper(adapter!));
            Assert.NotNull(api.GetFirmwareHelper(adapter!));
            Assert.NotNull(api.GetFrequencyHelper(adapter!));
            Assert.NotNull(api.GetLedHelper(adapter!));
            Assert.NotNull(api.GetMediaHelper(adapter!));
            Assert.NotNull(api.GetMemoryHelper(adapter!));
            Assert.NotNull(api.GetOverclockHelper(adapter!));
            Assert.NotNull(api.GetPciHelper(adapter!));
            Assert.NotNull(api.GetPowerHelper(adapter!));
            Assert.NotNull(api.GetTemperatureHelper(adapter!));
        }
    }
}
