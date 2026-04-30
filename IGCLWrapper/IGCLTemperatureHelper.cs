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

        /// <summary>
        /// Enumerate temperature sensor handles for the adapter.
        /// </summary>
        /// <returns>Read-only list of temperature sensor handles.</returns>
        public unsafe IReadOnlyList<IntPtr> EnumTemperatureSensors()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        /// <summary>
        /// Get temperature sensor properties using the native struct.
        /// </summary>
        /// <param name="sensorHandle">Temperature sensor handle.</param>
        /// <returns>Temperature properties struct.</returns>
        public unsafe ctl_temp_properties_t TemperatureGetPropertiesNative(IntPtr sensorHandle)
        {
            ThrowIfDisposed();
            var props = CreateTemperatureProperties();
            var result = IGCL.ctlTemperatureGetProperties((_ctl_temp_handle_t*)sensorHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get temperature properties");
            return props;
        }

        /// <summary>
        /// Get temperature sensor properties as a DTO.
        /// </summary>
        /// <param name="sensorHandle">Temperature sensor handle.</param>
        /// <returns>Temperature properties DTO.</returns>
        public TemperaturePropertiesDto TemperatureGetProperties(IntPtr sensorHandle)
        {
            var native = TemperatureGetPropertiesNative(sensorHandle);
            return TemperaturePropertiesDto.FromNative(native);
        }

        /// <summary>
        /// Get current temperature for a sensor.
        /// </summary>
        /// <param name="sensorHandle">Temperature sensor handle.</param>
        /// <returns>Temperature value in degrees C.</returns>
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

        /// <summary>
        /// Compare temperature properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreTemperaturePropertiesEqual(ctl_temp_properties_t left, ctl_temp_properties_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.type == right.type &&
                   left.maxTemperature.Equals(right.maxTemperature);
        }

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }

    public struct TemperaturePropertiesDto : IEquatable<TemperaturePropertiesDto>
    {
        public uint Size;
        public byte Version;
        public ctl_temp_sensors_t Type;
        public double MaxTemperature;

        public bool Equals(TemperaturePropertiesDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Type == other.Type &&
                   MaxTemperature.Equals(other.MaxTemperature);
        }

        public override bool Equals(object? obj) => obj is TemperaturePropertiesDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Type);
            hash.Add(MaxTemperature);
            return hash.ToHashCode();
        }

        public static TemperaturePropertiesDto FromNative(ctl_temp_properties_t native)
        {
            return new TemperaturePropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                Type = native.type,
                MaxTemperature = native.maxTemperature
            };
        }

        public unsafe ctl_temp_properties_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_temp_properties_t);
            return new ctl_temp_properties_t
            {
                Size = size,
                Version = Version,
                type = Type,
                maxTemperature = MaxTemperature
            };
        }
    }
}

