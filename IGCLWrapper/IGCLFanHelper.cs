using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

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
        /// Get fan properties as a DTO.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        /// <returns>Fan properties DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe FanPropertiesDto? FanGetProperties(IntPtr fanHandle)
        {
            ThrowIfDisposed();
            var props = CreateFanProperties();
            var result = IGCL.ctlFanGetProperties((_ctl_fan_handle_t*)fanHandle, &props);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return FanPropertiesDto.FromNative(props);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, "Failed to get fan properties");
        }

        /// <summary>
        /// Get fan configuration as a DTO.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        /// <returns>Fan config DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe FanConfigDto? FanGetConfig(IntPtr fanHandle)
        {
            ThrowIfDisposed();
            var config = CreateFanConfig();
            var result = IGCL.ctlFanGetConfig((_ctl_fan_handle_t*)fanHandle, &config);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return FanConfigDto.FromNative(config);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, "Failed to get fan config");
        }

        /// <summary>
        /// Set the fan to default control mode.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        /// <returns><c>true</c> if the setting was applied successfully; <c>false</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe bool FanSetDefaultMode(IntPtr fanHandle)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlFanSetDefaultMode((_ctl_fan_handle_t*)fanHandle);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return true;
            if (IsUnsupportedResult(result))
                return false;
            throw new IGCLException(result, "Failed to set fan default mode");
        }

        /// <summary>
        /// Set the fan to fixed speed mode using a DTO.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        /// <param name="speed">Fan speed settings DTO.</param>
        /// <returns><c>true</c> if the setting was applied successfully; <c>false</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe bool FanSetFixedSpeedMode(IntPtr fanHandle, FanSpeedDto speed)
        {
            ThrowIfDisposed();
            var native = speed.ToNative();
            var result = IGCL.ctlFanSetFixedSpeedMode((_ctl_fan_handle_t*)fanHandle, &native);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return true;
            if (IsUnsupportedResult(result))
                return false;
            throw new IGCLException(result, "Failed to set fan fixed speed");
        }

        /// <summary>
        /// Set the fan to speed table mode using a DTO.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        /// <param name="table">Fan speed table DTO.</param>
        /// <returns><c>true</c> if the setting was applied successfully; <c>false</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe bool FanSetSpeedTableMode(IntPtr fanHandle, FanSpeedTableDto table)
        {
            ThrowIfDisposed();
            var native = table.ToNative();
            var result = IGCL.ctlFanSetSpeedTableMode((_ctl_fan_handle_t*)fanHandle, &native);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return true;
            if (IsUnsupportedResult(result))
                return false;
            throw new IGCLException(result, "Failed to set fan speed table");
        }

        /// <summary>
        /// Get the current fan speed.
        /// </summary>
        /// <param name="fanHandle">Fan handle.</param>
        /// <param name="units">Speed units.</param>
        /// <returns>Fan speed value, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe int? FanGetState(IntPtr fanHandle, ctl_fan_speed_units_t units)
        {
            ThrowIfDisposed();
            int speed = 0;
            var result = IGCL.ctlFanGetState((_ctl_fan_handle_t*)fanHandle, units, &speed);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return speed;
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, "Failed to get fan state");
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
        /// Compare fan properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreFanPropertiesEqual(ctl_fan_properties_t left, ctl_fan_properties_t right)
        {
            return FanPropertiesDto.FromNative(left).Equals(FanPropertiesDto.FromNative(right));
        }

        /// <summary>
        /// Compare fan configuration while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left config struct.</param>
        /// <param name="right">Right config struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreFanConfigEqual(ctl_fan_config_t left, ctl_fan_config_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.mode == right.mode &&
                   AreFanSpeedEqual(left.speedFixed, right.speedFixed) &&
                   AreFanSpeedTableEqualInternal(left.speedTable, right.speedTable);
        }

        /// <summary>
        /// Compare fan speed while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left speed struct.</param>
        /// <param name="right">Right speed struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreFanSpeedEqual(ctl_fan_speed_t left, ctl_fan_speed_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.speed == right.speed &&
                   left.units == right.units;
        }

        /// <summary>
        /// Compare fan speed tables while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left table struct.</param>
        /// <param name="right">Right table struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreFanSpeedTableEqual(ctl_fan_speed_table_t left, ctl_fan_speed_table_t right)
        {
            return AreFanSpeedTableEqualInternal(left, right);
        }

        private static bool AreFanSpeedTableEqualInternal(ctl_fan_speed_table_t left, ctl_fan_speed_table_t right)
        {
            if (left.Size != right.Size || left.Version != right.Version || left.numPoints != right.numPoints)
                return false;

            var count = Math.Min(Math.Max(left.numPoints, 0), 32);
            var leftSpan = MemoryMarshal.CreateReadOnlySpan(ref left.table.e0, 32);
            var rightSpan = MemoryMarshal.CreateReadOnlySpan(ref right.table.e0, 32);
            for (var i = 0; i < count; i++)
            {
                if (!AreFanTempSpeedEqual(leftSpan[i], rightSpan[i]))
                    return false;
            }

            return true;
        }

        private static bool AreFanTempSpeedEqual(ctl_fan_temp_speed_t left, ctl_fan_temp_speed_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.temperature == right.temperature &&
                   left.speed.speed == right.speed.speed &&
                   left.speed.units == right.speed.units &&
                   left.speed.Size == right.speed.Size &&
                   left.speed.Version == right.speed.Version;
        }

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
    /// DTO for fan speed.
    /// </summary>
    public struct FanSpeedDto : IEquatable<FanSpeedDto>
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
        /// Speed value.
        /// </summary>
        public int Speed;
        /// <summary>
        /// Speed units.
        /// </summary>
        public ctl_fan_speed_units_t Units;

        /// <summary>
        /// Compare fan speeds.
        /// </summary>
        /// <param name="other">Other speed instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(FanSpeedDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Speed == other.Speed &&
                   Units == other.Units;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is FanSpeedDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Speed);
            hash.Add(Units);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Fan speed DTO.</returns>
        public static FanSpeedDto FromNative(ctl_fan_speed_t native)
        {
            return new FanSpeedDto
            {
                Size = native.Size,
                Version = native.Version,
                Speed = native.speed,
                Units = native.units
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Fan speed struct.</returns>
        public unsafe ctl_fan_speed_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_fan_speed_t);

            return new ctl_fan_speed_t
            {
                Size = size,
                Version = Version,
                speed = Speed,
                units = Units
            };
        }
    }

    /// <summary>
    /// DTO for temperature-speed pair in fan table.
    /// </summary>
    public struct FanTempSpeedDto : IEquatable<FanTempSpeedDto>
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
        /// Temperature threshold.
        /// </summary>
        public uint Temperature;
        /// <summary>
        /// Associated fan speed.
        /// </summary>
        public FanSpeedDto Speed;

        /// <summary>
        /// Compare temperature-speed pairs.
        /// </summary>
        /// <param name="other">Other pair instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(FanTempSpeedDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Temperature == other.Temperature &&
                   Speed.Equals(other.Speed);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is FanTempSpeedDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Temperature);
            hash.Add(Speed);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Temperature-speed pair DTO.</returns>
        public static FanTempSpeedDto FromNative(ctl_fan_temp_speed_t native)
        {
            return new FanTempSpeedDto
            {
                Size = native.Size,
                Version = native.Version,
                Temperature = native.temperature,
                Speed = FanSpeedDto.FromNative(native.speed)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Temperature-speed pair struct.</returns>
        public unsafe ctl_fan_temp_speed_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_fan_temp_speed_t);

            return new ctl_fan_temp_speed_t
            {
                Size = size,
                Version = Version,
                temperature = Temperature,
                speed = Speed.ToNative()
            };
        }
    }

    /// <summary>
    /// DTO for fan speed table.
    /// </summary>
    public struct FanSpeedTableDto : IEquatable<FanSpeedTableDto>
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
        /// Array of temperature-speed points.
        /// </summary>
        public List<FanTempSpeedDto> Table;

        /// <summary>
        /// Compare fan speed tables.
        /// </summary>
        /// <param name="other">Other table instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(FanSpeedTableDto other)
        {
            if (Size != other.Size || Version != other.Version)
                return false;
            if (Table == null && other.Table == null)
                return true;
            if (Table == null || other.Table == null)
                return false;
            if (Table.Count != other.Table.Count)
                return false;
            for (int i = 0; i < Table.Count; i++)
            {
                if (!Table[i].Equals(other.Table[i]))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is FanSpeedTableDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            if (Table != null)
                foreach (var item in Table)
                    hash.Add(item);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Fan speed table DTO.</returns>
        public static FanSpeedTableDto FromNative(ctl_fan_speed_table_t native)
        {
            var table = new List<FanTempSpeedDto>();
            int count = Math.Min(Math.Max(native.numPoints, 0), 32);
            var span = MemoryMarshal.CreateReadOnlySpan(ref native.table.e0, 32);
            for (int i = 0; i < count; i++)
            {
                table.Add(FanTempSpeedDto.FromNative(span[i]));
            }

            return new FanSpeedTableDto
            {
                Size = native.Size,
                Version = native.Version,
                Table = table
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Fan speed table struct.</returns>
        public unsafe ctl_fan_speed_table_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_fan_speed_table_t);

            var native = new ctl_fan_speed_table_t
            {
                Size = size,
                Version = Version,
                numPoints = Table?.Count ?? 0
            };

            if (Table != null && Table.Count > 0)
            {
                var count = Math.Min(Table.Count, 32);
                var span = MemoryMarshal.CreateSpan(ref native.table.e0, 32);
                for (int i = 0; i < count; i++)
                {
                    span[i] = Table[i].ToNative();
                }
            }

            return native;
        }
    }

    /// <summary>
    /// DTO for fan configuration.
    /// </summary>
    public struct FanConfigDto : IEquatable<FanConfigDto>
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
        /// Current fan mode.
        /// </summary>
        public ctl_fan_speed_mode_t Mode;
        /// <summary>
        /// Fixed speed settings.
        /// </summary>
        public FanSpeedDto SpeedFixed;
        /// <summary>
        /// Speed table settings.
        /// </summary>
        public FanSpeedTableDto SpeedTable;

        /// <summary>
        /// Compare fan configurations.
        /// </summary>
        /// <param name="other">Other config instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(FanConfigDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Mode == other.Mode &&
                   SpeedFixed.Equals(other.SpeedFixed) &&
                   SpeedTable.Equals(other.SpeedTable);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is FanConfigDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Mode);
            hash.Add(SpeedFixed);
            hash.Add(SpeedTable);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Fan config DTO.</returns>
        public static FanConfigDto FromNative(ctl_fan_config_t native)
        {
            return new FanConfigDto
            {
                Size = native.Size,
                Version = native.Version,
                Mode = native.mode,
                SpeedFixed = FanSpeedDto.FromNative(native.speedFixed),
                SpeedTable = FanSpeedTableDto.FromNative(native.speedTable)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Fan config struct.</returns>
        public unsafe ctl_fan_config_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_fan_config_t);

            return new ctl_fan_config_t
            {
                Size = size,
                Version = Version,
                mode = Mode,
                speedFixed = SpeedFixed.ToNative(),
                speedTable = SpeedTable.ToNative()
            };
        }
    }

    /// <summary>
    /// DTO for fan properties.
    /// </summary>
    public struct FanPropertiesDto : IEquatable<FanPropertiesDto>
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
        public bool SupportsDefaultMode
        {
            readonly get => HasEnumBit(SupportedModes, (int)ctl_fan_speed_mode_t.CTL_FAN_SPEED_MODE_DEFAULT);
            set => SupportedModes = SetEnumBit(SupportedModes, (int)ctl_fan_speed_mode_t.CTL_FAN_SPEED_MODE_DEFAULT, value);
        }
        public bool SupportsFixedMode
        {
            readonly get => HasEnumBit(SupportedModes, (int)ctl_fan_speed_mode_t.CTL_FAN_SPEED_MODE_FIXED);
            set => SupportedModes = SetEnumBit(SupportedModes, (int)ctl_fan_speed_mode_t.CTL_FAN_SPEED_MODE_FIXED, value);
        }
        public bool SupportsTableMode
        {
            readonly get => HasEnumBit(SupportedModes, (int)ctl_fan_speed_mode_t.CTL_FAN_SPEED_MODE_TABLE);
            set => SupportedModes = SetEnumBit(SupportedModes, (int)ctl_fan_speed_mode_t.CTL_FAN_SPEED_MODE_TABLE, value);
        }
        /// <summary>
        /// Supported speed units.
        /// </summary>
        public uint SupportedUnits;
        public bool SupportsRpmUnits
        {
            readonly get => HasEnumBit(SupportedUnits, (int)ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM);
            set => SupportedUnits = SetEnumBit(SupportedUnits, (int)ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM, value);
        }
        public bool SupportsPercentUnits
        {
            readonly get => HasEnumBit(SupportedUnits, (int)ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_PERCENT);
            set => SupportedUnits = SetEnumBit(SupportedUnits, (int)ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_PERCENT, value);
        }
        /// <summary>
        /// Maximum RPM.
        /// </summary>
        public int MaxRpm;
        /// <summary>
        /// Maximum points in the speed table.
        /// </summary>
        public int MaxPoints;

        /// <summary>
        /// Compare fan properties.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(FanPropertiesDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   CanControl == other.CanControl &&
                   SupportedModes == other.SupportedModes &&
                   SupportedUnits == other.SupportedUnits &&
                   MaxRpm == other.MaxRpm &&
                   MaxPoints == other.MaxPoints;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is FanPropertiesDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(CanControl);
            hash.Add(SupportedModes);
            hash.Add(SupportedUnits);
            hash.Add(MaxRpm);
            hash.Add(MaxPoints);
            return hash.ToHashCode();
        }

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

        private static bool HasEnumBit(uint value, int enumValue)
        {
            var bit = 1u << enumValue;
            return (value & bit) != 0;
        }

        private static uint SetEnumBit(uint value, int enumValue, bool enabled)
        {
            var bit = 1u << enumValue;
            return enabled ? (value | bit) : (value & ~bit);
        }
    }
}

