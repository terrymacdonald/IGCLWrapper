using System;
using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLAdapterHelperTests
    {
        [SkippableFact]
        public void GetProperties_And_Displays()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var props = adapter.GetProperties();
                Assert.True(props.Size > 0);
                var displays = adapter.GetDisplays();
                Assert.NotNull(displays);
            }
        }

        [SkippableFact]
        public void WaitForPropertyChange_ReturnsOrSkips()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var args = new ctl_wait_property_change_args_t { Size = 0, Version = 0, PropertyType = (uint)ctl_property_type_flag_t.CTL_PROPERTY_TYPE_FLAG_DISPLAY, TimeOutMilliSec = 0 };
                FacadeTestUtils.InvokeOrSkip(() => adapter.WaitForPropertyChange(args), "WaitForPropertyChange unsupported");
            }
        }
    }
}
