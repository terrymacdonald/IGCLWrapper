using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLPciHelperTests
    {
        [SkippableFact]
        public void PciGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetPciHelper(adapter);
                var props = helper.PciGetProperties();
                Assert.True(props.Size > 0);
                helper.PciGetState();
            }
        }
    }
}
