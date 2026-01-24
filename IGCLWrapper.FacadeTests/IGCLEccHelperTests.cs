using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
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
                if (props.Size == 0) throw new SkipException("ECC unsupported (empty props).");
                FacadeTestUtils.InvokeOrSkip(() => helper.EccGetState(), "ECC state unsupported");
            }
        }
    }
}
