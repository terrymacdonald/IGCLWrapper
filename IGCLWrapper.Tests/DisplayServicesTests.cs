using Xunit;
using System;
using IGCLWrapper; // Use the new bindings project

namespace IGCLWrapper.Tests
{
    public class DisplayServicesTests : IDisposable
    {
        private ctl_api_handle_t _apiHandle;
        private ctl_device_adapter_handle_t[] _adapters;
        private ctl_display_output_handle_t[] _displays;

        public DisplayServicesTests()
        {
            // Initialize IGCL
            InitializeIGCL();
        }

        public void Dispose()
        {
            CleanupIGCL();
        }

        private void InitializeIGCL()
        {
            // Initialize IGCL API
            ctl_init_args_t initArgs = new ctl_init_args_t();
            initArgs.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(initArgs);
            initArgs.Version = 0; // Use default version
            initArgs.flags = ctl_init_flags_t.CTL_INIT_FLAG_USE_LEVEL_ZERO;
            initArgs.AppVersion = 0;
            initArgs.SupportedVersion = 0;

            ctl_result_t result = IGCL.ctlInit(ref initArgs, ref _apiHandle);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            Assert.NotEqual(IntPtr.Zero, _apiHandle);

            // Enumerate adapters
            uint adapterCount = 0;
            result = IGCL.ctlEnumerateDevices(_apiHandle, ref adapterCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            Assert.True(adapterCount > 0, "No adapters found");

            _adapters = new ctl_device_adapter_handle_t[adapterCount];
            result = IGCL.ctlEnumerateDevices(_apiHandle, ref adapterCount, _adapters);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            // Enumerate displays for first adapter
            uint displayCount = 0;
            result = IGCL.ctlEnumerateDisplayOutputs(_adapters[0], ref displayCount, null);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS && displayCount > 0)
            {
                _displays = new ctl_display_output_handle_t[displayCount];
                result = IGCL.ctlEnumerateDisplayOutputs(_adapters[0], ref displayCount, _displays);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        private void CleanupIGCL()
        {
            if (_apiHandle != IntPtr.Zero)
            {
                IGCL.ctlClose(_apiHandle);
                _apiHandle = IntPtr.Zero;
            }
        }

        [Fact]
        public void InitializeIGCL_ShouldSucceed()
        {
            // Test already performed in constructor
            Assert.NotEqual(IntPtr.Zero, _apiHandle);
        }

        [Fact]
        public void EnumerateAdapters_ShouldReturnValidAdapters()
        {
            Assert.NotNull(_adapters);
            Assert.True(_adapters.Length > 0);
            Assert.NotEqual(IntPtr.Zero, _adapters[0]);
        }

        [Fact]
        public void GetAdapterProperties_ShouldReturnValidProperties()
        {
            ctl_device_adapter_properties_t properties = new ctl_device_adapter_properties_t();
            properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
            properties.Version = 1;

            ctl_result_t result = IGCL.ctlGetDeviceProperties(_adapters[0], ref properties);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            // Verify some basic properties
            Assert.True(properties.device_id_size > 0);
            Assert.NotEqual(ctl_device_type_t.CTL_DEVICE_TYPE_MAX, properties.device_type);
        }

        [Fact]
        public void EnumerateDisplays_ShouldWork()
        {
            // Skip if no displays available
            if (_displays == null || _displays.Length == 0)
            {
                return; // No displays to test
            }

            Assert.True(_displays.Length > 0);
            Assert.NotEqual(IntPtr.Zero, _displays[0]);
        }

        [Fact]
        public void GetDisplayProperties_ShouldReturnValidProperties()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_display_properties_t properties = new ctl_display_properties_t();
            properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
            properties.Version = 1;

            ctl_result_t result = IGCL.ctlGetDisplayProperties(_displays[0], ref properties);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            // Verify display timing info is populated
            Assert.True(properties.Display_Timing_Info.PixelClock >= 0);
            Assert.True(properties.Display_Timing_Info.HActive > 0);
            Assert.True(properties.Display_Timing_Info.VActive > 0);
        }

        [Fact]
        public void GetAdapterDisplayEncoderProperties_ShouldReturnValidProperties()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_adapter_display_encoder_properties_t properties = new ctl_adapter_display_encoder_properties_t();
            properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
            properties.Version = 1;

            ctl_result_t result = IGCL.ctlGetAdaperDisplayEncoderProperties(_displays[0], ref properties);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            // Verify encoder properties
            Assert.NotEqual(ctl_display_output_types_t.CTL_DISPLAY_OUTPUT_TYPES_INVALID, properties.Type);
        }

        [Fact]
        public void GetSharpnessCapabilities_ShouldReturnValidCapabilities()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_sharpness_caps_t caps = new ctl_sharpness_caps_t();
            caps.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(caps);
            caps.Version = 1;

            ctl_result_t result = IGCL.ctlGetSharpnessCaps(_displays[0], ref caps);
            // Sharpness may not be supported on all displays, so just check the call doesn't crash
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
        }

        [Fact]
        public void GetPowerOptimizationCapabilities_ShouldReturnValidCapabilities()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_power_optimization_caps_t caps = new ctl_power_optimization_caps_t();
            caps.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(caps);
            caps.Version = 1;

