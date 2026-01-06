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
        private const string OverclockError = "Failed to perform overclock operation";

        internal IGCLOverclockHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe ctl_oc_properties_t GetProperties()
        {
            ThrowIfDisposed();
            var props = CreateOverclockProperties();
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
                throw new IGCLException(result, OverclockError);
        }

        #region GPU frequency offset
        public unsafe double OverclockGpuFrequencyOffsetGet()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockGpuFrequencyOffsetGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        public unsafe void OverclockGpuFrequencyOffsetSet(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockGpuFrequencyOffsetSet((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        public unsafe double OverclockGpuFrequencyOffsetGetV2()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockGpuFrequencyOffsetGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        public unsafe void OverclockGpuFrequencyOffsetSetV2(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockGpuFrequencyOffsetSetV2((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }
        #endregion

        #region GPU voltage offset
        public unsafe double OverclockGpuVoltageOffsetGet()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockGpuVoltageOffsetGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        public unsafe void OverclockGpuVoltageOffsetSet(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockGpuVoltageOffsetSet((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        public unsafe double OverclockGpuMaxVoltageOffsetGetV2()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockGpuMaxVoltageOffsetGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        public unsafe void OverclockGpuMaxVoltageOffsetSetV2(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockGpuMaxVoltageOffsetSetV2((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }
        #endregion

        #region GPU lock
        public unsafe ctl_oc_vf_pair_t OverclockGpuLockGet()
        {
            ThrowIfDisposed();
            var pair = CreateVfPair();
            var result = IGCL.ctlOverclockGpuLockGet((_ctl_device_adapter_handle_t*)_adapter, &pair);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return pair;
        }

        public unsafe void OverclockGpuLockSet(ctl_oc_vf_pair_t pair)
        {
            ThrowIfDisposed();
            if (pair.Size == 0)
            {
                var init = CreateVfPair();
                init.Frequency = pair.Frequency;
                init.Voltage = pair.Voltage;
                pair = init;
            }
            var result = IGCL.ctlOverclockGpuLockSet((_ctl_device_adapter_handle_t*)_adapter, pair);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }
        #endregion

        #region VRAM offsets and speed limits
        public unsafe double OverclockVramFrequencyOffsetGet()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockVramFrequencyOffsetGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        public unsafe void OverclockVramFrequencyOffsetSet(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockVramFrequencyOffsetSet((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        public unsafe double OverclockVramVoltageOffsetGet()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockVramVoltageOffsetGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        public unsafe void OverclockVramVoltageOffsetSet(double voltage)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockVramVoltageOffsetSet((_ctl_device_adapter_handle_t*)_adapter, voltage);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        public unsafe double OverclockVramMemSpeedLimitGetV2()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockVramMemSpeedLimitGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        public unsafe void OverclockVramMemSpeedLimitSetV2(double speed)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockVramMemSpeedLimitSetV2((_ctl_device_adapter_handle_t*)_adapter, speed);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }
        #endregion

        #region Power limits
        public unsafe double OverclockPowerLimitGet()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockPowerLimitGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        public unsafe void OverclockPowerLimitSet(double limit)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockPowerLimitSet((_ctl_device_adapter_handle_t*)_adapter, limit);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        public unsafe double OverclockPowerLimitGetV2()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockPowerLimitGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        public unsafe void OverclockPowerLimitSetV2(double limit)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockPowerLimitSetV2((_ctl_device_adapter_handle_t*)_adapter, limit);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }
        #endregion

        #region Temperature limits
        public unsafe double OverclockTemperatureLimitGet()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockTemperatureLimitGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        public unsafe void OverclockTemperatureLimitSet(double value)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockTemperatureLimitSet((_ctl_device_adapter_handle_t*)_adapter, value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        public unsafe double OverclockTemperatureLimitGetV2()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockTemperatureLimitGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        public unsafe void OverclockTemperatureLimitSetV2(double value)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockTemperatureLimitSetV2((_ctl_device_adapter_handle_t*)_adapter, value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }
        #endregion

        public unsafe ctl_power_telemetry_t GetPowerTelemetry()
        {
            ThrowIfDisposed();
            var telemetry = CreatePowerTelemetry();
            var result = IGCL.ctlPowerTelemetryGet((_ctl_device_adapter_handle_t*)_adapter, &telemetry);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return telemetry;
        }

        public unsafe void ResetToDefault()
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockResetToDefault((_ctl_device_adapter_handle_t*)_adapter);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        #region VF curve
        public unsafe ctl_voltage_frequency_point_t[] OverclockReadVFCurve(ctl_vf_curve_type_t curveType, ctl_vf_curve_details_t detail)
        {
            ThrowIfDisposed();
            uint count = 0;
            var result = IGCL.ctlOverclockReadVFCurve((_ctl_device_adapter_handle_t*)_adapter, curveType, detail, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, OverclockError);
            if (count == 0)
                return Array.Empty<ctl_voltage_frequency_point_t>();

            var points = new ctl_voltage_frequency_point_t[count];
            fixed (ctl_voltage_frequency_point_t* pPoints = points)
            {
                result = IGCL.ctlOverclockReadVFCurve((_ctl_device_adapter_handle_t*)_adapter, curveType, detail, &count, pPoints);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, OverclockError);
            }

            return points;
        }

        public unsafe void OverclockWriteCustomVFCurve(ctl_voltage_frequency_point_t[] points)
        {
            ThrowIfDisposed();
            if (points == null || points.Length == 0)
                throw new ArgumentException("At least one VF point is required", nameof(points));

            uint numPoints = (uint)points.Length;
            fixed (ctl_voltage_frequency_point_t* pPoints = points)
            {
                var result = IGCL.ctlOverclockWriteCustomVFCurve((_ctl_device_adapter_handle_t*)_adapter, numPoints, pPoints);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, OverclockError);
            }
        }
        #endregion

        #region Convenience aliases (existing naming)
        public double GetGpuFrequencyOffset() => OverclockGpuFrequencyOffsetGetV2();
        public void SetGpuFrequencyOffset(double offset) => OverclockGpuFrequencyOffsetSetV2(offset);
        public double GetGpuVoltageOffset() => OverclockGpuMaxVoltageOffsetGetV2();
        public void SetGpuVoltageOffset(double offset) => OverclockGpuMaxVoltageOffsetSetV2(offset);
        public double GetVramFrequencyOffset() => OverclockVramFrequencyOffsetGet();
        public void SetVramFrequencyOffset(double offset) => OverclockVramFrequencyOffsetSet(offset);
        public double GetPowerLimit() => OverclockPowerLimitGetV2();
        public void SetPowerLimit(double limit) => OverclockPowerLimitSetV2(limit);
        public double GetTemperatureLimit() => OverclockTemperatureLimitGetV2();
        public void SetTemperatureLimit(double value) => OverclockTemperatureLimitSetV2(value);
        #endregion

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLOverclockHelper));
        }

        private static unsafe ctl_oc_properties_t CreateOverclockProperties() => new ctl_oc_properties_t { Size = (uint)sizeof(ctl_oc_properties_t), Version = 0 };
        public static unsafe ctl_oc_vf_pair_t CreateVfPair() => new ctl_oc_vf_pair_t { Size = (uint)sizeof(ctl_oc_vf_pair_t), Version = 0 };
        private static unsafe ctl_power_telemetry_t CreatePowerTelemetry() => new ctl_power_telemetry_t { Size = (uint)sizeof(ctl_power_telemetry_t), Version = 0 };

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
