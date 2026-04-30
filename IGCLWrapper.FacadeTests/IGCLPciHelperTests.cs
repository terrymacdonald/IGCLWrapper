using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
    public class IGCLPciHelperTests
    {
        [SkippableFact]
        public void PciGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetPciHelper(adapter);
                var props = helper.PciGetProperties();
                Assert.True(props.Size > 0);
                helper.PciGetState();
            }
        }

        [Fact]
        public void PciStateDto_ShouldRoundTripMetadata()
        {
            var nativeSpeed = new ctl_pci_speed_t
            {
                Size = 24u,
                Version = 1,
                gen = 4,
                width = 16,
                maxBandwidth = 32_000_000_000L
            };
            var native = new ctl_pci_state_t
            {
                Size = 40u,
                Version = 2,
                speed = nativeSpeed
            };

            var dto = PciStateDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.speed.gen, dto.Speed.Generation);
            Assert.Equal(native.speed.width, dto.Speed.Width);
            Assert.Equal(native.speed.maxBandwidth, dto.Speed.MaxBandwidth);

            var roundtrip = dto.ToNative();
            Assert.Equal(native.speed.gen, roundtrip.speed.gen);
            Assert.Equal(native.speed.width, roundtrip.speed.width);
            Assert.Equal(native.speed.maxBandwidth, roundtrip.speed.maxBandwidth);
        }
    }
}
