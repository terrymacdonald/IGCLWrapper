using System;
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
                var display = adapter.EnumerateDisplayOutputs().FirstOrDefault();
                Skip.If(display == null, "No displays connected.");
                var props = display!.GetProperties();
                Assert.True(props.Size > 0);
                var deviceProps = adapter.GetDeviceProperties();
                Assert.True(deviceProps.Size > 0);
            }
        }

        [SkippableFact]
        public void AdditionalDisplayGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var display = adapter.EnumerateDisplayOutputs().FirstOrDefault();
                Skip.If(display == null, "No displays connected.");

                FacadeTestUtils.InvokeOrSkip(() => display.GetAdapterDisplayEncoderProperties(), "Encoder properties unsupported");
                FacadeTestUtils.InvokeOrSkip(() => display.GetIntelArcSyncInfoForMonitor(), "ArcSync info unsupported");

                FacadeTestUtils.InvokeOrSkip(() => display.GetSharpnessCaps(), "Sharpness caps unsupported");
                FacadeTestUtils.InvokeOrSkip(() => display.GetCurrentSharpness(), "Sharpness settings unsupported");

                var powerCaps = FacadeTestUtils.InvokeOrSkip(() => display.GetPowerOptimizationCaps(), "Power optimization caps unsupported");
                if (powerCaps.SupportedFeatures != 0)
                {
                    var settings = new PowerOptimizationSettingsDto();
                    if ((powerCaps.SupportedFeatures & (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LRR) != 0)
                    {
                        settings.PowerOptimizationFeature = (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LRR;
                    }
                    else if ((powerCaps.SupportedFeatures & (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_FBC) != 0)
                    {
                        settings.PowerOptimizationFeature = (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_FBC;
                    }
                    else if ((powerCaps.SupportedFeatures & (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_PSR) != 0)
                    {
                        settings.PowerOptimizationFeature = (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_PSR;
                        settings.PowerSource = ctl_power_source_t.CTL_POWER_SOURCE_DC;
                        settings.PowerOptimizationPlan = ctl_power_optimization_plan_t.CTL_POWER_OPTIMIZATION_PLAN_BALANCED;
                    }
                    else if ((powerCaps.SupportedFeatures & (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_DPST) != 0)
                    {
                        settings.PowerOptimizationFeature = (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_DPST;
                        settings.PowerSource = ctl_power_source_t.CTL_POWER_SOURCE_DC;
                        settings.PowerOptimizationPlan = ctl_power_optimization_plan_t.CTL_POWER_OPTIMIZATION_PLAN_BALANCED;
                    }
                    else
                    {
                        throw new SkipException("Power optimization settings unsupported (no supported features).");
                    }

                    FacadeTestUtils.InvokeOrSkip(() => display.GetPowerOptimizationSetting(settings), "Power optimization settings unsupported");
                }
                else
                {
                    throw new SkipException("Power optimization settings unsupported (no supported features).");
                }

                FacadeTestUtils.InvokeOrSkip(() => display.GetSupportedScalingCapability(), "Scaling caps unsupported");
                FacadeTestUtils.InvokeOrSkip(() => display.GetCurrentScaling(), "Scaling settings unsupported");

                FacadeTestUtils.InvokeOrSkip(() => display.GetSupportedRetroScalingCapability(), "Retro scaling unsupported");
                FacadeTestUtils.InvokeOrSkip(() => display.GetRetroScalingSettings(), "Retro scaling settings unsupported");

                var encoderProps = FacadeTestUtils.InvokeOrSkip(() => display.GetAdapterDisplayEncoderProperties(), "Encoder properties unsupported");
                var isCompanion = (encoderProps.EncoderConfigFlags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COMPANION_DISPLAY) != 0;
                if (isCompanion)
                {
                    try
                    {
                        FacadeTestUtils.InvokeOrSkip(() => display.GetBrightnessSetting(), "Brightness unsupported");
                    }
                    catch (IGCLException ex) when (ex.Result == ctl_result_t.CTL_RESULT_ERROR_DISPLAY_NOT_ACTIVE ||
                                                   ex.Result == ctl_result_t.CTL_RESULT_ERROR_INVALID_OPERATION_TYPE)
                    {
                        throw new SkipException($"Brightness unsupported: {ex.Result}");
                    }
                }
                else
                {
                    throw new SkipException("Brightness unsupported: display is not a companion display.");
                }

                FacadeTestUtils.InvokeOrSkip(() => display.GetCustomModes(), "Custom mode unsupported");

                FacadeTestUtils.InvokeOrSkip(() => adapter.GetLinkedDisplayAdapters(), "Linked adapters unsupported");

                var pixtxQuery = IGCLDisplayHelper.CreatePixtxPipeGetConfig();
                pixtxQuery.QueryType = ctl_pixtx_config_query_type_t.CTL_PIXTX_CONFIG_QUERY_TYPE_CAPABILITY;
                FacadeTestUtils.InvokeOrSkip(() => display.PixelTransformationGetConfig(pixtxQuery), "Pixtx config unsupported");

                FacadeTestUtils.InvokeOrSkip(() => display.GetWireFormat(), "Wire format unsupported");

                FacadeTestUtils.InvokeOrSkip(() => display.GetDisplaySettings(), "Display settings unsupported");

                FacadeTestUtils.InvokeOrSkip(() => display.GetDynamicContrastEnhancement(), "DCE unsupported");

                FacadeTestUtils.InvokeOrSkip(() => display.GetVblankTimestamp(), "Vblank unsupported");

                var muxes = display.EnumerateMuxDevices();
                if (muxes.Length > 0)
                {
                    FacadeTestUtils.InvokeOrSkip(() => display.GetMuxProperties(muxes[0]), "Mux properties unsupported");
                }
            }
        }

        [SkippableFact]
        public void GetEdid_ShouldReturnBytesOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var display = adapter.EnumerateDisplayOutputs().FirstOrDefault();
                Skip.If(display == null, "No displays connected.");

                try
                {
                    var edid = display.GetEdid();
                    if (edid.Length == 0)
                        throw new SkipException("EDID not available.");

                    var (edidWithFlags, _) = display.GetEdidWithFlags();
                    Assert.True(edidWithFlags.Length > 0);
                }
                catch (IGCLException ex) when (ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                                               ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION ||
                                               ex.Result == ctl_result_t.CTL_RESULT_ERROR_DISPLAY_NOT_ATTACHED ||
                                               ex.Result == ctl_result_t.CTL_RESULT_ERROR_DATA_NOT_FOUND ||
                                               ex.Result == ctl_result_t.CTL_RESULT_ERROR_KMD_CALL)
                {
                    throw new SkipException($"EDID read unsupported: {ex.Result}");
                }
            }
        }        
        
    }
}
