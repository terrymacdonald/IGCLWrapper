using System.Linq;
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
            var (api, _) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var adapters = api.EnumerateAdapters();
                var discrete = adapters.FirstOrDefault(a =>
                    (a.GetProperties().graphics_adapter_properties & (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_INTEGRATED) == 0);

                Skip.If(discrete == null, "Firmware properties require a discrete adapter.");

                var helper = api.GetFirmwareHelper(discrete);
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
