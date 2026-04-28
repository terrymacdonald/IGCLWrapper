using System;

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
        /// <returns>3D feature capabilities struct.</returns>
        public unsafe ctl_3d_feature_caps_t GetSupported3DCapabilities()
        {
            ThrowIfDisposed();
            var caps = Create3DFeatureCaps();
            var result = IGCL.ctlGetSupported3DCapabilities((_ctl_device_adapter_handle_t*)_adapter, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get 3D capabilities");
            return caps;
        }

        /// <summary>
        /// Call the native get/set 3D feature API using the provided struct.
        /// </summary>
        /// <param name="feature">3D feature get/set struct.</param>
        /// <returns>Updated 3D feature get/set struct.</returns>
        public unsafe ctl_3d_feature_getset_t GetSet3DFeatureNative(ctl_3d_feature_getset_t feature)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlGetSet3DFeature((_ctl_device_adapter_handle_t*)_adapter, &feature);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to get/set 3D feature {feature.FeatureType}");
            return feature;
        }

        /// <summary>
        /// Get a 3D feature using a DTO request.
        /// </summary>
        /// <param name="feature">3D feature DTO.</param>
        /// <returns>Updated 3D feature DTO.</returns>
        public ThreeDFeatureGetSetDto Get3DFeature(ThreeDFeatureGetSetDto feature)
        {
            var request = feature;
            request.Set = false;
            var native = GetSet3DFeatureNative(request.ToNative());
            return ThreeDFeatureGetSetDto.FromNative(native);
        }

        /// <summary>
        /// Set a 3D feature using a DTO request.
        /// </summary>
        /// <param name="feature">3D feature DTO.</param>
        public void Set3DFeature(ThreeDFeatureGetSetDto feature)
        {
            var request = feature;
            request.Set = true;
            GetSet3DFeatureNative(request.ToNative());
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
        /// Compare 3D feature capabilities while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left capabilities struct.</param>
        /// <param name="right">Right capabilities struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool Are3dFeatureCapsEqual(ctl_3d_feature_caps_t left, ctl_3d_feature_caps_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.NumSupportedFeatures == right.NumSupportedFeatures;
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
        /// Pointer to application name (optional).
        /// </summary>
        public IntPtr ApplicationName;
        /// <summary>
        /// Length of the application name.
        /// </summary>
        public sbyte ApplicationNameLength;
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
        /// Size of the custom value buffer.
        /// </summary>
        public int CustomValueSize;
        /// <summary>
        /// Pointer to custom value buffer.
        /// </summary>
        public IntPtr CustomValue;

        /// <summary>
        /// Compare 3D feature get/set args while ignoring pointer fields.
        /// </summary>
        /// <param name="other">Other args instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(ThreeDFeatureGetSetDto other)
        {
            // ApplicationName and CustomValue are pointers and are intentionally excluded.
                 return FeatureType == other.FeatureType &&
                   ApplicationNameLength == other.ApplicationNameLength &&
                   Set == other.Set &&
                   ValueType == other.ValueType &&
                   Value.Equals(other.Value) &&
                   CustomValueSize == other.CustomValueSize;
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
            hash.Add(ApplicationNameLength);
            hash.Add(Set);
            hash.Add(ValueType);
            hash.Add(Value);
            hash.Add(CustomValueSize);
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
                ApplicationName = (IntPtr)native.ApplicationName,
                ApplicationNameLength = native.ApplicationNameLength,
                Set = IGCL3DDtoBool.ToBool(native.bSet),
                ValueType = native.ValueType,
                Value = PropertyDto.FromNative(native.Value),
                CustomValueSize = native.CustomValueSize,
                CustomValue = (IntPtr)native.pCustomValue
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
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
                ApplicationName = (sbyte*)ApplicationName,
                ApplicationNameLength = ApplicationNameLength,
                bSet = IGCL3DDtoBool.ToByte(Set),
                ValueType = ValueType,
                Value = Value.ToNative(),
                CustomValueSize = CustomValueSize,
                pCustomValue = (void*)CustomValue
            };
        }
    }
}

