using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLFirmwareHelperTests
    {
        [SkippableFact]
        public void FirmwareGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetFirmwareHelper(adapter);
                var props = helper.GetFirmwareProperties();
                Assert.True(props.Size > 0);
                var components = helper.EnumerateFirmwareComponents();
                if (components.Count > 0)
                {
                    FacadeTestUtils.InvokeOrSkip(() => helper.GetFirmwareComponentProperties(components[0]), "Firmware component properties unsupported");
                }
            }
        }
    }
}
