using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
    public class IGCLTemperatureHelperTests
    {
        [SkippableFact]
        public void TemperatureGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetTemperatureHelper(adapter);
                var sensors = helper.EnumTemperatureSensors();
                Skip.If(sensors.Count == 0, "No temperature sensors.");
                var props = helper.TemperatureGetProperties(sensors[0]);
                Assert.True(props.Size > 0);
                helper.TemperatureGetState(sensors[0]);
            }
        }
    }
}
