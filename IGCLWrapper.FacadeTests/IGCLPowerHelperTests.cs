using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLPowerHelperTests
    {
        [SkippableFact]
        public void PowerGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetPowerHelper(adapter);
                var domains = helper.EnumPowerDomains();
                Skip.If(domains.Count == 0, "No power domains.");
                var props = helper.PowerGetProperties(domains[0]);
                Assert.True(props.Size > 0);
                var energy = helper.PowerGetEnergyCounter(domains[0]);
                Assert.True(energy.timestamp >= 0);
                helper.PowerGetLimits(domains[0]);
            }
        }
    }
}
