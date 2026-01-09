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

        public unsafe ctl_oc_properties_t GetPropertiesNative()
        {
            ThrowIfDisposed();
            var props = CreateOverclockProperties();
            var result = IGCL.ctlOverclockGetProperties((_ctl_device_adapter_handle_t*)_adapter, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get overclock properties");
            return props;
        }

        public OverclockPropertiesDto GetProperties()
        {
            var native = GetPropertiesNative();
            return OverclockPropertiesDto.FromNative(native);
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

        public unsafe ctl_power_telemetry_t GetPowerTelemetryNative()
        {
            ThrowIfDisposed();
            var telemetry = CreatePowerTelemetry();
            var result = IGCL.ctlPowerTelemetryGet((_ctl_device_adapter_handle_t*)_adapter, &telemetry);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, OverclockError);
            return telemetry;
        }

        public PowerTelemetryDto GetPowerTelemetry()
        {
            var native = GetPowerTelemetryNative();
            return PowerTelemetryDto.FromNative(native);
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

    internal static class IGCLOverclockDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    public struct OcControlInfoDto
    {
        public bool IsSupported;
        public bool IsRelative;
        public bool IsReference;
        public ctl_units_t Units;
        public double Min;
        public double Max;
        public double Step;
        public double Default;
        public double Reference;

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

    public struct OverclockPropertiesDto
    {
        public uint Size;
        public byte Version;
        public bool IsSupported;
        public OcControlInfoDto GpuFrequencyOffset;
        public OcControlInfoDto GpuVoltageOffset;
        public OcControlInfoDto VramFrequencyOffset;
        public OcControlInfoDto VramVoltageOffset;
        public OcControlInfoDto PowerLimit;
        public OcControlInfoDto TemperatureLimit;
        public OcControlInfoDto VramMemSpeedLimit;
        public OcControlInfoDto GpuVfCurveVoltageLimit;
        public OcControlInfoDto GpuVfCurveFrequencyLimit;

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

        public ctl_oc_properties_t ToNative()
        {
            return new ctl_oc_properties_t
            {
                Size = Size,
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

    public struct OcTelemetryItemDto
    {
        public bool IsSupported;
        public ctl_units_t Units;
        public ctl_data_type_t Type;
        public ctl_data_value_t Value;

        public static OcTelemetryItemDto FromNative(ctl_oc_telemetry_item_t native)
        {
            return new OcTelemetryItemDto
            {
                IsSupported = IGCLOverclockDtoBool.ToBool(native.bSupported),
                Units = native.units,
                Type = native.type,
                Value = native.value
            };
        }

        public ctl_oc_telemetry_item_t ToNative()
        {
            return new ctl_oc_telemetry_item_t
            {
                bSupported = IGCLOverclockDtoBool.ToByte(IsSupported),
                units = Units,
                type = Type,
                value = Value
            };
        }
    }

    public struct PsuInfoDto
    {
        public bool IsSupported;
        public ctl_psu_type_t PsuType;
        public OcTelemetryItemDto EnergyCounter;
        public OcTelemetryItemDto Voltage;

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

    public struct PowerTelemetryDto
    {
        public uint Size;
        public byte Version;
        public OcTelemetryItemDto TimeStamp;
        public OcTelemetryItemDto GpuEnergyCounter;
        public OcTelemetryItemDto GpuVoltage;
        public OcTelemetryItemDto GpuCurrentClockFrequency;
        public OcTelemetryItemDto GpuCurrentTemperature;
        public OcTelemetryItemDto GlobalActivityCounter;
        public OcTelemetryItemDto RenderComputeActivityCounter;
        public OcTelemetryItemDto MediaActivityCounter;
        public bool GpuPowerLimited;
        public bool GpuTemperatureLimited;
        public bool GpuCurrentLimited;
        public bool GpuVoltageLimited;
        public bool GpuUtilizationLimited;
        public OcTelemetryItemDto VramEnergyCounter;
        public OcTelemetryItemDto VramVoltage;
        public OcTelemetryItemDto VramCurrentClockFrequency;
        public OcTelemetryItemDto VramCurrentEffectiveFrequency;
        public OcTelemetryItemDto VramReadBandwidthCounter;
        public OcTelemetryItemDto VramWriteBandwidthCounter;
        public OcTelemetryItemDto VramCurrentTemperature;
        public bool VramPowerLimited;
        public bool VramTemperatureLimited;
        public bool VramCurrentLimited;
        public bool VramVoltageLimited;
        public bool VramUtilizationLimited;
        public OcTelemetryItemDto TotalCardEnergyCounter;
        public PsuInfoDto[] Psu;
        public OcTelemetryItemDto[] FanSpeed;
        public OcTelemetryItemDto GpuVrTemp;
        public OcTelemetryItemDto VramVrTemp;
        public OcTelemetryItemDto SaVrTemp;
        public OcTelemetryItemDto GpuEffectiveClock;
        public OcTelemetryItemDto GpuOverVoltagePercent;
        public OcTelemetryItemDto GpuPowerPercent;
        public OcTelemetryItemDto GpuTemperaturePercent;
        public OcTelemetryItemDto VramReadBandwidth;
        public OcTelemetryItemDto VramWriteBandwidth;

        public static unsafe PowerTelemetryDto FromNative(ctl_power_telemetry_t native)
        {
            var psu = new PsuInfoDto[5];
            fixed (ctl_psu_info_t* pPsu = &native.psu.e0)
            {
                for (int i = 0; i < psu.Length; i++)
                    psu[i] = PsuInfoDto.FromNative(pPsu[i]);
            }

            var fan = new OcTelemetryItemDto[5];
            fixed (ctl_oc_telemetry_item_t* pFan = &native.fanSpeed.e0)
            {
                for (int i = 0; i < fan.Length; i++)
                    fan[i] = OcTelemetryItemDto.FromNative(pFan[i]);
            }

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

        public unsafe ctl_power_telemetry_t ToNative()
        {
            var native = new ctl_power_telemetry_t
            {
                Size = Size,
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

            var psu = Psu ?? Array.Empty<PsuInfoDto>();
            fixed (ctl_psu_info_t* pPsu = &native.psu.e0)
            {
                for (int i = 0; i < 5; i++)
                {
                    pPsu[i] = i < psu.Length ? psu[i].ToNative() : default;
                }
            }

            var fan = FanSpeed ?? Array.Empty<OcTelemetryItemDto>();
            fixed (ctl_oc_telemetry_item_t* pFan = &native.fanSpeed.e0)
            {
                for (int i = 0; i < 5; i++)
                {
                    pFan[i] = i < fan.Length ? fan[i].ToNative() : default;
                }
            }

            return native;
        }
    }
}
