using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLLedHelperTests
    {
        [SkippableFact]
        public void LedGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetLedHelper(adapter);
                var leds = helper.EnumLeds();
                Skip.If(leds.Count == 0, "No LEDs present.");
                var props = helper.LedGetProperties(leds[0]);
                Assert.True(props.Size > 0);
                FacadeTestUtils.InvokeOrSkip(() => helper.LedGetState(leds[0]), "LED state unsupported");
            }
        }
    }
}
