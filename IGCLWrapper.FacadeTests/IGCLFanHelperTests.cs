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
                Skip.If(!props.HasValue, "Fan properties not supported on this hardware.");
                Assert.True(props.Value.Size > 0);
                FacadeTestUtils.InvokeOrSkip(() => helper.FanGetConfig(fans[0]), "Fan config unsupported");
            }
        }

        [Fact]
        public void FanSpeedDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_fan_speed_t
            {
                Size = 123u,
                Version = 45,
                speed = 5000,
                units = ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM
            };

            var dto = FanSpeedDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.speed, dto.Speed);
            Assert.Equal(native.units, dto.Units);

            var roundtrip = dto.ToNative();
            Assert.True(IGCLFanHelper.AreFanSpeedEqual(native, roundtrip));
        }

        [Fact]
        public void FanTempSpeedDto_ShouldRoundTripMetadata()
        {
            var speed = new ctl_fan_speed_t
            {
                Size = 100u,
                Version = 1,
                speed = 3000,
                units = ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM
            };
            var native = new ctl_fan_temp_speed_t
            {
                Size = 200u,
                Version = 2,
                temperature = 50u,
                speed = speed
            };

            var dto = FanTempSpeedDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.temperature, dto.Temperature);
            Assert.Equal(native.speed.speed, dto.Speed.Speed);
            Assert.Equal(native.speed.units, dto.Speed.Units);

            var roundtrip = dto.ToNative();
            Assert.Equal(native.Size, roundtrip.Size);
            Assert.Equal(native.Version, roundtrip.Version);
            Assert.Equal(native.temperature, roundtrip.temperature);
            Assert.Equal(native.speed.speed, roundtrip.speed.speed);
        }

        [Fact]
        public void FanSpeedTableDto_ShouldRoundTripMetadata()
        {
            var point1 = new ctl_fan_temp_speed_t
            {
                Size = 100u,
                Version = 1,
                temperature = 40u,
                speed = new ctl_fan_speed_t { Size = 50u, Version = 1, speed = 2000, units = ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM }
            };
            var point2 = new ctl_fan_temp_speed_t
            {
                Size = 100u,
                Version = 1,
                temperature = 70u,
                speed = new ctl_fan_speed_t { Size = 50u, Version = 1, speed = 5000, units = ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM }
            };

            var native = new ctl_fan_speed_table_t
            {
                Size = 500u,
                Version = 2,
                numPoints = 2
            };
            // Manually set table points (simulating the fixed buffer)
            var span = System.Runtime.InteropServices.MemoryMarshal.CreateSpan(ref native.table.e0, 32);
            span[0] = point1;
            span[1] = point2;

            var dto = FanSpeedTableDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.NotNull(dto.Table);
            Assert.Equal(2, dto.Table.Count);
            Assert.Equal(40u, dto.Table[0].Temperature);
            Assert.Equal(70u, dto.Table[1].Temperature);

            var roundtrip = dto.ToNative();
            Assert.True(IGCLFanHelper.AreFanSpeedTableEqual(native, roundtrip));
        }

        [Fact]
        public void FanConfigDto_ShouldRoundTripMetadata()
        {
            var speedFixed = new ctl_fan_speed_t { Size = 50u, Version = 1, speed = 3000, units = ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM };
            var speedTable = new ctl_fan_speed_table_t { Size = 500u, Version = 2, numPoints = 0 };

            var native = new ctl_fan_config_t
            {
                Size = 600u,
                Version = 3,
                mode = ctl_fan_speed_mode_t.CTL_FAN_SPEED_MODE_FIXED,
                speedFixed = speedFixed,
                speedTable = speedTable
            };

            var dto = FanConfigDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.mode, dto.Mode);
            Assert.Equal(native.speedFixed.speed, dto.SpeedFixed.Speed);

            var roundtrip = dto.ToNative();
            Assert.True(IGCLFanHelper.AreFanConfigEqual(native, roundtrip));
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
