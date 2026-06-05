using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
    public class IGCLFrequencyHelperTests
    {
        [SkippableFact]
        public void FrequencyGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetFrequencyHelper(adapter);
                var domains = helper.EnumFrequencyDomains();
                Skip.If(domains.Count == 0, "No frequency domains.");
                var props = helper.FrequencyGetProperties(domains[0]);
                Skip.If(!props.HasValue, "Frequency properties not supported on this hardware.");
                Assert.True(props.Value.Size > 0);
                helper.FrequencyGetRange(domains[0]);
                helper.FrequencyGetState(domains[0]);
                FacadeTestUtils.InvokeOrSkip(() => helper.FrequencyGetThrottleTime(domains[0]), "Throttle time unsupported");
                FacadeTestUtils.InvokeOrSkip(() => helper.FrequencyGetAvailableClocks(domains[0]), "Available clocks unsupported");
            }
        }

        [Fact]
        public void FrequencyRangeDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_freq_range_t
            {
                Size = 123u,
                Version = 45,
                min = 100.5,
                max = 500.7
            };

            var dto = FrequencyRangeDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.min, dto.Min);
            Assert.Equal(native.max, dto.Max);

            var roundtrip = dto.ToNative();
            Assert.True(IGCLFrequencyHelper.AreFrequencyRangeEqual(native, roundtrip));
        }

        [Fact]
        public void FrequencyStateDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_freq_state_t
            {
                Size = 456u,
                Version = 78,
                currentVoltage = 1.2,
                request = 2400.0,
                tdp = 2700.0,
                efficient = 1600.0,
                actual = 2300.0,
                throttleReasons = 0u
            };

            var dto = FrequencyStateDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.currentVoltage, dto.CurrentVoltage);
            Assert.Equal(native.request, dto.Request);
            Assert.Equal(native.tdp, dto.Tdp);
            Assert.Equal(native.efficient, dto.Efficient);
            Assert.Equal(native.actual, dto.Actual);
            Assert.Equal(native.throttleReasons, dto.ThrottleReasons);

            var roundtrip = dto.ToNative();
            Assert.True(IGCLFrequencyHelper.AreFrequencyStateEqual(native, roundtrip));
        }

        [Fact]
        public void FrequencyThrottleTimeDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_freq_throttle_time_t
            {
                Size = 789u,
                Version = 12,
                throttleTime = 999ul,
                timestamp = 555ul
            };

            var dto = FrequencyThrottleTimeDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.throttleTime, dto.ThrottleTime);
            Assert.Equal(native.timestamp, dto.Timestamp);

            var roundtrip = dto.ToNative();
            Assert.True(IGCLFrequencyHelper.AreFrequencyThrottleTimeEqual(native, roundtrip));
        }
    }
}
