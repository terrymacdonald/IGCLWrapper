using System;
using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLAdapterHelperTests
    {
        [SkippableFact]
        public void GetProperties_And_Displays()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var props = adapter.GetProperties();
                Assert.True(props.Size > 0);
                var displays = adapter.GetDisplays();
                Assert.NotNull(displays);
            }
        }
    }
}
