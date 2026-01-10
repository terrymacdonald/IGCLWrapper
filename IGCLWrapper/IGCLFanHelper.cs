using System;
using System.Collections.Generic;

namespace IGCLWrapper
{
    /// <summary>
    /// Fan helper: enumerate fans, query properties, and set modes.
    /// </summary>
    public sealed class IGCLFanHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLFanHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        /// <summary>
        /// Enumerate fan handles for the adapter.
        /// </summary>
        /// <returns>Read-only list of fan handles.</returns>
        public unsafe IReadOnlyList<IntPtr> EnumFans()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        /// <summary>
        /// Get fan properties using the native struct.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        /// <returns>Fan properties struct.</returns>
        public unsafe ctl_fan_properties_t FanGetPropertiesNative(IntPtr fanHandle)
        {
            ThrowIfDisposed();
            var props = CreateFanProperties();
            var result = IGCL.ctlFanGetProperties((_ctl_fan_handle_t*)fanHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get fan properties");
            return props;
        }

        /// <summary>
        /// Get fan properties as a DTO.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        /// <returns>Fan properties DTO.</returns>
        public FanPropertiesDto FanGetProperties(IntPtr fanHandle)
        {
            var native = FanGetPropertiesNative(fanHandle);
            return FanPropertiesDto.FromNative(native);
        }

        /// <summary>
        /// Get fan configuration.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        /// <returns>Fan config struct.</returns>
        public unsafe ctl_fan_config_t FanGetConfig(IntPtr fanHandle)
        {
            ThrowIfDisposed();
            var config = CreateFanConfig();
            var result = IGCL.ctlFanGetConfig((_ctl_fan_handle_t*)fanHandle, &config);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get fan config");
            return config;
        }

        /// <summary>
        /// Set the fan to default control mode.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        public unsafe void FanSetDefaultMode(IntPtr fanHandle)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlFanSetDefaultMode((_ctl_fan_handle_t*)fanHandle);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set fan default mode");
        }

        /// <summary>
        /// Set the fan to fixed speed mode.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        /// <param name="speed">Fan speed settings.</param>
        public unsafe void FanSetFixedSpeedMode(IntPtr fanHandle, ctl_fan_speed_t speed)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlFanSetFixedSpeedMode((_ctl_fan_handle_t*)fanHandle, &speed);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set fan fixed speed");
        }

        /// <summary>
        /// Set the fan to speed table mode.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        /// <param name="table">Fan speed table.</param>
        public unsafe void FanSetSpeedTableMode(IntPtr fanHandle, ctl_fan_speed_table_t table)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlFanSetSpeedTableMode((_ctl_fan_handle_t*)fanHandle, &table);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set fan speed table");
        }

        /// <summary>
        /// Get the current fan speed.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        /// <param name="units">Speed units.</param>
        /// <returns>Fan speed value.</returns>
        public unsafe int FanGetState(IntPtr fanHandle, ctl_fan_speed_units_t units)
        {
            ThrowIfDisposed();
            int speed = 0;
            var result = IGCL.ctlFanGetState((_ctl_fan_handle_t*)fanHandle, units, &speed);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get fan state");
            return speed;
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumFans(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get fan count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumFans(adapter, &count, (_ctl_fan_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate fans");
            }
            return handles;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLFanHelper));
        }

        private static unsafe ctl_fan_properties_t CreateFanProperties() => new ctl_fan_properties_t { Size = (uint)sizeof(ctl_fan_properties_t), Version = 0 };
        private static unsafe ctl_fan_config_t CreateFanConfig() => new ctl_fan_config_t { Size = (uint)sizeof(ctl_fan_config_t), Version = 0 };
        /// <summary>
        /// Create a fan speed struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized fan speed struct.</returns>
        public static unsafe ctl_fan_speed_t CreateFanSpeed() => new ctl_fan_speed_t { Size = (uint)sizeof(ctl_fan_speed_t), Version = 0 };
        /// <summary>
        /// Create a fan speed table struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized fan speed table struct.</returns>
        public static unsafe ctl_fan_speed_table_t CreateFanSpeedTable() => new ctl_fan_speed_table_t { Size = (uint)sizeof(ctl_fan_speed_table_t), Version = 0 };

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCLFanDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    /// <summary>
    /// DTO for fan properties.
    /// </summary>
    public struct FanPropertiesDto
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
        /// Indicates whether the fan can be controlled.
        /// </summary>
        public bool CanControl;
        /// <summary>
        /// Supported control modes.
        /// </summary>
        public uint SupportedModes;
        /// <summary>
        /// Supported speed units.
        /// </summary>
        public uint SupportedUnits;
        /// <summary>
        /// Maximum RPM.
        /// </summary>
        public int MaxRpm;
        /// <summary>
        /// Maximum points in the speed table.
        /// </summary>
        public int MaxPoints;

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Fan properties DTO.</returns>
        public static FanPropertiesDto FromNative(ctl_fan_properties_t native)
        {
            return new FanPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                CanControl = IGCLFanDtoBool.ToBool(native.canControl),
                SupportedModes = native.supportedModes,
                SupportedUnits = native.supportedUnits,
                MaxRpm = native.maxRPM,
                MaxPoints = native.maxPoints
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Fan properties struct.</returns>
        public ctl_fan_properties_t ToNative()
        {
            return new ctl_fan_properties_t
            {
                Size = Size,
                Version = Version,
                canControl = IGCLFanDtoBool.ToByte(CanControl),
                supportedModes = SupportedModes,
                supportedUnits = SupportedUnits,
                maxRPM = MaxRpm,
                maxPoints = MaxPoints
            };
        }
    }
}
