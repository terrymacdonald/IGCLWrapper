using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
    public class IGCLMemoryHelperTests
    {
        [SkippableFact]
        public void MemoryGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetMemoryHelper(adapter);
                var modules = helper.EnumMemoryModules();
                Skip.If(modules.Count == 0, "No memory modules.");
                var props = helper.MemoryGetProperties(modules[0]);
                Assert.True(props.Size > 0);
                helper.MemoryGetState(modules[0]);
                FacadeTestUtils.InvokeOrSkip(() => helper.MemoryGetBandwidth(modules[0]), "Memory bandwidth unsupported");
            }
        }

        [Fact]
        public void MemoryPropertiesDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_mem_properties_t
            {
                Size = 64u,
                Version = 1,
                type = ctl_mem_type_t.CTL_MEM_TYPE_DDR4,
                location = ctl_mem_loc_t.CTL_MEM_LOC_DEVICE,
                physicalSize = 8_000_000_000UL,
                busWidth = 256,
                numChannels = 4
            };

            var dto = MemoryPropertiesDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.type, dto.Type);
            Assert.Equal(native.location, dto.Location);
            Assert.Equal(native.physicalSize, dto.PhysicalSize);
            Assert.Equal(native.busWidth, dto.BusWidth);
            Assert.Equal(native.numChannels, dto.NumChannels);

            var roundtrip = dto.ToNative();
            Assert.Equal(native.type, roundtrip.type);
            Assert.Equal(native.location, roundtrip.location);
            Assert.Equal(native.physicalSize, roundtrip.physicalSize);
            Assert.Equal(native.busWidth, roundtrip.busWidth);
            Assert.Equal(native.numChannels, roundtrip.numChannels);
        }

        [Fact]
        public void MemoryStateDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_mem_state_t
            {
                Size = 32u,
                Version = 2,
                free = 4_000_000_000UL,
                size = 8_000_000_000UL
            };

            var dto = MemoryStateDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.free, dto.Free);
            Assert.Equal(native.size, dto.TotalSize);

            var roundtrip = dto.ToNative();
            Assert.Equal(native.free, roundtrip.free);
            Assert.Equal(native.size, roundtrip.size);
        }

        [Fact]
        public void MemoryBandwidthDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_mem_bandwidth_t
            {
                Size = 48u,
                Version = 3,
                maxBandwidth = 500_000_000_000UL,
                timestamp = 123456789UL,
                readCounter = 1_000_000UL,
                writeCounter = 500_000UL
            };

            var dto = MemoryBandwidthDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.maxBandwidth, dto.MaxBandwidth);
            Assert.Equal(native.timestamp, dto.Timestamp);
            Assert.Equal(native.readCounter, dto.ReadCounter);
            Assert.Equal(native.writeCounter, dto.WriteCounter);

            var roundtrip = dto.ToNative();
            Assert.Equal(native.maxBandwidth, roundtrip.maxBandwidth);
            Assert.Equal(native.timestamp, roundtrip.timestamp);
            Assert.Equal(native.readCounter, roundtrip.readCounter);
            Assert.Equal(native.writeCounter, roundtrip.writeCounter);
        }
    }
}
