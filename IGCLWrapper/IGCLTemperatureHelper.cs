using System;
using System.Collections.Generic;

namespace IGCLWrapper
{
    /// <summary>
    /// Temperature helper: enumerate sensors, properties, and current temperature.
    /// </summary>
    public sealed class IGCLTemperatureHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLTemperatureHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe IReadOnlyList<IntPtr> EnumTemperatureSensors()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        public unsafe ctl_temp_properties_t TemperatureGetProperties(IntPtr sensorHandle)
        {
            ThrowIfDisposed();
            var props = CreateTemperatureProperties();
            var result = IGCL.ctlTemperatureGetProperties((_ctl_temp_handle_t*)sensorHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get temperature properties");
            return props;
        }

        public unsafe double TemperatureGetState(IntPtr sensorHandle)
        {
            ThrowIfDisposed();
            double temp = 0;
            var result = IGCL.ctlTemperatureGetState((_ctl_temp_handle_t*)sensorHandle, &temp);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get temperature state");
            return temp;
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumTemperatureSensors(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get temperature sensor count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumTemperatureSensors(adapter, &count, (_ctl_temp_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate temperature sensors");
            }
            return handles;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLTemperatureHelper));
        }

        private static unsafe ctl_temp_properties_t CreateTemperatureProperties() => new ctl_temp_properties_t { Size = (uint)sizeof(ctl_temp_properties_t), Version = 0 };

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
