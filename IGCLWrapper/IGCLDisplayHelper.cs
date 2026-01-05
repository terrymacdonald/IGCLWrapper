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

        public unsafe ctl_display_properties_t GetProperties()
        {
            ThrowIfDisposed();
            lock (_lock)
            {
                if (_properties.HasValue)
                {
                    return _properties.Value;
                }

                var props = IGCLApiHelper.CreateDisplayProperties();
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
            var props = IGCLApiHelper.CreateAdapterProperties();
            var result = IGCL.ctlGetDeviceProperties((_ctl_device_adapter_handle_t*)AdapterHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get device properties");
            return props;
        }

        public unsafe ctl_adapter_display_encoder_properties_t GetAdapterDisplayEncoderProperties()
        {
            ThrowIfDisposed();
            var props = new ctl_adapter_display_encoder_properties_t { Size = (uint)sizeof(ctl_adapter_display_encoder_properties_t), Version = 0 };
            var result = IGCL.ctlGetAdaperDisplayEncoderProperties((_ctl_display_output_handle_t*)DisplayHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get adapter display encoder properties");
            return props;
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

        public unsafe ctl_sharpness_caps_t GetSharpnessCaps(ctl_sharpness_caps_t caps)
        {
            ThrowIfDisposed();
            var copy = caps;
            var result = IGCL.ctlGetSharpnessCaps((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get sharpness caps");
            return copy;
        }

        public unsafe ctl_sharpness_settings_t GetCurrentSharpness(ctl_sharpness_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlGetCurrentSharpness((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get current sharpness");
            return copy;
        }

        public unsafe void SetCurrentSharpness(ctl_sharpness_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlSetCurrentSharpness((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set sharpness");
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

        public unsafe ctl_power_optimization_caps_t GetPowerOptimizationCaps(ctl_power_optimization_caps_t caps)
        {
            ThrowIfDisposed();
            var copy = caps;
            var result = IGCL.ctlGetPowerOptimizationCaps((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power optimization caps");
            return copy;
        }

        public unsafe ctl_power_optimization_settings_t GetPowerOptimizationSetting(ctl_power_optimization_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlGetPowerOptimizationSetting((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power optimization settings");
            return copy;
        }

        public unsafe void SetPowerOptimizationSetting(ctl_power_optimization_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlSetPowerOptimizationSetting((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set power optimization settings");
        }

        public unsafe void SetBrightnessSetting(ctl_set_brightness_t brightness)
        {
            ThrowIfDisposed();
            var copy = brightness;
            var result = IGCL.ctlSetBrightnessSetting((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set brightness");
        }

        public unsafe ctl_get_brightness_t GetBrightnessSetting(ctl_get_brightness_t brightness)
        {
            ThrowIfDisposed();
            var copy = brightness;
            var result = IGCL.ctlGetBrightnessSetting((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get brightness");
            return copy;
        }

        public unsafe ctl_pixtx_pipe_get_config_t PixelTransformationGetConfig(ctl_pixtx_pipe_get_config_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlPixelTransformationGetConfig((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get pixel transformation config");
            return copy;
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

        public unsafe ctl_retro_scaling_caps_t GetSupportedRetroScalingCapability(ctl_retro_scaling_caps_t caps)
        {
            ThrowIfDisposed();
            var copy = caps;
            var result = IGCL.ctlGetSupportedRetroScalingCapability((_ctl_device_adapter_handle_t*)AdapterHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get retro scaling capability");
            return copy;
        }

        public unsafe ctl_retro_scaling_settings_t GetSetRetroScaling(ctl_retro_scaling_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlGetSetRetroScaling((_ctl_device_adapter_handle_t*)AdapterHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set retro scaling");
            return copy;
        }

        public unsafe ctl_scaling_caps_t GetSupportedScalingCapability(ctl_scaling_caps_t caps)
        {
            ThrowIfDisposed();
            var copy = caps;
            var result = IGCL.ctlGetSupportedScalingCapability((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get scaling capability");
            return copy;
        }

        public unsafe ctl_scaling_settings_t GetCurrentScaling(ctl_scaling_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlGetCurrentScaling((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get current scaling");
            return copy;
        }

        public unsafe void SetCurrentScaling(ctl_scaling_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlSetCurrentScaling((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set scaling");
        }

        public unsafe ctl_lace_config_t GetLACEConfig(ctl_lace_config_t config)
        {
            ThrowIfDisposed();
            var copy = config;
            var result = IGCL.ctlGetLACEConfig((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get LACE config");
            return copy;
        }

        public unsafe void SetLACEConfig(ctl_lace_config_t config)
        {
            ThrowIfDisposed();
            var copy = config;
            var result = IGCL.ctlSetLACEConfig((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set LACE config");
        }

        public unsafe ctl_sw_psr_settings_t SoftwarePSR(ctl_sw_psr_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlSoftwarePSR((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set software PSR");
            return copy;
        }

        public unsafe ctl_intel_arc_sync_monitor_params_t GetIntelArcSyncInfoForMonitor(ctl_intel_arc_sync_monitor_params_t parameters)
        {
            ThrowIfDisposed();
            var copy = parameters;
            var result = IGCL.ctlGetIntelArcSyncInfoForMonitor((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get Intel Arc Sync info");
            return copy;
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
            var props = IGCLApiHelper.Init<ctl_mux_properties_t>();
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

        public unsafe ctl_intel_arc_sync_profile_params_t GetIntelArcSyncProfile(ctl_intel_arc_sync_profile_params_t parameters)
        {
            ThrowIfDisposed();
            var copy = parameters;
            var result = IGCL.ctlGetIntelArcSyncProfile((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get Intel Arc Sync profile");
            return copy;
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

        public unsafe ctl_get_set_custom_mode_args_t GetSetCustomMode(ctl_get_set_custom_mode_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlGetSetCustomMode((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set custom mode");
            return copy;
        }

        public unsafe ctl_combined_display_args_t GetSetCombinedDisplay(ctl_combined_display_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)AdapterHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set combined display");
            return copy;
        }

        public unsafe ctl_genlock_args_t GetSetDisplayGenlock(IntPtr[] adapters, ctl_genlock_args_t args, out IntPtr failureAdapter)
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

        public unsafe ctl_vblank_ts_args_t GetVblankTimestamp(ctl_vblank_ts_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlGetVblankTimestamp((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get vblank timestamp");
            return copy;
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

        public unsafe ctl_lda_args_t GetLinkedDisplayAdapters(ctl_lda_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlGetLinkedDisplayAdapters((_ctl_device_adapter_handle_t*)AdapterHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get linked display adapters");
            return copy;
        }

        public unsafe ctl_dce_args_t GetSetDynamicContrastEnhancement(ctl_dce_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlGetSetDynamicContrastEnhancement((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set dynamic contrast enhancement");
            return copy;
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

        public unsafe ctl_display_settings_t GetSetDisplaySettings(ctl_display_settings_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlGetSetDisplaySettings((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set display settings");
            return copy;
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
}
