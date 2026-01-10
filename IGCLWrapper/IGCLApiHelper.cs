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
        /// Enumerate displays for this adapter.
        /// </summary>
        /// <returns>Read-only list of display helpers.</returns>
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

}
