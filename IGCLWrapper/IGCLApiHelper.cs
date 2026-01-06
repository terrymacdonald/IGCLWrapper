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
        internal IntPtr ApiHandle => _api?.DangerousGetHandle() ?? IntPtr.Zero;

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

        private static unsafe ctl_device_adapter_properties_t CreateAdapterProperties() => new ctl_device_adapter_properties_t { Size = (uint)sizeof(ctl_device_adapter_properties_t), Version = 1 };

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

        public IReadOnlyList<IGCLDisplayHelper> GetDisplays()
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

        private static unsafe ctl_wait_property_change_args_t CreateWaitPropertyChangeArgs() => new ctl_wait_property_change_args_t { Size = (uint)sizeof(ctl_wait_property_change_args_t), Version = 0 };

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
