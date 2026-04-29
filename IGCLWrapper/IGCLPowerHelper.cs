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

        /// <summary>
        /// Enumerate power domain handles for the adapter.
        /// </summary>
        /// <returns>Read-only list of power domain handles.</returns>
        public unsafe IReadOnlyList<IntPtr> EnumPowerDomains()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        /// <summary>
        /// Get power domain properties using the native struct.
        /// </summary>
        /// <param name="powerHandle">Power domain handle.</param>
        /// <returns>Power properties struct.</returns>
        public unsafe ctl_power_properties_t PowerGetPropertiesNative(IntPtr powerHandle)
        {
            ThrowIfDisposed();
            var props = CreatePowerProperties();
            var result = IGCL.ctlPowerGetProperties((_ctl_pwr_handle_t*)powerHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power properties");
            return props;
        }

        /// <summary>
        /// Get power domain properties as a DTO.
        /// </summary>
        /// <param name="powerHandle">Power domain handle.</param>
        /// <returns>Power properties DTO.</returns>
        public PowerPropertiesDto PowerGetProperties(IntPtr powerHandle)
        {
            var native = PowerGetPropertiesNative(powerHandle);
            return PowerPropertiesDto.FromNative(native);
        }

        /// <summary>
        /// Get the power energy counter using the native struct.
        /// </summary>
        /// <param name="powerHandle">Power domain handle.</param>
        /// <returns>Power energy counter struct.</returns>
        public unsafe ctl_power_energy_counter_t PowerGetEnergyCounterNative(IntPtr powerHandle)
        {
            ThrowIfDisposed();
            var counter = CreatePowerEnergyCounter();
            var result = IGCL.ctlPowerGetEnergyCounter((_ctl_pwr_handle_t*)powerHandle, &counter);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power energy counter");
            return counter;
        }

        /// <summary>
        /// Get the power energy counter as a DTO.
        /// </summary>
        /// <param name="powerHandle">Power domain handle.</param>
        /// <returns>Power energy counter DTO.</returns>
        public PowerEnergyCounterDto PowerGetEnergyCounter(IntPtr powerHandle)
        {
            var native = PowerGetEnergyCounterNative(powerHandle);
            return PowerEnergyCounterDto.FromNative(native);
        }

        /// <summary>
        /// Get power limits using the native struct.
        /// </summary>
        /// <param name="powerHandle">Power domain handle.</param>
        /// <returns>Power limits struct.</returns>
        public unsafe ctl_power_limits_t PowerGetLimitsNative(IntPtr powerHandle)
        {
            ThrowIfDisposed();
            var limits = CreatePowerLimits();
            var result = IGCL.ctlPowerGetLimits((_ctl_pwr_handle_t*)powerHandle, &limits);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power limits");
            return limits;
        }

        /// <summary>
        /// Get power limits as a DTO.
        /// </summary>
        /// <param name="powerHandle">Power domain handle.</param>
        /// <returns>Power limits DTO.</returns>
        public PowerLimitsDto PowerGetLimits(IntPtr powerHandle)
        {
            var native = PowerGetLimitsNative(powerHandle);
            return PowerLimitsDto.FromNative(native);
        }

        /// <summary>
        /// Set power limits using the native struct.
        /// </summary>
        /// <param name="powerHandle">Power domain handle.</param>
        /// <param name="limits">Power limits struct.</param>
        public unsafe void PowerSetLimitsNative(IntPtr powerHandle, ctl_power_limits_t limits)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlPowerSetLimits((_ctl_pwr_handle_t*)powerHandle, &limits);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set power limits");
        }

        /// <summary>
        /// Set power limits using a DTO.
        /// </summary>
        /// <param name="powerHandle">Power domain handle.</param>
        /// <param name="limits">Power limits DTO.</param>
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
        /// <summary>
        /// Create a power limits struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized power limits struct.</returns>
        public static unsafe ctl_power_limits_t CreatePowerLimitsStruct() => CreatePowerLimits();

        /// <summary>
        /// Compare power properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool ArePowerPropertiesEqual(ctl_power_properties_t left, ctl_power_properties_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.canControl == right.canControl &&
                   left.defaultLimit == right.defaultLimit &&
                   left.minLimit == right.minLimit &&
                   left.maxLimit == right.maxLimit;
        }

        /// <summary>
        /// Compare power energy counters while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left counter struct.</param>
        /// <param name="right">Right counter struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool ArePowerEnergyCounterEqual(ctl_power_energy_counter_t left, ctl_power_energy_counter_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.energy == right.energy &&
                   left.timestamp == right.timestamp;
        }

        /// <summary>
        /// Compare power limits while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left limits struct.</param>
        /// <param name="right">Right limits struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool ArePowerLimitsEqual(ctl_power_limits_t left, ctl_power_limits_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   ArePowerSustainedLimitEqual(left.sustainedPowerLimit, right.sustainedPowerLimit) &&
                   ArePowerBurstLimitEqual(left.burstPowerLimit, right.burstPowerLimit) &&
                   ArePowerPeakLimitEqual(left.peakPowerLimits, right.peakPowerLimits);
        }

        private static bool ArePowerSustainedLimitEqual(ctl_power_sustained_limit_t left, ctl_power_sustained_limit_t right)
        {
            return left.enabled == right.enabled &&
                   left.power == right.power &&
                   left.interval == right.interval;
        }

        private static bool ArePowerBurstLimitEqual(ctl_power_burst_limit_t left, ctl_power_burst_limit_t right)
        {
            return left.enabled == right.enabled &&
                   left.power == right.power;
        }

        private static bool ArePowerPeakLimitEqual(ctl_power_peak_limit_t left, ctl_power_peak_limit_t right)
        {
            return left.powerAC == right.powerAC &&
                   left.powerDC == right.powerDC;
        }

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
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

    /// <summary>
    /// DTO for power energy counter.
    /// </summary>
    public struct PowerEnergyCounterDto : IEquatable<PowerEnergyCounterDto>
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
        /// Energy value.
        /// </summary>
        public ulong Energy;
        /// <summary>
        /// Timestamp.
        /// </summary>
        public ulong Timestamp;

        /// <summary>
        /// Compare power energy counters.
        /// </summary>
        /// <param name="other">Other counter instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(PowerEnergyCounterDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Energy == other.Energy &&
                   Timestamp == other.Timestamp;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is PowerEnergyCounterDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Energy);
            hash.Add(Timestamp);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Power energy counter DTO.</returns>
        public static PowerEnergyCounterDto FromNative(ctl_power_energy_counter_t native)
        {
            return new PowerEnergyCounterDto
            {
                Size = native.Size,
                Version = native.Version,
                Energy = native.energy,
                Timestamp = native.timestamp
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Power energy counter struct.</returns>
        public unsafe ctl_power_energy_counter_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_power_energy_counter_t);

            return new ctl_power_energy_counter_t
            {
                Size = size,
                Version = Version,
                energy = Energy,
                timestamp = Timestamp
            };
        }
    }

    /// <summary>
    /// DTO for power properties.
    /// </summary>
    public struct PowerPropertiesDto : IEquatable<PowerPropertiesDto>
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
        /// Indicates whether power limits can be controlled.
        /// </summary>
        public bool CanControl;
        /// <summary>
        /// Default power limit.
        /// </summary>
        public int DefaultLimit;
        /// <summary>
        /// Minimum power limit.
        /// </summary>
        public int MinLimit;
        /// <summary>
        /// Maximum power limit.
        /// </summary>
        public int MaxLimit;

        /// <summary>
        /// Compare power properties.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(PowerPropertiesDto other)
        {
            return CanControl == other.CanControl &&
                   DefaultLimit == other.DefaultLimit &&
                   MinLimit == other.MinLimit &&
                   MaxLimit == other.MaxLimit;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is PowerPropertiesDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(CanControl);
            hash.Add(DefaultLimit);
            hash.Add(MinLimit);
            hash.Add(MaxLimit);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Power properties DTO.</returns>
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

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Power properties struct.</returns>
        public unsafe ctl_power_properties_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_power_properties_t);

            return new ctl_power_properties_t
            {
                Size = size,
                Version = Version,
                canControl = IGCLPowerDtoBool.ToByte(CanControl),
                defaultLimit = DefaultLimit,
                minLimit = MinLimit,
                maxLimit = MaxLimit
            };
        }
    }

    /// <summary>
    /// DTO for sustained power limit settings.
    /// </summary>
    public struct PowerSustainedLimitDto : IEquatable<PowerSustainedLimitDto>
    {
        /// <summary>
        /// Enable flag.
        /// </summary>
        public bool Enabled;
        /// <summary>
        /// Power value.
        /// </summary>
        public int Power;
        /// <summary>
        /// Time interval.
        /// </summary>
        public int Interval;

        /// <summary>
        /// Compare sustained power limit settings.
        /// </summary>
        /// <param name="other">Other settings instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(PowerSustainedLimitDto other)
        {
            return Enabled == other.Enabled &&
                   Power == other.Power &&
                   Interval == other.Interval;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is PowerSustainedLimitDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Enabled);
            hash.Add(Power);
            hash.Add(Interval);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Sustained power limit DTO.</returns>
        public static PowerSustainedLimitDto FromNative(ctl_power_sustained_limit_t native)
        {
            return new PowerSustainedLimitDto
            {
                Enabled = IGCLPowerDtoBool.ToBool(native.enabled),
                Power = native.power,
                Interval = native.interval
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Sustained power limit struct.</returns>
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

    /// <summary>
    /// DTO for burst power limit settings.
    /// </summary>
    public struct PowerBurstLimitDto : IEquatable<PowerBurstLimitDto>
    {
        /// <summary>
        /// Enable flag.
        /// </summary>
        public bool Enabled;
        /// <summary>
        /// Power value.
        /// </summary>
        public int Power;

        /// <summary>
        /// Compare burst power limit settings.
        /// </summary>
        /// <param name="other">Other settings instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(PowerBurstLimitDto other)
        {
            return Enabled == other.Enabled &&
                   Power == other.Power;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is PowerBurstLimitDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Enabled);
            hash.Add(Power);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Burst power limit DTO.</returns>
        public static PowerBurstLimitDto FromNative(ctl_power_burst_limit_t native)
        {
            return new PowerBurstLimitDto
            {
                Enabled = IGCLPowerDtoBool.ToBool(native.enabled),
                Power = native.power
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Burst power limit struct.</returns>
        public ctl_power_burst_limit_t ToNative()
        {
            return new ctl_power_burst_limit_t
            {
                enabled = IGCLPowerDtoBool.ToByte(Enabled),
                power = Power
            };
        }
    }

    /// <summary>
    /// DTO for peak power limit settings.
    /// </summary>
    public struct PowerPeakLimitDto : IEquatable<PowerPeakLimitDto>
    {
        /// <summary>
        /// AC power value.
        /// </summary>
        public int PowerAc;
        /// <summary>
        /// DC power value.
        /// </summary>
        public int PowerDc;

        /// <summary>
        /// Compare peak power limit settings.
        /// </summary>
        /// <param name="other">Other settings instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(PowerPeakLimitDto other)
        {
            return PowerAc == other.PowerAc &&
                   PowerDc == other.PowerDc;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is PowerPeakLimitDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(PowerAc);
            hash.Add(PowerDc);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Peak power limit DTO.</returns>
        public static PowerPeakLimitDto FromNative(ctl_power_peak_limit_t native)
        {
            return new PowerPeakLimitDto
            {
                PowerAc = native.powerAC,
                PowerDc = native.powerDC
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Peak power limit struct.</returns>
        public ctl_power_peak_limit_t ToNative()
        {
            return new ctl_power_peak_limit_t
            {
                powerAC = PowerAc,
                powerDC = PowerDc
            };
        }
    }

    /// <summary>
    /// DTO for power limit settings.
    /// </summary>
    public struct PowerLimitsDto : IEquatable<PowerLimitsDto>
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
        /// Sustained power limit settings.
        /// </summary>
        public PowerSustainedLimitDto SustainedPowerLimit;
        /// <summary>
        /// Burst power limit settings.
        /// </summary>
        public PowerBurstLimitDto BurstPowerLimit;
        /// <summary>
        /// Peak power limit settings.
        /// </summary>
        public PowerPeakLimitDto PeakPowerLimits;

        /// <summary>
        /// Compare power limit settings.
        /// </summary>
        /// <param name="other">Other settings instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(PowerLimitsDto other)
        {
            return SustainedPowerLimit.Equals(other.SustainedPowerLimit) &&
                   BurstPowerLimit.Equals(other.BurstPowerLimit) &&
                   PeakPowerLimits.Equals(other.PeakPowerLimits);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is PowerLimitsDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(SustainedPowerLimit);
            hash.Add(BurstPowerLimit);
            hash.Add(PeakPowerLimits);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Power limits DTO.</returns>
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

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Power limits struct.</returns>
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

