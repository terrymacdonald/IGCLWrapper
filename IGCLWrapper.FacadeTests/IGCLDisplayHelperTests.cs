using System;
using System.Linq;
using System.Runtime.Versioning;
using EDIDParser;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
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
        public void GetDisplayPropertiesDto_ShouldBeSafeToConsume()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var display = adapter.EnumerateDisplayOutputs().FirstOrDefault();
                Skip.If(display == null, "No displays connected.");

                var props = display!.GetProperties();
                Assert.True(props.Size > 0);
                Assert.NotNull(props.ReservedFields);
                Assert.Equal(16, props.ReservedFields!.Count);
                Assert.True(props.Equals(props));
                _ = props.GetHashCode();
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
        public void GetWireFormatDto_ShouldBeSafeToConsume()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var display = adapter.EnumerateDisplayOutputs().FirstOrDefault();
                Skip.If(display == null, "No displays connected.");

                var wireFormat = FacadeTestUtils.InvokeOrSkip(() => display!.GetWireFormat(), "Wire format unsupported");
                Assert.True(wireFormat.Size > 0);
                Assert.NotNull(wireFormat.SupportedWireFormat);
                Assert.Equal(4, wireFormat.SupportedWireFormat!.Count);
                Assert.True(wireFormat.Equals(wireFormat));
                _ = wireFormat.GetHashCode();
            }
        }

        [SkippableFact]
        public void GetEdidManagement_ShouldReturnBytesOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var display = adapter.EnumerateDisplayOutputs().FirstOrDefault();
                Skip.If(display == null, "No displays connected.");

                try
                {
                    var edid = display.GetEdidManagement();
                    if (edid.Length == 0)
                        throw new SkipException("EDID not available.");

                    var (edidWithFlags, _) = display.GetEdidManagementWithFlags();
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

        [SkippableFact]
        public void PanelEdidData_ShouldParseWithEdidParser()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var display = adapter.EnumerateDisplayOutputs().FirstOrDefault();
                Skip.If(display == null, "No displays connected.");

                byte[] data;
                try
                {
                    data = display.GetPanelEdidData();
                }
                catch (IGCLException ex) when (ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                                               ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                {
                    throw new SkipException($"Panel descriptor access unsupported: {ex.Result}");
                }

                Skip.If(data.Length == 0, "Panel descriptor data unavailable.");

                var edid = new EDID(data);
                var manufacturerCode = edid.ManufacturerCode;
                var productCode = edid.ProductCode;

                Assert.False(string.IsNullOrWhiteSpace(manufacturerCode));
                Assert.True(productCode > 0);
            }
        }

        [Fact]
        public void DisplayPropertiesDto_FlagBooleans_ShouldTrackRawMasks()
        {
            var dto = new DisplayPropertiesDto
            {
                SupportedOutputBpcFlags = (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_8BPC |
                                         (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_10BPC,
                ProtocolConverterType = (uint)ctl_protocol_converter_location_flag_t.CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_EXTERNAL,
                DisplayConfigFlags = (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ACTIVE,
                FeatureSupportedFlags = (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR |
                                        (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDCP,
                AdvancedFeatureSupportedFlags = (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DPST
            };

            Assert.False(dto.Supports6Bpc);
            Assert.True(dto.Supports8Bpc);
            Assert.True(dto.Supports10Bpc);
            Assert.False(dto.Supports12Bpc);
            Assert.False(dto.HasOnboardProtocolConverter);
            Assert.True(dto.HasExternalProtocolConverter);
            Assert.True(dto.IsDisplayActive);
            Assert.False(dto.IsDisplayAttached);
            Assert.True(dto.SupportsHdr);
            Assert.True(dto.SupportsHdcp);
            Assert.True(dto.SupportsDpst);
            Assert.False(dto.SupportsLace);

            dto.Supports12Bpc = true;
            dto.IsDisplayAttached = true;
            dto.SupportsLace = true;

            Assert.True((dto.SupportedOutputBpcFlags & (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_12BPC) != 0);
            Assert.True((dto.DisplayConfigFlags & (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ATTACHED) != 0);
            Assert.True((dto.AdvancedFeatureSupportedFlags & (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_LACE) != 0);
        }

        [Fact]
        public void DisplayPropertiesDto_AllFlagBooleans_ShouldRoundTripMasks()
        {
            var dto = new DisplayPropertiesDto();

            dto.Supports6Bpc = true;
            dto.Supports8Bpc = true;
            dto.Supports10Bpc = true;
            dto.Supports12Bpc = true;
            dto.HasOnboardProtocolConverter = true;
            dto.HasExternalProtocolConverter = true;
            dto.IsDisplayActive = true;
            dto.IsDisplayAttached = true;
            dto.IsDongleConnectedToEncoder = true;
            dto.IsDitheringEnabled = true;
            dto.IsHdcpEnabled = true;
            dto.IsHdAudioEnabled = true;
            dto.IsPsrEnabled = true;
            dto.IsAdaptiveSyncVrrEnabled = true;
            dto.IsVesaCompressionEnabled = true;
            dto.IsHdrEnabled = true;
            dto.IsHdmiQmsEnabled = true;
            dto.IsHdr10PlusCertifiedEnabled = true;
            dto.IsVesaHdrCertifiedEnabled = true;
            dto.SupportsHdcp = true;
            dto.SupportsHdAudio = true;
            dto.SupportsPsr = true;
            dto.SupportsAdaptiveSyncVrr = true;
            dto.SupportsVesaCompression = true;
            dto.SupportsHdr = true;
            dto.SupportsHdmiQms = true;
            dto.SupportsHdr10PlusCertified = true;
            dto.SupportsVesaHdrCertified = true;
            dto.IsDpstEnabled = true;
            dto.IsLaceEnabled = true;
            dto.IsDrrsEnabled = true;
            dto.IsArcAdaptiveSyncCertifiedEnabled = true;
            dto.SupportsDpst = true;
            dto.SupportsLace = true;
            dto.SupportsDrrs = true;
            dto.SupportsArcAdaptiveSyncCertified = true;

            Assert.Equal(
                (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_6BPC |
                (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_8BPC |
                (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_10BPC |
                (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_12BPC,
                dto.SupportedOutputBpcFlags);

            Assert.Equal(
                (uint)ctl_protocol_converter_location_flag_t.CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_ONBOARD |
                (uint)ctl_protocol_converter_location_flag_t.CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_EXTERNAL,
                dto.ProtocolConverterType);

            Assert.Equal(
                (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ACTIVE |
                (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ATTACHED |
                (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_IS_DONGLE_CONNECTED_TO_ENCODER |
                (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DITHERING_ENABLED,
                dto.DisplayConfigFlags);

            Assert.Equal(
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDCP |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HD_AUDIO |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_PSR |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_ADAPTIVESYNC_VRR |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_COMPRESSION |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDMI_QMS |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR10_PLUS_CERTIFIED |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_HDR_CERTIFIED,
                dto.FeatureEnabledFlags);

            Assert.Equal(dto.FeatureEnabledFlags, dto.FeatureSupportedFlags);

            Assert.Equal(
                (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DPST |
                (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_LACE |
                (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DRRS |
                (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_ARC_ADAPTIVE_SYNC_CERTIFIED,
                dto.AdvancedFeatureEnabledFlags);

            Assert.Equal(dto.AdvancedFeatureEnabledFlags, dto.AdvancedFeatureSupportedFlags);
        }

        [Fact]
        public void AdapterDisplayEncoderPropertiesDto_FlagBooleans_ShouldTrackRawMasks()
        {
            var dto = new AdapterDisplayEncoderPropertiesDto
            {
                EncoderConfigFlags = (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COMPANION_DISPLAY |
                                     (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_VIRTUAL_DISPLAY,
                SupportedOutputBpcFlags = (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_6BPC
            };

            Assert.True(dto.IsCompanionDisplay);
            Assert.True(dto.IsVirtualDisplay);
            Assert.False(dto.IsInternalDisplay);
            Assert.True(dto.Supports6Bpc);
            Assert.False(dto.Supports8Bpc);

            dto.IsInternalDisplay = true;
            dto.Supports8Bpc = true;

            Assert.True((dto.EncoderConfigFlags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_INTERNAL_DISPLAY) != 0);
            Assert.True((dto.SupportedOutputBpcFlags & (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_8BPC) != 0);
        }

        [Fact]
        public void AdapterDisplayEncoderPropertiesDto_AllFlagBooleans_ShouldRoundTripMasks()
        {
            var dto = new AdapterDisplayEncoderPropertiesDto();

            dto.Supports6Bpc = true;
            dto.Supports8Bpc = true;
            dto.Supports10Bpc = true;
            dto.Supports12Bpc = true;
            dto.IsInternalDisplay = true;
            dto.IsVesaTiledDisplay = true;
            dto.IsTypeCCapable = true;
            dto.IsThunderboltCapable = true;
            dto.SupportsDithering = true;
            dto.IsVirtualDisplay = true;
            dto.IsHiddenDisplay = true;
            dto.IsCollageDisplay = true;
            dto.IsSplitDisplay = true;
            dto.IsCompanionDisplay = true;
            dto.IsMultiGpuCollageDisplay = true;
            dto.SupportsHdcp = true;
            dto.SupportsHdAudio = true;
            dto.SupportsPsr = true;
            dto.SupportsAdaptiveSyncVrr = true;
            dto.SupportsVesaCompression = true;
            dto.SupportsHdr = true;
            dto.SupportsHdmiQms = true;
            dto.SupportsHdr10PlusCertified = true;
            dto.SupportsVesaHdrCertified = true;
            dto.SupportsDpst = true;
            dto.SupportsLace = true;
            dto.SupportsDrrs = true;
            dto.SupportsArcAdaptiveSyncCertified = true;

            Assert.Equal(
                (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_6BPC |
                (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_8BPC |
                (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_10BPC |
                (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_12BPC,
                dto.SupportedOutputBpcFlags);

            Assert.Equal(
                (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_INTERNAL_DISPLAY |
                (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_VESA_TILED_DISPLAY |
                (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TYPEC_CAPABLE |
                (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TBT_CAPABLE |
                (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_DITHERING_SUPPORTED |
                (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_VIRTUAL_DISPLAY |
                (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_HIDDEN_DISPLAY |
                (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COLLAGE_DISPLAY |
                (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_SPLIT_DISPLAY |
                (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COMPANION_DISPLAY |
                (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_MGPU_COLLAGE_DISPLAY,
                dto.EncoderConfigFlags);

            Assert.Equal(
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDCP |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HD_AUDIO |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_PSR |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_ADAPTIVESYNC_VRR |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_COMPRESSION |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDMI_QMS |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR10_PLUS_CERTIFIED |
                (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_HDR_CERTIFIED,
                dto.FeatureSupportedFlags);

            Assert.Equal(
                (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DPST |
                (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_LACE |
                (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DRRS |
                (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_ARC_ADAPTIVE_SYNC_CERTIFIED,
                dto.AdvancedFeatureSupportedFlags);
        }

        [Fact]
        public void DisplaySettingsDto_FlagBooleans_ShouldTrackRawMasks()
        {
            var dto = new DisplaySettingsDto
            {
                SupportedFlags = (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY |
                                 (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO,
                ControllableFlags = (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_SOURCE_TM,
                ValidFlags = (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_CONTENT_TYPE,
                SupportedPictureAr = (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_16_9
            };

            Assert.True(dto.IsLowLatencySupported);
            Assert.True(dto.IsAudioSettingsSupported);
            Assert.False(dto.IsSourceTmSupported);
            Assert.True(dto.IsSourceTmControllable);
            Assert.True(dto.IsContentTypeValid);
            Assert.True(dto.SupportsPictureAr16By9);
            Assert.False(dto.SupportsPictureAr4By3);

            dto.IsQuantizationRangeSupported = true;
            dto.IsAudioSettingsControllable = true;
            dto.SupportsPictureAr4By3 = true;

            Assert.True((dto.SupportedFlags & (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_QUANTIZATION_RANGE) != 0);
            Assert.True((dto.ControllableFlags & (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO) != 0);
            Assert.True((dto.SupportedPictureAr & (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_4_3) != 0);
        }

        [Fact]
        public void DisplaySettingsDto_AllFlagBooleans_ShouldRoundTripMasks()
        {
            var dto = new DisplaySettingsDto();

            dto.IsLowLatencySupported = true;
            dto.IsSourceTmSupported = true;
            dto.IsContentTypeSupported = true;
            dto.IsQuantizationRangeSupported = true;
            dto.IsPictureArSupported = true;
            dto.IsAudioSettingsSupported = true;

            dto.IsLowLatencyControllable = true;
            dto.IsSourceTmControllable = true;
            dto.IsContentTypeControllable = true;
            dto.IsQuantizationRangeControllable = true;
            dto.IsPictureArControllable = true;
            dto.IsAudioSettingsControllable = true;

            dto.IsLowLatencyValid = true;
            dto.IsSourceTmValid = true;
            dto.IsContentTypeValid = true;
            dto.IsQuantizationRangeValid = true;
            dto.IsPictureArValid = true;
            dto.IsAudioSettingsValid = true;

            dto.SupportsPictureArDefault = true;
            dto.SupportsPictureArDisabled = true;
            dto.SupportsPictureAr4By3 = true;
            dto.SupportsPictureAr16By9 = true;
            dto.SupportsPictureAr64By27 = true;
            dto.SupportsPictureAr256By135 = true;

            var allDisplaySettingFlags =
                (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY |
                (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_SOURCE_TM |
                (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_CONTENT_TYPE |
                (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_QUANTIZATION_RANGE |
                (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_PICTURE_AR |
                (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO;

            Assert.Equal(allDisplaySettingFlags, dto.SupportedFlags);
            Assert.Equal(allDisplaySettingFlags, dto.ControllableFlags);
            Assert.Equal(allDisplaySettingFlags, dto.ValidFlags);

            Assert.Equal(
                (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_DEFAULT |
                (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_DISABLED |
                (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_4_3 |
                (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_16_9 |
                (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_64_27 |
                (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_256_135,
                dto.SupportedPictureAr);
        }

        [Fact]
        public void PowerOptimizationDto_FlagBooleans_ShouldTrackRawMasks()
        {
            var dpst = new PowerOptimizationDpstDto
            {
                SupportedFeatures = (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_BKLT |
                                    (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_APD,
                EnabledFeatures = (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_BKLT
            };
            Assert.True(dpst.SupportsBacklight);
            Assert.True(dpst.SupportsApd);
            Assert.True(dpst.IsBacklightEnabled);
            Assert.False(dpst.IsApdEnabled);
            dpst.IsApdEnabled = true;
            Assert.True((dpst.EnabledFeatures & (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_APD) != 0);

            var lrr = new PowerOptimizationLrrDto
            {
                SupportedLrrTypes = (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR20 |
                                    (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_ALRR,
                CurrentLrrTypes = (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_ALRR
            };
            Assert.True(lrr.SupportsLrr20);
            Assert.True(lrr.SupportsAlrr);
            Assert.True(lrr.IsAlrrCurrent);
            Assert.False(lrr.IsLrr20Current);
            lrr.IsLrr20Current = true;
            Assert.True((lrr.CurrentLrrTypes & (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR20) != 0);

            var settings = new PowerOptimizationSettingsDto
            {
                PowerOptimizationFeature = (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_PSR
            };
            Assert.True(settings.UsesPsr);
            Assert.False(settings.UsesDpst);
            settings.UsesDpst = true;
            Assert.True((settings.PowerOptimizationFeature & (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_DPST) != 0);
        }

        [Fact]
        public void PowerOptimizationDto_AllFlagBooleans_ShouldRoundTripMasks()
        {
            var dpst = new PowerOptimizationDpstDto();
            dpst.SupportsBacklight = true;
            dpst.SupportsPanelCabc = true;
            dpst.SupportsOpst = true;
            dpst.SupportsElp = true;
            dpst.SupportsEpsm = true;
            dpst.SupportsApd = true;
            dpst.SupportsPixoptix = true;
            dpst.IsBacklightEnabled = true;
            dpst.IsPanelCabcEnabled = true;
            dpst.IsOpstEnabled = true;
            dpst.IsElpEnabled = true;
            dpst.IsEpsmEnabled = true;
            dpst.IsApdEnabled = true;
            dpst.IsPixoptixEnabled = true;

            var allDpstFlags =
                (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_BKLT |
                (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_PANEL_CABC |
                (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_OPST |
                (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_ELP |
                (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_EPSM |
                (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_APD |
                (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_PIXOPTIX;

            Assert.Equal(allDpstFlags, dpst.SupportedFeatures);
            Assert.Equal(allDpstFlags, dpst.EnabledFeatures);

            var lrr = new PowerOptimizationLrrDto();
            lrr.SupportsLrr10 = true;
            lrr.SupportsLrr20 = true;
            lrr.SupportsLrr25 = true;
            lrr.SupportsAlrr = true;
            lrr.SupportsUblrr = true;
            lrr.SupportsUbzrr = true;
            lrr.IsLrr10Current = true;
            lrr.IsLrr20Current = true;
            lrr.IsLrr25Current = true;
            lrr.IsAlrrCurrent = true;
            lrr.IsUblrrCurrent = true;
            lrr.IsUbzrrCurrent = true;

            var allLrrFlags =
                (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR10 |
                (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR20 |
                (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR25 |
                (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_ALRR |
                (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_UBLRR |
                (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_UBZRR;

            Assert.Equal(allLrrFlags, lrr.SupportedLrrTypes);
            Assert.Equal(allLrrFlags, lrr.CurrentLrrTypes);

            var settings = new PowerOptimizationSettingsDto();
            settings.UsesFbc = true;
            settings.UsesPsr = true;
            settings.UsesDpst = true;
            settings.UsesLrr = true;
            settings.UsesLace = true;

            Assert.Equal(
                (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_FBC |
                (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_PSR |
                (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_DPST |
                (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LRR |
                (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LACE,
                settings.PowerOptimizationFeature);
        }

        [Fact]
        public void CreateDisplaySettingsSetRequest_ShouldInitializeUsefulDefaults()
        {
            var validFlags = (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY |
                             (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO;

            var dto = IGCLDisplayHelper.CreateDisplaySettingsSetRequest(validFlags);

            Assert.True(dto.Set);
            Assert.Equal(validFlags, dto.ValidFlags);
        }

        [Fact]
        public void CreatePowerOptimizationSettingsSetRequest_ShouldInitializeUsefulDefaults()
        {
            var featureFlags = (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_PSR;

            var dto = IGCLDisplayHelper.CreatePowerOptimizationSettingsSetRequest(featureFlags);

            Assert.True(dto.Enable);
            Assert.Equal(featureFlags, dto.PowerOptimizationFeature);
        }

        [Fact]
        public void ValidateSetDisplaySettingsRequest_WithoutValidFlags_ShouldThrow()
        {
            var ex = Assert.Throws<ArgumentException>(() => IGCLDisplayHelper.ValidateSetDisplaySettingsRequest(default));
            Assert.Contains("ValidFlags", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ValidateSetPowerOptimizationSettingsRequest_WithoutFeatures_ShouldThrow()
        {
            var ex = Assert.Throws<ArgumentException>(() => IGCLDisplayHelper.ValidateSetPowerOptimizationSettingsRequest(default));
            Assert.Contains("PowerOptimizationFeature", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void DisplayNativeDtoCoverage_NewDtoTypes_ShouldRoundTrip()
        {
            var getNative = new ctl_get_brightness_t
            {
                Size = 1,
                Version = 2,
                TargetBrightness = 55,
                CurrentBrightness = 44
            };
            var getDto = BrightnessGetDto.FromNative(getNative);
            var getRoundTrip = getDto.ToNative();
            Assert.Equal(getNative.TargetBrightness, getRoundTrip.TargetBrightness);
            Assert.Equal(getNative.CurrentBrightness, getRoundTrip.CurrentBrightness);

            var setDto = new BrightnessSetDto
            {
                TargetBrightness = 60,
                SmoothTransitionTimeInMs = 123
            };
            var setNative = setDto.ToNative();
            Assert.Equal((uint)60, setNative.TargetBrightness);
            Assert.Equal((uint)123, setNative.SmoothTransitionTimeInMs);

            var scaling = ScalingCapsDto.FromNative(new ctl_scaling_caps_t { Size = 4, Version = 0, SupportedScaling = 7 });
            Assert.Equal((uint)7, scaling.SupportedScaling);

            var retro = RetroScalingCapsDto.FromNative(new ctl_retro_scaling_caps_t { Size = 4, Version = 0, SupportedRetroScaling = 3 });
            Assert.Equal((uint)3, retro.SupportedRetroScaling);

            var powerCaps = PowerOptimizationCapsDto.FromNative(new ctl_power_optimization_caps_t { Size = 4, Version = 0, SupportedFeatures = 9 });
            Assert.Equal((uint)9, powerCaps.SupportedFeatures);

            var profile = new IntelArcSyncProfileParamsDto
            {
                IntelArcSyncProfile = ctl_intel_arc_sync_profile_t.CTL_INTEL_ARC_SYNC_PROFILE_RECOMMENDED,
                MaxRefreshRateInHz = 144,
                MinRefreshRateInHz = 48,
                MaxFrameTimeIncreaseInUs = 1000,
                MaxFrameTimeDecreaseInUs = 500
            };
            var profileNative = profile.ToNative();
            Assert.Equal(144, profileNative.MaxRefreshRateInHz);
            Assert.Equal(48, profileNative.MinRefreshRateInHz);

            var customResult = CustomModesResultDto.FromNative(
                new ctl_get_set_custom_mode_args_t { Size = 4, Version = 0, NumOfModes = 2 },
                new[]
                {
                    new ctl_custom_src_mode_t { SourceX = 1920, SourceY = 1080 },
                    new ctl_custom_src_mode_t { SourceX = 2560, SourceY = 1440 }
                });
            Assert.NotNull(customResult.Modes);
            Assert.Equal(2, customResult.Modes!.Count);

            var muxDto = MuxPropertiesDto.FromNative(
                new ctl_mux_properties_t { Size = 4, Version = 0, MuxId = 1, Count = 2, IndexOfDisplayOutputOwningMux = 1 },
                new[] { (IntPtr)10, (IntPtr)20 });
            Assert.NotNull(muxDto.DisplayOutputs);
            Assert.Equal(2, muxDto.DisplayOutputs!.Count);

            var vblank = new VblankTimestampArgsDto
            {
                NumOfTargets = 2,
                VblankTimestamps = new System.Collections.Generic.List<ulong> { 111, 222 }
            };
            var vblankNative = vblank.ToNative();
            Assert.Equal((byte)2, vblankNative.NumOfTargets);
            Assert.Equal((ulong)111, vblankNative.VblankTS[0]);
            Assert.Equal((ulong)222, vblankNative.VblankTS[1]);
        }
        
    }
}
