using System;
using System.Collections.Generic;

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
        /// Get frequency domain properties using the native struct.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <returns>Frequency properties struct.</returns>
        public unsafe ctl_freq_properties_t FrequencyGetPropertiesNative(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            var props = CreateFrequencyProperties();
            var result = IGCL.ctlFrequencyGetProperties((_ctl_freq_handle_t*)freqHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get frequency properties");
            return props;
        }

        /// <summary>
        /// Get frequency domain properties as a DTO.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <returns>Frequency properties DTO.</returns>
        public FrequencyPropertiesDto FrequencyGetProperties(IntPtr freqHandle)
        {
            var native = FrequencyGetPropertiesNative(freqHandle);
            return FrequencyPropertiesDto.FromNative(native);
        }

        /// <summary>
        /// Get available clocks for a frequency domain.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <returns>Array of available clock values.</returns>
        public unsafe double[] FrequencyGetAvailableClocks(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            uint count = 0;
            var result = IGCL.ctlFrequencyGetAvailableClocks((_ctl_freq_handle_t*)freqHandle, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get available clocks count");
            if (count == 0)
                return Array.Empty<double>();
            var freqs = new double[count];
            fixed (double* pFreqs = freqs)
            {
                result = IGCL.ctlFrequencyGetAvailableClocks((_ctl_freq_handle_t*)freqHandle, &count, pFreqs);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get available clocks");
            }
            return freqs;
        }

        /// <summary>
        /// Get the frequency range for a domain.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <returns>Frequency range struct.</returns>
        public unsafe ctl_freq_range_t FrequencyGetRange(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            var range = CreateFrequencyRange();
            var result = IGCL.ctlFrequencyGetRange((_ctl_freq_handle_t*)freqHandle, &range);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get frequency range");
            return range;
        }

        /// <summary>
        /// Set the frequency range for a domain.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <param name="range">Frequency range struct.</param>
        public unsafe void FrequencySetRange(IntPtr freqHandle, ctl_freq_range_t range)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlFrequencySetRange((_ctl_freq_handle_t*)freqHandle, &range);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set frequency range");
        }

        /// <summary>
        /// Get the current frequency state.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <returns>Frequency state struct.</returns>
        public unsafe ctl_freq_state_t FrequencyGetState(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            var state = CreateFrequencyState();
            var result = IGCL.ctlFrequencyGetState((_ctl_freq_handle_t*)freqHandle, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get frequency state");
            return state;
        }

        /// <summary>
        /// Get the throttle time for a frequency domain.
        /// </summary>
        /// <param name="freqHandle">Frequency domain handle.</param>
        /// <returns>Throttle time struct.</returns>
        public unsafe ctl_freq_throttle_time_t FrequencyGetThrottleTime(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            var tt = CreateFrequencyThrottleTime();
            var result = IGCL.ctlFrequencyGetThrottleTime((_ctl_freq_handle_t*)freqHandle, &tt);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get throttle time");
            return tt;
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
