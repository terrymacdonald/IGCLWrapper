using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace IGCLWrapper
{
    /// <summary>
    /// Firmware helper: base and component firmware queries, PCIe link speed control.
    /// </summary>
    public sealed class IGCLFirmwareHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLFirmwareHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        /// <summary>
        /// Get firmware properties for the adapter as a DTO.
        /// </summary>
        /// <returns>Firmware properties DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe FirmwarePropertiesDto? GetFirmwareProperties()
        {
            ThrowIfDisposed();
            var props = CreateFirmwareProperties();
            var result = IGCL.ctlGetFirmwareProperties((_ctl_device_adapter_handle_t*)_adapter, &props);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return FirmwarePropertiesDto.FromNative(props);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, $"Failed to get firmware properties: {result}");
        }

        /// <summary>
        /// Enumerate firmware component handles for the adapter.
        /// </summary>
        /// <returns>Read-only list of firmware component handles.</returns>
        public unsafe IReadOnlyList<IntPtr> EnumerateFirmwareComponents()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        /// <summary>
        /// Get firmware component properties as a DTO.
        /// </summary>
        /// <param name="firmwareHandle">Firmware component handle.</param>
        /// <returns>Firmware component properties DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe FirmwareComponentPropertiesDto? GetFirmwareComponentProperties(IntPtr firmwareHandle)
        {
            ThrowIfDisposed();
            var props = CreateFirmwareComponentProperties();
            var result = IGCL.ctlGetFirmwareComponentProperties((_ctl_firmware_component_handle_t*)firmwareHandle, &props);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return FirmwareComponentPropertiesDto.FromNative(props);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, "Failed to get firmware component properties");
        }

        /// <summary>
        /// Allow or disallow PCIe link speed updates.
        /// </summary>
        /// <param name="allow">True to allow updates; otherwise false.</param>
        /// <returns><c>true</c> if the setting was applied successfully; <c>false</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe bool AllowPCIeLinkSpeedUpdate(bool allow)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlAllowPCIeLinkSpeedUpdate((_ctl_device_adapter_handle_t*)_adapter, (byte)(allow ? 1 : 0));
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return true;
            if (IsUnsupportedResult(result))
                return false;
            throw new IGCLException(result, "Failed to update PCIe link speed allowance");
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumerateFirmwareComponents(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get firmware component count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumerateFirmwareComponents(adapter, &count, (_ctl_firmware_component_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate firmware components");
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
                throw new ObjectDisposedException(nameof(IGCLFirmwareHelper));
        }

        private static unsafe ctl_firmware_properties_t CreateFirmwareProperties() => new ctl_firmware_properties_t { Size = (uint)sizeof(ctl_firmware_properties_t), Version = 0 };
        private static unsafe ctl_firmware_component_properties_t CreateFirmwareComponentProperties() => new ctl_firmware_component_properties_t { Size = (uint)sizeof(ctl_firmware_component_properties_t), Version = 0 };

        /// <summary>
        /// Compare firmware properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreFirmwarePropertiesEqual(ctl_firmware_properties_t left, ctl_firmware_properties_t right)
        {
            return FirmwarePropertiesDto.FromNative(left).Equals(FirmwarePropertiesDto.FromNative(right));
        }

        /// <summary>
        /// Compare firmware component properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreFirmwareComponentPropertiesEqual(ctl_firmware_component_properties_t left, ctl_firmware_component_properties_t right)
        {
            return FirmwareComponentPropertiesDto.FromNative(left).Equals(FirmwareComponentPropertiesDto.FromNative(right));
        }

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }

    /// <summary>
    /// DTO for firmware properties.
    /// </summary>
    public struct FirmwarePropertiesDto : IEquatable<FirmwarePropertiesDto>
    {
        public FirmwarePropertiesDto() { Name = string.Empty; FirmwareVersion = string.Empty; }
        private const int NameLength = 64;
        private const int VersionLength = 64;
        private const int ReservedLength = 16;
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Firmware name.
        /// </summary>
        public string Name;
        /// <summary>
        /// Firmware version string.
        /// </summary>
        public string FirmwareVersion;
        /// <summary>
        /// Firmware config flags.
        /// </summary>
        public uint FirmwareConfig;
        /// <summary>
        /// Reserved native fields.
        /// </summary>
        public List<byte> Reserved = new();

        /// <summary>
        /// Compare firmware properties while ignoring reserved fields.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(FirmwarePropertiesDto other)
        {
            // Reserved is native-only.
            return Size == other.Size &&
                   Version == other.Version &&
                   string.Equals(Name, other.Name, StringComparison.Ordinal) &&
                   string.Equals(FirmwareVersion, other.FirmwareVersion, StringComparison.Ordinal) &&
                   FirmwareConfig == other.FirmwareConfig;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is FirmwarePropertiesDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Name, StringComparer.Ordinal);
            hash.Add(FirmwareVersion, StringComparer.Ordinal);
            hash.Add(FirmwareConfig);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Firmware properties DTO.</returns>
        public static FirmwarePropertiesDto FromNative(ctl_firmware_properties_t native)
        {
            return new FirmwarePropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                Name = ReadString(native.name, NameLength),
                FirmwareVersion = ReadString(native.version, VersionLength),
                FirmwareConfig = native.FirmwareConfig,
                Reserved = ReadReserved(native.reserved, ReservedLength)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Firmware properties struct.</returns>
        public unsafe ctl_firmware_properties_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_firmware_properties_t);

            var native = new ctl_firmware_properties_t
            {
                Size = size,
                Version = Version,
                FirmwareConfig = FirmwareConfig
            };

            WriteString(Name, NameLength, ref native.name);
            WriteString(FirmwareVersion, VersionLength, ref native.version);
            WriteReserved(Reserved, ReservedLength, ref native.reserved);
            return native;
        }

        private static unsafe string ReadString(ctl_firmware_properties_t._name_e__FixedBuffer buffer, int maxLength)
        {
            var bytes = new byte[maxLength];
            var pBuffer = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            var length = 0;
            for (var i = 0; i < maxLength; i++)
            {
                var value = pBuffer[i];
                if (value == 0)
                    break;
                bytes[i] = (byte)value;
                length++;
            }

            return length == 0 ? string.Empty : Encoding.ASCII.GetString(bytes, 0, length);
        }

        private static unsafe string ReadString(ctl_firmware_properties_t._version_e__FixedBuffer buffer, int maxLength)
        {
            var bytes = new byte[maxLength];
            var pBuffer = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            var length = 0;
            for (var i = 0; i < maxLength; i++)
            {
                var value = pBuffer[i];
                if (value == 0)
                    break;
                bytes[i] = (byte)value;
                length++;
            }

            return length == 0 ? string.Empty : Encoding.ASCII.GetString(bytes, 0, length);
        }

        private static unsafe List<byte> ReadReserved(ctl_firmware_properties_t._reserved_e__FixedBuffer buffer, int length)
        {
            var bytes = new List<byte>(length);
            var pBuffer = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < length; i++)
                bytes.Add((byte)pBuffer[i]);
            return bytes;
        }

        private static unsafe void WriteString(string? value, int maxLength, ref ctl_firmware_properties_t._name_e__FixedBuffer buffer)
        {
            var pBuffer = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < maxLength; i++)
                pBuffer[i] = 0;

            if (string.IsNullOrEmpty(value))
                return;

            var bytes = Encoding.ASCII.GetBytes(value);
            var count = Math.Min(bytes.Length, maxLength - 1);
            for (var i = 0; i < count; i++)
                pBuffer[i] = unchecked((sbyte)bytes[i]);
        }

        private static unsafe void WriteString(string? value, int maxLength, ref ctl_firmware_properties_t._version_e__FixedBuffer buffer)
        {
            var pBuffer = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < maxLength; i++)
                pBuffer[i] = 0;

            if (string.IsNullOrEmpty(value))
                return;

            var bytes = Encoding.ASCII.GetBytes(value);
            var count = Math.Min(bytes.Length, maxLength - 1);
            for (var i = 0; i < count; i++)
                pBuffer[i] = unchecked((sbyte)bytes[i]);
        }

        private static unsafe void WriteReserved(List<byte>? value, int maxLength, ref ctl_firmware_properties_t._reserved_e__FixedBuffer buffer)
        {
            var pBuffer = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < maxLength; i++)
                pBuffer[i] = 0;

            if (value == null || value.Count == 0)
                return;

            var count = Math.Min(value.Count, maxLength);
            for (var i = 0; i < count; i++)
                pBuffer[i] = unchecked((sbyte)value[i]);
        }
    }

    /// <summary>
    /// DTO for firmware component properties.
    /// </summary>
    public struct FirmwareComponentPropertiesDto : IEquatable<FirmwareComponentPropertiesDto>
    {
        public FirmwareComponentPropertiesDto() { Name = string.Empty; ComponentVersion = string.Empty; }
        private const int NameLength = 64;
        private const int VersionLength = 64;
        private const int ReservedLength = 20;
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Component name.
        /// </summary>
        public string Name;
        /// <summary>
        /// Component version string.
        /// </summary>
        public string ComponentVersion;
        /// <summary>
        /// Reserved native fields.
        /// </summary>
        public List<byte> Reserved = new();

        /// <summary>
        /// Compare firmware component properties while ignoring reserved fields.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(FirmwareComponentPropertiesDto other)
        {
            // Reserved is native-only.
            return Size == other.Size &&
                   Version == other.Version &&
                   string.Equals(Name, other.Name, StringComparison.Ordinal) &&
                   string.Equals(ComponentVersion, other.ComponentVersion, StringComparison.Ordinal);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is FirmwareComponentPropertiesDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Name, StringComparer.Ordinal);
            hash.Add(ComponentVersion, StringComparer.Ordinal);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Firmware component properties DTO.</returns>
        public static FirmwareComponentPropertiesDto FromNative(ctl_firmware_component_properties_t native)
        {
            return new FirmwareComponentPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                Name = ReadString(native.name, NameLength),
                ComponentVersion = ReadString(native.version, VersionLength),
                Reserved = ReadReserved(native.reserved, ReservedLength)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Firmware component properties struct.</returns>
        public unsafe ctl_firmware_component_properties_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_firmware_component_properties_t);

            var native = new ctl_firmware_component_properties_t
            {
                Size = size,
                Version = Version
            };

            WriteString(Name, NameLength, ref native.name);
            WriteString(ComponentVersion, VersionLength, ref native.version);
            WriteReserved(Reserved, ReservedLength, ref native.reserved);
            return native;
        }

        private static unsafe string ReadString(ctl_firmware_component_properties_t._name_e__FixedBuffer buffer, int maxLength)
        {
            var bytes = new byte[maxLength];
            var pBuffer = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            var length = 0;
            for (var i = 0; i < maxLength; i++)
            {
                var value = pBuffer[i];
                if (value == 0)
                    break;
                bytes[i] = (byte)value;
                length++;
            }

            return length == 0 ? string.Empty : Encoding.ASCII.GetString(bytes, 0, length);
        }

        private static unsafe string ReadString(ctl_firmware_component_properties_t._version_e__FixedBuffer buffer, int maxLength)
        {
            var bytes = new byte[maxLength];
            var pBuffer = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            var length = 0;
            for (var i = 0; i < maxLength; i++)
            {
                var value = pBuffer[i];
                if (value == 0)
                    break;
                bytes[i] = (byte)value;
                length++;
            }

            return length == 0 ? string.Empty : Encoding.ASCII.GetString(bytes, 0, length);
        }

        private static unsafe List<byte> ReadReserved(ctl_firmware_component_properties_t._reserved_e__FixedBuffer buffer, int length)
        {
            var bytes = new List<byte>(length);
            var pBuffer = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < length; i++)
                bytes.Add((byte)pBuffer[i]);
            return bytes;
        }

        private static unsafe void WriteString(string? value, int maxLength, ref ctl_firmware_component_properties_t._name_e__FixedBuffer buffer)
        {
            var pBuffer = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < maxLength; i++)
                pBuffer[i] = 0;

            if (string.IsNullOrEmpty(value))
                return;

            var bytes = Encoding.ASCII.GetBytes(value);
            var count = Math.Min(bytes.Length, maxLength - 1);
            for (var i = 0; i < count; i++)
                pBuffer[i] = unchecked((sbyte)bytes[i]);
        }

        private static unsafe void WriteString(string? value, int maxLength, ref ctl_firmware_component_properties_t._version_e__FixedBuffer buffer)
        {
            var pBuffer = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < maxLength; i++)
                pBuffer[i] = 0;

            if (string.IsNullOrEmpty(value))
                return;

            var bytes = Encoding.ASCII.GetBytes(value);
            var count = Math.Min(bytes.Length, maxLength - 1);
            for (var i = 0; i < count; i++)
                pBuffer[i] = unchecked((sbyte)bytes[i]);
        }

        private static unsafe void WriteReserved(List<byte>? value, int maxLength, ref ctl_firmware_component_properties_t._reserved_e__FixedBuffer buffer)
        {
            var pBuffer = (sbyte*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < maxLength; i++)
                pBuffer[i] = 0;

            if (value == null || value.Count == 0)
                return;

            var count = Math.Min(value.Count, maxLength);
            for (var i = 0; i < count; i++)
                pBuffer[i] = unchecked((sbyte)value[i]);
        }
    }
}

