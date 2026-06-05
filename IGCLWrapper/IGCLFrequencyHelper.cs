using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    /// <summary>
    /// Frequency helper: enumerate domains, query properties/state, and set ranges.
    /// </summary>
    public sealed class IGCLFrequencyHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLFrequencyHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        /// <summary>
        /// Enumerate frequency domain handles for the adapter.
        /// </summary>
        /// <returns>Read-only list of frequency domain handles.</returns>
        public unsafe IReadOnlyList<IntPtr> EnumFrequencyDomains()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        /// <summary>
        /// Get frequency domain properties as a DTO.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <returns>Frequency properties DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe FrequencyPropertiesDto? FrequencyGetProperties(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            var props = CreateFrequencyProperties();
            var result = IGCL.ctlFrequencyGetProperties((_ctl_freq_handle_t*)freqHandle, &props);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return FrequencyPropertiesDto.FromNative(props);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, "Failed to get frequency properties");
        }

        /// <summary>
        /// Get available clocks for a frequency domain.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <returns>Array of available clock values, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe double[]? FrequencyGetAvailableClocks(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            uint count = 0;
            var result = IGCL.ctlFrequencyGetAvailableClocks((_ctl_freq_handle_t*)freqHandle, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
            {
                if (IsUnsupportedResult(result))
                    return null;
                throw new IGCLException(result, "Failed to get available clocks count");
            }
            if (count == 0)
                return Array.Empty<double>();
            var freqs = new double[count];
            fixed (double* pFreqs = freqs)
            {
                result = IGCL.ctlFrequencyGetAvailableClocks((_ctl_freq_handle_t*)freqHandle, &count, pFreqs);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    if (IsUnsupportedResult(result))
                        return null;
                    throw new IGCLException(result, "Failed to get available clocks");
                }
            }
            return freqs;
        }

        /// <summary>
        /// Get the frequency range for a domain as a DTO.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <returns>Frequency range DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe FrequencyRangeDto? FrequencyGetRange(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            var range = CreateFrequencyRange();
            var result = IGCL.ctlFrequencyGetRange((_ctl_freq_handle_t*)freqHandle, &range);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return FrequencyRangeDto.FromNative(range);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, "Failed to get frequency range");
        }

        /// <summary>
        /// Set the frequency range for a domain using a DTO.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <param name="range">Frequency range DTO.</param>
        /// <returns><c>true</c> if the setting was applied successfully; <c>false</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe bool FrequencySetRange(IntPtr freqHandle, FrequencyRangeDto range)
        {
            ThrowIfDisposed();
            var native = range.ToNative();
            var result = IGCL.ctlFrequencySetRange((_ctl_freq_handle_t*)freqHandle, &native);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return true;
            if (IsUnsupportedResult(result))
                return false;
            throw new IGCLException(result, "Failed to set frequency range");
        }

        /// <summary>
        /// Get the current frequency state as a DTO.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <returns>Frequency state DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe FrequencyStateDto? FrequencyGetState(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            var state = CreateFrequencyState();
            var result = IGCL.ctlFrequencyGetState((_ctl_freq_handle_t*)freqHandle, &state);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return FrequencyStateDto.FromNative(state);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, "Failed to get frequency state");
        }

        /// <summary>
        /// Get the throttle time for a frequency domain as a DTO.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <returns>Throttle time DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe FrequencyThrottleTimeDto? FrequencyGetThrottleTime(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            var tt = CreateFrequencyThrottleTime();
            var result = IGCL.ctlFrequencyGetThrottleTime((_ctl_freq_handle_t*)freqHandle, &tt);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return FrequencyThrottleTimeDto.FromNative(tt);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, "Failed to get throttle time");
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumFrequencyDomains(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get frequency domain count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumFrequencyDomains(adapter, &count, (_ctl_freq_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate frequency domains");
            }
            return handles;
        }

        /// <summary>
        /// Returns true when the result code indicates a feature is not available
        /// on the current hardware or driver, rather than a genuine API failure.
        /// </summary>
        private static bool IsUnsupportedResult(ctl_result_t result)
        {
            return result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                || result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION
                || result == ctl_result_t.CTL_RESULT_ERROR_INVALID_OPERATION_TYPE
                || result == ctl_result_t.CTL_RESULT_ERROR_INVALID_ARGUMENT;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLFrequencyHelper));
        }

        private static unsafe ctl_freq_properties_t CreateFrequencyProperties() => new ctl_freq_properties_t { Size = (uint)sizeof(ctl_freq_properties_t), Version = 0 };
        private static unsafe ctl_freq_range_t CreateFrequencyRange() => new ctl_freq_range_t { Size = (uint)sizeof(ctl_freq_range_t), Version = 0 };
        private static unsafe ctl_freq_state_t CreateFrequencyState() => new ctl_freq_state_t { Size = (uint)sizeof(ctl_freq_state_t), Version = 0 };
        private static unsafe ctl_freq_throttle_time_t CreateFrequencyThrottleTime() => new ctl_freq_throttle_time_t { Size = (uint)sizeof(ctl_freq_throttle_time_t), Version = 0 };
        /// <summary>
        /// Create a frequency range struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized frequency range struct.</returns>
        public static unsafe ctl_freq_range_t CreateFrequencyRangeStruct() => CreateFrequencyRange();

        /// <summary>
        /// Compare frequency properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreFrequencyPropertiesEqual(ctl_freq_properties_t left, ctl_freq_properties_t right)
        {
            return FrequencyPropertiesDto.FromNative(left).Equals(FrequencyPropertiesDto.FromNative(right));
        }

        /// <summary>
        /// Compare frequency ranges while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left range struct.</param>
        /// <param name="right">Right range struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreFrequencyRangeEqual(ctl_freq_range_t left, ctl_freq_range_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.min.Equals(right.min) &&
                   left.max.Equals(right.max);
        }

        /// <summary>
        /// Compare frequency state while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left state struct.</param>
        /// <param name="right">Right state struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreFrequencyStateEqual(ctl_freq_state_t left, ctl_freq_state_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.currentVoltage.Equals(right.currentVoltage) &&
                   left.request.Equals(right.request) &&
                   left.tdp.Equals(right.tdp) &&
                   left.efficient.Equals(right.efficient) &&
                   left.actual.Equals(right.actual) &&
                   left.throttleReasons == right.throttleReasons;
        }

        /// <summary>
        /// Compare frequency throttle times while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left throttle time struct.</param>
        /// <param name="right">Right throttle time struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreFrequencyThrottleTimeEqual(ctl_freq_throttle_time_t left, ctl_freq_throttle_time_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.throttleTime == right.throttleTime &&
                   left.timestamp == right.timestamp;
        }

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCLFrequencyDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    /// <summary>
    /// DTO for frequency range.
    /// </summary>
    public struct FrequencyRangeDto : IEquatable<FrequencyRangeDto>
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
        /// Minimum frequency.
        /// </summary>
        public double Min;
        /// <summary>
        /// Maximum frequency.
        /// </summary>
        public double Max;

        /// <summary>
        /// Compare frequency ranges.
        /// </summary>
        /// <param name="other">Other range instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(FrequencyRangeDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Min.Equals(other.Min) &&
                   Max.Equals(other.Max);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is FrequencyRangeDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Min);
            hash.Add(Max);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Frequency range DTO.</returns>
        public static FrequencyRangeDto FromNative(ctl_freq_range_t native)
        {
            return new FrequencyRangeDto
            {
                Size = native.Size,
                Version = native.Version,
                Min = native.min,
                Max = native.max
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Frequency range struct.</returns>
        public unsafe ctl_freq_range_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_freq_range_t);

            return new ctl_freq_range_t
            {
                Size = size,
                Version = Version,
                min = Min,
                max = Max
            };
        }
    }

    /// <summary>
    /// DTO for frequency state.
    /// </summary>
    public struct FrequencyStateDto : IEquatable<FrequencyStateDto>
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
        /// Current voltage.
        /// </summary>
        public double CurrentVoltage;
        /// <summary>
        /// Requested frequency.
        /// </summary>
        public double Request;
        /// <summary>
        /// TDP frequency.
        /// </summary>
        public double Tdp;
        /// <summary>
        /// Efficient frequency.
        /// </summary>
        public double Efficient;
        /// <summary>
        /// Actual frequency.
        /// </summary>
        public double Actual;
        /// <summary>
        /// Throttle reasons bitmask.
        /// </summary>
        public uint ThrottleReasons;

        /// <summary>
        /// Compare frequency states.
        /// </summary>
        /// <param name="other">Other state instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(FrequencyStateDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   CurrentVoltage.Equals(other.CurrentVoltage) &&
                   Request.Equals(other.Request) &&
                   Tdp.Equals(other.Tdp) &&
                   Efficient.Equals(other.Efficient) &&
                   Actual.Equals(other.Actual) &&
                   ThrottleReasons == other.ThrottleReasons;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is FrequencyStateDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(CurrentVoltage);
            hash.Add(Request);
            hash.Add(Tdp);
            hash.Add(Efficient);
            hash.Add(Actual);
            hash.Add(ThrottleReasons);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Frequency state DTO.</returns>
        public static FrequencyStateDto FromNative(ctl_freq_state_t native)
        {
            return new FrequencyStateDto
            {
                Size = native.Size,
                Version = native.Version,
                CurrentVoltage = native.currentVoltage,
                Request = native.request,
                Tdp = native.tdp,
                Efficient = native.efficient,
                Actual = native.actual,
                ThrottleReasons = native.throttleReasons
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Frequency state struct.</returns>
        public unsafe ctl_freq_state_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_freq_state_t);

            return new ctl_freq_state_t
            {
                Size = size,
                Version = Version,
                currentVoltage = CurrentVoltage,
                request = Request,
                tdp = Tdp,
                efficient = Efficient,
                actual = Actual,
                throttleReasons = ThrottleReasons
            };
        }
    }

    /// <summary>
    /// DTO for frequency throttle time.
    /// </summary>
    public struct FrequencyThrottleTimeDto : IEquatable<FrequencyThrottleTimeDto>
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
        /// Throttle time value.
        /// </summary>
        public ulong ThrottleTime;
        /// <summary>
        /// Timestamp.
        /// </summary>
        public ulong Timestamp;

        /// <summary>
        /// Compare frequency throttle times.
        /// </summary>
        /// <param name="other">Other throttle time instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(FrequencyThrottleTimeDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   ThrottleTime == other.ThrottleTime &&
                   Timestamp == other.Timestamp;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is FrequencyThrottleTimeDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(ThrottleTime);
            hash.Add(Timestamp);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Frequency throttle time DTO.</returns>
        public static FrequencyThrottleTimeDto FromNative(ctl_freq_throttle_time_t native)
        {
            return new FrequencyThrottleTimeDto
            {
                Size = native.Size,
                Version = native.Version,
                ThrottleTime = native.throttleTime,
                Timestamp = native.timestamp
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Frequency throttle time struct.</returns>
        public unsafe ctl_freq_throttle_time_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_freq_throttle_time_t);

            return new ctl_freq_throttle_time_t
            {
                Size = size,
                Version = Version,
                throttleTime = ThrottleTime,
                timestamp = Timestamp
            };
        }
    }

    /// <summary>
    /// DTO for frequency domain properties.
    /// </summary>
    public struct FrequencyPropertiesDto : IEquatable<FrequencyPropertiesDto>
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
        /// Frequency domain type.
        /// </summary>
        public ctl_freq_domain_t Type;
        /// <summary>
        /// Indicates whether the domain can be controlled.
        /// </summary>
        public bool CanControl;
        /// <summary>
        /// Minimum frequency.
        /// </summary>
        public double Min;
        /// <summary>
        /// Maximum frequency.
        /// </summary>
        public double Max;

        /// <summary>
        /// Compare frequency properties.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(FrequencyPropertiesDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Type == other.Type &&
                   CanControl == other.CanControl &&
                   Min.Equals(other.Min) &&
                   Max.Equals(other.Max);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is FrequencyPropertiesDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Type);
            hash.Add(CanControl);
            hash.Add(Min);
            hash.Add(Max);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Frequency properties DTO.</returns>
        public static FrequencyPropertiesDto FromNative(ctl_freq_properties_t native)
        {
            return new FrequencyPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                Type = native.type,
                CanControl = IGCLFrequencyDtoBool.ToBool(native.canControl),
                Min = native.min,
                Max = native.max
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Frequency properties struct.</returns>
        public ctl_freq_properties_t ToNative()
        {
            return new ctl_freq_properties_t
            {
                Size = Size,
                Version = Version,
                type = Type,
                canControl = IGCLFrequencyDtoBool.ToByte(CanControl),
                min = Min,
                max = Max
            };
        }
    }
}

