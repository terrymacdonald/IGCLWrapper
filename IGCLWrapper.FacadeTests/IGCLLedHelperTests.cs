using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
    public class IGCLLedHelperTests
    {
        [SkippableFact]
        public void LedGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetLedHelper(adapter);
                var leds = FacadeTestUtils.InvokeOrSkip(() => helper.EnumLeds(), "LED enumeration unsupported");
                Skip.If(leds.Count == 0, "No LEDs present.");
                var props = FacadeTestUtils.InvokeOrSkip(() => helper.LedGetProperties(leds[0]), "LED properties unsupported");
                if (props.Size == 0) throw new SkipException("LED properties unsupported (empty).");
                FacadeTestUtils.InvokeOrSkip(() => helper.LedGetState(leds[0]), "LED state unsupported");
            }
        }
    }
}
