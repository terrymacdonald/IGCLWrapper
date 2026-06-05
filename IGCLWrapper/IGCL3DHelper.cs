using System;
using System.Collections.Generic;
using System.Text;

namespace IGCLWrapper
{
    /// <summary>
    /// 3D feature helper: capabilities and get/set operations.
    /// </summary>
    public sealed class IGCL3DHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCL3DHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        /// <summary>
        /// Get supported 3D feature capabilities for the adapter.
        /// </summary>
        /// <returns>3D feature capabilities DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe ThreeDFeatureCapsDto? GetSupported3DCapabilities()
        {
            ThrowIfDisposed();
            var caps = Create3DFeatureCaps();
            var result = IGCL.ctlGetSupported3DCapabilities((_ctl_device_adapter_handle_t*)_adapter, &caps);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return ThreeDFeatureCapsDto.FromNative(caps);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, "Failed to get 3D capabilities");
        }

        /// <summary>
        /// Get a 3D feature using a DTO request.
        /// </summary>
        /// <param name="feature">3D feature DTO.</param>
        /// <returns>Updated 3D feature DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public ThreeDFeatureGetSetDto? Get3DFeature(ThreeDFeatureGetSetDto feature)
        {
            ThrowIfDisposed();
            try
            {
                var request = feature;
                request.Set = false;
                return ExecuteGetSet3DFeature(request);
            }
            catch (IGCLException ex) when (IsUnsupportedResult(ex.Result))
            {
                return null;
            }
        }

        /// <summary>
        /// Set a 3D feature using a DTO request.
        /// </summary>
        /// <param name="feature">3D feature DTO.</param>
        /// <returns><c>true</c> if the setting was applied successfully; <c>false</c> if the feature is not supported on this hardware or driver.</returns>
        public bool Set3DFeature(ThreeDFeatureGetSetDto feature)
        {
            ThrowIfDisposed();
            try
            {
                var request = feature;
                request.Set = true;
                ValidateSet3DFeatureRequest(request);
                _ = ExecuteGetSet3DFeature(request);
                return true;
            }
            catch (IGCLException ex) when (IsUnsupportedResult(ex.Result))
            {
                return false;
            }
        }

        private unsafe ThreeDFeatureGetSetDto ExecuteGetSet3DFeature(ThreeDFeatureGetSetDto request)
        {
            var native = request.ToNative();

            var appName = request.ApplicationName;
            if (!string.IsNullOrEmpty(appName))
            {
                var maxLen = Math.Min(appName.Length, sbyte.MaxValue);
                unsafe
                {
                    sbyte* pApplicationName = stackalloc sbyte[maxLen + 1];
                    for (var i = 0; i < maxLen; i++)
                    {
                        var c = appName[i];
                        pApplicationName[i] = c <= sbyte.MaxValue ? unchecked((sbyte)c) : (sbyte)'?';
                    }
                    pApplicationName[maxLen] = 0;
                    native.ApplicationName = pApplicationName;
                    native.ApplicationNameLength = (sbyte)maxLen;
                }
            }

            var customValue = request.CustomValue;
            if (customValue != null && customValue.Count > 0)
            {
                unsafe
                {
                    byte* pCustomValue = stackalloc byte[customValue.Count];
                    for (var i = 0; i < customValue.Count; i++)
                        pCustomValue[i] = customValue[i];
                    native.pCustomValue = pCustomValue;
                    native.CustomValueSize = customValue.Count;
                }
            }

            var result2 = IGCL.ctlGetSet3DFeature((_ctl_device_adapter_handle_t*)_adapter, &native);
            if (result2 != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result2, $"Failed to get/set 3D feature {native.FeatureType}");
            return ThreeDFeatureGetSetDto.FromNative(native);
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
                throw new ObjectDisposedException(nameof(IGCL3DHelper));
        }

        private static unsafe ctl_3d_feature_caps_t Create3DFeatureCaps() => new ctl_3d_feature_caps_t { Size = (uint)sizeof(ctl_3d_feature_caps_t), Version = 0 };
        /// <summary>
        /// Create a 3D feature get/set struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized 3D feature get/set struct.</returns>
        public static unsafe ctl_3d_feature_getset_t Create3DFeatureGetSet() => new ctl_3d_feature_getset_t { Size = (uint)sizeof(ctl_3d_feature_getset_t), Version = 0 };

        /// <summary>
        /// Create a DTO request for a 3D get operation.
        /// </summary>
        /// <param name="featureType">Feature to query.</param>
        /// <param name="valueType">Expected value type.</param>
        /// <returns>Initialized get request DTO.</returns>
        public static ThreeDFeatureGetSetDto Create3DFeatureGetRequest(ctl_3d_feature_t featureType, ctl_property_value_type_t valueType)
        {
            return new ThreeDFeatureGetSetDto
            {
                FeatureType = featureType,
                ValueType = valueType,
                Set = false
            };
        }

        /// <summary>
        /// Create a DTO request for a 3D set operation.
        /// </summary>
        /// <param name="featureType">Feature to set.</param>
        /// <param name="valueType">Value type for the feature.</param>
        /// <param name="value">Feature value payload.</param>
        /// <param name="applicationName">Optional application name.</param>
        /// <param name="customValue">Optional custom payload bytes.</param>
        /// <returns>Initialized set request DTO.</returns>
        public static ThreeDFeatureGetSetDto Create3DFeatureSetRequest(
            ctl_3d_feature_t featureType,
            ctl_property_value_type_t valueType,
            PropertyDto value,
            string? applicationName = null,
            List<byte>? customValue = null)
        {
            return new ThreeDFeatureGetSetDto
            {
                FeatureType = featureType,
                ValueType = valueType,
                Value = value,
                ApplicationName = applicationName,
                CustomValue = customValue ?? new List<byte>(),
                Set = true
            };
        }

        /// <summary>
        /// Validate a 3D set request to catch accidental default DTO usage.
        /// </summary>
        /// <param name="request">Request DTO.</param>
        /// <exception cref="ArgumentException">Thrown when request appears to be an accidental default payload.</exception>
        public static void ValidateSet3DFeatureRequest(ThreeDFeatureGetSetDto request)
        {
            if (request.Equals(default))
            {
                throw new ArgumentException(
                    "3D set request cannot be default. Use Create3DFeatureSetRequest and provide explicit feature/value fields.",
                    nameof(request));
            }
        }

        /// <summary>
        /// Compare 3D feature capabilities while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left capabilities struct.</param>
        /// <param name="right">Right capabilities struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool Are3dFeatureCapsEqual(ctl_3d_feature_caps_t left, ctl_3d_feature_caps_t right)
        {
            return ThreeDFeatureCapsDto.FromNative(left).Equals(ThreeDFeatureCapsDto.FromNative(right));
        }

        /// <summary>
        /// Compare 3D feature get/set data while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left get/set struct.</param>
        /// <param name="right">Right get/set struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool Are3dFeatureGetSetEqual(ctl_3d_feature_getset_t left, ctl_3d_feature_getset_t right)
        {
            return ThreeDFeatureGetSetDto.FromNative(left).Equals(ThreeDFeatureGetSetDto.FromNative(right));
        }

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCL3DDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    /// <summary>
    /// DTO for boolean property values.
    /// </summary>
    public struct PropertyBooleanDto : IEquatable<PropertyBooleanDto>
    {
        /// <summary>
        /// Enable value.
        /// </summary>
        public bool Enable;

        public bool Equals(PropertyBooleanDto other) => Enable == other.Enable;
        public override bool Equals(object? obj) => obj is PropertyBooleanDto other && Equals(other);
        public override int GetHashCode() => Enable.GetHashCode();

        public static PropertyBooleanDto FromNative(ctl_property_boolean_t native)
        {
            return new PropertyBooleanDto { Enable = IGCL3DDtoBool.ToBool(native.Enable) };
        }

        public ctl_property_boolean_t ToNative()
        {
            return new ctl_property_boolean_t { Enable = IGCL3DDtoBool.ToByte(Enable) };
        }
    }

    /// <summary>
    /// DTO for float property values.
    /// </summary>
    public struct PropertyFloatDto : IEquatable<PropertyFloatDto>
    {
        /// <summary>
        /// Enable value.
        /// </summary>
        public bool Enable;
        /// <summary>
        /// Float value.
        /// </summary>
        public float Value;

        public bool Equals(PropertyFloatDto other) => Enable == other.Enable && Value.Equals(other.Value);
        public override bool Equals(object? obj) => obj is PropertyFloatDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Enable, Value);

        public static PropertyFloatDto FromNative(ctl_property_float_t native)
        {
            return new PropertyFloatDto { Enable = IGCL3DDtoBool.ToBool(native.Enable), Value = native.Value };
        }

        public ctl_property_float_t ToNative()
        {
            return new ctl_property_float_t { Enable = IGCL3DDtoBool.ToByte(Enable), Value = Value };
        }
    }

    /// <summary>
    /// DTO for int property values.
    /// </summary>
    public struct PropertyIntDto : IEquatable<PropertyIntDto>
    {
        /// <summary>
        /// Enable value.
        /// </summary>
        public bool Enable;
        /// <summary>
        /// Int value.
        /// </summary>
        public int Value;

        public bool Equals(PropertyIntDto other) => Enable == other.Enable && Value == other.Value;
        public override bool Equals(object? obj) => obj is PropertyIntDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Enable, Value);

        public static PropertyIntDto FromNative(ctl_property_int_t native)
        {
            return new PropertyIntDto { Enable = IGCL3DDtoBool.ToBool(native.Enable), Value = native.Value };
        }

        public ctl_property_int_t ToNative()
        {
            return new ctl_property_int_t { Enable = IGCL3DDtoBool.ToByte(Enable), Value = Value };
        }
    }

    /// <summary>
    /// DTO for enum property values.
    /// </summary>
    public struct PropertyEnumDto : IEquatable<PropertyEnumDto>
    {
        /// <summary>
        /// Enum enable type value.
        /// </summary>
        public uint EnableType;

        public bool Equals(PropertyEnumDto other) => EnableType == other.EnableType;
        public override bool Equals(object? obj) => obj is PropertyEnumDto other && Equals(other);
        public override int GetHashCode() => EnableType.GetHashCode();

        public static PropertyEnumDto FromNative(ctl_property_enum_t native)
        {
            return new PropertyEnumDto { EnableType = native.EnableType };
        }

        public ctl_property_enum_t ToNative()
        {
            return new ctl_property_enum_t { EnableType = EnableType };
        }
    }

    /// <summary>
    /// DTO for uint property values.
    /// </summary>
    public struct PropertyUIntDto : IEquatable<PropertyUIntDto>
    {
        /// <summary>
        /// Enable value.
        /// </summary>
        public bool Enable;
        /// <summary>
        /// UInt value.
        /// </summary>
        public uint Value;

        public bool Equals(PropertyUIntDto other) => Enable == other.Enable && Value == other.Value;
        public override bool Equals(object? obj) => obj is PropertyUIntDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Enable, Value);

        public static PropertyUIntDto FromNative(ctl_property_uint_t native)
        {
            return new PropertyUIntDto { Enable = IGCL3DDtoBool.ToBool(native.Enable), Value = native.Value };
        }

        public ctl_property_uint_t ToNative()
        {
            return new ctl_property_uint_t { Enable = IGCL3DDtoBool.ToByte(Enable), Value = Value };
        }
    }

    /// <summary>
    /// DTO for property union values.
    /// </summary>
    public struct PropertyDto : IEquatable<PropertyDto>
    {
        /// <summary>
        /// Boolean property value.
        /// </summary>
        public PropertyBooleanDto BoolType;
        /// <summary>
        /// Float property value.
        /// </summary>
        public PropertyFloatDto FloatType;
        /// <summary>
        /// Int property value.
        /// </summary>
        public PropertyIntDto IntType;
        /// <summary>
        /// Enum property value.
        /// </summary>
        public PropertyEnumDto EnumType;
        /// <summary>
        /// UInt property value.
        /// </summary>
        public PropertyUIntDto UIntType;

        public bool Equals(PropertyDto other)
        {
            return BoolType.Equals(other.BoolType) &&
                   FloatType.Equals(other.FloatType) &&
                   IntType.Equals(other.IntType) &&
                   EnumType.Equals(other.EnumType) &&
                   UIntType.Equals(other.UIntType);
        }

        public override bool Equals(object? obj) => obj is PropertyDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(BoolType);
            hash.Add(FloatType);
            hash.Add(IntType);
            hash.Add(EnumType);
            hash.Add(UIntType);
            return hash.ToHashCode();
        }

        public static PropertyDto FromNative(ctl_property_t native)
        {
            return new PropertyDto
            {
                BoolType = PropertyBooleanDto.FromNative(native.BoolType),
                FloatType = PropertyFloatDto.FromNative(native.FloatType),
                IntType = PropertyIntDto.FromNative(native.IntType),
                EnumType = PropertyEnumDto.FromNative(native.EnumType),
                UIntType = PropertyUIntDto.FromNative(native.UIntType)
            };
        }

        public ctl_property_t ToNative()
        {
            var native = new ctl_property_t();
            if (FloatType.Enable)
                native.FloatType = FloatType.ToNative();
            else if (IntType.Enable)
                native.IntType = IntType.ToNative();
            else if (UIntType.Enable)
                native.UIntType = UIntType.ToNative();
            else if (BoolType.Enable)
                native.BoolType = BoolType.ToNative();
            else
                native.EnumType = EnumType.ToNative();
            return native;
        }
    }

    /// <summary>
    /// DTO for 3D feature get/set operations.
    /// </summary>
    public unsafe struct ThreeDFeatureGetSetDto : IEquatable<ThreeDFeatureGetSetDto>
    {
        public ThreeDFeatureGetSetDto() {}
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Feature type identifier.
        /// </summary>
        public ctl_3d_feature_t FeatureType;
        /// <summary>
        /// Optional application name.
        /// </summary>
        public string? ApplicationName;
        /// <summary>
        /// True to set the feature, false to get.
        /// </summary>
        public bool Set;
        /// <summary>
        /// Value type for the feature.
        /// </summary>
        public ctl_property_value_type_t ValueType;
        /// <summary>
        /// Feature value.
        /// </summary>
        public PropertyDto Value;
        /// <summary>
        /// Custom value bytes.
        /// </summary>
        public List<byte> CustomValue = new List<byte>();

        /// <summary>
        /// Compare 3D feature get/set data while ignoring pointer fields.
        /// </summary>
        /// <param name="other">Other args instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(ThreeDFeatureGetSetDto other)
        {
            return FeatureType == other.FeatureType &&
                   Set == other.Set &&
                   ValueType == other.ValueType &&
                   Value.Equals(other.Value) &&
                   string.Equals(ApplicationName, other.ApplicationName, StringComparison.Ordinal) &&
                   AreByteListsEqual(CustomValue, other.CustomValue);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is ThreeDFeatureGetSetDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(FeatureType);
            hash.Add(ApplicationName, StringComparer.Ordinal);
            hash.Add(Set);
            hash.Add(ValueType);
            hash.Add(Value);
            if (CustomValue != null)
            {
                hash.Add(CustomValue.Count);
                for (var i = 0; i < CustomValue.Count; i++)
                    hash.Add(CustomValue[i]);
            }
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>3D feature DTO.</returns>
        public static ThreeDFeatureGetSetDto FromNative(ctl_3d_feature_getset_t native)
        {
            return new ThreeDFeatureGetSetDto
            {
                Size = native.Size,
                Version = native.Version,
                FeatureType = native.FeatureType,
                ApplicationName = ReadAsciiString(native.ApplicationName, native.ApplicationNameLength),
                Set = IGCL3DDtoBool.ToBool(native.bSet),
                ValueType = native.ValueType,
                Value = PropertyDto.FromNative(native.Value),
                CustomValue = ReadCustomValue(native.pCustomValue, native.CustomValueSize) ?? new List<byte>()
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct (pointers are null; pin at call site).
        /// </summary>
        /// <returns>3D feature get/set struct.</returns>
        public ctl_3d_feature_getset_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_3d_feature_getset_t);

            return new ctl_3d_feature_getset_t
            {
                Size = size,
                Version = Version,
                FeatureType = FeatureType,
                ApplicationName = null,
                ApplicationNameLength = string.IsNullOrEmpty(ApplicationName) ? (sbyte)0 : (sbyte)Math.Min(ApplicationName.Length, sbyte.MaxValue),
                bSet = IGCL3DDtoBool.ToByte(Set),
                ValueType = ValueType,
                Value = Value.ToNative(),
                CustomValueSize = CustomValue == null ? 0 : CustomValue.Count,
                pCustomValue = null
            };
        }

        private static unsafe string ReadAsciiString(sbyte* pValue, sbyte length)
        {
            if (pValue == null || length <= 0)
                return string.Empty;

            return new string(pValue, 0, length, Encoding.ASCII);
        }

        private static unsafe List<byte>? ReadCustomValue(void* pValue, int size)
        {
            if (pValue == null || size <= 0)
                return null;

            var values = new List<byte>(size);
            var pBytes = (byte*)pValue;
            for (var i = 0; i < size; i++)
                values.Add(pBytes[i]);

            return values;
        }

        private static bool AreByteListsEqual(List<byte>? left, List<byte>? right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left == null || right == null)
                return false;
            if (left.Count != right.Count)
                return false;
            for (var i = 0; i < left.Count; i++)
            {
                if (left[i] != right[i])
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// DTO for 3D feature capabilities.
    /// </summary>
    public struct ThreeDFeatureCapsDto : IEquatable<ThreeDFeatureCapsDto>
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
        /// Number of supported features.
        /// </summary>
        public uint NumSupportedFeatures;

        public bool Equals(ThreeDFeatureCapsDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   NumSupportedFeatures == other.NumSupportedFeatures;
        }

        public override bool Equals(object? obj) => obj is ThreeDFeatureCapsDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(NumSupportedFeatures);
            return hash.ToHashCode();
        }

        public static ThreeDFeatureCapsDto FromNative(ctl_3d_feature_caps_t native)
        {
            return new ThreeDFeatureCapsDto
            {
                Size = native.Size,
                Version = native.Version,
                NumSupportedFeatures = native.NumSupportedFeatures
            };
        }

        public unsafe ctl_3d_feature_caps_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_3d_feature_caps_t);
            return new ctl_3d_feature_caps_t
            {
                Size = size,
                Version = Version,
                NumSupportedFeatures = NumSupportedFeatures
            };
        }
    }
}

