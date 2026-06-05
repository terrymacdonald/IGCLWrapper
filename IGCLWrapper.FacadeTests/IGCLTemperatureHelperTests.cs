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
                Skip.If(!props.HasValue, "Temperature properties not supported on this hardware.");
                Assert.True(props.Value.Size > 0);
                helper.TemperatureGetState(sensors[0]);
            }
        }

        [Fact]
        public void TemperaturePropertiesDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_temp_properties_t
            {
                Size = 32u,
                Version = 1,
                type = ctl_temp_sensors_t.CTL_TEMP_SENSORS_GPU,
                maxTemperature = 100.0
            };

            var dto = TemperaturePropertiesDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.type, dto.Type);
            Assert.Equal(native.maxTemperature, dto.MaxTemperature);

            var roundtrip = dto.ToNative();
            Assert.Equal(native.type, roundtrip.type);
            Assert.Equal(native.maxTemperature, roundtrip.maxTemperature);
        }
    }
}
