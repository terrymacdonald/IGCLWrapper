using System;

namespace IGCLWrapper
{
    /// <summary>
    /// Overclock helper: properties, offsets, limits, telemetry, and reset.
    /// </summary>
    public sealed class IGCLOverclockHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLOverclockHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe ctl_oc_properties_t GetProperties()
        {
            ThrowIfDisposed();
            var props = IGCLApiHelper.CreateOverclockProperties();
            var result = IGCL.ctlOverclockGetProperties((_ctl_device_adapter_handle_t*)_adapter, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get overclock properties");
            return props;
        }

        public unsafe void SetWaiver()
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockWaiverSet((_ctl_device_adapter_handle_t*)_adapter);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set overclock waiver");
        }

        public unsafe double GetGpuFrequencyOffset()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockGpuFrequencyOffsetGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get GPU frequency offset");
            return value;
        }

        public unsafe void SetGpuFrequencyOffset(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockGpuFrequencyOffsetSetV2((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set GPU frequency offset");
        }

        public unsafe double GetGpuVoltageOffset()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockGpuMaxVoltageOffsetGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get GPU voltage offset");
            return value;
        }

        public unsafe void SetGpuVoltageOffset(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockGpuMaxVoltageOffsetSetV2((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set GPU voltage offset");
        }

        public unsafe double GetVramFrequencyOffset()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockVramFrequencyOffsetGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get VRAM frequency offset");
            return value;
        }

        public unsafe void SetVramFrequencyOffset(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockVramFrequencyOffsetSet((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set VRAM frequency offset");
        }

        public unsafe double GetPowerLimit()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockPowerLimitGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power limit");
            return value;
        }

        public unsafe void SetPowerLimit(double limit)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockPowerLimitSetV2((_ctl_device_adapter_handle_t*)_adapter, limit);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set power limit");
        }

        public unsafe double GetTemperatureLimit()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockTemperatureLimitGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get temperature limit");
            return value;
        }

        public unsafe void SetTemperatureLimit(double value)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockTemperatureLimitSetV2((_ctl_device_adapter_handle_t*)_adapter, value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set temperature limit");
        }

        public unsafe ctl_power_telemetry_t GetPowerTelemetry()
        {
            ThrowIfDisposed();
            var telemetry = IGCLApiHelper.CreatePowerTelemetry();
            var result = IGCL.ctlPowerTelemetryGet((_ctl_device_adapter_handle_t*)_adapter, &telemetry);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power telemetry");
            return telemetry;
        }

        public unsafe void ResetToDefault()
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockResetToDefault((_ctl_device_adapter_handle_t*)_adapter);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to reset overclock to default");
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLOverclockHelper));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
