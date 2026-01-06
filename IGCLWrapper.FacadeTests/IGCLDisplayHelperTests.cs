using System.Linq;
using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLDisplayHelperTests
    {
        [SkippableFact]
        public void GetDisplayProperties_WhenPresent()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var display = adapter.GetDisplays().FirstOrDefault();
                Skip.If(display == null, "No displays connected.");
                var props = display!.GetProperties();
                Assert.True(props.Size > 0);
                var deviceProps = display.GetDeviceProperties();
                Assert.True(deviceProps.Size > 0);
            }
        }

        [SkippableFact]
        public void AdditionalDisplayGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var display = adapter.GetDisplays().FirstOrDefault();
                Skip.If(display == null, "No displays connected.");

                FacadeTestUtils.InvokeOrSkip(() => display.GetAdapterDisplayEncoderProperties(), "Encoder properties unsupported");
                FacadeTestUtils.InvokeOrSkip(() => display.GetIntelArcSyncInfoForMonitor(), "ArcSync info unsupported");

                var sharpCaps = new ctl_sharpness_caps_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetSharpnessCaps(sharpCaps), "Sharpness caps unsupported");
                FacadeTestUtils.InvokeOrSkip(() => display.GetCurrentSharpness(), "Sharpness settings unsupported");

                FacadeTestUtils.InvokeOrSkip(() => display.GetPowerOptimizationCaps(), "Power optimization caps unsupported");
                FacadeTestUtils.InvokeOrSkip(() => display.GetPowerOptimizationSetting(), "Power optimization settings unsupported");

                FacadeTestUtils.InvokeOrSkip(() => display.GetSupportedScalingCapability(), "Scaling caps unsupported");
                FacadeTestUtils.InvokeOrSkip(() => display.GetCurrentScaling(), "Scaling settings unsupported");

                FacadeTestUtils.InvokeOrSkip(() => display.GetSupportedRetroScalingCapability(), "Retro scaling unsupported");
                var retroSettings = new ctl_retro_scaling_settings_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetSetRetroScaling(retroSettings), "Retro scaling settings unsupported");

                FacadeTestUtils.InvokeOrSkip(() => display.GetBrightnessSetting(), "Brightness unsupported");

                var wireFormat = new ctl_get_set_wire_format_config_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetSetWireFormat(wireFormat), "Wire format unsupported");

                var displaySettings = new ctl_display_settings_t { Size = 0, Version = 0, Set = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetSetDisplaySettings(displaySettings), "Display settings unsupported");

                var vblank = new ctl_vblank_ts_args_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetVblankTimestamp(vblank), "Vblank unsupported");

                var muxes = display.EnumerateMuxDevices();
                if (muxes.Length > 0)
                {
                    FacadeTestUtils.InvokeOrSkip(() => display.GetMuxProperties(muxes[0]), "Mux properties unsupported");
                }
            }
        }
    }
}
