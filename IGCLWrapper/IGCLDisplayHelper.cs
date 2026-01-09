using System;

namespace IGCLWrapper
{
    /// <summary>
    /// Display helper facade for IGCL display handles.
    /// </summary>
    public sealed class IGCLDisplayHelper : IDisposable
    {
        private readonly object _lock = new();
        private ctl_display_properties_t? _properties;
        private bool _disposed;
        internal IGCLApiHelper Api { get; }
        internal IntPtr AdapterHandle { get; }
        internal IntPtr DisplayHandle { get; }

        internal IGCLDisplayHelper(IGCLApiHelper api, IntPtr adapterHandle, IntPtr displayHandle)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            AdapterHandle = adapterHandle;
            DisplayHandle = displayHandle;
        }

        private static unsafe ctl_display_properties_t CreateDisplayProperties() => new ctl_display_properties_t { Size = (uint)sizeof(ctl_display_properties_t), Version = 0 };
        private static unsafe ctl_device_adapter_properties_t CreateAdapterProperties() => new ctl_device_adapter_properties_t { Size = (uint)sizeof(ctl_device_adapter_properties_t), Version = 1 };
        private static unsafe ctl_mux_properties_t CreateMuxProperties() => new ctl_mux_properties_t { Size = (uint)sizeof(ctl_mux_properties_t), Version = 0 };
        private static unsafe ctl_retro_scaling_caps_t CreateRetroScalingCaps() => new ctl_retro_scaling_caps_t { Size = (uint)sizeof(ctl_retro_scaling_caps_t), Version = 0 };
        private static unsafe ctl_scaling_caps_t CreateScalingCaps() => new ctl_scaling_caps_t { Size = (uint)sizeof(ctl_scaling_caps_t), Version = 0 };
        public static unsafe ctl_scaling_settings_t CreateScalingSettings() => new ctl_scaling_settings_t { Size = (uint)sizeof(ctl_scaling_settings_t), Version = 0 };
        public static unsafe ctl_sharpness_settings_t CreateSharpnessSettings() => new ctl_sharpness_settings_t { Size = (uint)sizeof(ctl_sharpness_settings_t), Version = 0 };
        private static unsafe ctl_sharpness_caps_t CreateSharpnessCaps() => new ctl_sharpness_caps_t { Size = (uint)sizeof(ctl_sharpness_caps_t), Version = 0 };
        private static unsafe ctl_power_optimization_caps_t CreatePowerOptimizationCaps() => new ctl_power_optimization_caps_t { Size = (uint)sizeof(ctl_power_optimization_caps_t), Version = 0 };
        public static unsafe ctl_power_optimization_settings_t CreatePowerOptimizationSettings() => new ctl_power_optimization_settings_t { Size = (uint)sizeof(ctl_power_optimization_settings_t), Version = 0 };
        private static unsafe ctl_get_brightness_t CreateGetBrightness() => new ctl_get_brightness_t { Size = (uint)sizeof(ctl_get_brightness_t), Version = 0 };
        public static unsafe ctl_lace_config_t CreateLaceConfig() => new ctl_lace_config_t { Size = (uint)sizeof(ctl_lace_config_t), Version = 0 };
        private static unsafe ctl_intel_arc_sync_monitor_params_t CreateArcSyncMonitorParams() => new ctl_intel_arc_sync_monitor_params_t { Size = (uint)sizeof(ctl_intel_arc_sync_monitor_params_t), Version = 0 };
        public static unsafe ctl_intel_arc_sync_profile_params_t CreateArcSyncProfileParams() => new ctl_intel_arc_sync_profile_params_t { Size = (uint)sizeof(ctl_intel_arc_sync_profile_params_t), Version = 0 };
        public static unsafe ctl_set_brightness_t CreateSetBrightness() => new ctl_set_brightness_t { Size = (uint)sizeof(ctl_set_brightness_t), Version = 0 };
        public static unsafe ctl_retro_scaling_settings_t CreateRetroScalingSettings() => new ctl_retro_scaling_settings_t { Size = (uint)sizeof(ctl_retro_scaling_settings_t), Version = 0 };
        public static unsafe ctl_combined_display_args_t CreateCombinedDisplayArgs() => new ctl_combined_display_args_t { Size = (uint)sizeof(ctl_combined_display_args_t), Version = 0 };
        public static unsafe ctl_genlock_args_t CreateGenlockArgs() => new ctl_genlock_args_t { Size = (uint)sizeof(ctl_genlock_args_t), Version = 0 };
        public static unsafe ctl_sw_psr_settings_t CreateSoftwarePsrSettings() => new ctl_sw_psr_settings_t { Size = (uint)sizeof(ctl_sw_psr_settings_t), Version = 0 };
        public static unsafe ctl_get_set_wire_format_config_t CreateWireFormatConfig() => new ctl_get_set_wire_format_config_t { Size = (uint)sizeof(ctl_get_set_wire_format_config_t), Version = 0 };
        public static unsafe ctl_display_settings_t CreateDisplaySettings() => new ctl_display_settings_t { Size = (uint)sizeof(ctl_display_settings_t), Version = 0 };
        public static unsafe ctl_edid_management_args_t CreateEdidManagementArgs() => new ctl_edid_management_args_t { Size = (uint)sizeof(ctl_edid_management_args_t), Version = 0 };
        public static unsafe ctl_panel_descriptor_access_args_t CreatePanelDescriptorArgs() => new ctl_panel_descriptor_access_args_t { Size = (uint)sizeof(ctl_panel_descriptor_access_args_t), Version = 0 };
        public static unsafe ctl_pixtx_pipe_set_config_t CreatePixtxPipeSetConfig() => new ctl_pixtx_pipe_set_config_t { Size = (uint)sizeof(ctl_pixtx_pipe_set_config_t), Version = 0 };
        public static unsafe ctl_pixtx_pipe_get_config_t CreatePixtxPipeGetConfig() => new ctl_pixtx_pipe_get_config_t { Size = (uint)sizeof(ctl_pixtx_pipe_get_config_t), Version = 0 };
        public static unsafe ctl_lda_args_t CreateLinkedDisplayAdaptersArgs() => new ctl_lda_args_t { Size = (uint)sizeof(ctl_lda_args_t), Version = 0 };
        public static unsafe ctl_vblank_ts_args_t CreateVblankTimestampArgs() => new ctl_vblank_ts_args_t { Size = (uint)sizeof(ctl_vblank_ts_args_t), Version = 0 };
        public static unsafe ctl_get_set_custom_mode_args_t CreateCustomModeArgs() => new ctl_get_set_custom_mode_args_t { Size = (uint)sizeof(ctl_get_set_custom_mode_args_t), Version = 0 };
        public static unsafe ctl_dce_args_t CreateDceArgs() => new ctl_dce_args_t { Size = (uint)sizeof(ctl_dce_args_t), Version = 0 };


