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

        public unsafe ctl_3d_feature_caps_t GetSupported3DCapabilities()
        {
            ThrowIfDisposed();
            var caps = Create3DFeatureCaps();
            var result = IGCL.ctlGetSupported3DCapabilities((_ctl_device_adapter_handle_t*)_adapter, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get 3D capabilities");
            return caps;
        }

        public unsafe ctl_3d_feature_getset_t GetSet3DFeatureNative(ctl_3d_feature_getset_t feature)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlGetSet3DFeature((_ctl_device_adapter_handle_t*)_adapter, &feature);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to get/set 3D feature {feature.FeatureType}");
            return feature;
        }

        public ThreeDFeatureGetSetDto Get3DFeature(ThreeDFeatureGetSetDto feature)
        {
            var request = feature;
            request.Set = false;
            var native = GetSet3DFeatureNative(request.ToNative());
            return ThreeDFeatureGetSetDto.FromNative(native);
        }

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
        public static unsafe ctl_3d_feature_getset_t Create3DFeatureGetSet() => new ctl_3d_feature_getset_t { Size = (uint)sizeof(ctl_3d_feature_getset_t), Version = 0 };

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

    public unsafe struct ThreeDFeatureGetSetDto
    {
        public uint Size;
        public byte Version;
        public ctl_3d_feature_t FeatureType;
        public IntPtr ApplicationName;
        public sbyte ApplicationNameLength;
        public bool Set;
        public ctl_property_value_type_t ValueType;
        public ctl_property_t Value;
        public int CustomValueSize;
        public IntPtr CustomValue;

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
