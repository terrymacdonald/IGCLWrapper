using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

using System.Runtime.Versioning;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
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

        [Fact]
        public unsafe void RuntimePathArgsDto_ShouldRoundTripNative()
        {
            var dto = IGCLApiHelper.CreateRuntimePathRequest(@"C:\Temp\IGCL");
            dto.UnlockID = new ApplicationIdDto
            {
                Data1 = 1,
                Data2 = 2,
                Data3 = 3,
                Data4 = new List<byte> { 4, 5, 6, 7, 8, 9, 10, 11 }
            };
            dto.DeviceID = 0x1234;
            dto.RevID = 5;

            IGCLApiHelper.ValidateSetRuntimePathRequest(dto);

            var native = dto.ToNative();
            Assert.Equal((uint)sizeof(ctl_runtime_path_args_t), native.Size);
            Assert.True(native.pRuntimePath == null);

            unsafe
            {
                fixed (char* pRuntimePath = dto.RuntimePath)
                {
                    native.pRuntimePath = (ushort*)pRuntimePath;
                    var roundTrip = RuntimePathArgsDto.FromNative(native);

                    Assert.Equal(dto.RuntimePath, roundTrip.RuntimePath);
                    Assert.Equal(dto.DeviceID, roundTrip.DeviceID);
                    Assert.Equal(dto.RevID, roundTrip.RevID);
                    Assert.Equal(dto.UnlockID, roundTrip.UnlockID);
                }
            }
        }

        [Fact]
        public void RuntimePathArgsDto_Validate_ShouldRejectMissingPath()
        {
            var dto = new RuntimePathArgsDto();
            Assert.Throws<ArgumentException>(() => IGCLApiHelper.ValidateSetRuntimePathRequest(dto));
        }
    }
}
