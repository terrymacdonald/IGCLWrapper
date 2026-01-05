using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLMemoryHelperTests
    {
        [SkippableFact]
        public void MemoryGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetMemoryHelper(adapter);
                var modules = helper.EnumMemoryModules();
                Skip.If(modules.Count == 0, "No memory modules.");
                var props = helper.MemoryGetProperties(modules[0]);
                Assert.True(props.Size > 0);
                helper.MemoryGetState(modules[0]);
                helper.MemoryGetBandwidth(modules[0]);
            }
        }
    }
}
