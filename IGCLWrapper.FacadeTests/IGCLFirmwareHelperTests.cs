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
                var props = FacadeTestUtils.InvokeOrSkip(() => helper.GetFirmwareProperties(), "Firmware properties unsupported");
                if (props.Size == 0) throw new SkipException("Firmware properties unsupported (empty).");
                var components = helper.EnumerateFirmwareComponents();
                if (components.Count > 0)
                {
                    FacadeTestUtils.InvokeOrSkip(() => helper.GetFirmwareComponentProperties(components[0]), "Firmware component properties unsupported");
                }
            }
        }
    }
}
