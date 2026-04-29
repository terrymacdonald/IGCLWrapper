using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Net.Mail;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace IGCLWrapper
{
    /// <summary>
    /// High-level IGCL entry point that owns the native API handle and exposes common helpers and feature factories.
    /// </summary>
    public sealed class IGCLApiHelper : IDisposable
    {
        private IGCLApi? _api;
        private bool _disposed;
        private static unsafe ctl_runtime_path_args_t CreateRuntimePathArgs() => new ctl_runtime_path_args_t { Size = (uint)sizeof(ctl_runtime_path_args_t), Version = 0 };

        private IGCLApiHelper(IGCLApi api)
        {
            _api = api;
        }

        /// <summary>
        /// Initialize IGCL and return a helper that owns the API lifetime.
        /// </summary>
        /// <returns>Initialized API helper.</returns>
        public static IGCLApiHelper Initialize()
        {
            return new IGCLApiHelper(IGCLApi.Initialize());
        }

        /// <summary>
        /// Expose DLL availability check without requiring direct use of IGCLApi.
        /// </summary>
        /// <param name="errorMessage">Details about why the DLL could not be loaded.</param>
        /// <returns>True if the IGCL DLL can be loaded; otherwise, false.</returns>
        public static bool IsIGCLDllAvailable(out string errorMessage)
        {
            return IGCLApi.IsIGCLDllAvailable(out errorMessage);
        }

        #region Version helpers
        /// <summary>
        /// Create a version value from major and minor components.
        /// </summary>
        /// <param name="major">Major version.</param>
        /// <param name="minor">Minor version.</param>
        /// <returns>Combined version value.</returns>
        public static uint MakeVersion(uint major, uint minor) => IGCLApi.MakeVersion(major, minor);
        /// <summary>
        /// Extract the major version from a combined value.
        /// </summary>
        /// <param name="version">Combined version value.</param>
        /// <returns>Major version.</returns>
        public static uint GetMajorVersion(uint version) => IGCLApi.GetMajorVersion(version);
        /// <summary>
        /// Extract the minor version from a combined value.
        /// </summary>
        /// <param name="version">Combined version value.</param>
        /// <returns>Minor version.</returns>
        public static uint GetMinorVersion(uint version) => IGCLApi.GetMinorVersion(version);
        /// <summary>
        /// Get the IGCL implementation version.
        /// </summary>
        /// <returns>Implementation version value.</returns>
        public static uint GetImplVersion() => IGCLApi.GetImplVersion();
        #endregion

        /// <summary>
        /// Enumerate adapter helpers for all detected Intel GPU adapters.
        /// </summary>
        /// <returns>Read-only list of adapter helpers.</returns>
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

        /// <summary>
        /// Enumerate display output handles for the specified adapter.
        /// </summary>
        /// <param name="adapterHandle">Adapter handle.</param>
        /// <returns>Array of display output handles.</returns>
        internal IntPtr[] EnumerateDisplays(IntPtr adapterHandle)
        {
            ThrowIfDisposed();
            return _api!.EnumerateDisplays(adapterHandle);
        }

        /// <summary>
        /// Get the underlying native API wrapper.
        /// </summary>
        internal IGCLApi Api => _api ?? throw new ObjectDisposedException(nameof(IGCLApiHelper));
        /// <summary>
        /// Get the native API handle.
        /// </summary>
        internal IntPtr ApiHandle => _api?.DangerousGetHandle() ?? IntPtr.Zero;

        #region Feature helper factories
        /// <summary>
        /// Create a 3D helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>3D helper.</returns>
        public IGCL3DHelper Get3DHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCL3DHelper(this, h));
        /// <summary>
        /// Create an ECC helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>ECC helper.</returns>
        public IGCLEccHelper GetEccHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLEccHelper(this, h));
        /// <summary>
        /// Create an engine helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>Engine helper.</returns>
        public IGCLEngineHelper GetEngineHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLEngineHelper(this, h));
        /// <summary>
        /// Create a fan helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>Fan helper.</returns>
        public IGCLFanHelper GetFanHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLFanHelper(this, h));
        /// <summary>
        /// Create a firmware helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>Firmware helper.</returns>
        public IGCLFirmwareHelper GetFirmwareHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLFirmwareHelper(this, h));
        /// <summary>
        /// Create a frequency helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>Frequency helper.</returns>
        public IGCLFrequencyHelper GetFrequencyHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLFrequencyHelper(this, h));
        /// <summary>
        /// Create an LED helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>LED helper.</returns>
        public IGCLLedHelper GetLedHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLLedHelper(this, h));
        /// <summary>
        /// Create a media helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>Media helper.</returns>
        public IGCLMediaHelper GetMediaHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLMediaHelper(this, h));
        /// <summary>
        /// Create a memory helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>Memory helper.</returns>
        public IGCLMemoryHelper GetMemoryHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLMemoryHelper(this, h));
        /// <summary>
        /// Create an overclock helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>Overclock helper.</returns>
        public IGCLOverclockHelper GetOverclockHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLOverclockHelper(this, h));
        /// <summary>
        /// Create a PCI helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>PCI helper.</returns>
        public IGCLPciHelper GetPciHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLPciHelper(this, h));
        /// <summary>
        /// Create a power helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>Power helper.</returns>
        public IGCLPowerHelper GetPowerHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLPowerHelper(this, h));
        /// <summary>
        /// Create a temperature helper for the specified adapter.
        /// </summary>
        /// <param name="adapter">Adapter helper.</param>
        /// <returns>Temperature helper.</returns>
        public IGCLTemperatureHelper GetTemperatureHelper(IGCLAdapterHelper adapter) => CreateAdapterFeatureHelper(adapter, h => new IGCLTemperatureHelper(this, h));

        /// <summary>
        /// Create a feature helper for the specified adapter handle.
        /// </summary>
        /// <typeparam name="TFeature">Helper type.</typeparam>
        /// <param name="adapter">Adapter helper.</param>
        /// <param name="factory">Factory for the helper.</param>
        /// <returns>Feature helper instance.</returns>
        private TFeature CreateAdapterFeatureHelper<TFeature>(IGCLAdapterHelper adapter, Func<IntPtr, TFeature> factory)
        {
            if (adapter == null)
                throw new ArgumentNullException(nameof(adapter));

            ThrowIfDisposed();
            adapter.ThrowIfDisposed();
            return factory(adapter.AdapterHandle);
        }
        #endregion

        /// <summary>
        /// Set the IGCL runtime path.
        /// </summary>
        /// <param name="args">Runtime path arguments.</param>
        public unsafe void SetRuntimePath(ctl_runtime_path_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            if (copy.Size == 0)
                copy.Size = (uint)sizeof(ctl_runtime_path_args_t);
            if (copy.Version == 0)
                copy.Version = 0;
            var result = IGCL.ctlSetRuntimePath(&copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set runtime path");
        }

        /// <summary>
        /// Compare runtime path arguments while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left runtime path args struct.</param>
        /// <param name="right">Right runtime path args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreRuntimePathArgsEqual(ctl_runtime_path_args_t left, ctl_runtime_path_args_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   AreApplicationIdsEqual(left.UnlockID, right.UnlockID) &&
                   left.DeviceID == right.DeviceID &&
                   left.RevID == right.RevID;
        }

        private static bool AreApplicationIdsEqual(ctl_application_id_t left, ctl_application_id_t right)
        {
            if (left.Data1 != right.Data1 ||
                left.Data2 != right.Data2 ||
                left.Data3 != right.Data3)
            {
                return false;
            }

            var leftSpan = MemoryMarshal.CreateReadOnlySpan(ref left.Data4.e0, 8);
            var rightSpan = MemoryMarshal.CreateReadOnlySpan(ref right.Data4.e0, 8);
            return leftSpan.SequenceEqual(rightSpan);
        }

        /// <summary>
        /// Dispose the helper and release the underlying API handle.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose implementation.
        /// </summary>
        /// <param name="disposing">True when called from Dispose.</param>
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

        /// <summary>
        /// Throw if this helper has been disposed.
        /// </summary>
        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLApiHelper));
        }
    }

    /// <summary>
    /// Adapter helper facade for IGCL adapter handles.
    /// </summary>
    public sealed class IGCLAdapterHelper : IDisposable
    {
        private readonly object _lock = new();
        private ctl_device_adapter_properties_t? _properties;
        private bool _disposed;
        /// <summary>
        /// Owning API helper.
        /// </summary>
        internal IGCLApiHelper Api { get; }
        /// <summary>
        /// Adapter handle.
        /// </summary>
        internal IntPtr AdapterHandle { get; }

        internal IGCLAdapterHelper(IGCLApiHelper api, IntPtr adapterHandle)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            AdapterHandle = adapterHandle;
        }

        private static unsafe ctl_device_adapter_properties_t CreateAdapterProperties() => new ctl_device_adapter_properties_t { Size = (uint)sizeof(ctl_device_adapter_properties_t), Version = 1 };
        /// <summary>
        /// Create a combined display args struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized combined display args struct.</returns>
        public static unsafe ctl_combined_display_args_t CreateCombinedDisplayArgs() => new ctl_combined_display_args_t { Size = (uint)sizeof(ctl_combined_display_args_t), Version = 1 };
        /// <summary>
        /// Create a genlock args struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized genlock args struct.</returns>
        public static unsafe ctl_genlock_args_t CreateGenlockArgs() => new ctl_genlock_args_t { Size = (uint)sizeof(ctl_genlock_args_t), Version = 0 };
        /// <summary>
        /// Create linked display adapters args with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized linked display adapters args struct.</returns>
        public static unsafe ctl_lda_args_t CreateLinkedDisplayAdaptersArgs() => new ctl_lda_args_t { Size = (uint)sizeof(ctl_lda_args_t), Version = 0 };

        /// <summary>
        /// Compare device adapter properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreDeviceAdapterPropertiesEqual(ctl_device_adapter_properties_t left, ctl_device_adapter_properties_t right)
        {
            return DeviceAdapterPropertiesDto.FromNative(left).Equals(DeviceAdapterPropertiesDto.FromNative(right));
        }

        /// <summary>
        /// Compare combined display args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreCombinedDisplayArgsEqual(ctl_combined_display_args_t left, ctl_combined_display_args_t right)
        {
            return CombinedDisplayArgsDto.FromNative(left).Equals(CombinedDisplayArgsDto.FromNative(right));
        }

        /// <summary>
        /// Compare genlock args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreGenlockArgsEqual(ctl_genlock_args_t left, ctl_genlock_args_t right)
        {
            return GenlockArgsDto.FromNative(left).Equals(GenlockArgsDto.FromNative(right));
        }

        /// <summary>
        /// Compare linked display adapters args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreLinkedDisplayAdaptersArgsEqual(ctl_lda_args_t left, ctl_lda_args_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.NumAdapters == right.NumAdapters;
        }

        /// <summary>
        /// Compare wait property change args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreWaitPropertyChangeArgsEqual(ctl_wait_property_change_args_t left, ctl_wait_property_change_args_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.PropertyType == right.PropertyType &&
                   left.TimeOutMilliSec == right.TimeOutMilliSec &&
                   left.EventMiscFlags == right.EventMiscFlags &&
                   left.ReservedOutFlags == right.ReservedOutFlags;
        }

        /// <summary>
        /// Get adapter properties.
        /// </summary>
        /// <returns>Adapter properties struct.</returns>
        public unsafe ctl_device_adapter_properties_t GetPropertiesNative()
        {
            ThrowIfDisposed();
            lock (_lock)
            {
                if (_properties.HasValue)
                {
                    return _properties.Value;
                }

                var props = CreateAdapterProperties();
                var result = IGCL.ctlGetDeviceProperties((_ctl_device_adapter_handle_t*)AdapterHandle, &props);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    throw new IGCLException(result, "Failed to get adapter properties");
                }

                _properties = props;
                return props;
            }
        }

        /// <summary>
        /// Get adapter properties as a DTO.
        /// </summary>
        /// <returns>Adapter properties DTO.</returns>
        public unsafe DeviceAdapterPropertiesDto GetProperties()
        {
            ThrowIfDisposed();
            var props = CreateAdapterProperties();
            var result = IGCL.ctlGetDeviceProperties((_ctl_device_adapter_handle_t*)AdapterHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
            {
                throw new IGCLException(result, "Failed to get adapter properties");
            }

            return DeviceAdapterPropertiesDto.FromNative(props);
        }

        /// <summary>
        /// Enumerate device adapter handles available to this API instance.
        /// </summary>
        /// <returns>Array of adapter handles.</returns>
        public unsafe IntPtr[] EnumerateDevicesNative()
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

        /// <summary>
        /// Enumerate display output handles for this adapter.
        /// </summary>
        /// <returns>Array of display output handles.</returns>
        public unsafe IntPtr[] EnumerateDisplayOutputsNative()
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

        /// <summary>
        /// Enumerate I2C pin pair handles for this adapter.
        /// </summary>
        /// <returns>Array of I2C pin pair handles.</returns>
        public unsafe IntPtr[] EnumerateI2CPinPairsNative()
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

        /// <summary>
        /// Get device adapter properties for this adapter.
        /// </summary>
        /// <returns>Adapter properties struct.</returns>
        public unsafe ctl_device_adapter_properties_t GetDevicePropertiesNative()
        {
            ThrowIfDisposed();
            var props = CreateAdapterProperties();
            var result = IGCL.ctlGetDeviceProperties((_ctl_device_adapter_handle_t*)AdapterHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get device properties");
            return props;
        }

        /// <summary>
        /// Get device adapter properties for this adapter as a DTO.
        /// </summary>
        /// <returns>Adapter properties DTO.</returns>
        public unsafe DeviceAdapterPropertiesDto GetDeviceProperties()
        {
            ThrowIfDisposed();
            var props = CreateAdapterProperties();
            var result = IGCL.ctlGetDeviceProperties((_ctl_device_adapter_handle_t*)AdapterHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get device properties");
            return DeviceAdapterPropertiesDto.FromNative(props);
        }

        /// <summary>
        /// Get the Level Zero device and instance for this adapter.
        /// </summary>
        /// <returns>Tuple containing zeDevice and instance handles.</returns>
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

        /// <summary>
        /// Enumerate displays for this adapter.
        /// </summary>
        /// <returns>Read-only list of display helpers.</returns>
        public IReadOnlyList<IGCLDisplayHelper> EnumerateDisplayOutputs()
        {
            ThrowIfDisposed();
            var handles = Api.EnumerateDisplays(AdapterHandle);
            var displays = new List<IGCLDisplayHelper>(handles.Length);
            foreach (var h in handles)
            {
                displays.Add(new IGCLDisplayHelper(Api, AdapterHandle, h));
            }
            return displays;
        }

        /// <summary>
        /// Adapter name as a string.
        /// </summary>
        public unsafe string Name
        {
            get
            {
                var props = GetPropertiesNative();
                var pName = (sbyte*)Unsafe.AsPointer(ref props.name);
                return new string(pName);
            }
        }

        /// <summary>
        /// PCI vendor identifier as a hexadecimal string.
        /// </summary>
        public string PciVendorId => GetPropertiesNative().pci_vendor_id.ToString("X4");

        /// <summary>
        /// Call the native get/set combined display API using the provided struct.
        /// </summary>
        /// <param name="args">Combined display args struct.</param>
        /// <returns>Updated combined display args struct.</returns>
        public unsafe ctl_combined_display_args_t GetSetCombinedDisplayNative(ctl_combined_display_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            if (copy.Size == 0)
                copy.Size = (uint)sizeof(ctl_combined_display_args_t);
            if (copy.Version == 0)
                copy.Version = 1;

            var result = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)AdapterHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set combined display");

            return copy;
        }

        private unsafe byte GetCombinedDisplayMaxOutputs(IntPtr combinedDisplayOutput)
        {
            var args = CreateCombinedDisplayArgs();
            args.OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_IS_SUPPORTED_CONFIG;
            if (combinedDisplayOutput != IntPtr.Zero)
                args.hCombinedDisplayOutput = (_ctl_display_output_handle_t*)combinedDisplayOutput;

            var result = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)AdapterHandle, &args);
            if (result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
            {
                throw new IGCLException(result, $"GetCombinedDisplayMaxOutputs: Get Max Outputs Unsupported: {result}");
            }
            return args.NumOutputs;
        }

        private unsafe IntPtr FindCombinedDisplayOutputHandle()
        {
            ThrowIfDisposed();
            var displays = EnumerateDisplayOutputsNative();
            if (displays == null || displays.Length == 0)
                return IntPtr.Zero;

            foreach (var display in displays)
            {
                if (display == IntPtr.Zero)
                    continue;

                var encoderProps = new ctl_adapter_display_encoder_properties_t
                {
                    Size = (uint)sizeof(ctl_adapter_display_encoder_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlGetAdaperDisplayEncoderProperties((_ctl_display_output_handle_t*)display, &encoderProps);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    continue;

                var flags = encoderProps.EncoderConfigFlags;
                var isCombined = (flags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COLLAGE_DISPLAY) != 0 ||
                                 (flags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_MGPU_COLLAGE_DISPLAY) != 0 ||
                                 (flags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_SPLIT_DISPLAY) != 0;

                if (isCombined)
                    return display;
            }

            return IntPtr.Zero;
        }

        private static unsafe List<CombinedDisplayChildInfoDto> CopyCombinedDisplayChildInfos(ctl_combined_display_child_info_t* childInfo, int count)
        {
            if (childInfo == null || count <= 0)
                return new List<CombinedDisplayChildInfoDto>();

            var results = new List<CombinedDisplayChildInfoDto>(count);
            for (var i = 0; i < count; i++)
            {
                results.Add(CombinedDisplayChildInfoDto.FromNative(childInfo[i]));
            }
            return results;
        }

        /// <summary>
        /// Get combined display settings using the provided DTO.
        /// </summary>
        /// <returns>Updated combined display args DTO.</returns>
        public unsafe CombinedDisplayArgsDto GetCombinedDisplay()
        {
            ThrowIfDisposed();
            var probe = new ctl_combined_display_args_t
            {
                Size = (uint)sizeof(ctl_combined_display_args_t),
                Version = 1,
                // OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_IS_SUPPORTED_CONFIG
                OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_IS_SUPPORTED_CONFIG
            };

            var probeResult = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)AdapterHandle, &probe);
            byte maxOutputs = 0;
            if (probeResult == ctl_result_t.CTL_RESULT_SUCCESS)
            {
                maxOutputs = probe.NumOutputs;
            }
            else if (probeResult != ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE &&
                     probeResult != ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
            {
                throw new IGCLException(probeResult, $"Combined display probe unsupported: {probeResult}");
            }

            IntPtr combinedHandle = IntPtr.Zero;
            var displays = EnumerateDisplayOutputsNative();
            foreach (var display in displays)
            {
                if (display == IntPtr.Zero)
                    continue;

                var encoderProps = new ctl_adapter_display_encoder_properties_t
                {
                    Size = (uint)sizeof(ctl_adapter_display_encoder_properties_t),
                    Version = 0
                };

                var encoderResult = IGCL.ctlGetAdaperDisplayEncoderProperties((_ctl_display_output_handle_t*)display, &encoderProps);
                if (encoderResult != ctl_result_t.CTL_RESULT_SUCCESS)
                    continue;

                var flags = encoderProps.EncoderConfigFlags;
                var isCombined = (flags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COLLAGE_DISPLAY) != 0 ||
                                 (flags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_MGPU_COLLAGE_DISPLAY) != 0 ||
                                 (flags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_SPLIT_DISPLAY) != 0;
                if (isCombined)
                {
                    combinedHandle = display;
                    break;
                }
            }

            var childCapacity = maxOutputs > 0 ? maxOutputs : IGCL.CTL_MAX_DISPLAYS_FOR_MGPU_COLLAGE;
            var pChildren = stackalloc ctl_combined_display_child_info_t[childCapacity];
            for (var i = 0; i < childCapacity; i++)
                pChildren[i] = default;
            {
                var query = new ctl_combined_display_args_t
                {
                    Size = (uint)sizeof(ctl_combined_display_args_t),
                    Version = 1,
                    OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_QUERY_CONFIG,
                    NumOutputs = 0,
                    pChildInfo = pChildren,
                    hCombinedDisplayOutput = (_ctl_display_output_handle_t*)combinedHandle
                };

                var queryResult = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)AdapterHandle, &query);
                if (queryResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    queryResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                {
                    throw new IGCLException(queryResult, $"Combined display query unsupported: {queryResult}");
                }
                if (queryResult == ctl_result_t.CTL_RESULT_ERROR_NULL_OS_DISPLAY_OUTPUT_HANDLE ||
                    queryResult == ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_HANDLE ||
                    queryResult == ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_POINTER)
                {
                    return new CombinedDisplayArgsDto
                    {
                        OpType = query.OpType,
                        NumOutputs = 0
                    };
                }
                if (queryResult != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(queryResult, $"Combined display query failed: {queryResult}");

                if (query.NumOutputs == 0)
                {
                    return new CombinedDisplayArgsDto
                    {
                        Size = query.Size,
                        Version = query.Version,
                        OpType = query.OpType,
                        NumOutputs = 0,
                        CombinedDesktopWidth = query.CombinedDesktopWidth,
                        CombinedDesktopHeight = query.CombinedDesktopHeight,
                        CombinedDisplayOutputWindowsDisplayEncoderId = 0,
                        ChildInfos = new List<CombinedDisplayChildInfoDto>()
                    };
                }

                var dto = CombinedDisplayArgsDto.FromNative(query);
                dto.IsSupported = false;
                dto.ChildInfos = CopyCombinedDisplayChildInfos(pChildren, query.NumOutputs);
                return dto;
            }
        }

        /// <summary>
        /// Set combined display settings using the provided DTO.
        /// </summary>
        /// <param name="args">Combined display args DTO.</param>
        public void SetCombinedDisplay(CombinedDisplayArgsDto args)
        {
            ThrowIfDisposed();
            if (args.OpType == 0)
                throw new ArgumentException("OpType must be set for combined display operations.", nameof(args));

            if (args.OpType == ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_DISABLE)
            {
                var combinedOutput = FindCombinedDisplayOutputHandle();
                if (combinedOutput == IntPtr.Zero)
                    throw new InvalidOperationException("Combined display output handle not found for disable.");

                unsafe
                {
                    var pDisableChildren = stackalloc ctl_combined_display_child_info_t[1];
                    pDisableChildren[0] = default;
                    {
                        var disableArgs = new ctl_combined_display_args_t
                        {
                            Size = (uint)sizeof(ctl_combined_display_args_t),
                            Version = 1,
                            OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_DISABLE,
                            NumOutputs = 1,
                            CombinedDesktopWidth = 0,
                            CombinedDesktopHeight = 0,
                            pChildInfo = pDisableChildren,
                            hCombinedDisplayOutput = (_ctl_display_output_handle_t*)combinedOutput
                        };
                        GetSetCombinedDisplayNative(disableArgs);
                    }
                }
                return;
            }

            var opType = args.OpType;
            if (opType == ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_QUERY_CONFIG)
                opType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_ENABLE;

            if (opType != ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_ENABLE &&
                opType != ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_IS_SUPPORTED_CONFIG)
            {
                throw new ArgumentException($"Unsupported combined display operation: {args.OpType}", nameof(args));
            }

            var childInfos = args.ChildInfos;
            if (childInfos == null || childInfos.Count == 0)
                throw new ArgumentException("ChildInfos must be provided to enable combined display.", nameof(args));

            var numOutputs = args.NumOutputs == 0 ? (byte)childInfos.Count : args.NumOutputs;
            if (numOutputs < 2 || numOutputs > 4)
                throw new ArgumentException("Combined display requires between 2 and 4 outputs.", nameof(args));
            if (childInfos.Count < numOutputs)
                throw new ArgumentException("ChildInfos length does not match NumOutputs.", nameof(args));

            var desiredChildren = new List<CombinedDisplayChildInfoDto>(numOutputs);
            for (var i = 0; i < numOutputs; i++)
                desiredChildren.Add(childInfos[i]);

            for (var i = 0; i < desiredChildren.Count; i++)
            {
                var orientation = desiredChildren[i].DisplayOrientation;
                if (orientation != ctl_display_orientation_t.CTL_DISPLAY_ORIENTATION_0 &&
                    orientation != ctl_display_orientation_t.CTL_DISPLAY_ORIENTATION_180)
                {
                    throw new ArgumentException("Only 0/180 degree rotation is supported.", nameof(args));
                }
            }

            var activeOutputs = new List<(IntPtr Handle, int Width, int Height, uint EncoderId)>();
            var displayHandles = EnumerateDisplayOutputsNative();
            if (displayHandles != null)
            {
                uint combinedAllowedEncoderTypes =
                    (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TYPEC_CAPABLE |
                    (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TBT_CAPABLE |
                    (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_DITHERING_SUPPORTED |
                    (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_INTERNAL_DISPLAY;

                foreach (var display in displayHandles)
                {
                    if (display == IntPtr.Zero)
                        continue;

                    unsafe
                    {
                        var props = new ctl_display_properties_t
                        {
                            Size = (uint)sizeof(ctl_display_properties_t),
                            Version = 0
                        };
                        if (IGCL.ctlGetDisplayProperties((_ctl_display_output_handle_t*)display, &props) != ctl_result_t.CTL_RESULT_SUCCESS)
                            continue;

                        var encoderProps = new ctl_adapter_display_encoder_properties_t
                        {
                            Size = (uint)sizeof(ctl_adapter_display_encoder_properties_t),
                            Version = 0
                        };
                        if (IGCL.ctlGetAdaperDisplayEncoderProperties((_ctl_display_output_handle_t*)display, &encoderProps) != ctl_result_t.CTL_RESULT_SUCCESS)
                            continue;

                        var isDisplayActive = (props.DisplayConfigFlags & (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ACTIVE) != 0;
                        var isDisplayAttached = (props.DisplayConfigFlags & (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ATTACHED) != 0;
                        var encoderFlags = encoderProps.EncoderConfigFlags;
                        var isCombinedAvailable = encoderFlags == 0 || (encoderFlags & combinedAllowedEncoderTypes) != 0;

                        if (!isDisplayActive || !isDisplayAttached || !isCombinedAvailable)
                            continue;

                        var width = (int)props.Display_Timing_Info.HActive;
                        var height = (int)props.Display_Timing_Info.VActive;
                        if (width <= 0 || height <= 0)
                            continue;

                        var encoderId = encoderProps.Os_display_encoder_handle.WindowsDisplayEncoderID;
                        activeOutputs.Add((display, width, height, encoderId));
                    }
                }
            }

            if (activeOutputs.Count < numOutputs)
            {
                throw new InvalidOperationException($"Combined display requires {numOutputs} active outputs but only {activeOutputs.Count} are available.");
            }

            var activeEncoderIds = new HashSet<uint>();
            for (var i = 0; i < activeOutputs.Count; i++)
                activeEncoderIds.Add(activeOutputs[i].EncoderId);

            var allConnected = true;
            for (var i = 0; i < desiredChildren.Count; i++)
            {
                var encoderId = desiredChildren[i].DisplayOutputWindowsDisplayEncoderId;
                if (encoderId == 0 || !activeEncoderIds.Contains(encoderId))
                {
                    allConnected = false;
                    break;
                }
            }

            if (!allConnected)
            {
                var remainingOutputs = new List<(IntPtr Handle, int Width, int Height, uint EncoderId)>(activeOutputs);

                var childIndexes = new List<int>(desiredChildren.Count);
                for (var i = 0; i < desiredChildren.Count; i++)
                    childIndexes.Add(i);

                // Order children by intended src layout (bottom-to-top, left-to-right).
                childIndexes.Sort((a, b) =>
                {
                    var aPos = desiredChildren[a].FbSrc;
                    var bPos = desiredChildren[b].FbSrc;
                    var top = bPos.Top.CompareTo(aPos.Top);
                    if (top != 0) return top;
                    var left = aPos.Left.CompareTo(bPos.Left);
                    if (left != 0) return left;
                    return a.CompareTo(b);
                });

                // Stable output order (encoder id, then handle).
                remainingOutputs.Sort((a, b) =>
                {
                    var cmp = a.EncoderId.CompareTo(b.EncoderId);
                    if (cmp != 0) return cmp;
                    return a.Handle.ToInt64().CompareTo(b.Handle.ToInt64());
                });

                foreach (var childIndex in childIndexes)
                {
                    var target = desiredChildren[childIndex].TargetMode;
                    var matchIndex = -1;

                    if (target.Width > 0 && target.Height > 0)
                    {
                        for (var j = 0; j < remainingOutputs.Count; j++)
                        {
                            if (remainingOutputs[j].Width == target.Width &&
                                remainingOutputs[j].Height == target.Height)
                            {
                                matchIndex = j;
                                break;
                            }
                        }
                    }

                    if (matchIndex < 0)
                        matchIndex = 0;

                    var child = desiredChildren[childIndex];
                    child.DisplayOutputWindowsDisplayEncoderId = remainingOutputs[matchIndex].EncoderId;
                    desiredChildren[childIndex] = child;
                    remainingOutputs.RemoveAt(matchIndex);
                }
            }

            var combinedWidth = args.CombinedDesktopWidth;
            var combinedHeight = args.CombinedDesktopHeight;
            if (combinedWidth == 0 || combinedHeight == 0)
            {
                var maxRight = 0;
                var maxBottom = 0;
                for (var i = 0; i < desiredChildren.Count; i++)
                {
                    var rect = desiredChildren[i].FbSrc;
                    if (rect.Right > maxRight)
                        maxRight = rect.Right;
                    if (rect.Bottom > maxBottom)
                        maxBottom = rect.Bottom;
                }

                if (combinedWidth == 0 && maxRight > 0)
                    combinedWidth = (uint)maxRight;
                if (combinedHeight == 0 && maxBottom > 0)
                    combinedHeight = (uint)maxBottom;
            }

            unsafe
            {
                var pChildInfo = stackalloc ctl_combined_display_child_info_t[numOutputs];
                for (var i = 0; i < numOutputs; i++)
                {
                    pChildInfo[i] = desiredChildren[i].ToNative();
                    // Resolve native handle from encoder ID
                    var encoderId = desiredChildren[i].DisplayOutputWindowsDisplayEncoderId;
                    for (var j = 0; j < activeOutputs.Count; j++)
                    {
                        if (activeOutputs[j].EncoderId == encoderId)
                        {
                            pChildInfo[i].hDisplayOutput = (_ctl_display_output_handle_t*)activeOutputs[j].Handle;
                            break;
                        }
                    }
                }
                {
                    var supportArgs = new ctl_combined_display_args_t
                    {
                        Size = (uint)sizeof(ctl_combined_display_args_t),
                        Version = 1,
                        OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_IS_SUPPORTED_CONFIG,
                        NumOutputs = numOutputs,
                        CombinedDesktopWidth = combinedWidth,
                        CombinedDesktopHeight = combinedHeight,
                        pChildInfo = pChildInfo,
                        hCombinedDisplayOutput = null
                    };

                    var supportResult = GetSetCombinedDisplayNative(supportArgs);
                    if (supportResult.IsSupported == 0)
                        throw new IGCLException(ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE, "Combined display configuration is not supported.");

                    if (opType == ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_IS_SUPPORTED_CONFIG)
                        return;

                    var enableArgs = supportResult;
                    enableArgs.OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_ENABLE;
                    GetSetCombinedDisplayNative(enableArgs);
                }
            }
        }

        /// <summary>
        /// Call the native get/set display genlock API using the provided struct.
        /// </summary>
        /// <param name="adapters">Adapter handles.</param>
        /// <param name="args">Genlock args struct.</param>
        /// <param name="failureAdapter">Adapter handle that failed, if any.</param>
        /// <returns>Updated genlock args struct.</returns>
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

        /// <summary>
        /// Get display genlock settings using a DTO.
        /// </summary>
        /// <param name="adapters">Adapter handles.</param>
        /// <param name="operation">Genlock operation.</param>
        /// <param name="args">Genlock args DTO.</param>
        /// <param name="failureAdapter">Adapter handle that failed, if any.</param>
        /// <returns>Updated genlock args DTO.</returns>
        public GenlockArgsDto GetDisplayGenlock(IntPtr[] adapters, ctl_genlock_operation_t operation, GenlockArgsDto args, out IntPtr failureAdapter)
        {
            var request = args;
            request.Operation = operation;
            var native = GetSetDisplayGenlockNative(adapters, request.ToNative(), out failureAdapter);
            return GenlockArgsDto.FromNative(native);
        }

        /// <summary>
        /// Set display genlock settings using a DTO.
        /// </summary>
        /// <param name="adapters">Adapter handles.</param>
        /// <param name="operation">Genlock operation.</param>
        /// <param name="args">Genlock args DTO.</param>
        /// <param name="failureAdapter">Adapter handle that failed, if any.</param>
        public void SetDisplayGenlockNative(IntPtr[] adapters, ctl_genlock_operation_t operation, GenlockArgsDto args, out IntPtr failureAdapter)
        {
            var request = args;
            request.Operation = operation;
            GetSetDisplayGenlockNative(adapters, request.ToNative(), out failureAdapter);
        }

        /// <summary>
        /// Link display adapters using the provided args.
        /// </summary>
        /// <param name="args">Linked display adapters args.</param>
        public unsafe void LinkDisplayAdapters(ctl_lda_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlLinkDisplayAdapters((_ctl_device_adapter_handle_t*)AdapterHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to link display adapters");
        }

        /// <summary>
        /// Unlink previously linked display adapters.
        /// </summary>
        public unsafe void UnlinkDisplayAdapters()
        {
            ThrowIfDisposed();
            var result = IGCL.ctlUnlinkDisplayAdapters((_ctl_device_adapter_handle_t*)AdapterHandle);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to unlink display adapters");
        }

        /// <summary>
        /// Get linked display adapter information.
        /// </summary>
        /// <returns>Tuple containing args and adapter handles.</returns>
        public unsafe (ctl_lda_args_t args, IntPtr[] adapters) GetLinkedDisplayAdapters()
        {
            ThrowIfDisposed();
            var args = CreateLinkedDisplayAdaptersArgs();
            var result = IGCL.ctlGetLinkedDisplayAdapters((_ctl_device_adapter_handle_t*)AdapterHandle, &args);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && args.NumAdapters == 0)
                throw new IGCLException(result, "Failed to get linked display adapter count");

            if (args.NumAdapters == 0)
                return (args, Array.Empty<IntPtr>());

            var handles = new IntPtr[args.NumAdapters];
            fixed (IntPtr* pHandles = handles)
            {
                args.hLinkedAdapters = (_ctl_device_adapter_handle_t**)pHandles;
                result = IGCL.ctlGetLinkedDisplayAdapters((_ctl_device_adapter_handle_t*)AdapterHandle, &args);
                args.hLinkedAdapters = null;
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate linked display adapters");
            }

            return (args, handles);
        }

        /// <summary>
        /// Get linked display adapter information as a DTO.
        /// </summary>
        /// <returns>Linked display adapters result DTO.</returns>
        public LinkedDisplayAdaptersResultDto GetLinkedDisplayAdaptersDto()
        {
            var native = GetLinkedDisplayAdapters();
            return LinkedDisplayAdaptersResultDto.FromNative(native.args, native.adapters);
        }

        private static unsafe ctl_wait_property_change_args_t CreateWaitPropertyChangeArgs() => new ctl_wait_property_change_args_t { Size = (uint)sizeof(ctl_wait_property_change_args_t), Version = 0 };

        /// <summary>
        /// Wait for a property change event.
        /// </summary>
        /// <param name="args">Wait arguments.</param>
        /// <returns>Updated wait arguments.</returns>
        public unsafe ctl_wait_property_change_args_t WaitForPropertyChange(ctl_wait_property_change_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            if (copy.Size == 0)
                copy.Size = (uint)sizeof(ctl_wait_property_change_args_t);
            if (copy.Version == 0)
                copy.Version = 0;
            var result = IGCL.ctlWaitForPropertyChange((_ctl_device_adapter_handle_t*)AdapterHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to wait for property change");
            return copy;
        }

        /// <summary>
        /// Throw if this helper has been disposed.
        /// </summary>
        internal void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLAdapterHelper));
        }

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }

    /// <summary>
    /// DTO for firmware version information.
    /// </summary>
    public struct FirmwareVersionDto : IEquatable<FirmwareVersionDto>
    {
        /// <summary>
        /// Major firmware version.
        /// </summary>
        public ulong MajorVersion;
        /// <summary>
        /// Minor firmware version.
        /// </summary>
        public ulong MinorVersion;
        /// <summary>
        /// Firmware build number.
        /// </summary>
        public ulong BuildNumber;

        public bool Equals(FirmwareVersionDto other)
        {
            return MajorVersion == other.MajorVersion &&
                   MinorVersion == other.MinorVersion &&
                   BuildNumber == other.BuildNumber;
        }

        public override bool Equals(object? obj) => obj is FirmwareVersionDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(MajorVersion);
            hash.Add(MinorVersion);
            hash.Add(BuildNumber);
            return hash.ToHashCode();
        }

        public static FirmwareVersionDto FromNative(ctl_firmware_version_t native)
        {
            return new FirmwareVersionDto
            {
                MajorVersion = native.major_version,
                MinorVersion = native.minor_version,
                BuildNumber = native.build_number
            };
        }

        public ctl_firmware_version_t ToNative()
        {
            return new ctl_firmware_version_t
            {
                major_version = MajorVersion,
                minor_version = MinorVersion,
                build_number = BuildNumber
            };
        }
    }

    /// <summary>
    /// DTO for adapter bus-device-function identifier.
    /// </summary>
    public struct AdapterBdfDto : IEquatable<AdapterBdfDto>
    {
        /// <summary>
        /// PCI bus identifier.
        /// </summary>
        public byte Bus;
        /// <summary>
        /// PCI device identifier.
        /// </summary>
        public byte Device;
        /// <summary>
        /// PCI function identifier.
        /// </summary>
        public byte Function;

        public bool Equals(AdapterBdfDto other)
        {
            return Bus == other.Bus &&
                   Device == other.Device &&
                   Function == other.Function;
        }

        public override bool Equals(object? obj) => obj is AdapterBdfDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Bus);
            hash.Add(Device);
            hash.Add(Function);
            return hash.ToHashCode();
        }

        public static AdapterBdfDto FromNative(ctl_adapter_bdf_t native)
        {
            return new AdapterBdfDto
            {
                Bus = native.bus,
                Device = native.device,
                Function = native.function
            };
        }

        public ctl_adapter_bdf_t ToNative()
        {
            return new ctl_adapter_bdf_t
            {
                bus = Bus,
                device = Device,
                function = Function
            };
        }
    }

    /// <summary>
    /// DTO for rectangle coordinates.
    /// </summary>
    public struct RectDto : IEquatable<RectDto>
    {
        /// <summary>
        /// Left coordinate.
        /// </summary>
        public int Left;
        /// <summary>
        /// Top coordinate.
        /// </summary>
        public int Top;
        /// <summary>
        /// Right coordinate.
        /// </summary>
        public int Right;
        /// <summary>
        /// Bottom coordinate.
        /// </summary>
        public int Bottom;

        public bool Equals(RectDto other)
        {
            return Left == other.Left &&
                   Top == other.Top &&
                   Right == other.Right &&
                   Bottom == other.Bottom;
        }

        public override bool Equals(object? obj) => obj is RectDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Left);
            hash.Add(Top);
            hash.Add(Right);
            hash.Add(Bottom);
            return hash.ToHashCode();
        }

        public static RectDto FromNative(ctl_rect_t native)
        {
            return new RectDto
            {
                Left = native.Left,
                Top = native.Top,
                Right = native.Right,
                Bottom = native.Bottom
            };
        }

        public ctl_rect_t ToNative()
        {
            return new ctl_rect_t
            {
                Left = Left,
                Top = Top,
                Right = Right,
                Bottom = Bottom
            };
        }
    }

    /// <summary>
    /// DTO for child display target mode.
    /// </summary>
    public struct ChildDisplayTargetModeDto : IEquatable<ChildDisplayTargetModeDto>
    {
        private const int ReservedFieldCount = 4;
        /// <summary>
        /// Target width.
        /// </summary>
        public uint Width;
        /// <summary>
        /// Target height.
        /// </summary>
        public uint Height;
        /// <summary>
        /// Target refresh rate.
        /// </summary>
        public float RefreshRate;
        /// <summary>
        /// Reserved mode fields.
        /// </summary>
        public List<uint>? ReservedFields;

        public bool Equals(ChildDisplayTargetModeDto other)
        {
            return Width == other.Width &&
                   Height == other.Height &&
                   RefreshRate.Equals(other.RefreshRate);
        }

        public override bool Equals(object? obj) => obj is ChildDisplayTargetModeDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Width);
            hash.Add(Height);
            hash.Add(RefreshRate);
            return hash.ToHashCode();
        }

        public static ChildDisplayTargetModeDto FromNative(ctl_child_display_target_mode_t native)
        {
            return new ChildDisplayTargetModeDto
            {
                Width = native.Width,
                Height = native.Height,
                RefreshRate = native.RefreshRate,
                ReservedFields = ReadReservedFields(native.ReservedFields)
            };
        }

        public ctl_child_display_target_mode_t ToNative()
        {
            var native = new ctl_child_display_target_mode_t
            {
                Width = Width,
                Height = Height,
                RefreshRate = RefreshRate
            };

            WriteReservedFields(ReservedFields, ref native.ReservedFields);
            return native;
        }

        private static unsafe List<uint> ReadReservedFields(ctl_child_display_target_mode_t._ReservedFields_e__FixedBuffer buffer)
        {
            var values = new List<uint>(ReservedFieldCount);
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedFieldCount; i++)
                values.Add(pValues[i]);
            return values;
        }

        private static unsafe void WriteReservedFields(List<uint>? values, ref ctl_child_display_target_mode_t._ReservedFields_e__FixedBuffer buffer)
        {
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedFieldCount; i++)
                pValues[i] = 0;

            if (values == null || values.Count == 0)
                return;

            var count = Math.Min(values.Count, ReservedFieldCount);
            for (var i = 0; i < count; i++)
                pValues[i] = values[i];
        }
    }

    /// <summary>
    /// DTO for a single genlock display info entry.
    /// </summary>
    public struct GenlockDisplayInfoDto : IEquatable<GenlockDisplayInfoDto>
    {
        /// <summary>
        /// Indicates whether this display is the primary genlock display.
        /// </summary>
        public bool IsPrimary;

        public bool Equals(GenlockDisplayInfoDto other) => IsPrimary == other.IsPrimary;
        public override bool Equals(object? obj) => obj is GenlockDisplayInfoDto other && Equals(other);
        public override int GetHashCode() => IsPrimary.GetHashCode();

        public static GenlockDisplayInfoDto FromNative(ctl_genlock_display_info_t native)
        {
            return new GenlockDisplayInfoDto
            {
                IsPrimary = native.IsPrimary != 0
            };
        }
    }

    /// <summary>
    /// DTO for a single genlock target mode list entry.
    /// </summary>
    public struct GenlockTargetModeListDto : IEquatable<GenlockTargetModeListDto>
    {
        /// <summary>
        /// Target modes available for this display.
        /// </summary>
        public List<DisplayTimingDto>? TargetModes;

        public bool Equals(GenlockTargetModeListDto other)
        {
            if (TargetModes == null && other.TargetModes == null) return true;
            if (TargetModes == null || other.TargetModes == null) return false;
            if (TargetModes.Count != other.TargetModes.Count) return false;
            for (var i = 0; i < TargetModes.Count; i++)
                if (!TargetModes[i].Equals(other.TargetModes[i])) return false;
            return true;
        }

        public override bool Equals(object? obj) => obj is GenlockTargetModeListDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            if (TargetModes != null)
                for (var i = 0; i < TargetModes.Count; i++)
                    hash.Add(TargetModes[i]);
            return hash.ToHashCode();
        }

        public static unsafe GenlockTargetModeListDto FromNative(ctl_genlock_target_mode_list_t native)
        {
            List<DisplayTimingDto>? modes = null;
            if (native.pTargetModes != null && native.NumModes > 0)
            {
                modes = new List<DisplayTimingDto>((int)native.NumModes);
                for (var i = 0; i < (int)native.NumModes; i++)
                    modes.Add(DisplayTimingDto.FromNative(native.pTargetModes[i]));
            }
            return new GenlockTargetModeListDto { TargetModes = modes };
        }
    }

    /// <summary>
    /// DTO for genlock topology.
    /// </summary>
    public struct GenlockTopologyDto : IEquatable<GenlockTopologyDto>
    {
        /// <summary>
        /// Number of displays in the genlock topology.
        /// </summary>
        public byte NumGenlockDisplays;
        /// <summary>
        /// Indicates whether this is the primary genlock system.
        /// </summary>
        public bool IsPrimaryGenlockSystem;
        /// <summary>
        /// Common target mode for genlock displays.
        /// </summary>
        public DisplayTimingDto CommonTargetMode;
        /// <summary>
        /// Managed genlock display info list.
        /// </summary>
        public List<GenlockDisplayInfoDto>? GenlockDisplayInfos;
        /// <summary>
        /// Managed genlock mode lists.
        /// </summary>
        public List<GenlockTargetModeListDto>? GenlockModeLists;

        public bool Equals(GenlockTopologyDto other)
        {
            return NumGenlockDisplays == other.NumGenlockDisplays &&
                   IsPrimaryGenlockSystem == other.IsPrimaryGenlockSystem &&
                   CommonTargetMode.Equals(other.CommonTargetMode);
        }

        public override bool Equals(object? obj) => obj is GenlockTopologyDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(NumGenlockDisplays);
            hash.Add(IsPrimaryGenlockSystem);
            hash.Add(CommonTargetMode);
            return hash.ToHashCode();
        }

        public static unsafe GenlockTopologyDto FromNative(ctl_genlock_topology_t native)
        {
            return new GenlockTopologyDto
            {
                NumGenlockDisplays = native.NumGenlockDisplays,
                IsPrimaryGenlockSystem = IGCLDisplayDtoBool.ToBool(native.IsPrimaryGenlockSystem),
                CommonTargetMode = DisplayTimingDto.FromNative(native.CommonTargetMode),
                GenlockDisplayInfos = null,
                GenlockModeLists = null
            };
        }

        public unsafe ctl_genlock_topology_t ToNative()
        {
            return new ctl_genlock_topology_t
            {
                NumGenlockDisplays = NumGenlockDisplays == 0 && GenlockDisplayInfos != null ? (byte)GenlockDisplayInfos.Count : NumGenlockDisplays,
                IsPrimaryGenlockSystem = IGCLDisplayDtoBool.ToByte(IsPrimaryGenlockSystem),
                CommonTargetMode = CommonTargetMode.ToNative(),
                pGenlockDisplayInfo = null,
                pGenlockModeList = null
            };
        }
    }

    /// <summary>
    /// DTO for adapter properties.
    /// </summary>
    public struct DeviceAdapterPropertiesDto : IEquatable<DeviceAdapterPropertiesDto>
    {
        private const int NameLength = 100;
        private const int ReservedLength = 108;
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Size of the device ID buffer reported by native calls.
        /// </summary>
        public uint DeviceIdSize;
        /// <summary>
        /// Device type.
        /// </summary>
        public ctl_device_type_t DeviceType;
        /// <summary>
        /// Supported functions bitmask.
        /// </summary>
        public uint SupportedSubfunctionFlags;
        /// <summary>
        /// True when display APIs are supported.
        /// </summary>
        public bool SupportsDisplay
        {
            readonly get => HasFlag(SupportedSubfunctionFlags, (uint)ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_DISPLAY);
            set => SupportedSubfunctionFlags = SetFlag(SupportedSubfunctionFlags, (uint)ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_DISPLAY, value);
        }
        /// <summary>
        /// True when 3D APIs are supported.
        /// </summary>
        public bool Supports3D
        {
            readonly get => HasFlag(SupportedSubfunctionFlags, (uint)ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_3D);
            set => SupportedSubfunctionFlags = SetFlag(SupportedSubfunctionFlags, (uint)ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_3D, value);
        }
        /// <summary>
        /// True when media APIs are supported.
        /// </summary>
        public bool SupportsMedia
        {
            readonly get => HasFlag(SupportedSubfunctionFlags, (uint)ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_MEDIA);
            set => SupportedSubfunctionFlags = SetFlag(SupportedSubfunctionFlags, (uint)ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_MEDIA, value);
        }
        /// <summary>
        /// Driver version value.
        /// </summary>
        public ulong DriverVersion;
        /// <summary>
        /// Firmware version info.
        /// </summary>
        public FirmwareVersionDto FirmwareVersion;
        /// <summary>
        /// PCI vendor ID.
        /// </summary>
        public uint PciVendorId;
        /// <summary>
        /// PCI device ID.
        /// </summary>
        public uint PciDeviceId;
        /// <summary>
        /// PCI revision ID.
        /// </summary>
        public uint RevId;
        /// <summary>
        /// Number of EUs per sub-slice.
        /// </summary>
        public uint NumEusPerSubSlice;
        /// <summary>
        /// Number of sub-slices per slice.
        /// </summary>
        public uint NumSubSlicesPerSlice;
        /// <summary>
        /// Number of slices.
        /// </summary>
        public uint NumSlices;
        /// <summary>
        /// Adapter name.
        /// </summary>
        public string Name;
        /// <summary>
        /// Graphics adapter properties flags.
        /// </summary>
        public uint GraphicsAdapterProperties;
        /// <summary>
        /// True when the adapter is integrated.
        /// </summary>
        public bool IsIntegratedGraphicsAdapter
        {
            readonly get => HasFlag(GraphicsAdapterProperties, (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_INTEGRATED);
            set => GraphicsAdapterProperties = SetFlag(GraphicsAdapterProperties, (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_INTEGRATED, value);
        }
        /// <summary>
        /// True when this is the primary LDA adapter.
        /// </summary>
        public bool IsLdaPrimary
        {
            readonly get => HasFlag(GraphicsAdapterProperties, (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_LDA_PRIMARY);
            set => GraphicsAdapterProperties = SetFlag(GraphicsAdapterProperties, (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_LDA_PRIMARY, value);
        }
        /// <summary>
        /// True when this is the secondary LDA adapter.
        /// </summary>
        public bool IsLdaSecondary
        {
            readonly get => HasFlag(GraphicsAdapterProperties, (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_LDA_SECONDARY);
            set => GraphicsAdapterProperties = SetFlag(GraphicsAdapterProperties, (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_LDA_SECONDARY, value);
        }
        /// <summary>
        /// Average graphics clock (MHz).
        /// </summary>
        public uint Frequency;
        /// <summary>
        /// PCI sub-system ID.
        /// </summary>
        public ushort PciSubsysId;
        /// <summary>
        /// PCI sub-system vendor ID.
        /// </summary>
        public ushort PciSubsysVendorId;
        /// <summary>
        /// Adapter BDF.
        /// </summary>
        public AdapterBdfDto AdapterBdf;
        /// <summary>
        /// Number of Xe cores.
        /// </summary>
        public uint NumXeCores;
        /// <summary>
        /// Reserved native fields.
        /// </summary>
        public List<byte>? Reserved;

        /// <summary>
        /// Compare adapter properties while ignoring reserved native fields.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(DeviceAdapterPropertiesDto other)
        {
            // Reserved is intentionally excluded from comparisons.
                 return DeviceIdSize == other.DeviceIdSize &&
                   DeviceType == other.DeviceType &&
                   SupportedSubfunctionFlags == other.SupportedSubfunctionFlags &&
                   DriverVersion == other.DriverVersion &&
                   FirmwareVersion.Equals(other.FirmwareVersion) &&
                   PciVendorId == other.PciVendorId &&
                   PciDeviceId == other.PciDeviceId &&
                   RevId == other.RevId &&
                   NumEusPerSubSlice == other.NumEusPerSubSlice &&
                   NumSubSlicesPerSlice == other.NumSubSlicesPerSlice &&
                   NumSlices == other.NumSlices &&
                   string.Equals(Name, other.Name, StringComparison.Ordinal) &&
                   GraphicsAdapterProperties == other.GraphicsAdapterProperties &&
                   Frequency == other.Frequency &&
                   PciSubsysId == other.PciSubsysId &&
                   PciSubsysVendorId == other.PciSubsysVendorId &&
                   AdapterBdf.Equals(other.AdapterBdf) &&
                   NumXeCores == other.NumXeCores;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is DeviceAdapterPropertiesDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(DeviceIdSize);
            hash.Add(DeviceType);
            hash.Add(SupportedSubfunctionFlags);
            hash.Add(DriverVersion);
            hash.Add(FirmwareVersion);
            hash.Add(PciVendorId);
            hash.Add(PciDeviceId);
            hash.Add(RevId);
            hash.Add(NumEusPerSubSlice);
            hash.Add(NumSubSlicesPerSlice);
            hash.Add(NumSlices);
            hash.Add(Name, StringComparer.Ordinal);
            hash.Add(GraphicsAdapterProperties);
            hash.Add(Frequency);
            hash.Add(PciSubsysId);
            hash.Add(PciSubsysVendorId);
            hash.Add(AdapterBdf);
            hash.Add(NumXeCores);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Adapter properties DTO.</returns>
        public static DeviceAdapterPropertiesDto FromNative(ctl_device_adapter_properties_t native)
        {
            return new DeviceAdapterPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                DeviceIdSize = native.device_id_size,
                DeviceType = native.device_type,
                SupportedSubfunctionFlags = native.supported_subfunction_flags,
                DriverVersion = native.driver_version,
                FirmwareVersion = FirmwareVersionDto.FromNative(native.firmware_version),
                PciVendorId = native.pci_vendor_id,
                PciDeviceId = native.pci_device_id,
                RevId = native.rev_id,
                NumEusPerSubSlice = native.num_eus_per_sub_slice,
                NumSubSlicesPerSlice = native.num_sub_slices_per_slice,
                NumSlices = native.num_slices,
                Name = ReadName(native.name),
                GraphicsAdapterProperties = native.graphics_adapter_properties,
                Frequency = native.Frequency,
                PciSubsysId = native.pci_subsys_id,
                PciSubsysVendorId = native.pci_subsys_vendor_id,
                AdapterBdf = AdapterBdfDto.FromNative(native.adapter_bdf),
                NumXeCores = native.num_xe_cores,
                Reserved = ReadReserved(native.reserved)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Adapter properties struct.</returns>
        public unsafe ctl_device_adapter_properties_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_device_adapter_properties_t);
            var version = Version == 0 ? (byte)1 : Version;

            var native = new ctl_device_adapter_properties_t
            {
                Size = size,
                Version = version,
                pDeviceID = null,
                device_id_size = DeviceIdSize,
                device_type = DeviceType,
                supported_subfunction_flags = SupportedSubfunctionFlags,
                driver_version = DriverVersion,
                firmware_version = FirmwareVersion.ToNative(),
                pci_vendor_id = PciVendorId,
                pci_device_id = PciDeviceId,
                rev_id = RevId,
                num_eus_per_sub_slice = NumEusPerSubSlice,
                num_sub_slices_per_slice = NumSubSlicesPerSlice,
                num_slices = NumSlices,
                graphics_adapter_properties = GraphicsAdapterProperties,
                Frequency = Frequency,
                pci_subsys_id = PciSubsysId,
                pci_subsys_vendor_id = PciSubsysVendorId,
                adapter_bdf = AdapterBdf.ToNative(),
                num_xe_cores = NumXeCores
            };

            WriteName(Name, ref native.name);
            WriteReserved(Reserved, ref native.reserved);
            return native;
        }

        private static unsafe string ReadName(ctl_device_adapter_properties_t._name_e__FixedBuffer buffer)
        {
            var bytes = new byte[NameLength];
            var pName = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            var length = 0;
            for (var i = 0; i < NameLength; i++)
            {
                var value = pName[i];
                if (value == 0)
                    break;
                bytes[i] = (byte)value;
                length++;
            }

            return length == 0 ? string.Empty : Encoding.ASCII.GetString(bytes, 0, length);
        }

        private static unsafe List<byte> ReadReserved(ctl_device_adapter_properties_t._reserved_e__FixedBuffer buffer)
        {
            var bytes = new List<byte>(ReservedLength);
            var pReserved = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedLength; i++)
                bytes.Add((byte)pReserved[i]);
            return bytes;
        }

        private static unsafe void WriteName(string? value, ref ctl_device_adapter_properties_t._name_e__FixedBuffer buffer)
        {
            var pName = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < NameLength; i++)
                pName[i] = 0;

            if (string.IsNullOrEmpty(value))
                return;

            var bytes = Encoding.ASCII.GetBytes(value);
            var count = Math.Min(bytes.Length, NameLength - 1);
            for (var i = 0; i < count; i++)
                pName[i] = unchecked((sbyte)bytes[i]);
        }

        private static unsafe void WriteReserved(List<byte>? value, ref ctl_device_adapter_properties_t._reserved_e__FixedBuffer buffer)
        {
            var pReserved = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedLength; i++)
                pReserved[i] = 0;

            if (value == null || value.Count == 0)
                return;

            var count = Math.Min(value.Count, ReservedLength);
            for (var i = 0; i < count; i++)
                pReserved[i] = unchecked((sbyte)value[i]);
        }

        private static bool HasFlag(uint value, uint flag) => (value & flag) != 0;

        private static uint SetFlag(uint value, uint flag, bool enabled)
        {
            return enabled ? (value | flag) : (value & ~flag);
        }
    }

    /// <summary>
    /// DTO for combined display arguments.
    /// </summary>
    public unsafe struct CombinedDisplayArgsDto : IEquatable<CombinedDisplayArgsDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Operation type.
        /// </summary>
        public ctl_combined_display_optype_t OpType;
        /// <summary>
        /// Indicates whether the combined display configuration is supported for IS_SUPPORTED_CONFIG.
        /// </summary>
        public bool IsSupported;
        /// <summary>
        /// Number of outputs in the combined display.
        /// </summary>
        public byte NumOutputs;
        /// <summary>
        /// Combined desktop width.
        /// </summary>
        public uint CombinedDesktopWidth;
        /// <summary>
        /// Combined desktop height.
        /// </summary>
        public uint CombinedDesktopHeight;
        /// <summary>
        /// Managed child display info list.
        /// </summary>
        public List<CombinedDisplayChildInfoDto>? ChildInfos;
        /// <summary>
        /// Combined display output Windows display encoder identifier.
        /// </summary>
        public uint CombinedDisplayOutputWindowsDisplayEncoderId;

        /// <summary>
        /// Compare combined display args while ignoring pointer fields.
        /// </summary>
        /// <param name="other">Other args instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(CombinedDisplayArgsDto other)
        {
              // CombinedDisplayOutput is a pointer and is intentionally excluded.
                 return OpType == other.OpType &&
                   IsSupported == other.IsSupported &&
                   NumOutputs == other.NumOutputs &&
                   CombinedDesktopWidth == other.CombinedDesktopWidth &&
                   CombinedDesktopHeight == other.CombinedDesktopHeight &&
                   AreChildInfosEqual(ChildInfos, other.ChildInfos);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is CombinedDisplayArgsDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(OpType);
            hash.Add(IsSupported);
            hash.Add(NumOutputs);
            hash.Add(CombinedDesktopWidth);
            hash.Add(CombinedDesktopHeight);
            if (ChildInfos != null)
            {
                hash.Add(ChildInfos.Count);
                for (var i = 0; i < ChildInfos.Count; i++)
                    hash.Add(ChildInfos[i]);
            }
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Combined display args DTO.</returns>
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
                CombinedDisplayOutputWindowsDisplayEncoderId = 0
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Combined display args struct.</returns>
        public unsafe ctl_combined_display_args_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_combined_display_args_t);
            var version = Version == 0 ? (byte)1 : Version;

            return new ctl_combined_display_args_t
            {
                Size = size,
                Version = version,
                OpType = OpType,
                IsSupported = IGCLDisplayDtoBool.ToByte(IsSupported),
                NumOutputs = NumOutputs == 0 && ChildInfos != null ? (byte)ChildInfos.Count : NumOutputs,
                CombinedDesktopWidth = CombinedDesktopWidth,
                CombinedDesktopHeight = CombinedDesktopHeight,
                pChildInfo = null,
                hCombinedDisplayOutput = null
            };
        }

        private static bool AreChildInfosEqual(List<CombinedDisplayChildInfoDto>? left, List<CombinedDisplayChildInfoDto>? right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left == null || right == null)
                return false;
            if (left.Count != right.Count)
                return false;
            for (var i = 0; i < left.Count; i++)
            {
                if (!left[i].Equals(right[i]))
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// DTO for combined display child information.
    /// </summary>
    public unsafe struct CombinedDisplayChildInfoDto : IEquatable<CombinedDisplayChildInfoDto>
    {
        /// <summary>
        /// Windows display encoder identifier for the display output.
        /// </summary>
        public uint DisplayOutputWindowsDisplayEncoderId;
        /// <summary>
        /// Framebuffer source rect.
        /// </summary>
        public RectDto FbSrc;
        /// <summary>
        /// Framebuffer target rect.
        /// </summary>
        public RectDto FbPos;
        /// <summary>
        /// Display orientation.
        /// </summary>
        public ctl_display_orientation_t DisplayOrientation;
        /// <summary>
        /// Target mode info.
        /// </summary>
        public ChildDisplayTargetModeDto TargetMode;

        /// <summary>
        /// Compare child display info while ignoring pointer and reserved fields.
        /// </summary>
        /// <param name="other">Other child info instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(CombinedDisplayChildInfoDto other)
        {
            return FbSrc.Equals(other.FbSrc) &&
                   FbPos.Equals(other.FbPos) &&
                   DisplayOrientation == other.DisplayOrientation &&
                   TargetMode.Width == other.TargetMode.Width &&
                   TargetMode.Height == other.TargetMode.Height &&
                   TargetMode.RefreshRate.Equals(other.TargetMode.RefreshRate);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is CombinedDisplayChildInfoDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(FbSrc);
            hash.Add(FbPos);
            hash.Add(DisplayOrientation);
            hash.Add(TargetMode.Width);
            hash.Add(TargetMode.Height);
            hash.Add(TargetMode.RefreshRate);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Child info DTO.</returns>
        public static CombinedDisplayChildInfoDto FromNative(ctl_combined_display_child_info_t native)
        {
            return new CombinedDisplayChildInfoDto
            {
                DisplayOutputWindowsDisplayEncoderId = 0,
                FbSrc = RectDto.FromNative(native.FbSrc),
                FbPos = RectDto.FromNative(native.FbPos),
                DisplayOrientation = native.DisplayOrientation,
                TargetMode = ChildDisplayTargetModeDto.FromNative(native.TargetMode)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct (hDisplayOutput is null; caller must set it).
        /// </summary>
        /// <returns>Child info struct.</returns>
        public ctl_combined_display_child_info_t ToNative()
        {
            return new ctl_combined_display_child_info_t
            {
                hDisplayOutput = null,
                FbSrc = FbSrc.ToNative(),
                FbPos = FbPos.ToNative(),
                DisplayOrientation = DisplayOrientation,
                TargetMode = TargetMode.ToNative()
            };
        }
    }

    /// <summary>
    /// DTO for genlock arguments.
    /// </summary>
    public struct GenlockArgsDto : IEquatable<GenlockArgsDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Genlock operation.
        /// </summary>
        public ctl_genlock_operation_t Operation;
        /// <summary>
        /// Genlock topology.
        /// </summary>
        public GenlockTopologyDto GenlockTopology;
        /// <summary>
        /// Indicates whether genlock is enabled.
        /// </summary>
        public bool IsGenlockEnabled;
        /// <summary>
        /// Indicates whether genlock is possible.
        /// </summary>
        public bool IsGenlockPossible;

        /// <summary>
        /// Compare genlock args.
        /// </summary>
        /// <param name="other">Other args instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(GenlockArgsDto other)
        {
                 return Operation == other.Operation &&
                     GenlockTopology.Equals(other.GenlockTopology) &&
                   IsGenlockEnabled == other.IsGenlockEnabled &&
                   IsGenlockPossible == other.IsGenlockPossible;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is GenlockArgsDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Operation);
            hash.Add(GenlockTopology);
            hash.Add(IsGenlockEnabled);
            hash.Add(IsGenlockPossible);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Genlock args DTO.</returns>
        public static GenlockArgsDto FromNative(ctl_genlock_args_t native)
        {
            return new GenlockArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                Operation = native.Operation,
                GenlockTopology = GenlockTopologyDto.FromNative(native.GenlockTopology),
                IsGenlockEnabled = IGCLDisplayDtoBool.ToBool(native.IsGenlockEnabled),
                IsGenlockPossible = IGCLDisplayDtoBool.ToBool(native.IsGenlockPossible)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Genlock args struct.</returns>
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
                GenlockTopology = GenlockTopology.ToNative(),
                IsGenlockEnabled = IGCLDisplayDtoBool.ToByte(IsGenlockEnabled),
                IsGenlockPossible = IGCLDisplayDtoBool.ToByte(IsGenlockPossible)
            };
        }
    }

    /// <summary>
    /// DTO for linked display adapters arguments.
    /// </summary>
    public struct LinkedDisplayAdaptersArgsDto
    {
        public uint Size;
        public byte Version;
        public byte NumAdapters;
        public List<ulong>? Reserved;

        public static unsafe LinkedDisplayAdaptersArgsDto FromNative(ctl_lda_args_t native)
        {
            return new LinkedDisplayAdaptersArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                NumAdapters = native.NumAdapters,
                Reserved = ReadReserved(native.Reserved)
            };
        }

        public unsafe ctl_lda_args_t ToNative()
        {
            var size = Size == 0 ? (uint)sizeof(ctl_lda_args_t) : Size;
            var native = new ctl_lda_args_t
            {
                Size = size,
                Version = Version,
                NumAdapters = NumAdapters,
                hLinkedAdapters = null
            };

            WriteReserved(Reserved, ref native.Reserved);
            return native;
        }

        private static unsafe List<ulong> ReadReserved(ctl_lda_args_t._Reserved_e__FixedBuffer buffer)
        {
            const int count = 4;
            var values = new List<ulong>(count);
            var pValues = (ulong*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < count; i++)
                values.Add(pValues[i]);
            return values;
        }

        private static unsafe void WriteReserved(List<ulong>? values, ref ctl_lda_args_t._Reserved_e__FixedBuffer buffer)
        {
            const int count = 4;
            var pValues = (ulong*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < count; i++)
                pValues[i] = 0;

            if (values == null)
                return;

            var writeCount = Math.Min(values.Count, count);
            for (var i = 0; i < writeCount; i++)
                pValues[i] = values[i];
        }
    }

    /// <summary>
    /// DTO for linked display adapters query result.
    /// </summary>
    public struct LinkedDisplayAdaptersResultDto
    {
        public LinkedDisplayAdaptersArgsDto Args;
        public List<nint>? LinkedAdapters;

        public static LinkedDisplayAdaptersResultDto FromNative(ctl_lda_args_t args, IntPtr[] linkedAdapters)
        {
            var adapters = new List<nint>(linkedAdapters.Length);
            for (var i = 0; i < linkedAdapters.Length; i++)
                adapters.Add(linkedAdapters[i]);

            return new LinkedDisplayAdaptersResultDto
            {
                Args = LinkedDisplayAdaptersArgsDto.FromNative(args),
                LinkedAdapters = adapters
            };
        }
    }

}


