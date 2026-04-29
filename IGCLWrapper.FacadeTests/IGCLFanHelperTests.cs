using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
    public class IGCLFanHelperTests
    {
        [SkippableFact]
        public void FanGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetFanHelper(adapter);
                var fans = helper.EnumFans();
                Skip.If(fans.Count == 0, "No fans present.");
                var props = FacadeTestUtils.InvokeOrSkip(() => helper.FanGetProperties(fans[0]), "Fan properties unsupported");
                Assert.True(props.Size > 0);
                FacadeTestUtils.InvokeOrSkip(() => helper.FanGetConfig(fans[0]), "Fan config unsupported");
            }
        }

        [Fact]
        public void FanPropertiesDto_FlagBooleans_ShouldTrackRawMasks()
        {
            var dto = new FanPropertiesDto
            {
                SupportedModes = (1u << (int)ctl_fan_speed_mode_t.CTL_FAN_SPEED_MODE_DEFAULT) |
                                 (1u << (int)ctl_fan_speed_mode_t.CTL_FAN_SPEED_MODE_FIXED),
                SupportedUnits = (1u << (int)ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM)
            };

            Assert.True(dto.SupportsDefaultMode);
            Assert.True(dto.SupportsFixedMode);
            Assert.False(dto.SupportsTableMode);
            Assert.True(dto.SupportsRpmUnits);
            Assert.False(dto.SupportsPercentUnits);

            dto.SupportsTableMode = true;
            dto.SupportsPercentUnits = true;

            Assert.True((dto.SupportedModes & (1u << (int)ctl_fan_speed_mode_t.CTL_FAN_SPEED_MODE_TABLE)) != 0);
            Assert.True((dto.SupportedUnits & (1u << (int)ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_PERCENT)) != 0);
        }
    }
}
