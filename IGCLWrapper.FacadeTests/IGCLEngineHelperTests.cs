using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLEngineHelperTests
    {
        [SkippableFact]
        public void EngineGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetEngineHelper(adapter);
                var engines = helper.EnumEngineGroups();
                Skip.If(engines.Count == 0, "No engine groups.");
                var props = helper.EngineGetProperties(engines[0]);
                Assert.True(props.Size > 0);
                helper.EngineGetActivity(engines[0]);
            }
        }
    }
}
