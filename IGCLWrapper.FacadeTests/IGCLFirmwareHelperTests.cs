using System.Linq;
using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
    public class IGCLFirmwareHelperTests
    {
        [SkippableFact]
        public void FirmwareGetters_ShouldSucceedOrSkip()
        {
            var (api, _) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var adapters = api.EnumerateAdapters();
                var discrete = adapters.FirstOrDefault(a => !a.GetProperties().IsIntegratedGraphicsAdapter);

                Skip.If(discrete == null, "Firmware properties require a discrete adapter.");

                var helper = api.GetFirmwareHelper(discrete);
                var props = FacadeTestUtils.InvokeOrSkip(() => helper.GetFirmwareProperties(), "Firmware properties unsupported");
                if (!props.HasValue || props.Value.Size == 0) throw new SkipException("Firmware properties unsupported (empty).");
                var components = helper.EnumerateFirmwareComponents();
                if (components.Count > 0)
                {
                    FacadeTestUtils.InvokeOrSkip(() => helper.GetFirmwareComponentProperties(components[0]), "Firmware component properties unsupported");
                }
            }
        }

        [SkippableFact]
        public void GetFirmwarePropertiesDto_ShouldBeSafeToConsume()
        {
            var (api, _) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var adapters = api.EnumerateAdapters();
                var discrete = adapters.FirstOrDefault(a => !a.GetProperties().IsIntegratedGraphicsAdapter);

                Skip.If(discrete == null, "Firmware properties require a discrete adapter.");

                var helper = api.GetFirmwareHelper(discrete);
                var props = FacadeTestUtils.InvokeOrSkip(() => helper.GetFirmwareProperties(), "Firmware properties unsupported");
                Skip.If(!props.HasValue, "Firmware properties not supported on this hardware.");
                Assert.True(props.Value.Size > 0);
                Assert.NotNull(props.Value.Name);
                Assert.NotNull(props.Value.FirmwareVersion);
                Assert.NotNull(props.Value.Reserved);
                Assert.Equal(16, props.Value.Reserved!.Count);
                Assert.True(props.Value.Equals(props.Value));
                _ = props.Value.GetHashCode();
            }
        }

        [SkippableFact]
        public void GetFirmwareComponentPropertiesDto_ShouldBeSafeToConsume()
        {
            var (api, _) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var adapters = api.EnumerateAdapters();
                var discrete = adapters.FirstOrDefault(a => !a.GetProperties().IsIntegratedGraphicsAdapter);

                Skip.If(discrete == null, "Firmware component properties require a discrete adapter.");

                var helper = api.GetFirmwareHelper(discrete);
                var components = helper.EnumerateFirmwareComponents();
                Skip.If(components.Count == 0, "No firmware components reported.");

                var props = FacadeTestUtils.InvokeOrSkip(() => helper.GetFirmwareComponentProperties(components[0]), "Firmware component properties unsupported");
                Skip.If(!props.HasValue, "Firmware component properties not supported on this hardware.");
                Assert.True(props.Value.Size > 0);
                Assert.NotNull(props.Value.Name);
                Assert.NotNull(props.Value.ComponentVersion);
                Assert.NotNull(props.Value.Reserved);
                Assert.Equal(20, props.Value.Reserved!.Count);
                Assert.True(props.Value.Equals(props.Value));
                _ = props.Value.GetHashCode();
            }
        }
    }
}