        public unsafe ctl_display_properties_t GetProperties()
        {
            ThrowIfDisposed();
            lock (_lock)
            {
                if (_properties.HasValue)
                {
                    return _properties.Value;
                }

                var props = CreateDisplayProperties();
                var result = IGCL.ctlGetDisplayProperties((_ctl_display_output_handle_t*)DisplayHandle, &props);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    throw new IGCLException(result, "Failed to get display properties");
                }

                _properties = props;
                return props;
            }
        }

        public ctl_display_timing_t GetTiming()
        {
            var props = GetProperties();
            return props.Display_Timing_Info;
        }

        public bool IsActive()
        {
            var timing = GetTiming();
            return timing.HActive > 0 && timing.VActive > 0;
        }

        public (uint width, uint height) GetResolution()
        {
            var timing = GetTiming();
            return (timing.HActive, timing.VActive);
        }

        public double GetRefreshRateHz()
        {
            var timing = GetTiming();
            return timing.RefreshRate / 1000.0;
        }

        public string Name => $"Display-{DisplayHandle.ToInt64():X}";

        public unsafe ctl_result_t CheckDriverVersion(uint versionInfo)
        {
            ThrowIfDisposed();
            return IGCL.ctlCheckDriverVersion((_ctl_device_adapter_handle_t*)AdapterHandle, versionInfo);
        }

        public unsafe IntPtr[] EnumerateDevices()
        {
            ThrowIfDisposed();
            uint count = 0;
            var result = IGCL.ctlEnumerateDevices((_ctl_api_handle_t*)Api.ApiHandle, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get device count");
            if (count == 0)
                return Array.Empty<IntPtr>();

            var devices = new IntPtr[count];
            fixed (IntPtr* pDevices = devices)
            {
                result = IGCL.ctlEnumerateDevices((_ctl_api_handle_t*)Api.ApiHandle, &count, (_ctl_device_adapter_handle_t**)pDevices);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate devices");
            }
            return devices;
        }

        public unsafe IntPtr[] EnumerateDisplayOutputs()
        {
            ThrowIfDisposed();
            uint count = 0;
            var result = IGCL.ctlEnumerateDisplayOutputs((_ctl_device_adapter_handle_t*)AdapterHandle, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get display count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var outputs = new IntPtr[count];
            fixed (IntPtr* pOutputs = outputs)
            {
                result = IGCL.ctlEnumerateDisplayOutputs((_ctl_device_adapter_handle_t*)AdapterHandle, &count, (_ctl_display_output_handle_t**)pOutputs);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate displays");
            }
            return outputs;
        }

        public unsafe IntPtr[] EnumerateI2CPinPairs()
        {
            ThrowIfDisposed();
            uint count = 0;
            var result = IGCL.ctlEnumerateI2CPinPairs((_ctl_device_adapter_handle_t*)AdapterHandle, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get I2C pin pair count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var pins = new IntPtr[count];
            fixed (IntPtr* pPins = pins)
            {
                result = IGCL.ctlEnumerateI2CPinPairs((_ctl_device_adapter_handle_t*)AdapterHandle, &count, (_ctl_i2c_pin_pair_handle_t**)pPins);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate I2C pin pairs");
            }
            return pins;
        }

        public unsafe ctl_device_adapter_properties_t GetDeviceProperties()
        {
            ThrowIfDisposed();
            var props = CreateAdapterProperties();
            var result = IGCL.ctlGetDeviceProperties((_ctl_device_adapter_handle_t*)AdapterHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get device properties");
            return props;
        }

        public unsafe ctl_adapter_display_encoder_properties_t GetAdapterDisplayEncoderPropertiesNative()
        {
            ThrowIfDisposed();
            var props = new ctl_adapter_display_encoder_properties_t { Size = (uint)sizeof(ctl_adapter_display_encoder_properties_t), Version = 0 };
            var result = IGCL.ctlGetAdaperDisplayEncoderProperties((_ctl_display_output_handle_t*)DisplayHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get adapter display encoder properties");
            return props;
        }

        public AdapterDisplayEncoderPropertiesDto GetAdapterDisplayEncoderProperties()
        {
            var native = GetAdapterDisplayEncoderPropertiesNative();
            return AdapterDisplayEncoderPropertiesDto.FromNative(native);
        }

        public unsafe (IntPtr zeDevice, IntPtr instance) GetZeDevice()
        {
            ThrowIfDisposed();
            IntPtr zeDevice = IntPtr.Zero;
            void* instance = null;
            var result = IGCL.ctlGetZeDevice((_ctl_device_adapter_handle_t*)AdapterHandle, &zeDevice, &instance);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get Level0 device");
            return (zeDevice, (IntPtr)instance);
        }

        public unsafe (ctl_sharpness_caps_t caps, ctl_sharpness_filter_properties_t[] filters) GetSharpnessCaps()
        {
            ThrowIfDisposed();
            var caps = CreateSharpnessCaps();

            // First pass: get count
            var result = IGCL.ctlGetSharpnessCaps((_ctl_display_output_handle_t*)DisplayHandle, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && caps.NumFilterTypes == 0)
                throw new IGCLException(result, "Failed to get sharpness caps");

            if (caps.NumFilterTypes == 0)
                return (caps, Array.Empty<ctl_sharpness_filter_properties_t>());

            var filters = new ctl_sharpness_filter_properties_t[caps.NumFilterTypes];
            fixed (ctl_sharpness_filter_properties_t* pFilters = filters)
            {
                caps.pFilterProperty = pFilters;
                result = IGCL.ctlGetSharpnessCaps((_ctl_display_output_handle_t*)DisplayHandle, &caps);
                caps.pFilterProperty = null;
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get sharpness caps");
            }

            return (caps, filters);
        }

        public unsafe ctl_sharpness_settings_t GetCurrentSharpnessNative()
        {
            ThrowIfDisposed();
            var settings = CreateSharpnessSettings();
            var result = IGCL.ctlGetCurrentSharpness((_ctl_display_output_handle_t*)DisplayHandle, &settings);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get current sharpness");
            return settings;
        }

        public SharpnessSettingsDto GetCurrentSharpness()
        {
            var native = GetCurrentSharpnessNative();
            return SharpnessSettingsDto.FromNative(native);
        }

        public unsafe void SetCurrentSharpnessNative(ctl_sharpness_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlSetCurrentSharpness((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set sharpness");
        }

        public void SetCurrentSharpness(SharpnessSettingsDto settings)
        {
            SetCurrentSharpnessNative(settings.ToNative());
        }

        public unsafe void I2CAccess(ref ctl_i2c_access_args_t args)
        {
            ThrowIfDisposed();
            fixed (ctl_i2c_access_args_t* pArgs = &args)
            {
                var result = IGCL.ctlI2CAccess((_ctl_display_output_handle_t*)DisplayHandle, pArgs);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "I2C access failed");
            }
        }

        public unsafe void I2CAccessOnPinPair(IntPtr pinPair, ref ctl_i2c_access_pinpair_args_t args)
        {
            ThrowIfDisposed();
            fixed (ctl_i2c_access_pinpair_args_t* pArgs = &args)
            {
                var result = IGCL.ctlI2CAccessOnPinPair((_ctl_i2c_pin_pair_handle_t*)pinPair, pArgs);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "I2C access on pin pair failed");
            }
        }

        public unsafe void AUXAccess(ref ctl_aux_access_args_t args)
        {
            ThrowIfDisposed();
            fixed (ctl_aux_access_args_t* pArgs = &args)
            {
                var result = IGCL.ctlAUXAccess((_ctl_display_output_handle_t*)DisplayHandle, pArgs);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "AUX access failed");
            }
        }

        public unsafe ctl_power_optimization_caps_t GetPowerOptimizationCaps()
        {
            ThrowIfDisposed();
            var caps = CreatePowerOptimizationCaps();
            var result = IGCL.ctlGetPowerOptimizationCaps((_ctl_display_output_handle_t*)DisplayHandle, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power optimization caps");
            return caps;
        }

        public unsafe ctl_power_optimization_settings_t GetPowerOptimizationSettingNative(ctl_power_optimization_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            if (copy.Size == 0)
                copy.Size = (uint)sizeof(ctl_power_optimization_settings_t);
            if (copy.Version == 0)
                copy.Version = 0;
            var result = IGCL.ctlGetPowerOptimizationSetting((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power optimization settings");
            return copy;
        }

        public PowerOptimizationSettingsDto GetPowerOptimizationSetting(PowerOptimizationSettingsDto settings)
        {
            var native = GetPowerOptimizationSettingNative(settings.ToNative());
            return PowerOptimizationSettingsDto.FromNative(native);
        }

        public unsafe void SetPowerOptimizationSettingNative(ctl_power_optimization_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlSetPowerOptimizationSetting((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set power optimization settings");
        }

        public void SetPowerOptimizationSetting(PowerOptimizationSettingsDto settings)
        {
            SetPowerOptimizationSettingNative(settings.ToNative());
        }

        public unsafe void SetBrightnessSetting(ctl_set_brightness_t brightness)
        {
            ThrowIfDisposed();
            var copy = brightness;
            var result = IGCL.ctlSetBrightnessSetting((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set brightness");
        }

        public unsafe ctl_get_brightness_t GetBrightnessSetting()
        {
            ThrowIfDisposed();
            var brightness = CreateGetBrightness();
            var result = IGCL.ctlGetBrightnessSetting((_ctl_display_output_handle_t*)DisplayHandle, &brightness);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to get brightness: {result}");
            return brightness;
        }

        public unsafe (ctl_pixtx_pipe_get_config_t config, ctl_pixtx_block_config_t[] blocks) PixelTransformationGetConfig(ctl_pixtx_pipe_get_config_t args)
        {
            ThrowIfDisposed();
            var config = args;

            // First pass: get NumBlocks
            var result = IGCL.ctlPixelTransformationGetConfig((_ctl_display_output_handle_t*)DisplayHandle, &config);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && config.NumBlocks == 0)
                throw new IGCLException(result, "Failed to get pixel transformation config");

            if (config.NumBlocks == 0)
                return (config, Array.Empty<ctl_pixtx_block_config_t>());

            var blocks = new ctl_pixtx_block_config_t[config.NumBlocks];
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].Size = (uint)sizeof(ctl_pixtx_block_config_t);
                blocks[i].Version = 0;
            }

            fixed (ctl_pixtx_block_config_t* pBlocks = blocks)
            {
                config.pBlockConfigs = pBlocks;
                result = IGCL.ctlPixelTransformationGetConfig((_ctl_display_output_handle_t*)DisplayHandle, &config);
                config.pBlockConfigs = null;
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get pixel transformation config");
            }

            return (config, blocks);
        }

        public unsafe void PixelTransformationSetConfig(ctl_pixtx_pipe_set_config_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlPixelTransformationSetConfig((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set pixel transformation config");
        }

        public unsafe ctl_panel_descriptor_access_args_t PanelDescriptorAccess(ctl_panel_descriptor_access_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlPanelDescriptorAccess((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to access panel descriptor");
            return copy;
        }

        public unsafe ctl_retro_scaling_caps_t GetSupportedRetroScalingCapability()
        {
            ThrowIfDisposed();
            var caps = CreateRetroScalingCaps();
            var result = IGCL.ctlGetSupportedRetroScalingCapability((_ctl_device_adapter_handle_t*)AdapterHandle, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get retro scaling capability");
            return caps;
        }

        public unsafe ctl_retro_scaling_settings_t GetSetRetroScalingNative(ctl_retro_scaling_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            if (copy.Size == 0)
                copy.Size = (uint)sizeof(ctl_retro_scaling_settings_t);
            if (copy.Version == 0)
                copy.Version = 0;
            var result = IGCL.ctlGetSetRetroScaling((_ctl_device_adapter_handle_t*)AdapterHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set retro scaling");
            return copy;
        }

        public RetroScalingSettingsDto GetSetRetroScaling(RetroScalingSettingsDto settings)
        {
            var native = GetSetRetroScalingNative(settings.ToNative());
            return RetroScalingSettingsDto.FromNative(native);
        }

        public unsafe ctl_scaling_caps_t GetSupportedScalingCapability()
        {
            ThrowIfDisposed();
            var caps = CreateScalingCaps();
            var result = IGCL.ctlGetSupportedScalingCapability((_ctl_display_output_handle_t*)DisplayHandle, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get scaling capability");
            return caps;
        }

        public unsafe ctl_scaling_settings_t GetCurrentScalingNative()
        {
            ThrowIfDisposed();
            var settings = CreateScalingSettings();
            var result = IGCL.ctlGetCurrentScaling((_ctl_display_output_handle_t*)DisplayHandle, &settings);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get current scaling");
            return settings;
        }

        public ScalingSettingsDto GetCurrentScaling()
        {
            var native = GetCurrentScalingNative();
            return ScalingSettingsDto.FromNative(native);
        }

        public unsafe void SetCurrentScalingNative(ctl_scaling_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlSetCurrentScaling((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set scaling");
        }

        public void SetCurrentScaling(ScalingSettingsDto settings)
        {
            SetCurrentScalingNative(settings.ToNative());
        }

        public unsafe ctl_lace_config_t GetLACEConfigNative()
        {
            ThrowIfDisposed();
            var config = CreateLaceConfig();
            var result = IGCL.ctlGetLACEConfig((_ctl_display_output_handle_t*)DisplayHandle, &config);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get LACE config");
            return config;
        }

        public LaceConfigDto GetLACEConfig()
        {
            var native = GetLACEConfigNative();
            return LaceConfigDto.FromNative(native);
        }

        public unsafe void SetLACEConfigNative(ctl_lace_config_t config)
        {
            ThrowIfDisposed();
            var copy = config;
            var result = IGCL.ctlSetLACEConfig((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set LACE config");
        }

        public void SetLACEConfig(LaceConfigDto config)
        {
            SetLACEConfigNative(config.ToNative());
        }

        public unsafe ctl_sw_psr_settings_t SoftwarePSRNative(ctl_sw_psr_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlSoftwarePSR((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set software PSR");
            return copy;
        }

        public SwPsrSettingsDto SoftwarePSR(SwPsrSettingsDto settings)
        {
            var native = SoftwarePSRNative(settings.ToNative());
            return SwPsrSettingsDto.FromNative(native);
        }

        public unsafe ctl_intel_arc_sync_monitor_params_t GetIntelArcSyncInfoForMonitorNative()
        {
            ThrowIfDisposed();
            var parameters = CreateArcSyncMonitorParams();
            var result = IGCL.ctlGetIntelArcSyncInfoForMonitor((_ctl_display_output_handle_t*)DisplayHandle, &parameters);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get Intel Arc Sync info");
            return parameters;
        }

        public IntelArcSyncMonitorParamsDto GetIntelArcSyncInfoForMonitor()
        {
            var native = GetIntelArcSyncInfoForMonitorNative();
            return IntelArcSyncMonitorParamsDto.FromNative(native);
        }

        public unsafe IntPtr[] EnumerateMuxDevices()
        {
            ThrowIfDisposed();
            uint count = 0;
            var result = IGCL.ctlEnumerateMuxDevices((_ctl_api_handle_t*)Api.ApiHandle, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get mux device count");
            if (count == 0)
                return Array.Empty<IntPtr>();

            var muxes = new IntPtr[count];
            fixed (IntPtr* pMuxes = muxes)
            {
                result = IGCL.ctlEnumerateMuxDevices((_ctl_api_handle_t*)Api.ApiHandle, &count, (_ctl_mux_output_handle_t**)pMuxes);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate mux devices");
            }
            return muxes;
        }

        public unsafe (ctl_mux_properties_t properties, IntPtr[] displayOutputs) GetMuxProperties(IntPtr muxHandle)
        {
            ThrowIfDisposed();
            var props = CreateMuxProperties();
            var result = IGCL.ctlGetMuxProperties((_ctl_mux_output_handle_t*)muxHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && props.Count == 0)
                throw new IGCLException(result, "Failed to get mux properties");

            var outputs = Array.Empty<IntPtr>();
            if (props.Count > 0)
            {
                outputs = new IntPtr[props.Count];
                fixed (IntPtr* pOutputs = outputs)
                {
                    props.phDisplayOutputs = (_ctl_display_output_handle_t**)pOutputs;
                    result = IGCL.ctlGetMuxProperties((_ctl_mux_output_handle_t*)muxHandle, &props);
                    props.phDisplayOutputs = null;
                    if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                        throw new IGCLException(result, "Failed to get mux properties");
                }
            }

            return (props, outputs);
        }

        public unsafe void SwitchMux(IntPtr muxHandle, IntPtr inactiveDisplayOutput)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlSwitchMux((_ctl_mux_output_handle_t*)muxHandle, (_ctl_display_output_handle_t*)inactiveDisplayOutput);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to switch mux output");
        }

        public unsafe ctl_intel_arc_sync_profile_params_t GetIntelArcSyncProfile()
        {
            ThrowIfDisposed();
            var parameters = CreateArcSyncProfileParams();
            var result = IGCL.ctlGetIntelArcSyncProfile((_ctl_display_output_handle_t*)DisplayHandle, &parameters);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get Intel Arc Sync profile");
            return parameters;
        }

        public unsafe void SetIntelArcSyncProfile(ctl_intel_arc_sync_profile_params_t parameters)
        {
            ThrowIfDisposed();
            var copy = parameters;
            var result = IGCL.ctlSetIntelArcSyncProfile((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set Intel Arc Sync profile");
        }

        public unsafe ctl_edid_management_args_t EdidManagement(ctl_edid_management_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlEdidManagement((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to perform EDID management");
            return copy;
        }

        public unsafe (ctl_get_set_custom_mode_args_t args, ctl_custom_src_mode_t[] modes) GetSetCustomMode(ctl_get_set_custom_mode_args_t args, ctl_custom_src_mode_t[]? modes = null)
        {
            ThrowIfDisposed();
            var request = args;

            // Set path: caller provided modes to write
            if (modes != null && modes.Length > 0)
            {
                request.NumOfModes = (uint)modes.Length;
                fixed (ctl_custom_src_mode_t* pModes = modes)
                {
                    request.pCustomSrcModeList = pModes;
                    var setResult = IGCL.ctlGetSetCustomMode((_ctl_display_output_handle_t*)DisplayHandle, &request);
                    request.pCustomSrcModeList = null;
                    if (setResult != ctl_result_t.CTL_RESULT_SUCCESS)
                        throw new IGCLException(setResult, "Failed to set custom mode");
                }
                return (request, modes);
            }

            // Get path: two-pass to retrieve modes
            var result = IGCL.ctlGetSetCustomMode((_ctl_display_output_handle_t*)DisplayHandle, &request);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && request.NumOfModes == 0)
                throw new IGCLException(result, "Failed to get custom modes");

            if (request.NumOfModes == 0)
                return (request, Array.Empty<ctl_custom_src_mode_t>());

            var modesOut = new ctl_custom_src_mode_t[request.NumOfModes];
            fixed (ctl_custom_src_mode_t* pModes = modesOut)
            {
                request.pCustomSrcModeList = pModes;
                result = IGCL.ctlGetSetCustomMode((_ctl_display_output_handle_t*)DisplayHandle, &request);
                request.pCustomSrcModeList = null;
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get custom modes");
            }

            return (request, modesOut);
        }

        public unsafe ctl_combined_display_args_t GetSetCombinedDisplayNative(ctl_combined_display_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)AdapterHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set combined display");
            return copy;
        }

        public CombinedDisplayArgsDto GetSetCombinedDisplay(CombinedDisplayArgsDto args)
        {
            var native = GetSetCombinedDisplayNative(args.ToNative());
            return CombinedDisplayArgsDto.FromNative(native);
        }

        public unsafe ctl_genlock_args_t GetSetDisplayGenlockNative(IntPtr[] adapters, ctl_genlock_args_t args, out IntPtr failureAdapter)
        {
            ThrowIfDisposed();
            if (adapters == null || adapters.Length == 0)
                throw new ArgumentException("At least one adapter handle is required", nameof(adapters));

            var copy = args;
            failureAdapter = IntPtr.Zero;
            uint count = (uint)adapters.Length;
            fixed (IntPtr* pAdapters = adapters)
            fixed (IntPtr* pFailure = &failureAdapter)
            {
                var result = IGCL.ctlGetSetDisplayGenlock((_ctl_device_adapter_handle_t**)pAdapters, &copy, count, (_ctl_device_adapter_handle_t**)pFailure);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get/set display genlock");
            }

            return copy;
        }

        public GenlockArgsDto GetSetDisplayGenlock(IntPtr[] adapters, GenlockArgsDto args, out IntPtr failureAdapter)
        {
            var native = GetSetDisplayGenlockNative(adapters, args.ToNative(), out failureAdapter);
            return GenlockArgsDto.FromNative(native);
        }

        public unsafe ctl_vblank_ts_args_t GetVblankTimestamp()
        {
            ThrowIfDisposed();
            var args = CreateVblankTimestampArgs();
            args.NumOfTargets = 16; // max entries in the fixed buffer

            var result = IGCL.ctlGetVblankTimestamp((_ctl_display_output_handle_t*)DisplayHandle, &args);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get vblank timestamp");
            return args;
        }

        public unsafe void LinkDisplayAdapters(ctl_lda_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlLinkDisplayAdapters((_ctl_device_adapter_handle_t*)AdapterHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to link display adapters");
        }

        public unsafe void UnlinkDisplayAdapters()
        {
            ThrowIfDisposed();
            var result = IGCL.ctlUnlinkDisplayAdapters((_ctl_device_adapter_handle_t*)AdapterHandle);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to unlink display adapters");
        }

        public unsafe (ctl_lda_args_t args, IntPtr[] adapters) GetLinkedDisplayAdapters()
        {
            ThrowIfDisposed();
            var args = CreateLinkedDisplayAdaptersArgs();

            // First pass: get count
            var result = IGCL.ctlGetLinkedDisplayAdapters((_ctl_device_adapter_handle_t*)AdapterHandle, &args);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && args.NumAdapters == 0)
                throw new IGCLException(result, "Failed to get linked display adapters");

            if (args.NumAdapters == 0)
                return (args, Array.Empty<IntPtr>());

            var adapters = new IntPtr[args.NumAdapters];
            fixed (IntPtr* pAdapters = adapters)
            {
                args.hLinkedAdapters = (_ctl_device_adapter_handle_t**)pAdapters;
                result = IGCL.ctlGetLinkedDisplayAdapters((_ctl_device_adapter_handle_t*)AdapterHandle, &args);
                args.hLinkedAdapters = null;
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get linked display adapters");
            }

            return (args, adapters);
        }

        public unsafe (ctl_dce_args_t args, uint[] histogram) GetSetDynamicContrastEnhancementNative(ctl_dce_args_t args, uint[]? histogram = null)
        {
            ThrowIfDisposed();
            var request = args;

            // Set path: caller provided histogram to write
            if (histogram != null && histogram.Length > 0)
            {
                request.NumBins = (uint)histogram.Length;
                fixed (uint* pHist = histogram)
                {
                    request.pHistogram = pHist;
                    var setResult = IGCL.ctlGetSetDynamicContrastEnhancement((_ctl_display_output_handle_t*)DisplayHandle, &request);
                    request.pHistogram = null;
                    if (setResult != ctl_result_t.CTL_RESULT_SUCCESS)
                        throw new IGCLException(setResult, "Failed to set dynamic contrast enhancement");
                }
                return (request, histogram);
            }

            // Get path: first call to discover NumBins
            var result = IGCL.ctlGetSetDynamicContrastEnhancement((_ctl_display_output_handle_t*)DisplayHandle, &request);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && request.NumBins == 0)
                throw new IGCLException(result, "Failed to get dynamic contrast enhancement");

            if (request.NumBins == 0)
                return (request, Array.Empty<uint>());

            var bins = new uint[request.NumBins];
            fixed (uint* pBins = bins)
            {
                request.pHistogram = pBins;
                result = IGCL.ctlGetSetDynamicContrastEnhancement((_ctl_display_output_handle_t*)DisplayHandle, &request);
                request.pHistogram = null;
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get dynamic contrast enhancement");
            }

            return (request, bins);
        }

        public (DceArgsDto args, uint[] histogram) GetSetDynamicContrastEnhancement(DceArgsDto args, uint[]? histogram = null)
        {
            var result = GetSetDynamicContrastEnhancementNative(args.ToNative(), histogram);
            return (DceArgsDto.FromNative(result.args), result.histogram);
        }

        public unsafe ctl_get_set_wire_format_config_t GetSetWireFormat(ctl_get_set_wire_format_config_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlGetSetWireFormat((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set wire format");
            return copy;
        }

        public unsafe ctl_display_settings_t GetSetDisplaySettingsNative(ctl_display_settings_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlGetSetDisplaySettings((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set display settings");
            return copy;
        }

        public DisplaySettingsDto GetSetDisplaySettings(DisplaySettingsDto args)
        {
            var native = GetSetDisplaySettingsNative(args.ToNative());
            return DisplaySettingsDto.FromNative(native);
        }


        internal void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLDisplayHelper));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCLDisplayDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    public struct AdapterDisplayEncoderPropertiesDto
    {
        public uint Size;
        public byte Version;
        public ctl_os_display_encoder_identifier_t OsDisplayEncoderHandle;
        public ctl_display_output_types_t Type;
        public bool IsOnBoardProtocolConverterOutputPresent;
        public ctl_revision_datatype_t SupportedSpec;
        public uint SupportedOutputBpcFlags;
        public uint EncoderConfigFlags;
        public uint FeatureSupportedFlags;
        public uint AdvancedFeatureSupportedFlags;
        public ctl_adapter_display_encoder_properties_t._ReservedFields_e__FixedBuffer ReservedFields;

        public static AdapterDisplayEncoderPropertiesDto FromNative(ctl_adapter_display_encoder_properties_t native)
        {
            return new AdapterDisplayEncoderPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                OsDisplayEncoderHandle = native.Os_display_encoder_handle,
                Type = native.Type,
                IsOnBoardProtocolConverterOutputPresent = IGCLDisplayDtoBool.ToBool(native.IsOnBoardProtocolConverterOutputPresent),
                SupportedSpec = native.SupportedSpec,
                SupportedOutputBpcFlags = native.SupportedOutputBPCFlags,
                EncoderConfigFlags = native.EncoderConfigFlags,
                FeatureSupportedFlags = native.FeatureSupportedFlags,
                AdvancedFeatureSupportedFlags = native.AdvancedFeatureSupportedFlags,
                ReservedFields = native.ReservedFields
            };
        }

        public ctl_adapter_display_encoder_properties_t ToNative()
        {
            return new ctl_adapter_display_encoder_properties_t
            {
                Size = Size,
                Version = Version,
                Os_display_encoder_handle = OsDisplayEncoderHandle,
                Type = Type,
                IsOnBoardProtocolConverterOutputPresent = IGCLDisplayDtoBool.ToByte(IsOnBoardProtocolConverterOutputPresent),
                SupportedSpec = SupportedSpec,
                SupportedOutputBPCFlags = SupportedOutputBpcFlags,
                EncoderConfigFlags = EncoderConfigFlags,
                FeatureSupportedFlags = FeatureSupportedFlags,
                AdvancedFeatureSupportedFlags = AdvancedFeatureSupportedFlags,
                ReservedFields = ReservedFields
            };
        }
    }

    public unsafe struct CombinedDisplayArgsDto
    {
        public uint Size;
        public byte Version;
        public ctl_combined_display_optype_t OpType;
        public bool IsSupported;
        public byte NumOutputs;
        public uint CombinedDesktopWidth;
        public uint CombinedDesktopHeight;
        public IntPtr ChildInfo;
        public IntPtr CombinedDisplayOutput;

        public static CombinedDisplayArgsDto FromNative(ctl_combined_display_args_t native)
        {
            return new CombinedDisplayArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                OpType = native.OpType,
                IsSupported = IGCLDisplayDtoBool.ToBool(native.IsSupported),
                NumOutputs = native.NumOutputs,
                CombinedDesktopWidth = native.CombinedDesktopWidth,
                CombinedDesktopHeight = native.CombinedDesktopHeight,
                ChildInfo = (IntPtr)native.pChildInfo,
                CombinedDisplayOutput = (IntPtr)native.hCombinedDisplayOutput
            };
        }

        public unsafe ctl_combined_display_args_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_combined_display_args_t);

            return new ctl_combined_display_args_t
            {
                Size = size,
                Version = Version,
                OpType = OpType,
                IsSupported = IGCLDisplayDtoBool.ToByte(IsSupported),
                NumOutputs = NumOutputs,
                CombinedDesktopWidth = CombinedDesktopWidth,
                CombinedDesktopHeight = CombinedDesktopHeight,
                pChildInfo = (ctl_combined_display_child_info_t*)ChildInfo,
                hCombinedDisplayOutput = (_ctl_display_output_handle_t*)CombinedDisplayOutput
            };
        }
    }

    public unsafe struct DceArgsDto
    {
        public uint Size;
        public byte Version;
        public bool Set;
        public uint TargetBrightnessPercent;
        public double PhaseinSpeedMultiplier;
        public uint NumBins;
        public bool Enable;
        public bool IsSupported;
        public IntPtr Histogram;

        public static DceArgsDto FromNative(ctl_dce_args_t native)
        {
            return new DceArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                Set = IGCLDisplayDtoBool.ToBool(native.Set),
                TargetBrightnessPercent = native.TargetBrightnessPercent,
                PhaseinSpeedMultiplier = native.PhaseinSpeedMultiplier,
                NumBins = native.NumBins,
                Enable = IGCLDisplayDtoBool.ToBool(native.Enable),
                IsSupported = IGCLDisplayDtoBool.ToBool(native.IsSupported),
                Histogram = (IntPtr)native.pHistogram
            };
        }

        public unsafe ctl_dce_args_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_dce_args_t);

            return new ctl_dce_args_t
            {
                Size = size,
                Version = Version,
                Set = IGCLDisplayDtoBool.ToByte(Set),
                TargetBrightnessPercent = TargetBrightnessPercent,
                PhaseinSpeedMultiplier = PhaseinSpeedMultiplier,
                NumBins = NumBins,
                Enable = IGCLDisplayDtoBool.ToByte(Enable),
                IsSupported = IGCLDisplayDtoBool.ToByte(IsSupported),
                pHistogram = (uint*)Histogram
            };
        }
    }

    public struct DisplaySettingsDto
    {
        public uint Size;
        public byte Version;
        public bool Set;
        public uint SupportedFlags;
        public uint ControllableFlags;
        public uint ValidFlags;
        public ctl_display_setting_low_latency_t LowLatency;
        public ctl_display_setting_sourcetm_t SourceTm;
        public ctl_display_setting_content_type_t ContentType;
        public ctl_display_setting_quantization_range_t QuantizationRange;
        public uint SupportedPictureAr;
        public ctl_display_setting_picture_ar_flag_t PictureAr;
        public ctl_display_setting_audio_t AudioSettings;
        public ctl_display_settings_t._Reserved_e__FixedBuffer Reserved;

        public static DisplaySettingsDto FromNative(ctl_display_settings_t native)
        {
            return new DisplaySettingsDto
            {
                Size = native.Size,
                Version = native.Version,
                Set = IGCLDisplayDtoBool.ToBool(native.Set),
                SupportedFlags = native.SupportedFlags,
                ControllableFlags = native.ControllableFlags,
                ValidFlags = native.ValidFlags,
                LowLatency = native.LowLatency,
                SourceTm = native.SourceTM,
                ContentType = native.ContentType,
                QuantizationRange = native.QuantizationRange,
                SupportedPictureAr = native.SupportedPictureAR,
                PictureAr = native.PictureAR,
                AudioSettings = native.AudioSettings,
                Reserved = native.Reserved
            };
        }

        public unsafe ctl_display_settings_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_display_settings_t);

            return new ctl_display_settings_t
            {
                Size = size,
                Version = Version,
                Set = IGCLDisplayDtoBool.ToByte(Set),
                SupportedFlags = SupportedFlags,
                ControllableFlags = ControllableFlags,
                ValidFlags = ValidFlags,
                LowLatency = LowLatency,
                SourceTM = SourceTm,
                ContentType = ContentType,
                QuantizationRange = QuantizationRange,
                SupportedPictureAR = SupportedPictureAr,
                PictureAR = PictureAr,
                AudioSettings = AudioSettings,
                Reserved = Reserved
            };
        }
    }

    public struct GenlockArgsDto
    {
        public uint Size;
        public byte Version;
        public ctl_genlock_operation_t Operation;
        public ctl_genlock_topology_t GenlockTopology;
        public bool IsGenlockEnabled;
        public bool IsGenlockPossible;

        public static GenlockArgsDto FromNative(ctl_genlock_args_t native)
        {
            return new GenlockArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                Operation = native.Operation,
                GenlockTopology = native.GenlockTopology,
                IsGenlockEnabled = IGCLDisplayDtoBool.ToBool(native.IsGenlockEnabled),
                IsGenlockPossible = IGCLDisplayDtoBool.ToBool(native.IsGenlockPossible)
            };
        }

        public unsafe ctl_genlock_args_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_genlock_args_t);

            return new ctl_genlock_args_t
            {
                Size = size,
                Version = Version,
                Operation = Operation,
                GenlockTopology = GenlockTopology,
                IsGenlockEnabled = IGCLDisplayDtoBool.ToByte(IsGenlockEnabled),
                IsGenlockPossible = IGCLDisplayDtoBool.ToByte(IsGenlockPossible)
            };
        }
    }

    public struct IntelArcSyncMonitorParamsDto
    {
        public uint Size;
        public byte Version;
        public bool IsIntelArcSyncSupported;
        public float MinimumRefreshRateInHz;
        public float MaximumRefreshRateInHz;
        public uint MaxFrameTimeIncreaseInUs;
        public uint MaxFrameTimeDecreaseInUs;

        public static IntelArcSyncMonitorParamsDto FromNative(ctl_intel_arc_sync_monitor_params_t native)
        {
            return new IntelArcSyncMonitorParamsDto
            {
                Size = native.Size,
                Version = native.Version,
                IsIntelArcSyncSupported = IGCLDisplayDtoBool.ToBool(native.IsIntelArcSyncSupported),
                MinimumRefreshRateInHz = native.MinimumRefreshRateInHz,
                MaximumRefreshRateInHz = native.MaximumRefreshRateInHz,
                MaxFrameTimeIncreaseInUs = native.MaxFrameTimeIncreaseInUs,
                MaxFrameTimeDecreaseInUs = native.MaxFrameTimeDecreaseInUs
            };
        }

        public unsafe ctl_intel_arc_sync_monitor_params_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_intel_arc_sync_monitor_params_t);

            return new ctl_intel_arc_sync_monitor_params_t
            {
                Size = size,
                Version = Version,
                IsIntelArcSyncSupported = IGCLDisplayDtoBool.ToByte(IsIntelArcSyncSupported),
                MinimumRefreshRateInHz = MinimumRefreshRateInHz,
                MaximumRefreshRateInHz = MaximumRefreshRateInHz,
                MaxFrameTimeIncreaseInUs = MaxFrameTimeIncreaseInUs,
                MaxFrameTimeDecreaseInUs = MaxFrameTimeDecreaseInUs
            };
        }
    }

    public struct LaceConfigDto
    {
        public uint Size;
        public byte Version;
        public bool Enabled;
        public uint OpTypeGet;
        public ctl_set_operation_t OpTypeSet;
        public uint Trigger;
        public ctl_lace_aggr_config_t LaceConfig;

        public static LaceConfigDto FromNative(ctl_lace_config_t native)
        {
            return new LaceConfigDto
            {
                Size = native.Size,
                Version = native.Version,
                Enabled = IGCLDisplayDtoBool.ToBool(native.Enabled),
                OpTypeGet = native.OpTypeGet,
                OpTypeSet = native.OpTypeSet,
                Trigger = native.Trigger,
                LaceConfig = native.LaceConfig
            };
        }

        public unsafe ctl_lace_config_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_lace_config_t);

            return new ctl_lace_config_t
            {
                Size = size,
                Version = Version,
                Enabled = IGCLDisplayDtoBool.ToByte(Enabled),
                OpTypeGet = OpTypeGet,
                OpTypeSet = OpTypeSet,
                Trigger = Trigger,
                LaceConfig = LaceConfig
            };
        }
    }

    public struct RetroScalingSettingsDto
    {
        public uint Size;
        public byte Version;
        public bool Get;
        public bool Enable;
        public uint RetroScalingType;

        public static RetroScalingSettingsDto FromNative(ctl_retro_scaling_settings_t native)
        {
            return new RetroScalingSettingsDto
            {
                Size = native.Size,
                Version = native.Version,
                Get = IGCLDisplayDtoBool.ToBool(native.Get),
                Enable = IGCLDisplayDtoBool.ToBool(native.Enable),
                RetroScalingType = native.RetroScalingType
            };
        }

        public unsafe ctl_retro_scaling_settings_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_retro_scaling_settings_t);

            return new ctl_retro_scaling_settings_t
            {
                Size = size,
                Version = Version,
                Get = IGCLDisplayDtoBool.ToByte(Get),
                Enable = IGCLDisplayDtoBool.ToByte(Enable),
                RetroScalingType = RetroScalingType
            };
        }
    }

    public struct ScalingSettingsDto
    {
        public uint Size;
        public byte Version;
        public bool Enable;
        public uint ScalingType;
        public uint CustomScalingX;
        public uint CustomScalingY;
        public bool HardwareModeSet;
        public uint PreferredScalingType;

        public static ScalingSettingsDto FromNative(ctl_scaling_settings_t native)
        {
            return new ScalingSettingsDto
            {
                Size = native.Size,
                Version = native.Version,
                Enable = IGCLDisplayDtoBool.ToBool(native.Enable),
                ScalingType = native.ScalingType,
                CustomScalingX = native.CustomScalingX,
                CustomScalingY = native.CustomScalingY,
                HardwareModeSet = IGCLDisplayDtoBool.ToBool(native.HardwareModeSet),
                PreferredScalingType = native.PreferredScalingType
            };
        }

        public unsafe ctl_scaling_settings_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_scaling_settings_t);

            return new ctl_scaling_settings_t
            {
                Size = size,
                Version = Version,
                Enable = IGCLDisplayDtoBool.ToByte(Enable),
                ScalingType = ScalingType,
                CustomScalingX = CustomScalingX,
                CustomScalingY = CustomScalingY,
                HardwareModeSet = IGCLDisplayDtoBool.ToByte(HardwareModeSet),
                PreferredScalingType = PreferredScalingType
            };
        }
    }

    public struct SharpnessSettingsDto
    {
        public uint Size;
        public byte Version;
        public bool Enable;
        public uint FilterType;
        public float Intensity;

        public static SharpnessSettingsDto FromNative(ctl_sharpness_settings_t native)
        {
            return new SharpnessSettingsDto
            {
                Size = native.Size,
                Version = native.Version,
                Enable = IGCLDisplayDtoBool.ToBool(native.Enable),
                FilterType = native.FilterType,
                Intensity = native.Intensity
            };
        }

        public unsafe ctl_sharpness_settings_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_sharpness_settings_t);

            return new ctl_sharpness_settings_t
            {
                Size = size,
                Version = Version,
                Enable = IGCLDisplayDtoBool.ToByte(Enable),
                FilterType = FilterType,
                Intensity = Intensity
            };
        }
    }

    public struct SwPsrSettingsDto
    {
        public uint Size;
        public byte Version;
        public bool Set;
        public bool Supported;
        public bool Enable;

        public static SwPsrSettingsDto FromNative(ctl_sw_psr_settings_t native)
        {
            return new SwPsrSettingsDto
            {
                Size = native.Size,
                Version = native.Version,
                Set = IGCLDisplayDtoBool.ToBool(native.Set),
                Supported = IGCLDisplayDtoBool.ToBool(native.Supported),
                Enable = IGCLDisplayDtoBool.ToBool(native.Enable)
            };
        }

        public unsafe ctl_sw_psr_settings_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_sw_psr_settings_t);

            return new ctl_sw_psr_settings_t
            {
                Size = size,
                Version = Version,
                Set = IGCLDisplayDtoBool.ToByte(Set),
                Supported = IGCLDisplayDtoBool.ToByte(Supported),
                Enable = IGCLDisplayDtoBool.ToByte(Enable)
            };
        }
    }

    public struct PowerOptimizationSettingsDto
    {
        public uint Size;
        public byte Version;
        public ctl_power_optimization_plan_t PowerOptimizationPlan;
        public uint PowerOptimizationFeature;
        public bool Enable;
        public ctl_power_optimization_feature_specific_info_t FeatureSpecificData;
        public ctl_power_source_t PowerSource;

        public static PowerOptimizationSettingsDto FromNative(ctl_power_optimization_settings_t native)
        {
            return new PowerOptimizationSettingsDto
            {
                Size = native.Size,
                Version = native.Version,
                PowerOptimizationPlan = native.PowerOptimizationPlan,
                PowerOptimizationFeature = native.PowerOptimizationFeature,
                Enable = IGCLDisplayDtoBool.ToBool(native.Enable),
                FeatureSpecificData = native.FeatureSpecificData,
                PowerSource = native.PowerSource
            };
        }

        public unsafe ctl_power_optimization_settings_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_power_optimization_settings_t);

            return new ctl_power_optimization_settings_t
            {
                Size = size,
                Version = Version,
                PowerOptimizationPlan = PowerOptimizationPlan,
                PowerOptimizationFeature = PowerOptimizationFeature,
                Enable = IGCLDisplayDtoBool.ToByte(Enable),
                FeatureSpecificData = FeatureSpecificData,
                PowerSource = PowerSource
            };
        }
    }
}
