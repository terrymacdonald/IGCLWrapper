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
                Assert.True(props.Size > 0);
                FacadeTestUtils.InvokeOrSkip(() => helper.GetPowerTelemetry(), "Power telemetry unsupported");
            }
        }
    }
}
