using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCL3DHelperTests
    {
        [SkippableFact]
        public void GetSupported3DCapabilities_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.Get3DHelper(adapter);
                var caps = FacadeTestUtils.InvokeOrSkip(() => helper.GetSupported3DCapabilities(), "3D capabilities unsupported");
                Assert.True(caps.Size > 0);
            }
        }
    }
}
