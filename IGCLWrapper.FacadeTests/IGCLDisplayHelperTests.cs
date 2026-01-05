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
                FacadeTestUtils.InvokeOrSkip(() => display.GetIntelArcSyncInfoForMonitor(new ctl_intel_arc_sync_monitor_params_t { Size = 0, Version = 0 }), "ArcSync info unsupported");

                var sharpCaps = new ctl_sharpness_caps_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetSharpnessCaps(sharpCaps), "Sharpness caps unsupported");
                var sharpSettings = new ctl_sharpness_settings_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetCurrentSharpness(sharpSettings), "Sharpness settings unsupported");

                var powCaps = new ctl_power_optimization_caps_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetPowerOptimizationCaps(powCaps), "Power optimization caps unsupported");
                var powSettings = new ctl_power_optimization_settings_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetPowerOptimizationSetting(powSettings), "Power optimization settings unsupported");

                var scalingCaps = new ctl_scaling_caps_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetSupportedScalingCapability(scalingCaps), "Scaling caps unsupported");
                var scalingSettings = new ctl_scaling_settings_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetCurrentScaling(scalingSettings), "Scaling settings unsupported");

                var retroCaps = new ctl_retro_scaling_caps_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetSupportedRetroScalingCapability(retroCaps), "Retro scaling unsupported");
                var retroSettings = new ctl_retro_scaling_settings_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetSetRetroScaling(retroSettings), "Retro scaling settings unsupported");

                var brightness = new ctl_get_brightness_t { Size = 0, Version = 0 };
                FacadeTestUtils.InvokeOrSkip(() => display.GetBrightnessSetting(brightness), "Brightness unsupported");

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
