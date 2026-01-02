using System;
using System.Collections.Generic;

namespace IGCLWrapper
{
    /// <summary>
    /// Firmware helper: base and component firmware queries, PCIe link speed control.
    /// </summary>
    public sealed class IGCLFirmwareHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLFirmwareHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe ctl_firmware_properties_t GetProperties()
        {
            ThrowIfDisposed();
            var props = IGCLApiHelper.CreateFirmwareProperties();
            var result = IGCL.ctlGetFirmwareProperties((_ctl_device_adapter_handle_t*)_adapter, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get firmware properties");
            return props;
        }

        public unsafe IReadOnlyList<IntPtr> EnumerateComponents()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        public unsafe ctl_firmware_component_properties_t GetComponentProperties(IntPtr firmwareHandle)
        {
            ThrowIfDisposed();
            var props = IGCLApiHelper.CreateFirmwareComponentProperties();
            var result = IGCL.ctlGetFirmwareComponentProperties((_ctl_firmware_component_handle_t*)firmwareHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get firmware component properties");
            return props;
        }

        public unsafe void AllowPcieLinkSpeedUpdate(bool allow)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlAllowPCIeLinkSpeedUpdate((_ctl_device_adapter_handle_t*)_adapter, (byte)(allow ? 1 : 0));
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to update PCIe link speed allowance");
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumerateFirmwareComponents(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get firmware component count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumerateFirmwareComponents(adapter, &count, (_ctl_firmware_component_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate firmware components");
            }
            return handles;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLFirmwareHelper));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
