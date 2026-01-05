using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLFanHelperTests
    {
        [SkippableFact]
        public void FanGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetFanHelper(adapter);
                var fans = helper.EnumFans();
                Skip.If(fans.Count == 0, "No fans present.");
                var props = FacadeTestUtils.InvokeOrSkip(() => helper.FanGetProperties(fans[0]), "Fan properties unsupported");
                Assert.True(props.Size > 0);
                FacadeTestUtils.InvokeOrSkip(() => helper.FanGetConfig(fans[0]), "Fan config unsupported");
            }
        }
    }
}
