using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
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
                Skip.If(!props.HasValue, "Engine properties not supported on this hardware.");
                Assert.True(props.Value.Size > 0);
                helper.EngineGetActivity(engines[0]);
            }
        }

        [Fact]
        public void EnginePropertiesDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_engine_properties_t
            {
                Size = 24u,
                Version = 1,
                type = ctl_engine_group_t.CTL_ENGINE_GROUP_RENDER
            };

            var dto = EnginePropertiesDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.type, dto.Type);

            var roundtrip = dto.ToNative();
            Assert.Equal(native.type, roundtrip.type);
        }

        [Fact]
        public void EngineStatsDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_engine_stats_t
            {
                Size = 32u,
                Version = 2,
                activeTime = 12345678UL,
                timestamp = 98765432UL
            };

            var dto = EngineStatsDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.activeTime, dto.ActiveTime);
            Assert.Equal(native.timestamp, dto.Timestamp);

            var roundtrip = dto.ToNative();
            Assert.Equal(native.activeTime, roundtrip.activeTime);
            Assert.Equal(native.timestamp, roundtrip.timestamp);
        }
    }
}
