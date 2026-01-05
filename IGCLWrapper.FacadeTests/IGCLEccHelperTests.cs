using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLEccHelperTests
    {
        [SkippableFact]
        public void EccGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetEccHelper(adapter);
                var props = FacadeTestUtils.InvokeOrSkip(() => helper.EccGetProperties(), "ECC unsupported");
                Assert.True(props.Size > 0);
                FacadeTestUtils.InvokeOrSkip(() => helper.EccGetState(), "ECC state unsupported");
            }
        }
    }
}
