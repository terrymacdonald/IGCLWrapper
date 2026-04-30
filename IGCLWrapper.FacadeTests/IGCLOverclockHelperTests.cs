using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
    public class IGCLOverclockHelperTests
    {
        [SkippableFact]
        public void OverclockGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetOverclockHelper(adapter);
                var props = helper.GetProperties();
                Skip.If(props.Size == 0, "Overclock unsupported.");
                FacadeTestUtils.InvokeOrSkip(() => helper.GetPowerTelemetry(), "Power telemetry unsupported");

                FacadeTestUtils.InvokeOrSkip(() => helper.OverclockGpuFrequencyOffsetGet(), "GPU freq offset unsupported");
                FacadeTestUtils.InvokeOrSkip(() => helper.OverclockGpuMaxVoltageOffsetGetV2(), "GPU voltage offset unsupported");
                FacadeTestUtils.InvokeOrSkip(() => helper.OverclockPowerLimitGetV2(), "Power limit unsupported");
                FacadeTestUtils.InvokeOrSkip(() => helper.OverclockTemperatureLimitGetV2(), "Temp limit unsupported");
            }
        }

        [Fact]
        public void OcVfPairDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_oc_vf_pair_t
            {
                Size = 24u,
                Version = 1,
                Voltage = 1050.5,
                Frequency = 1800.0
            };

            var dto = OcVfPairDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.Voltage, dto.Voltage);
            Assert.Equal(native.Frequency, dto.Frequency);

            var roundtrip = dto.ToNative();
            Assert.Equal(native.Voltage, roundtrip.Voltage);
            Assert.Equal(native.Frequency, roundtrip.Frequency);
        }
    }
}