            ctl_result_t result = IGCL.ctlGetPowerOptimizationCaps(_displays[0], ref caps);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }

        [Fact]
        public void GetScalingCapabilities_ShouldReturnValidCapabilities()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_scaling_caps_t caps = new ctl_scaling_caps_t();
            caps.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(caps);
            caps.Version = 1;

            ctl_result_t result = IGCL.ctlGetSupportedScalingCapability(_displays[0], ref caps);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }

        [Fact]
        public void GetDisplaySettings_ShouldReturnValidSettings()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_display_settings_t settings = new ctl_display_settings_t();
            settings.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(settings);
            settings.Version = 1;
            settings.Set = false; // Get operation

            ctl_result_t result = IGCL.ctlGetSetDisplaySettings(_displays[0], ref settings);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }

        [Fact]
        public void GetBrightnessSettings_ShouldReturnValidSettings()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_get_brightness_t brightness = new ctl_get_brightness_t();
            brightness.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(brightness);
            brightness.Version = 1;

            ctl_result_t result = IGCL.ctlGetBrightnessSetting(_displays[0], ref brightness);
            // Brightness may not be supported on all displays
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_INVALID_OPERATION_TYPE);
        }

        [Fact]
        public void GetIntelArcSyncInfo_ShouldReturnValidInfo()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_intel_arc_sync_monitor_params_t syncInfo = new ctl_intel_arc_sync_monitor_params_t();
            syncInfo.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(syncInfo);
            syncInfo.Version = 1;

            ctl_result_t result = IGCL.ctlGetIntelArcSyncInfoForMonitor(_displays[0], ref syncInfo);
            // Intel Arc Sync may not be supported on all displays
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
        }

        [Fact]
        public void GetLACEConfig_ShouldReturnValidConfig()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_lace_config_t config = new ctl_lace_config_t();
            config.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(config);
            config.Version = 1;
            config.OpTypeGet = ctl_get_operation_flags_t.CTL_GET_OPERATION_FLAG_CURRENT;

            ctl_result_t result = IGCL.ctlGetLACEConfig(_displays[0], ref config);
            // LACE may not be supported on all displays
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
        }

        [Fact]
        public void GetDynamicContrastEnhancement_ShouldWork()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_dce_args_t dceArgs = new ctl_dce_args_t();
            dceArgs.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(dceArgs);
            dceArgs.Version = 1;
            dceArgs.Set = false; // Get operation

            ctl_result_t result = IGCL.ctlGetSetDynamicContrastEnhancement(_displays[0], ref dceArgs);
            // DCE may not be supported on all displays
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
        }

        [Fact]
        public void GetWireFormat_ShouldReturnValidFormat()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_get_set_wire_format_config_t wireFormat = new ctl_get_set_wire_format_config_t();
            wireFormat.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(wireFormat);
            wireFormat.Version = 1;
            wireFormat.Operation = ctl_wire_format_operation_type_t.CTL_WIRE_FORMAT_OPERATION_TYPE_GET;

            ctl_result_t result = IGCL.ctlGetSetWireFormat(_displays[0], ref wireFormat);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }

        [Fact]
        public void GetPixelTransformationConfig_ShouldReturnValidConfig()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_pixtx_pipe_get_config_t config = new ctl_pixtx_pipe_get_config_t();
            config.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(config);
            config.Version = 1;
            config.QueryType = ctl_pixtx_config_query_type_t.CTL_PIXTX_CONFIG_QUERY_TYPE_CAPABILITY;

            ctl_result_t result = IGCL.ctlPixelTransformationGetConfig(_displays[0], ref config);
            // Pixel transformation may not be supported on all displays
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
        }

        [Fact]
        public void GetVblankTimestamp_ShouldReturnValidTimestamp()
        {
            if (_displays == null || _displays.Length == 0)
            {
                return; // Skip if no displays
            }

            ctl_vblank_ts_args_t vblankArgs = new ctl_vblank_ts_args_t();
            vblankArgs.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(vblankArgs);
            vblankArgs.Version = 1;

            ctl_result_t result = IGCL.ctlGetVblankTimestamp(_displays[0], ref vblankArgs);
            // Vblank timestamp may not be supported on all displays
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
        }

        [Fact]
        public void EnumerateI2CPinPairs_ShouldWork()
        {
            uint pinPairCount = 0;
            ctl_result_t result = IGCL.ctlEnumerateI2CPinPairs(_adapters[0], ref pinPairCount, null);
            // I2C may not be supported on all systems
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
        }

        [Fact]
        public void EnumerateMuxDevices_ShouldWork()
        {
            uint muxCount = 0;
            ctl_result_t result = IGCL.ctlEnumerateMuxDevices(_apiHandle, ref muxCount, null);
            // MUX devices may not be present on all systems
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
        }

        [Fact]
        public void GetSupportedRetroScalingCapability_ShouldReturnValidCapabilities()
        {
            ctl_retro_scaling_caps_t caps = new ctl_retro_scaling_caps_t();
            caps.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(caps);
            caps.Version = 1;

            ctl_result_t result = IGCL.ctlGetSupportedRetroScalingCapability(_adapters[0], ref caps);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }

        [Fact]
        public void GetSupported3DCapabilities_ShouldReturnValidCapabilities()
        {
            ctl_3d_feature_caps_t caps = new ctl_3d_feature_caps_t();
            caps.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(caps);
            caps.Version = 1;

            ctl_result_t result = IGCL.ctlGetSupported3DCapabilities(_adapters[0], ref caps);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }

        [Fact]
        public void GetSupportedVideoProcessingCapabilities_ShouldReturnValidCapabilities()
        {
            ctl_video_processing_feature_caps_t caps = new ctl_video_processing_feature_caps_t();
            caps.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(caps);
            caps.Version = 1;

            ctl_result_t result = IGCL.ctlGetSupportedVideoProcessingCapabilities(_adapters[0], ref caps);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }
    }
}
