using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLFrequencyHelperTests
    {
        [SkippableFact]
        public void FrequencyGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetFrequencyHelper(adapter);
                var domains = helper.EnumFrequencyDomains();
                Skip.If(domains.Count == 0, "No frequency domains.");
                var props = helper.FrequencyGetProperties(domains[0]);
                Assert.True(props.Size > 0);
                helper.FrequencyGetRange(domains[0]);
                helper.FrequencyGetState(domains[0]);
                FacadeTestUtils.InvokeOrSkip(() => helper.FrequencyGetThrottleTime(domains[0]), "Throttle time unsupported");
                FacadeTestUtils.InvokeOrSkip(() => helper.FrequencyGetAvailableClocks(domains[0]), "Available clocks unsupported");
            }
        }
    }
}
