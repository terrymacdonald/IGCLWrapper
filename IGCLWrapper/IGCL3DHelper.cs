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
    /// DTO for 3D feature get/set operations.
    /// </summary>
    public unsafe struct ThreeDFeatureGetSetDto
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
        public ctl_property_t Value;
        /// <summary>
        /// Size of the custom value buffer.
        /// </summary>
        public int CustomValueSize;
        /// <summary>
        /// Pointer to custom value buffer.
        /// </summary>
        public IntPtr CustomValue;

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
                Value = native.Value,
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
            return new ctl_3d_feature_getset_t
            {
                Size = Size,
                Version = Version,
                FeatureType = FeatureType,
                ApplicationName = (sbyte*)ApplicationName,
                ApplicationNameLength = ApplicationNameLength,
                bSet = IGCL3DDtoBool.ToByte(Set),
                ValueType = ValueType,
                Value = Value,
                CustomValueSize = CustomValueSize,
                pCustomValue = (void*)CustomValue
            };
        }
    }
}
