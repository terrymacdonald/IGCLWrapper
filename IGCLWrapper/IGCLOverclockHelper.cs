using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

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

        /// <summary>
        /// Get overclock properties as a DTO.
        /// </summary>
        /// <returns>Overclock properties DTO.</returns>
        public unsafe OverclockPropertiesDto GetProperties()
        {
            ThrowIfDisposed();
            var props = CreateOverclockProperties();
            var result = IGCL.ctlOverclockGetProperties((_ctl_device_adapter_handle_t*)_adapter, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get overclock properties");
            return OverclockPropertiesDto.FromNative(props);
        }

        /// <summary>
        /// Set the overclocking waiver for this adapter.
        /// </summary>
        public unsafe void SetWaiver()
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockWaiverSet((_ctl_device_adapter_handle_t*)_adapter);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        #region GPU frequency offset
        /// <summary>
        /// Get the GPU frequency offset.
        /// </summary>
        /// <returns>GPU frequency offset.</returns>
        public unsafe double OverclockGpuFrequencyOffsetGet()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockGpuFrequencyOffsetGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        /// <summary>
        /// Set the GPU frequency offset.
        /// </summary>
        /// <param name="offset">Frequency offset value.</param>
        public unsafe void OverclockGpuFrequencyOffsetSet(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockGpuFrequencyOffsetSet((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        /// <summary>
        /// Get the GPU frequency offset (V2).
        /// </summary>
        /// <returns>GPU frequency offset.</returns>
        public unsafe double OverclockGpuFrequencyOffsetGetV2()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockGpuFrequencyOffsetGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        /// <summary>
        /// Set the GPU frequency offset (V2).
        /// </summary>
        /// <param name="offset">Frequency offset value.</param>
        public unsafe void OverclockGpuFrequencyOffsetSetV2(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockGpuFrequencyOffsetSetV2((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }
        #endregion

        #region GPU voltage offset
        /// <summary>
        /// Get the GPU voltage offset.
        /// </summary>
        /// <returns>GPU voltage offset.</returns>
        public unsafe double OverclockGpuVoltageOffsetGet()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockGpuVoltageOffsetGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        /// <summary>
        /// Set the GPU voltage offset.
        /// </summary>
        /// <param name="offset">Voltage offset value.</param>
        public unsafe void OverclockGpuVoltageOffsetSet(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockGpuVoltageOffsetSet((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        /// <summary>
        /// Get the GPU max voltage offset (V2).
        /// </summary>
        /// <returns>GPU max voltage offset.</returns>
        public unsafe double OverclockGpuMaxVoltageOffsetGetV2()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockGpuMaxVoltageOffsetGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        /// <summary>
        /// Set the GPU max voltage offset (V2).
        /// </summary>
        /// <param name="offset">Voltage offset value.</param>
        public unsafe void OverclockGpuMaxVoltageOffsetSetV2(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockGpuMaxVoltageOffsetSetV2((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }
        #endregion

        #region GPU lock
        /// <summary>
        /// Get the GPU lock voltage/frequency pair.
        /// </summary>
        /// <returns>Voltage/frequency pair DTO.</returns>
        public unsafe OcVfPairDto OverclockGpuLockGet()
        {
            ThrowIfDisposed();
            var pair = CreateVfPair();
            var result = IGCL.ctlOverclockGpuLockGet((_ctl_device_adapter_handle_t*)_adapter, &pair);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return OcVfPairDto.FromNative(pair);
        }

        /// <summary>
        /// Set the GPU lock voltage/frequency pair.
        /// </summary>
        /// <param name="pair">Voltage/frequency pair DTO.</param>
        public unsafe void OverclockGpuLockSet(OcVfPairDto pair)
        {
            ThrowIfDisposed();
            var native = pair.ToNative();
            if (native.Size == 0)
            {
                var init = CreateVfPair();
                init.Frequency = native.Frequency;
                init.Voltage = native.Voltage;
                native = init;
            }
            var result = IGCL.ctlOverclockGpuLockSet((_ctl_device_adapter_handle_t*)_adapter, native);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }
        #endregion

        #region VRAM offsets and speed limits
        /// <summary>
        /// Get the VRAM frequency offset.
        /// </summary>
        /// <returns>VRAM frequency offset.</returns>
        public unsafe double OverclockVramFrequencyOffsetGet()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockVramFrequencyOffsetGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        /// <summary>
        /// Set the VRAM frequency offset.
        /// </summary>
        /// <param name="offset">Frequency offset value.</param>
        public unsafe void OverclockVramFrequencyOffsetSet(double offset)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockVramFrequencyOffsetSet((_ctl_device_adapter_handle_t*)_adapter, offset);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        /// <summary>
        /// Get the VRAM voltage offset.
        /// </summary>
        /// <returns>VRAM voltage offset.</returns>
        public unsafe double OverclockVramVoltageOffsetGet()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockVramVoltageOffsetGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        /// <summary>
        /// Set the VRAM voltage offset.
        /// </summary>
        /// <param name="voltage">Voltage offset value.</param>
        public unsafe void OverclockVramVoltageOffsetSet(double voltage)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockVramVoltageOffsetSet((_ctl_device_adapter_handle_t*)_adapter, voltage);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        /// <summary>
        /// Get the VRAM memory speed limit (V2).
        /// </summary>
        /// <returns>VRAM memory speed limit.</returns>
        public unsafe double OverclockVramMemSpeedLimitGetV2()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockVramMemSpeedLimitGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        /// <summary>
        /// Set the VRAM memory speed limit (V2).
        /// </summary>
        /// <param name="speed">Speed limit value.</param>
        public unsafe void OverclockVramMemSpeedLimitSetV2(double speed)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockVramMemSpeedLimitSetV2((_ctl_device_adapter_handle_t*)_adapter, speed);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }
        #endregion

        #region Power limits
        /// <summary>
        /// Get the overclock power limit.
        /// </summary>
        /// <returns>Power limit value.</returns>
        public unsafe double OverclockPowerLimitGet()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockPowerLimitGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        /// <summary>
        /// Set the overclock power limit.
        /// </summary>
        /// <param name="limit">Power limit value.</param>
        public unsafe void OverclockPowerLimitSet(double limit)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockPowerLimitSet((_ctl_device_adapter_handle_t*)_adapter, limit);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        /// <summary>
        /// Get the overclock power limit (V2).
        /// </summary>
        /// <returns>Power limit value.</returns>
        public unsafe double OverclockPowerLimitGetV2()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockPowerLimitGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        /// <summary>
        /// Set the overclock power limit (V2).
        /// </summary>
        /// <param name="limit">Power limit value.</param>
        public unsafe void OverclockPowerLimitSetV2(double limit)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockPowerLimitSetV2((_ctl_device_adapter_handle_t*)_adapter, limit);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }
        #endregion

        #region Temperature limits
        /// <summary>
        /// Get the overclock temperature limit.
        /// </summary>
        /// <returns>Temperature limit value.</returns>
        public unsafe double OverclockTemperatureLimitGet()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockTemperatureLimitGet((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        /// <summary>
        /// Set the overclock temperature limit.
        /// </summary>
        /// <param name="value">Temperature limit value.</param>
        public unsafe void OverclockTemperatureLimitSet(double value)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockTemperatureLimitSet((_ctl_device_adapter_handle_t*)_adapter, value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        /// <summary>
        /// Get the overclock temperature limit (V2).
        /// </summary>
        /// <returns>Temperature limit value.</returns>
        public unsafe double OverclockTemperatureLimitGetV2()
        {
            ThrowIfDisposed();
            double value = 0;
            var result = IGCL.ctlOverclockTemperatureLimitGetV2((_ctl_device_adapter_handle_t*)_adapter, &value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return value;
        }

        /// <summary>
        /// Set the overclock temperature limit (V2).
        /// </summary>
        /// <param name="value">Temperature limit value.</param>
        public unsafe void OverclockTemperatureLimitSetV2(double value)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockTemperatureLimitSetV2((_ctl_device_adapter_handle_t*)_adapter, value);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }
        #endregion

        /// <summary>
        /// Get power telemetry as a DTO.
        /// </summary>
        /// <returns>Power telemetry DTO.</returns>
        public unsafe PowerTelemetryDto GetPowerTelemetry()
        {
            ThrowIfDisposed();
            var telemetry = CreatePowerTelemetry();
            var result = IGCL.ctlPowerTelemetryGet((_ctl_device_adapter_handle_t*)_adapter, &telemetry);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return PowerTelemetryDto.FromNative(telemetry);
        }

        /// <summary>
        /// Reset overclock settings to default.
        /// </summary>
        public unsafe void ResetToDefault()
        {
            ThrowIfDisposed();
            var result = IGCL.ctlOverclockResetToDefault((_ctl_device_adapter_handle_t*)_adapter);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
        }

        #region VF curve
        /// <summary>
        /// Read the voltage/frequency curve.
        /// </summary>
        /// <param name="curveType">Curve type.</param>
        /// <param name="detail">Curve detail flags.</param>
        /// <returns>Array of voltage/frequency points.</returns>
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

        /// <summary>
        /// Write a custom voltage/frequency curve.
        /// </summary>
        /// <param name="points">Voltage/frequency points.</param>
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
        /// <summary>
        /// Get the GPU frequency offset (V2 wrapper).
        /// </summary>
        /// <returns>GPU frequency offset.</returns>
        public double GetGpuFrequencyOffset() => OverclockGpuFrequencyOffsetGetV2();
        /// <summary>
        /// Set the GPU frequency offset (V2 wrapper).
        /// </summary>
        /// <param name="offset">Frequency offset value.</param>
        public void SetGpuFrequencyOffset(double offset) => OverclockGpuFrequencyOffsetSetV2(offset);
        /// <summary>
        /// Get the GPU voltage offset (V2 wrapper).
        /// </summary>
        /// <returns>GPU voltage offset.</returns>
        public double GetGpuVoltageOffset() => OverclockGpuMaxVoltageOffsetGetV2();
        /// <summary>
        /// Set the GPU voltage offset (V2 wrapper).
        /// </summary>
        /// <param name="offset">Voltage offset value.</param>
        public void SetGpuVoltageOffset(double offset) => OverclockGpuMaxVoltageOffsetSetV2(offset);
        /// <summary>
        /// Get the VRAM frequency offset.
        /// </summary>
        /// <returns>VRAM frequency offset.</returns>
        public double GetVramFrequencyOffset() => OverclockVramFrequencyOffsetGet();
        /// <summary>
        /// Set the VRAM frequency offset.
        /// </summary>
        /// <param name="offset">Frequency offset value.</param>
        public void SetVramFrequencyOffset(double offset) => OverclockVramFrequencyOffsetSet(offset);
        /// <summary>
        /// Get the power limit (V2 wrapper).
        /// </summary>
        /// <returns>Power limit value.</returns>
        public double GetPowerLimit() => OverclockPowerLimitGetV2();
        /// <summary>
        /// Set the power limit (V2 wrapper).
        /// </summary>
        /// <param name="limit">Power limit value.</param>
        public void SetPowerLimit(double limit) => OverclockPowerLimitSetV2(limit);
        /// <summary>
        /// Get the temperature limit (V2 wrapper).
        /// </summary>
        /// <returns>Temperature limit value.</returns>
        public double GetTemperatureLimit() => OverclockTemperatureLimitGetV2();
        /// <summary>
        /// Set the temperature limit (V2 wrapper).
        /// </summary>
        /// <param name="value">Temperature limit value.</param>
        public void SetTemperatureLimit(double value) => OverclockTemperatureLimitSetV2(value);
        #endregion

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLOverclockHelper));
        }

        private static unsafe ctl_oc_properties_t CreateOverclockProperties() => new ctl_oc_properties_t { Size = (uint)sizeof(ctl_oc_properties_t), Version = 0 };
        /// <summary>
        /// Create a voltage/frequency pair struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized voltage/frequency pair struct.</returns>
        public static unsafe ctl_oc_vf_pair_t CreateVfPair() => new ctl_oc_vf_pair_t { Size = (uint)sizeof(ctl_oc_vf_pair_t), Version = 0 };
        private static unsafe ctl_power_telemetry_t CreatePowerTelemetry() => new ctl_power_telemetry_t { Size = (uint)sizeof(ctl_power_telemetry_t), Version = 0 };

        /// <summary>
        /// Compare overclock properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreOverclockPropertiesEqual(ctl_oc_properties_t left, ctl_oc_properties_t right)
        {
            return OverclockPropertiesDto.FromNative(left).Equals(OverclockPropertiesDto.FromNative(right));
        }

        /// <summary>
        /// Compare voltage/frequency pairs while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left pair struct.</param>
        /// <param name="right">Right pair struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreVfPairEqual(ctl_oc_vf_pair_t left, ctl_oc_vf_pair_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.Voltage.Equals(right.Voltage) &&
                   left.Frequency.Equals(right.Frequency);
        }

        /// <summary>
        /// Compare power telemetry while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left telemetry struct.</param>
        /// <param name="right">Right telemetry struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool ArePowerTelemetryEqual(ctl_power_telemetry_t left, ctl_power_telemetry_t right)
        {
            return PowerTelemetryDto.FromNative(left).Equals(PowerTelemetryDto.FromNative(right));
        }

        /// <summary>
        /// Compare voltage/frequency points while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left point struct.</param>
        /// <param name="right">Right point struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreVoltageFrequencyPointEqual(ctl_voltage_frequency_point_t left, ctl_voltage_frequency_point_t right)
        {
            return left.Voltage.Equals(right.Voltage) &&
                   left.Frequency.Equals(right.Frequency);
        }

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCLOverclockDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    /// <summary>
    /// DTO for telemetry data value union.
    /// </summary>
    public struct DataValueDto : IEquatable<DataValueDto>
    {
        /// <summary>
        /// Signed 8-bit value.
        /// </summary>
        public sbyte Data8;
        /// <summary>
        /// Unsigned 8-bit value.
        /// </summary>
        public byte DataU8;
        /// <summary>
        /// Signed 16-bit value.
        /// </summary>
        public short Data16;
        /// <summary>
        /// Unsigned 16-bit value.
        /// </summary>
        public ushort DataU16;
        /// <summary>
        /// Signed 32-bit value.
        /// </summary>
        public int Data32;
        /// <summary>
        /// Unsigned 32-bit value.
        /// </summary>
        public uint DataU32;
        /// <summary>
        /// Signed 64-bit value.
        /// </summary>
        public long Data64;
        /// <summary>
        /// Unsigned 64-bit value.
        /// </summary>
        public ulong DataU64;
        /// <summary>
        /// Float value.
        /// </summary>
        public float DataFloat;
        /// <summary>
        /// Double value.
        /// </summary>
        public double DataDouble;

        public bool Equals(DataValueDto other)
        {
            return Data8 == other.Data8 &&
                   DataU8 == other.DataU8 &&
                   Data16 == other.Data16 &&
                   DataU16 == other.DataU16 &&
                   Data32 == other.Data32 &&
                   DataU32 == other.DataU32 &&
                   Data64 == other.Data64 &&
                   DataU64 == other.DataU64 &&
                   DataFloat.Equals(other.DataFloat) &&
                   DataDouble.Equals(other.DataDouble);
        }

        public override bool Equals(object? obj) => obj is DataValueDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Data8);
            hash.Add(DataU8);
            hash.Add(Data16);
            hash.Add(DataU16);
            hash.Add(Data32);
            hash.Add(DataU32);
            hash.Add(Data64);
            hash.Add(DataU64);
            hash.Add(DataFloat);
            hash.Add(DataDouble);
            return hash.ToHashCode();
        }

        public static DataValueDto FromNative(ctl_data_value_t native)
        {
            return new DataValueDto
            {
                Data8 = native.data8,
                DataU8 = native.datau8,
                Data16 = native.data16,
                DataU16 = native.datau16,
                Data32 = native.data32,
                DataU32 = native.datau32,
                Data64 = native.data64,
                DataU64 = native.datau64,
                DataFloat = native.datafloat,
                DataDouble = native.datadouble
            };
        }

        public ctl_data_value_t ToNative()
        {
            var native = new ctl_data_value_t();
            if (!double.IsNaN(DataDouble) && DataDouble != 0d)
                native.datadouble = DataDouble;
            else if (!float.IsNaN(DataFloat) && DataFloat != 0f)
                native.datafloat = DataFloat;
            else if (DataU64 != 0)
                native.datau64 = DataU64;
            else if (Data64 != 0)
                native.data64 = Data64;
            else if (DataU32 != 0)
                native.datau32 = DataU32;
            else if (Data32 != 0)
                native.data32 = Data32;
            else if (DataU16 != 0)
                native.datau16 = DataU16;
            else if (Data16 != 0)
                native.data16 = Data16;
            else if (DataU8 != 0)
                native.datau8 = DataU8;
            else
                native.data8 = Data8;
            return native;
        }
    }

    /// <summary>
    /// DTO for overclock control information.
    /// </summary>
    public struct OcControlInfoDto : IEquatable<OcControlInfoDto>
    {
        /// <summary>
        /// Indicates whether the control is supported.
        /// </summary>
        public bool IsSupported;
        /// <summary>
        /// Indicates whether the control is relative.
        /// </summary>
        public bool IsRelative;
        /// <summary>
        /// Indicates whether the control is reference-based.
        /// </summary>
        public bool IsReference;
        /// <summary>
        /// Units for the control values.
        /// </summary>
        public ctl_units_t Units;
        /// <summary>
        /// Minimum value.
        /// </summary>
        public double Min;
        /// <summary>
        /// Maximum value.
        /// </summary>
        public double Max;
        /// <summary>
        /// Step size.
        /// </summary>
        public double Step;
        /// <summary>
        /// Default value.
        /// </summary>
        public double Default;
        /// <summary>
        /// Reference value.
        /// </summary>
        public double Reference;

        /// <summary>
        /// Compare overclock control info.
        /// </summary>
        /// <param name="other">Other control info instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(OcControlInfoDto other)
        {
            return IsSupported == other.IsSupported &&
                   IsRelative == other.IsRelative &&
                   IsReference == other.IsReference &&
                   Units == other.Units &&
                   Min.Equals(other.Min) &&
                   Max.Equals(other.Max) &&
                   Step.Equals(other.Step) &&
                   Default.Equals(other.Default) &&
                   Reference.Equals(other.Reference);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is OcControlInfoDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(IsSupported);
            hash.Add(IsRelative);
            hash.Add(IsReference);
            hash.Add(Units);
            hash.Add(Min);
            hash.Add(Max);
            hash.Add(Step);
            hash.Add(Default);
            hash.Add(Reference);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Overclock control info DTO.</returns>
        public static OcControlInfoDto FromNative(ctl_oc_control_info_t native)
        {
            return new OcControlInfoDto
            {
                IsSupported = IGCLOverclockDtoBool.ToBool(native.bSupported),
                IsRelative = IGCLOverclockDtoBool.ToBool(native.bRelative),
                IsReference = IGCLOverclockDtoBool.ToBool(native.bReference),
                Units = native.units,
                Min = native.min,
                Max = native.max,
                Step = native.step,
                Default = native.Default,
                Reference = native.reference
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Overclock control info struct.</returns>
        public ctl_oc_control_info_t ToNative()
        {
            return new ctl_oc_control_info_t
            {
                bSupported = IGCLOverclockDtoBool.ToByte(IsSupported),
                bRelative = IGCLOverclockDtoBool.ToByte(IsRelative),
                bReference = IGCLOverclockDtoBool.ToByte(IsReference),
                units = Units,
                min = Min,
                max = Max,
                step = Step,
                Default = Default,
                reference = Reference
            };
        }
    }

    /// <summary>
    /// DTO for overclock properties.
    /// </summary>
    public struct OverclockPropertiesDto : IEquatable<OverclockPropertiesDto>
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
        /// Indicates whether overclocking is supported.
        /// </summary>
        public bool IsSupported;
        /// <summary>
        /// GPU frequency offset control info.
        /// </summary>
        public OcControlInfoDto GpuFrequencyOffset;
        /// <summary>
        /// GPU voltage offset control info.
        /// </summary>
        public OcControlInfoDto GpuVoltageOffset;
        /// <summary>
        /// VRAM frequency offset control info.
        /// </summary>
        public OcControlInfoDto VramFrequencyOffset;
        /// <summary>
        /// VRAM voltage offset control info.
        /// </summary>
        public OcControlInfoDto VramVoltageOffset;
        /// <summary>
        /// Power limit control info.
        /// </summary>
        public OcControlInfoDto PowerLimit;
        /// <summary>
        /// Temperature limit control info.
        /// </summary>
        public OcControlInfoDto TemperatureLimit;
        /// <summary>
        /// VRAM memory speed limit control info.
        /// </summary>
        public OcControlInfoDto VramMemSpeedLimit;
        /// <summary>
        /// GPU VF curve voltage limit control info.
        /// </summary>
        public OcControlInfoDto GpuVfCurveVoltageLimit;
        /// <summary>
        /// GPU VF curve frequency limit control info.
        /// </summary>
        public OcControlInfoDto GpuVfCurveFrequencyLimit;

        /// <summary>
        /// Compare overclock properties.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(OverclockPropertiesDto other)
        {
            return IsSupported == other.IsSupported &&
                   GpuFrequencyOffset.Equals(other.GpuFrequencyOffset) &&
                   GpuVoltageOffset.Equals(other.GpuVoltageOffset) &&
                   VramFrequencyOffset.Equals(other.VramFrequencyOffset) &&
                   VramVoltageOffset.Equals(other.VramVoltageOffset) &&
                   PowerLimit.Equals(other.PowerLimit) &&
                   TemperatureLimit.Equals(other.TemperatureLimit) &&
                   VramMemSpeedLimit.Equals(other.VramMemSpeedLimit) &&
                   GpuVfCurveVoltageLimit.Equals(other.GpuVfCurveVoltageLimit) &&
                   GpuVfCurveFrequencyLimit.Equals(other.GpuVfCurveFrequencyLimit);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is OverclockPropertiesDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(IsSupported);
            hash.Add(GpuFrequencyOffset);
            hash.Add(GpuVoltageOffset);
            hash.Add(VramFrequencyOffset);
            hash.Add(VramVoltageOffset);
            hash.Add(PowerLimit);
            hash.Add(TemperatureLimit);
            hash.Add(VramMemSpeedLimit);
            hash.Add(GpuVfCurveVoltageLimit);
            hash.Add(GpuVfCurveFrequencyLimit);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Overclock properties DTO.</returns>
        public static OverclockPropertiesDto FromNative(ctl_oc_properties_t native)
        {
            return new OverclockPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                IsSupported = IGCLOverclockDtoBool.ToBool(native.bSupported),
                GpuFrequencyOffset = OcControlInfoDto.FromNative(native.gpuFrequencyOffset),
                GpuVoltageOffset = OcControlInfoDto.FromNative(native.gpuVoltageOffset),
                VramFrequencyOffset = OcControlInfoDto.FromNative(native.vramFrequencyOffset),
                VramVoltageOffset = OcControlInfoDto.FromNative(native.vramVoltageOffset),
                PowerLimit = OcControlInfoDto.FromNative(native.powerLimit),
                TemperatureLimit = OcControlInfoDto.FromNative(native.temperatureLimit),
                VramMemSpeedLimit = OcControlInfoDto.FromNative(native.vramMemSpeedLimit),
                GpuVfCurveVoltageLimit = OcControlInfoDto.FromNative(native.gpuVFCurveVoltageLimit),
                GpuVfCurveFrequencyLimit = OcControlInfoDto.FromNative(native.gpuVFCurveFrequencyLimit)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Overclock properties struct.</returns>
        public unsafe ctl_oc_properties_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_oc_properties_t);

            return new ctl_oc_properties_t
            {
                Size = size,
                Version = Version,
                bSupported = IGCLOverclockDtoBool.ToByte(IsSupported),
                gpuFrequencyOffset = GpuFrequencyOffset.ToNative(),
                gpuVoltageOffset = GpuVoltageOffset.ToNative(),
                vramFrequencyOffset = VramFrequencyOffset.ToNative(),
                vramVoltageOffset = VramVoltageOffset.ToNative(),
                powerLimit = PowerLimit.ToNative(),
                temperatureLimit = TemperatureLimit.ToNative(),
                vramMemSpeedLimit = VramMemSpeedLimit.ToNative(),
                gpuVFCurveVoltageLimit = GpuVfCurveVoltageLimit.ToNative(),
                gpuVFCurveFrequencyLimit = GpuVfCurveFrequencyLimit.ToNative()
            };
        }
    }

    /// <summary>
    /// DTO for overclock telemetry item.
    /// </summary>
    public struct OcTelemetryItemDto : IEquatable<OcTelemetryItemDto>
    {
        /// <summary>
        /// Indicates whether this item is supported.
        /// </summary>
        public bool IsSupported;
        /// <summary>
        /// Units for the value.
        /// </summary>
        public ctl_units_t Units;
        /// <summary>
        /// Data type for the value.
        /// </summary>
        public ctl_data_type_t Type;
        /// <summary>
        /// Telemetry value.
        /// </summary>
        public DataValueDto Value;

        /// <summary>
        /// Compare telemetry items.
        /// </summary>
        /// <param name="other">Other telemetry item.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(OcTelemetryItemDto other)
        {
            return IsSupported == other.IsSupported &&
                   Units == other.Units &&
                   Type == other.Type &&
                   Value.Equals(other.Value);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is OcTelemetryItemDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(IsSupported);
            hash.Add(Units);
            hash.Add(Type);
            hash.Add(Value);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Telemetry item DTO.</returns>
        public static OcTelemetryItemDto FromNative(ctl_oc_telemetry_item_t native)
        {
            return new OcTelemetryItemDto
            {
                IsSupported = IGCLOverclockDtoBool.ToBool(native.bSupported),
                Units = native.units,
                Type = native.type,
                Value = DataValueDto.FromNative(native.value)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Telemetry item struct.</returns>
        public ctl_oc_telemetry_item_t ToNative()
        {
            return new ctl_oc_telemetry_item_t
            {
                bSupported = IGCLOverclockDtoBool.ToByte(IsSupported),
                units = Units,
                type = Type,
                value = Value.ToNative()
            };
        }
    }

    /// <summary>
    /// DTO for PSU information.
    /// </summary>
    public struct PsuInfoDto : IEquatable<PsuInfoDto>
    {
        /// <summary>
        /// Indicates whether PSU info is supported.
        /// </summary>
        public bool IsSupported;
        /// <summary>
        /// PSU type.
        /// </summary>
        public ctl_psu_type_t PsuType;
        /// <summary>
        /// Energy counter telemetry item.
        /// </summary>
        public OcTelemetryItemDto EnergyCounter;
        /// <summary>
        /// Voltage telemetry item.
        /// </summary>
        public OcTelemetryItemDto Voltage;

        /// <summary>
        /// Compare PSU info.
        /// </summary>
        /// <param name="other">Other PSU info instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(PsuInfoDto other)
        {
            return IsSupported == other.IsSupported &&
                   PsuType == other.PsuType &&
                   EnergyCounter.Equals(other.EnergyCounter) &&
                   Voltage.Equals(other.Voltage);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is PsuInfoDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(IsSupported);
            hash.Add(PsuType);
            hash.Add(EnergyCounter);
            hash.Add(Voltage);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>PSU info DTO.</returns>
        public static PsuInfoDto FromNative(ctl_psu_info_t native)
        {
            return new PsuInfoDto
            {
                IsSupported = IGCLOverclockDtoBool.ToBool(native.bSupported),
                PsuType = native.psuType,
                EnergyCounter = OcTelemetryItemDto.FromNative(native.energyCounter),
                Voltage = OcTelemetryItemDto.FromNative(native.voltage)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>PSU info struct.</returns>
        public ctl_psu_info_t ToNative()
        {
            return new ctl_psu_info_t
            {
                bSupported = IGCLOverclockDtoBool.ToByte(IsSupported),
                psuType = PsuType,
                energyCounter = EnergyCounter.ToNative(),
                voltage = Voltage.ToNative()
            };
        }
    }

    /// <summary>
    /// DTO for power telemetry.
    /// </summary>
    public struct PowerTelemetryDto : IEquatable<PowerTelemetryDto>
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
        /// Timestamp telemetry item.
        /// </summary>
        public OcTelemetryItemDto TimeStamp;
        /// <summary>
        /// GPU energy counter telemetry item.
        /// </summary>
        public OcTelemetryItemDto GpuEnergyCounter;
        /// <summary>
        /// GPU voltage telemetry item.
        /// </summary>
        public OcTelemetryItemDto GpuVoltage;
        /// <summary>
        /// GPU current clock frequency telemetry item.
        /// </summary>
        public OcTelemetryItemDto GpuCurrentClockFrequency;
        /// <summary>
        /// GPU current temperature telemetry item.
        /// </summary>
        public OcTelemetryItemDto GpuCurrentTemperature;
        /// <summary>
        /// Global activity counter telemetry item.
        /// </summary>
        public OcTelemetryItemDto GlobalActivityCounter;
        /// <summary>
        /// Render/compute activity counter telemetry item.
        /// </summary>
        public OcTelemetryItemDto RenderComputeActivityCounter;
        /// <summary>
        /// Media activity counter telemetry item.
        /// </summary>
        public OcTelemetryItemDto MediaActivityCounter;
        /// <summary>
        /// Indicates GPU power limit is active.
        /// </summary>
        public bool GpuPowerLimited;
        /// <summary>
        /// Indicates GPU temperature limit is active.
        /// </summary>
        public bool GpuTemperatureLimited;
        /// <summary>
        /// Indicates GPU current limit is active.
        /// </summary>
        public bool GpuCurrentLimited;
        /// <summary>
        /// Indicates GPU voltage limit is active.
        /// </summary>
        public bool GpuVoltageLimited;
        /// <summary>
        /// Indicates GPU utilization limit is active.
        /// </summary>
        public bool GpuUtilizationLimited;
        /// <summary>
        /// VRAM energy counter telemetry item.
        /// </summary>
        public OcTelemetryItemDto VramEnergyCounter;
        /// <summary>
        /// VRAM voltage telemetry item.
        /// </summary>
        public OcTelemetryItemDto VramVoltage;
        /// <summary>
        /// VRAM current clock frequency telemetry item.
        /// </summary>
        public OcTelemetryItemDto VramCurrentClockFrequency;
        /// <summary>
        /// VRAM current effective frequency telemetry item.
        /// </summary>
        public OcTelemetryItemDto VramCurrentEffectiveFrequency;
        /// <summary>
        /// VRAM read bandwidth counter telemetry item.
        /// </summary>
        public OcTelemetryItemDto VramReadBandwidthCounter;
        /// <summary>
        /// VRAM write bandwidth counter telemetry item.
        /// </summary>
        public OcTelemetryItemDto VramWriteBandwidthCounter;
        /// <summary>
        /// VRAM current temperature telemetry item.
        /// </summary>
        public OcTelemetryItemDto VramCurrentTemperature;
        /// <summary>
        /// Indicates VRAM power limit is active.
        /// </summary>
        public bool VramPowerLimited;
        /// <summary>
        /// Indicates VRAM temperature limit is active.
        /// </summary>
        public bool VramTemperatureLimited;
        /// <summary>
        /// Indicates VRAM current limit is active.
        /// </summary>
        public bool VramCurrentLimited;
        /// <summary>
        /// Indicates VRAM voltage limit is active.
        /// </summary>
        public bool VramVoltageLimited;
        /// <summary>
        /// Indicates VRAM utilization limit is active.
        /// </summary>
        public bool VramUtilizationLimited;
        /// <summary>
        /// Total card energy counter telemetry item.
        /// </summary>
        public OcTelemetryItemDto TotalCardEnergyCounter;
        /// <summary>
        /// PSU telemetry items.
        /// </summary>
        public List<PsuInfoDto>? Psu;
        /// <summary>
        /// Fan speed telemetry items.
        /// </summary>
        public List<OcTelemetryItemDto>? FanSpeed;
        /// <summary>
        /// GPU VR temperature telemetry item.
        /// </summary>
        public OcTelemetryItemDto GpuVrTemp;
        /// <summary>
        /// VRAM VR temperature telemetry item.
        /// </summary>
        public OcTelemetryItemDto VramVrTemp;
        /// <summary>
        /// SA VR temperature telemetry item.
        /// </summary>
        public OcTelemetryItemDto SaVrTemp;
        /// <summary>
        /// GPU effective clock telemetry item.
        /// </summary>
        public OcTelemetryItemDto GpuEffectiveClock;
        /// <summary>
        /// GPU over-voltage percent telemetry item.
        /// </summary>
        public OcTelemetryItemDto GpuOverVoltagePercent;
        /// <summary>
        /// GPU power percent telemetry item.
        /// </summary>
        public OcTelemetryItemDto GpuPowerPercent;
        /// <summary>
        /// GPU temperature percent telemetry item.
        /// </summary>
        public OcTelemetryItemDto GpuTemperaturePercent;
        /// <summary>
        /// VRAM read bandwidth telemetry item.
        /// </summary>
        public OcTelemetryItemDto VramReadBandwidth;
        /// <summary>
        /// VRAM write bandwidth telemetry item.
        /// </summary>
        public OcTelemetryItemDto VramWriteBandwidth;

        /// <summary>
        /// Compare power telemetry values.
        /// </summary>
        /// <param name="other">Other telemetry instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(PowerTelemetryDto other)
        {
            return TimeStamp.Equals(other.TimeStamp) &&
                   GpuEnergyCounter.Equals(other.GpuEnergyCounter) &&
                   GpuVoltage.Equals(other.GpuVoltage) &&
                   GpuCurrentClockFrequency.Equals(other.GpuCurrentClockFrequency) &&
                   GpuCurrentTemperature.Equals(other.GpuCurrentTemperature) &&
                   GlobalActivityCounter.Equals(other.GlobalActivityCounter) &&
                   RenderComputeActivityCounter.Equals(other.RenderComputeActivityCounter) &&
                   MediaActivityCounter.Equals(other.MediaActivityCounter) &&
                   GpuPowerLimited == other.GpuPowerLimited &&
                   GpuTemperatureLimited == other.GpuTemperatureLimited &&
                   GpuCurrentLimited == other.GpuCurrentLimited &&
                   GpuVoltageLimited == other.GpuVoltageLimited &&
                   GpuUtilizationLimited == other.GpuUtilizationLimited &&
                   VramEnergyCounter.Equals(other.VramEnergyCounter) &&
                   VramVoltage.Equals(other.VramVoltage) &&
                   VramCurrentClockFrequency.Equals(other.VramCurrentClockFrequency) &&
                   VramCurrentEffectiveFrequency.Equals(other.VramCurrentEffectiveFrequency) &&
                   VramReadBandwidthCounter.Equals(other.VramReadBandwidthCounter) &&
                   VramWriteBandwidthCounter.Equals(other.VramWriteBandwidthCounter) &&
                   VramCurrentTemperature.Equals(other.VramCurrentTemperature) &&
                   VramPowerLimited == other.VramPowerLimited &&
                   VramTemperatureLimited == other.VramTemperatureLimited &&
                   VramCurrentLimited == other.VramCurrentLimited &&
                   VramVoltageLimited == other.VramVoltageLimited &&
                   VramUtilizationLimited == other.VramUtilizationLimited &&
                   TotalCardEnergyCounter.Equals(other.TotalCardEnergyCounter) &&
                   ArePsuEqual(Psu, other.Psu) &&
                   AreTelemetryEqual(FanSpeed, other.FanSpeed) &&
                   GpuVrTemp.Equals(other.GpuVrTemp) &&
                   VramVrTemp.Equals(other.VramVrTemp) &&
                   SaVrTemp.Equals(other.SaVrTemp) &&
                   GpuEffectiveClock.Equals(other.GpuEffectiveClock) &&
                   GpuOverVoltagePercent.Equals(other.GpuOverVoltagePercent) &&
                   GpuPowerPercent.Equals(other.GpuPowerPercent) &&
                   GpuTemperaturePercent.Equals(other.GpuTemperaturePercent) &&
                   VramReadBandwidth.Equals(other.VramReadBandwidth) &&
                   VramWriteBandwidth.Equals(other.VramWriteBandwidth);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is PowerTelemetryDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(TimeStamp);
            hash.Add(GpuEnergyCounter);
            hash.Add(GpuVoltage);
            hash.Add(GpuCurrentClockFrequency);
            hash.Add(GpuCurrentTemperature);
            hash.Add(GlobalActivityCounter);
            hash.Add(RenderComputeActivityCounter);
            hash.Add(MediaActivityCounter);
            hash.Add(GpuPowerLimited);
            hash.Add(GpuTemperatureLimited);
            hash.Add(GpuCurrentLimited);
            hash.Add(GpuVoltageLimited);
            hash.Add(GpuUtilizationLimited);
            hash.Add(VramEnergyCounter);
            hash.Add(VramVoltage);
            hash.Add(VramCurrentClockFrequency);
            hash.Add(VramCurrentEffectiveFrequency);
            hash.Add(VramReadBandwidthCounter);
            hash.Add(VramWriteBandwidthCounter);
            hash.Add(VramCurrentTemperature);
            hash.Add(VramPowerLimited);
            hash.Add(VramTemperatureLimited);
            hash.Add(VramCurrentLimited);
            hash.Add(VramVoltageLimited);
            hash.Add(VramUtilizationLimited);
            hash.Add(TotalCardEnergyCounter);
            if (Psu != null)
            {
                hash.Add(Psu.Count);
                for (var i = 0; i < Psu.Count; i++)
                    hash.Add(Psu[i]);
            }
            if (FanSpeed != null)
            {
                hash.Add(FanSpeed.Count);
                for (var i = 0; i < FanSpeed.Count; i++)
                    hash.Add(FanSpeed[i]);
            }
            hash.Add(GpuVrTemp);
            hash.Add(VramVrTemp);
            hash.Add(SaVrTemp);
            hash.Add(GpuEffectiveClock);
            hash.Add(GpuOverVoltagePercent);
            hash.Add(GpuPowerPercent);
            hash.Add(GpuTemperaturePercent);
            hash.Add(VramReadBandwidth);
            hash.Add(VramWriteBandwidth);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Power telemetry DTO.</returns>
        public static unsafe PowerTelemetryDto FromNative(ctl_power_telemetry_t native)
        {
            var psu = new List<PsuInfoDto>(5);
            var pPsu = (ctl_psu_info_t*)Unsafe.AsPointer(ref native.psu.e0);
            for (int i = 0; i < 5; i++)
                psu.Add(PsuInfoDto.FromNative(pPsu[i]));

            var fan = new List<OcTelemetryItemDto>(5);
            var pFan = (ctl_oc_telemetry_item_t*)Unsafe.AsPointer(ref native.fanSpeed.e0);
            for (int i = 0; i < 5; i++)
                fan.Add(OcTelemetryItemDto.FromNative(pFan[i]));

            return new PowerTelemetryDto
            {
                Size = native.Size,
                Version = native.Version,
                TimeStamp = OcTelemetryItemDto.FromNative(native.timeStamp),
                GpuEnergyCounter = OcTelemetryItemDto.FromNative(native.gpuEnergyCounter),
                GpuVoltage = OcTelemetryItemDto.FromNative(native.gpuVoltage),
                GpuCurrentClockFrequency = OcTelemetryItemDto.FromNative(native.gpuCurrentClockFrequency),
                GpuCurrentTemperature = OcTelemetryItemDto.FromNative(native.gpuCurrentTemperature),
                GlobalActivityCounter = OcTelemetryItemDto.FromNative(native.globalActivityCounter),
                RenderComputeActivityCounter = OcTelemetryItemDto.FromNative(native.renderComputeActivityCounter),
                MediaActivityCounter = OcTelemetryItemDto.FromNative(native.mediaActivityCounter),
                GpuPowerLimited = IGCLOverclockDtoBool.ToBool(native.gpuPowerLimited),
                GpuTemperatureLimited = IGCLOverclockDtoBool.ToBool(native.gpuTemperatureLimited),
                GpuCurrentLimited = IGCLOverclockDtoBool.ToBool(native.gpuCurrentLimited),
                GpuVoltageLimited = IGCLOverclockDtoBool.ToBool(native.gpuVoltageLimited),
                GpuUtilizationLimited = IGCLOverclockDtoBool.ToBool(native.gpuUtilizationLimited),
                VramEnergyCounter = OcTelemetryItemDto.FromNative(native.vramEnergyCounter),
                VramVoltage = OcTelemetryItemDto.FromNative(native.vramVoltage),
                VramCurrentClockFrequency = OcTelemetryItemDto.FromNative(native.vramCurrentClockFrequency),
                VramCurrentEffectiveFrequency = OcTelemetryItemDto.FromNative(native.vramCurrentEffectiveFrequency),
                VramReadBandwidthCounter = OcTelemetryItemDto.FromNative(native.vramReadBandwidthCounter),
                VramWriteBandwidthCounter = OcTelemetryItemDto.FromNative(native.vramWriteBandwidthCounter),
                VramCurrentTemperature = OcTelemetryItemDto.FromNative(native.vramCurrentTemperature),
                VramPowerLimited = IGCLOverclockDtoBool.ToBool(native.vramPowerLimited),
                VramTemperatureLimited = IGCLOverclockDtoBool.ToBool(native.vramTemperatureLimited),
                VramCurrentLimited = IGCLOverclockDtoBool.ToBool(native.vramCurrentLimited),
                VramVoltageLimited = IGCLOverclockDtoBool.ToBool(native.vramVoltageLimited),
                VramUtilizationLimited = IGCLOverclockDtoBool.ToBool(native.vramUtilizationLimited),
                TotalCardEnergyCounter = OcTelemetryItemDto.FromNative(native.totalCardEnergyCounter),
                Psu = psu,
                FanSpeed = fan,
                GpuVrTemp = OcTelemetryItemDto.FromNative(native.gpuVrTemp),
                VramVrTemp = OcTelemetryItemDto.FromNative(native.vramVrTemp),
                SaVrTemp = OcTelemetryItemDto.FromNative(native.saVrTemp),
                GpuEffectiveClock = OcTelemetryItemDto.FromNative(native.gpuEffectiveClock),
                GpuOverVoltagePercent = OcTelemetryItemDto.FromNative(native.gpuOverVoltagePercent),
                GpuPowerPercent = OcTelemetryItemDto.FromNative(native.gpuPowerPercent),
                GpuTemperaturePercent = OcTelemetryItemDto.FromNative(native.gpuTemperaturePercent),
                VramReadBandwidth = OcTelemetryItemDto.FromNative(native.vramReadBandwidth),
                VramWriteBandwidth = OcTelemetryItemDto.FromNative(native.vramWriteBandwidth)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Power telemetry struct.</returns>
        public unsafe ctl_power_telemetry_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_power_telemetry_t);

            var native = new ctl_power_telemetry_t
            {
                Size = size,
                Version = Version,
                timeStamp = TimeStamp.ToNative(),
                gpuEnergyCounter = GpuEnergyCounter.ToNative(),
                gpuVoltage = GpuVoltage.ToNative(),
                gpuCurrentClockFrequency = GpuCurrentClockFrequency.ToNative(),
                gpuCurrentTemperature = GpuCurrentTemperature.ToNative(),
                globalActivityCounter = GlobalActivityCounter.ToNative(),
                renderComputeActivityCounter = RenderComputeActivityCounter.ToNative(),
                mediaActivityCounter = MediaActivityCounter.ToNative(),
                gpuPowerLimited = IGCLOverclockDtoBool.ToByte(GpuPowerLimited),
                gpuTemperatureLimited = IGCLOverclockDtoBool.ToByte(GpuTemperatureLimited),
                gpuCurrentLimited = IGCLOverclockDtoBool.ToByte(GpuCurrentLimited),
                gpuVoltageLimited = IGCLOverclockDtoBool.ToByte(GpuVoltageLimited),
                gpuUtilizationLimited = IGCLOverclockDtoBool.ToByte(GpuUtilizationLimited),
                vramEnergyCounter = VramEnergyCounter.ToNative(),
                vramVoltage = VramVoltage.ToNative(),
                vramCurrentClockFrequency = VramCurrentClockFrequency.ToNative(),
                vramCurrentEffectiveFrequency = VramCurrentEffectiveFrequency.ToNative(),
                vramReadBandwidthCounter = VramReadBandwidthCounter.ToNative(),
                vramWriteBandwidthCounter = VramWriteBandwidthCounter.ToNative(),
                vramCurrentTemperature = VramCurrentTemperature.ToNative(),
                vramPowerLimited = IGCLOverclockDtoBool.ToByte(VramPowerLimited),
                vramTemperatureLimited = IGCLOverclockDtoBool.ToByte(VramTemperatureLimited),
                vramCurrentLimited = IGCLOverclockDtoBool.ToByte(VramCurrentLimited),
                vramVoltageLimited = IGCLOverclockDtoBool.ToByte(VramVoltageLimited),
                vramUtilizationLimited = IGCLOverclockDtoBool.ToByte(VramUtilizationLimited),
                totalCardEnergyCounter = TotalCardEnergyCounter.ToNative(),
                gpuVrTemp = GpuVrTemp.ToNative(),
                vramVrTemp = VramVrTemp.ToNative(),
                saVrTemp = SaVrTemp.ToNative(),
                gpuEffectiveClock = GpuEffectiveClock.ToNative(),
                gpuOverVoltagePercent = GpuOverVoltagePercent.ToNative(),
                gpuPowerPercent = GpuPowerPercent.ToNative(),
                gpuTemperaturePercent = GpuTemperaturePercent.ToNative(),
                vramReadBandwidth = VramReadBandwidth.ToNative(),
                vramWriteBandwidth = VramWriteBandwidth.ToNative()
            };

            var psu = Psu;
            var pPsu = (ctl_psu_info_t*)Unsafe.AsPointer(ref native.psu.e0);
            for (int i = 0; i < 5; i++)
            {
                pPsu[i] = psu != null && i < psu.Count ? psu[i].ToNative() : default;
            }

            var fan = FanSpeed;
            var pFan = (ctl_oc_telemetry_item_t*)Unsafe.AsPointer(ref native.fanSpeed.e0);
            for (int i = 0; i < 5; i++)
            {
                pFan[i] = fan != null && i < fan.Count ? fan[i].ToNative() : default;
            }

            return native;
        }

        private static bool ArePsuEqual(List<PsuInfoDto>? left, List<PsuInfoDto>? right)
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

        private static bool AreTelemetryEqual(List<OcTelemetryItemDto>? left, List<OcTelemetryItemDto>? right)
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
    /// DTO for GPU overclock voltage/frequency lock pair.
    /// </summary>
    public struct OcVfPairDto : IEquatable<OcVfPairDto>
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
        /// Lock voltage in mV.
        /// </summary>
        public double Voltage;
        /// <summary>
        /// Lock frequency in MHz.
        /// </summary>
        public double Frequency;

        public bool Equals(OcVfPairDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Voltage == other.Voltage &&
                   Frequency == other.Frequency;
        }

        public override bool Equals(object? obj) => obj is OcVfPairDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Voltage);
            hash.Add(Frequency);
            return hash.ToHashCode();
        }

        public static OcVfPairDto FromNative(ctl_oc_vf_pair_t native)
        {
            return new OcVfPairDto
            {
                Size = native.Size,
                Version = native.Version,
                Voltage = native.Voltage,
                Frequency = native.Frequency
            };
        }

        public unsafe ctl_oc_vf_pair_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_oc_vf_pair_t);
            return new ctl_oc_vf_pair_t
            {
                Size = size,
                Version = Version,
                Voltage = Voltage,
                Frequency = Frequency
            };
        }
    }
}

