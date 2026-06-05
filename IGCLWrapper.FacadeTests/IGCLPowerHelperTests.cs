using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
    public class IGCLPowerHelperTests
    {
        [SkippableFact]
        public void PowerGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetPowerHelper(adapter);
                var domains = helper.EnumPowerDomains();
                Skip.If(domains.Count == 0, "No power domains.");
                var props = helper.PowerGetProperties(domains[0]);
                Skip.If(!props.HasValue, "Power properties not supported on this hardware.");
                Assert.True(props.Value.Size > 0);
                var energy = helper.PowerGetEnergyCounter(domains[0]);
                Skip.If(!energy.HasValue, "Power energy counter not supported on this hardware.");
                Assert.True(energy.Value.Timestamp >= 0);
                helper.PowerGetLimits(domains[0]);
            }
        }

        [Fact]
        public void PowerEnergyCounterDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_power_energy_counter_t
            {
                Size = 123u,
                Version = 45,
                energy = 999ul,
                timestamp = 777ul
            };

            var dto = PowerEnergyCounterDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.energy, dto.Energy);
            Assert.Equal(native.timestamp, dto.Timestamp);

            var roundtrip = dto.ToNative();
            Assert.True(IGCLPowerHelper.ArePowerEnergyCounterEqual(native, roundtrip));
        }
    }
}
