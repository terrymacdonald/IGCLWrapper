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
        public static unsafe ctl_combined_display_args_t CreateCombinedDisplayArgs() => new ctl_combined_display_args_t { Size = (uint)sizeof(ctl_combined_display_args_t), Version = 0 };
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
        /// Get adapter properties.
        /// </summary>
        /// <returns>Adapter properties struct.</returns>
        public unsafe ctl_device_adapter_properties_t GetProperties()
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
        public unsafe ctl_device_adapter_properties_t GetDeviceProperties()
        {
            ThrowIfDisposed();
            var props = CreateAdapterProperties();
            var result = IGCL.ctlGetDeviceProperties((_ctl_device_adapter_handle_t*)AdapterHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get device properties");
            return props;
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
                var props = GetProperties();
                var pName = (sbyte*)Unsafe.AsPointer(ref props.name);
                return new string(pName);
            }
        }

        /// <summary>
        /// PCI vendor identifier as a hexadecimal string.
        /// </summary>
        public string PciVendorId => GetProperties().pci_vendor_id.ToString("X4");

        /// <summary>
        /// Call the native get/set combined display API using the provided struct.
        /// </summary>
        /// <param name="args">Combined display args struct.</param>
        /// <returns>Updated combined display args struct.</returns>
        public unsafe ctl_combined_display_args_t GetSetCombinedDisplayNative(ctl_combined_display_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)AdapterHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set combined display");
            return copy;
        }

        private unsafe byte GetCombinedDisplayMaxOutputs()
        {
            var args = CreateCombinedDisplayArgs();
            args.OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_IS_SUPPORTED_CONFIG;
            var native = GetSetCombinedDisplayNative(args);
            return native.NumOutputs;
        }

        private static unsafe CombinedDisplayChildInfoDto[] CopyCombinedDisplayChildInfos(ctl_combined_display_child_info_t* childInfo, int count)
        {
            if (childInfo == null || count <= 0)
                return Array.Empty<CombinedDisplayChildInfoDto>();

            var results = new CombinedDisplayChildInfoDto[count];
            for (var i = 0; i < count; i++)
            {
                results[i] = CombinedDisplayChildInfoDto.FromNative(childInfo[i]);
            }
            return results;
        }

        /// <summary>
        /// Get combined display settings as a DTO.
        /// </summary>
        /// <returns>Combined display args DTO.</returns>
        public CombinedDisplayArgsDto GetCombinedDisplay()
        {
            var args = new CombinedDisplayArgsDto
            {
                OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_QUERY_CONFIG
            };
            return GetCombinedDisplay(args);
        }

        /// <summary>
        /// Get combined display settings using the provided DTO.
        /// </summary>
        /// <param name="args">Combined display args DTO.</param>
        /// <returns>Updated combined display args DTO.</returns>
        public CombinedDisplayArgsDto GetCombinedDisplay(CombinedDisplayArgsDto args)
        {
            var request = args;
            if (request.OpType == 0)
                request.OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_QUERY_CONFIG;

            var childInfos = request.ChildInfos;
            var needsChildInfo = request.OpType == ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_QUERY_CONFIG;
            var maxOutputs = (byte)0;

            if (needsChildInfo && (childInfos == null || childInfos.Length == 0))
            {
                maxOutputs = GetCombinedDisplayMaxOutputs();
            }

            if (childInfos != null && childInfos.Length > 0)
            {
                var nativeChildren = new ctl_combined_display_child_info_t[childInfos.Length];
                for (var i = 0; i < childInfos.Length; i++)
                {
                    nativeChildren[i] = childInfos[i].ToNative();
                }

                unsafe
                {
                    fixed (ctl_combined_display_child_info_t* pChildInfo = nativeChildren)
                    {
                        var nativeRequest = request.ToNative();
                        nativeRequest.pChildInfo = pChildInfo;
                        if (nativeRequest.NumOutputs == 0)
                            nativeRequest.NumOutputs = (byte)nativeChildren.Length;

                        var native = GetSetCombinedDisplayNative(nativeRequest);
                        var dto = CombinedDisplayArgsDto.FromNative(native);
                        dto.ChildInfos = CopyCombinedDisplayChildInfos(pChildInfo, native.NumOutputs);
                        return dto;
                    }
                }
            }

            if (needsChildInfo && maxOutputs > 0)
            {
                var nativeChildren = new ctl_combined_display_child_info_t[maxOutputs];
                unsafe
                {
                    fixed (ctl_combined_display_child_info_t* pChildInfo = nativeChildren)
                    {
                        var nativeRequest = request.ToNative();
                        nativeRequest.pChildInfo = pChildInfo;
                        nativeRequest.NumOutputs = 0;

                        var native = GetSetCombinedDisplayNative(nativeRequest);
                        var dto = CombinedDisplayArgsDto.FromNative(native);
                        dto.ChildInfos = CopyCombinedDisplayChildInfos(pChildInfo, native.NumOutputs);
                        return dto;
                    }
                }
            }

            var fallback = GetSetCombinedDisplayNative(request.ToNative());
            return CombinedDisplayArgsDto.FromNative(fallback);
        }

        /// <summary>
        /// Set combined display settings using the provided DTO.
        /// </summary>
        /// <param name="args">Combined display args DTO.</param>
        public void SetCombinedDisplay(CombinedDisplayArgsDto args)
        {
            if (args.OpType == 0)
                throw new ArgumentException("OpType must be set for combined display operations.", nameof(args));

            var childInfos = args.ChildInfos;
            if (childInfos == null || childInfos.Length == 0)
            {
                GetSetCombinedDisplayNative(args.ToNative());
                return;
            }

            var nativeChildren = new ctl_combined_display_child_info_t[childInfos.Length];
            for (var i = 0; i < childInfos.Length; i++)
            {
                nativeChildren[i] = childInfos[i].ToNative();
            }

            unsafe
            {
                fixed (ctl_combined_display_child_info_t* pChildInfo = nativeChildren)
                {
                    var nativeRequest = args.ToNative();
                    nativeRequest.pChildInfo = pChildInfo;
                    if (nativeRequest.NumOutputs == 0)
                        nativeRequest.NumOutputs = (byte)nativeChildren.Length;
                    GetSetCombinedDisplayNative(nativeRequest);
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
    /// DTO for combined display arguments.
    /// </summary>
    public unsafe struct CombinedDisplayArgsDto
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
        /// Indicates whether the feature is supported.
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
        /// Pointer to child info.
        /// </summary>
        public IntPtr ChildInfo;
        /// <summary>
        /// Managed child display info list.
        /// </summary>
        public CombinedDisplayChildInfoDto[]? ChildInfos;
        /// <summary>
        /// Combined display output handle.
        /// </summary>
        public IntPtr CombinedDisplayOutput;

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
                ChildInfo = (IntPtr)native.pChildInfo,
                CombinedDisplayOutput = (IntPtr)native.hCombinedDisplayOutput
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

    /// <summary>
    /// DTO for combined display child information.
    /// </summary>
    public unsafe struct CombinedDisplayChildInfoDto
    {
        /// <summary>
        /// Display output handle.
        /// </summary>
        public IntPtr DisplayOutput;
        /// <summary>
        /// Framebuffer source rect.
        /// </summary>
        public ctl_rect_t FbSrc;
        /// <summary>
        /// Framebuffer target rect.
        /// </summary>
        public ctl_rect_t FbPos;
        /// <summary>
        /// Display orientation.
        /// </summary>
        public ctl_display_orientation_t DisplayOrientation;
        /// <summary>
        /// Target mode info.
        /// </summary>
        public ctl_child_display_target_mode_t TargetMode;

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Child info DTO.</returns>
        public static CombinedDisplayChildInfoDto FromNative(ctl_combined_display_child_info_t native)
        {
            return new CombinedDisplayChildInfoDto
            {
                DisplayOutput = (IntPtr)native.hDisplayOutput,
                FbSrc = native.FbSrc,
                FbPos = native.FbPos,
                DisplayOrientation = native.DisplayOrientation,
                TargetMode = native.TargetMode
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Child info struct.</returns>
        public ctl_combined_display_child_info_t ToNative()
        {
            return new ctl_combined_display_child_info_t
            {
                hDisplayOutput = (_ctl_display_output_handle_t*)DisplayOutput,
                FbSrc = FbSrc,
                FbPos = FbPos,
                DisplayOrientation = DisplayOrientation,
                TargetMode = TargetMode
            };
        }
    }

    /// <summary>
    /// DTO for genlock arguments.
    /// </summary>
    public struct GenlockArgsDto
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
        public ctl_genlock_topology_t GenlockTopology;
        /// <summary>
        /// Indicates whether genlock is enabled.
        /// </summary>
        public bool IsGenlockEnabled;
        /// <summary>
        /// Indicates whether genlock is possible.
        /// </summary>
        public bool IsGenlockPossible;

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
                GenlockTopology = native.GenlockTopology,
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
                GenlockTopology = GenlockTopology,
                IsGenlockEnabled = IGCLDisplayDtoBool.ToByte(IsGenlockEnabled),
                IsGenlockPossible = IGCLDisplayDtoBool.ToByte(IsGenlockPossible)
            };
        }
    }

}
