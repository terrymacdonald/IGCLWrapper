using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLMediaHelperTests
    {
        [SkippableFact]
        public void MediaGetCapabilities_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetMediaHelper(adapter);
                var caps = helper.GetSupportedVideoProcessingCapabilities();
                Assert.True(caps.Size > 0);
            }
        }
    }
}
