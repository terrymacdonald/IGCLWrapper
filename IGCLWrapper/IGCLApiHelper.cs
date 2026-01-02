using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <summary>
    /// High-level IGCL entry point that owns the native API handle and exposes common helpers and feature factories.
    /// </summary>
    public sealed class IGCLApiHelper : IDisposable
    {
        private IGCLApi? _api;
        private bool _disposed;

        private IGCLApiHelper(IGCLApi api)
        {
            _api = api;
        }

        /// <summary>
        /// Initialize IGCL and return a helper that owns the API lifetime.
        /// </summary>
        public static IGCLApiHelper Initialize()
        {
            return new IGCLApiHelper(IGCLApi.Initialize());
        }

        /// <summary>
        /// Expose DLL availability check without requiring direct use of IGCLApi.
        /// </summary>
        public static bool IsIGCLDllAvailable(out string errorMessage)
        {
            return IGCLApi.IsIGCLDllAvailable(out errorMessage);
        }

        #region Version helpers
        public static uint MakeVersion(uint major, uint minor) => IGCLApi.MakeVersion(major, minor);
        public static uint GetMajorVersion(uint version) => IGCLApi.GetMajorVersion(version);
        public static uint GetMinorVersion(uint version) => IGCLApi.GetMinorVersion(version);
        public static uint GetImplVersion() => IGCLApi.GetImplVersion();
        #endregion

        #region Struct initialization helpers
        public static unsafe T Init<T>() where T : unmanaged
        {
            var value = default(T);
            var sizePtr = (uint*)&value;
            *sizePtr = (uint)sizeof(T);
            if (sizeof(T) > sizeof(uint))
            {
                var versionPtr = (byte*)((byte*)&value + sizeof(uint));
                *versionPtr = 0;
            }
            return value;
        }

        public static unsafe ctl_init_args_t CreateInitArgs()
        {
            var args = Init<ctl_init_args_t>();
            args.AppVersion = GetImplVersion();
            args.flags = (uint)ctl_init_flag_t.CTL_INIT_FLAG_USE_LEVEL_ZERO;
            args.SupportedVersion = GetImplVersion();
            args.ApplicationUID = default;
            return args;
        }

        public static unsafe ctl_device_adapter_properties_t CreateAdapterProperties()
        {
            var props = Init<ctl_device_adapter_properties_t>();
            props.Version = 1;
            return props;
        }

        public static unsafe ctl_display_properties_t CreateDisplayProperties()
        {
            var props = Init<ctl_display_properties_t>();
            props.Version = 0;
            return props;
        }

        public static unsafe ctl_3d_feature_caps_t Create3DFeatureCaps() => Init<ctl_3d_feature_caps_t>();
        public static unsafe ctl_3d_feature_getset_t Create3DFeatureGetSet(ctl_3d_feature_t feature) => new ctl_3d_feature_getset_t { Size = (uint)sizeof(ctl_3d_feature_getset_t), Version = 0, FeatureType = feature };
        public static unsafe ctl_power_telemetry_t CreatePowerTelemetry() => Init<ctl_power_telemetry_t>();
        public static unsafe ctl_ecc_properties_t CreateEccProperties() => Init<ctl_ecc_properties_t>();
        public static unsafe ctl_ecc_state_desc_t CreateEccState() => Init<ctl_ecc_state_desc_t>();
        public static unsafe ctl_engine_properties_t CreateEngineProperties() => Init<ctl_engine_properties_t>();
        public static unsafe ctl_engine_stats_t CreateEngineStats() => Init<ctl_engine_stats_t>();
        public static unsafe ctl_fan_properties_t CreateFanProperties() => Init<ctl_fan_properties_t>();
        public static unsafe ctl_fan_config_t CreateFanConfig() => Init<ctl_fan_config_t>();
        public static unsafe ctl_fan_speed_t CreateFanSpeed() => Init<ctl_fan_speed_t>();
        public static unsafe ctl_fan_speed_table_t CreateFanSpeedTable() => Init<ctl_fan_speed_table_t>();
        public static unsafe ctl_firmware_properties_t CreateFirmwareProperties() => Init<ctl_firmware_properties_t>();
        public static unsafe ctl_firmware_component_properties_t CreateFirmwareComponentProperties() => Init<ctl_firmware_component_properties_t>();
        public static unsafe ctl_freq_properties_t CreateFrequencyProperties() => Init<ctl_freq_properties_t>();
        public static unsafe ctl_freq_range_t CreateFrequencyRange() => Init<ctl_freq_range_t>();
        public static unsafe ctl_freq_state_t CreateFrequencyState() => Init<ctl_freq_state_t>();
        public static unsafe ctl_freq_throttle_time_t CreateFrequencyThrottleTime() => Init<ctl_freq_throttle_time_t>();
        public static unsafe ctl_led_properties_t CreateLedProperties() => Init<ctl_led_properties_t>();
        public static unsafe ctl_led_state_t CreateLedState() => new ctl_led_state_t { Size = (uint)sizeof(ctl_led_state_t), Version = 0, color = new ctl_led_color_t { Size = (uint)sizeof(ctl_led_color_t), Version = 0 } };
        public static unsafe ctl_video_processing_feature_caps_t CreateVideoProcessingCaps() => Init<ctl_video_processing_feature_caps_t>();
        public static unsafe ctl_video_processing_feature_getset_t CreateVideoProcessingGetSet() => Init<ctl_video_processing_feature_getset_t>();
        public static unsafe ctl_mem_properties_t CreateMemoryProperties() => Init<ctl_mem_properties_t>();
        public static unsafe ctl_mem_state_t CreateMemoryState() => Init<ctl_mem_state_t>();
        public static unsafe ctl_mem_bandwidth_t CreateMemoryBandwidth() => Init<ctl_mem_bandwidth_t>();
        public static unsafe ctl_oc_properties_t CreateOverclockProperties() => Init<ctl_oc_properties_t>();
        public static unsafe ctl_oc_vf_pair_t CreateVfPair() => Init<ctl_oc_vf_pair_t>();
        public static unsafe ctl_power_properties_t CreatePowerProperties() => Init<ctl_power_properties_t>();
        public static unsafe ctl_power_energy_counter_t CreatePowerEnergyCounter() => Init<ctl_power_energy_counter_t>();
        public static unsafe ctl_power_limits_t CreatePowerLimits() => Init<ctl_power_limits_t>();
        public static unsafe ctl_temp_properties_t CreateTemperatureProperties() => Init<ctl_temp_properties_t>();
        #endregion

        public IReadOnlyList<IGCLAdapterHelper> EnumerateAdapters()
        {
            ThrowIfDisposed();
            var handles = _api!.EnumerateAdapters();
            var adapters = new List<IGCLAdapterHelper>(handles.Length);
            foreach (var handle in handles)
            {
                adapters.Add(new IGCLAdapterHelper(this, handle));
            }
            return adapters;
        }

        internal IntPtr[] EnumerateDisplays(IntPtr adapterHandle)
        {
            ThrowIfDisposed();
            return _api!.EnumerateDisplays(adapterHandle);
        }

        internal IGCLApi Api => _api ?? throw new ObjectDisposedException(nameof(IGCLApiHelper));

        #region Feature helper factories
        public IGCL3DHelper Get3DHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCL3DHelper(this, h));
        public IGCLEccHelper GetEccHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLEccHelper(this, h));
        public IGCLEngineHelper GetEngineHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLEngineHelper(this, h));
        public IGCLFanHelper GetFanHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLFanHelper(this, h));
        public IGCLFirmwareHelper GetFirmwareHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLFirmwareHelper(this, h));
        public IGCLFrequencyHelper GetFrequencyHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLFrequencyHelper(this, h));
        public IGCLLedHelper GetLedHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLLedHelper(this, h));
        public IGCLMediaHelper GetMediaHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLMediaHelper(this, h));
        public IGCLMemoryHelper GetMemoryHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLMemoryHelper(this, h));
        public IGCLOverclockHelper GetOverclockHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLOverclockHelper(this, h));
        public IGCLPciHelper GetPciHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLPciHelper(this, h));
        public IGCLPowerHelper GetPowerHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLPowerHelper(this, h));
        public IGCLTemperatureHelper GetTemperatureHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLTemperatureHelper(this, h));

        private TFeature CreateAdapterFeatureHelper<TFeature>(IGCLAdapterHelper adapter, Func<IntPtr, TFeature> factory)
        {
            if (adapter == null)
                throw new ArgumentNullException(nameof(adapter));

            ThrowIfDisposed();
            adapter.ThrowIfDisposed();
            return factory(adapter.AdapterHandle);
        }
        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (_api != null)
            {
                _api.Dispose();
                _api = null;
            }

            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLApiHelper));
        }
    }

    public sealed class IGCLAdapterHelper : IDisposable
    {
        private readonly object _lock = new();
        private ctl_device_adapter_properties_t? _properties;
        private bool _disposed;
        internal IGCLApiHelper Api { get; }
        internal IntPtr AdapterHandle { get; }

        internal IGCLAdapterHelper(IGCLApiHelper api, IntPtr adapterHandle)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            AdapterHandle = adapterHandle;
        }

        public unsafe ctl_device_adapter_properties_t GetProperties()
        {
            ThrowIfDisposed();
            lock (_lock)
            {
                if (_properties.HasValue)
                {
                    return _properties.Value;
                }

                var props = IGCLApiHelper.CreateAdapterProperties();
                var result = IGCL.ctlGetDeviceProperties((_ctl_device_adapter_handle_t*)AdapterHandle, &props);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    throw new IGCLException(result, "Failed to get adapter properties");
                }

                _properties = props;
                return props;
            }
        }

        public IReadOnlyList<IGCLDisplayHelper> GetDisplays()
        {
            ThrowIfDisposed();
            var handles = Api.EnumerateDisplays(AdapterHandle);
            var displays = new List<IGCLDisplayHelper>(handles.Length);
            foreach (var h in handles)
            {
                displays.Add(new IGCLDisplayHelper(Api, h));
            }
            return displays;
        }

        public unsafe string Name
        {
            get
            {
                var props = GetProperties();
                var pName = (sbyte*)Unsafe.AsPointer(ref props.name);
                return new string(pName);
            }
        }

        public string PciVendorId => GetProperties().pci_vendor_id.ToString("X4");

        internal void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLAdapterHelper));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }

}
