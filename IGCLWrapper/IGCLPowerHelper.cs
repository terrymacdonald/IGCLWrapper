using System;
using System.Collections.Generic;

namespace IGCLWrapper
{
    /// <summary>
    /// Power helper: enumerate power domains, properties, energy counters, limits.
    /// </summary>
    public sealed class IGCLPowerHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLPowerHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe IReadOnlyList<IntPtr> EnumPowerDomains()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        public unsafe ctl_power_properties_t PowerGetPropertiesNative(IntPtr powerHandle)
        {
            ThrowIfDisposed();
            var props = CreatePowerProperties();
            var result = IGCL.ctlPowerGetProperties((_ctl_pwr_handle_t*)powerHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power properties");
            return props;
        }

        public PowerPropertiesDto PowerGetProperties(IntPtr powerHandle)
        {
            var native = PowerGetPropertiesNative(powerHandle);
            return PowerPropertiesDto.FromNative(native);
        }

        public unsafe ctl_power_energy_counter_t PowerGetEnergyCounter(IntPtr powerHandle)
        {
            ThrowIfDisposed();
            var counter = CreatePowerEnergyCounter();
            var result = IGCL.ctlPowerGetEnergyCounter((_ctl_pwr_handle_t*)powerHandle, &counter);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power energy counter");
            return counter;
        }

        public unsafe ctl_power_limits_t PowerGetLimitsNative(IntPtr powerHandle)
        {
            ThrowIfDisposed();
            var limits = CreatePowerLimits();
            var result = IGCL.ctlPowerGetLimits((_ctl_pwr_handle_t*)powerHandle, &limits);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power limits");
            return limits;
        }

        public PowerLimitsDto PowerGetLimits(IntPtr powerHandle)
        {
            var native = PowerGetLimitsNative(powerHandle);
            return PowerLimitsDto.FromNative(native);
        }

        public unsafe void PowerSetLimitsNative(IntPtr powerHandle, ctl_power_limits_t limits)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlPowerSetLimits((_ctl_pwr_handle_t*)powerHandle, &limits);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set power limits");
        }

        public void PowerSetLimits(IntPtr powerHandle, PowerLimitsDto limits)
        {
            PowerSetLimitsNative(powerHandle, limits.ToNative());
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumPowerDomains(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get power domain count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumPowerDomains(adapter, &count, (_ctl_pwr_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate power domains");
            }
            return handles;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLPowerHelper));
        }

        private static unsafe ctl_power_properties_t CreatePowerProperties() => new ctl_power_properties_t { Size = (uint)sizeof(ctl_power_properties_t), Version = 0 };
        private static unsafe ctl_power_energy_counter_t CreatePowerEnergyCounter() => new ctl_power_energy_counter_t { Size = (uint)sizeof(ctl_power_energy_counter_t), Version = 0 };
        private static unsafe ctl_power_limits_t CreatePowerLimits() => new ctl_power_limits_t { Size = (uint)sizeof(ctl_power_limits_t), Version = 0 };
        public static unsafe ctl_power_limits_t CreatePowerLimitsStruct() => CreatePowerLimits();

        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCLPowerDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    public struct PowerPropertiesDto
    {
        public uint Size;
        public byte Version;
        public bool CanControl;
        public int DefaultLimit;
        public int MinLimit;
        public int MaxLimit;

        public static PowerPropertiesDto FromNative(ctl_power_properties_t native)
        {
            return new PowerPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                CanControl = IGCLPowerDtoBool.ToBool(native.canControl),
                DefaultLimit = native.defaultLimit,
                MinLimit = native.minLimit,
                MaxLimit = native.maxLimit
            };
        }

        public ctl_power_properties_t ToNative()
        {
            return new ctl_power_properties_t
            {
                Size = Size,
                Version = Version,
                canControl = IGCLPowerDtoBool.ToByte(CanControl),
                defaultLimit = DefaultLimit,
                minLimit = MinLimit,
                maxLimit = MaxLimit
            };
        }
    }

    public struct PowerSustainedLimitDto
    {
        public bool Enabled;
        public int Power;
        public int Interval;

        public static PowerSustainedLimitDto FromNative(ctl_power_sustained_limit_t native)
        {
            return new PowerSustainedLimitDto
            {
                Enabled = IGCLPowerDtoBool.ToBool(native.enabled),
                Power = native.power,
                Interval = native.interval
            };
        }

        public ctl_power_sustained_limit_t ToNative()
        {
            return new ctl_power_sustained_limit_t
            {
                enabled = IGCLPowerDtoBool.ToByte(Enabled),
                power = Power,
                interval = Interval
            };
        }
    }

    public struct PowerBurstLimitDto
    {
        public bool Enabled;
        public int Power;

        public static PowerBurstLimitDto FromNative(ctl_power_burst_limit_t native)
        {
            return new PowerBurstLimitDto
            {
                Enabled = IGCLPowerDtoBool.ToBool(native.enabled),
                Power = native.power
            };
        }

        public ctl_power_burst_limit_t ToNative()
        {
            return new ctl_power_burst_limit_t
            {
                enabled = IGCLPowerDtoBool.ToByte(Enabled),
                power = Power
            };
        }
    }

    public struct PowerPeakLimitDto
    {
        public int PowerAc;
        public int PowerDc;

        public static PowerPeakLimitDto FromNative(ctl_power_peak_limit_t native)
        {
            return new PowerPeakLimitDto
            {
                PowerAc = native.powerAC,
                PowerDc = native.powerDC
            };
        }

        public ctl_power_peak_limit_t ToNative()
        {
            return new ctl_power_peak_limit_t
            {
                powerAC = PowerAc,
                powerDC = PowerDc
            };
        }
    }

    public struct PowerLimitsDto
    {
        public uint Size;
        public byte Version;
        public PowerSustainedLimitDto SustainedPowerLimit;
        public PowerBurstLimitDto BurstPowerLimit;
        public PowerPeakLimitDto PeakPowerLimits;

        public static PowerLimitsDto FromNative(ctl_power_limits_t native)
        {
            return new PowerLimitsDto
            {
                Size = native.Size,
                Version = native.Version,
                SustainedPowerLimit = PowerSustainedLimitDto.FromNative(native.sustainedPowerLimit),
                BurstPowerLimit = PowerBurstLimitDto.FromNative(native.burstPowerLimit),
                PeakPowerLimits = PowerPeakLimitDto.FromNative(native.peakPowerLimits)
            };
        }

        public unsafe ctl_power_limits_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_power_limits_t);

            return new ctl_power_limits_t
            {
                Size = size,
                Version = Version,
                sustainedPowerLimit = SustainedPowerLimit.ToNative(),
                burstPowerLimit = BurstPowerLimit.ToNative(),
                peakPowerLimits = PeakPowerLimits.ToNative()
            };
        }
    }
}
