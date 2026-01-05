using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
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
    }
}
