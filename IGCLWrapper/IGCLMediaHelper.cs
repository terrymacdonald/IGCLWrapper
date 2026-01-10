using System;

namespace IGCLWrapper
{
    /// <summary>
    /// Media helper: video processing capabilities and get/set.
    /// </summary>
    public sealed class IGCLMediaHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLMediaHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe ctl_video_processing_feature_caps_t GetSupportedVideoProcessingCapabilities()
        {
            ThrowIfDisposed();
            var caps = CreateVideoProcessingCaps();
            var result = IGCL.ctlGetSupportedVideoProcessingCapabilities((_ctl_device_adapter_handle_t*)_adapter, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get video processing capabilities");
            return caps;
        }

        public unsafe ctl_video_processing_feature_getset_t GetSetVideoProcessingFeatureNative(ctl_video_processing_feature_getset_t featureGetSet)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlGetSetVideoProcessingFeature((_ctl_device_adapter_handle_t*)_adapter, &featureGetSet);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to get/set video processing feature {featureGetSet.FeatureType}");
            return featureGetSet;
        }

        public VideoProcessingFeatureGetSetDto GetVideoProcessingFeature(VideoProcessingFeatureGetSetDto featureGetSet)
        {
            var request = featureGetSet;
            request.Set = false;
            var native = GetSetVideoProcessingFeatureNative(request.ToNative());
            return VideoProcessingFeatureGetSetDto.FromNative(native);
        }

        public void SetVideoProcessingFeature(VideoProcessingFeatureGetSetDto featureGetSet)
        {
            var request = featureGetSet;
            request.Set = true;
            GetSetVideoProcessingFeatureNative(request.ToNative());
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLMediaHelper));
        }

        private static unsafe ctl_video_processing_feature_caps_t CreateVideoProcessingCaps() => new ctl_video_processing_feature_caps_t { Size = (uint)sizeof(ctl_video_processing_feature_caps_t), Version = 0 };
        public static unsafe ctl_video_processing_feature_getset_t CreateVideoProcessingFeatureGetSet() => new ctl_video_processing_feature_getset_t { Size = (uint)sizeof(ctl_video_processing_feature_getset_t), Version = 0 };

        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCLMediaDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    public unsafe struct VideoProcessingFeatureGetSetDto
    {
        public uint Size;
        public byte Version;
        public ctl_video_processing_feature_t FeatureType;
        public IntPtr ApplicationName;
        public sbyte ApplicationNameLength;
        public bool Set;
        public ctl_property_value_type_t ValueType;
        public ctl_property_t Value;
        public int CustomValueSize;
        public IntPtr CustomValue;
        public ctl_video_processing_feature_getset_t._ReservedFields_e__FixedBuffer ReservedFields;

        public static VideoProcessingFeatureGetSetDto FromNative(ctl_video_processing_feature_getset_t native)
        {
            return new VideoProcessingFeatureGetSetDto
            {
                Size = native.Size,
                Version = native.Version,
                FeatureType = native.FeatureType,
                ApplicationName = (IntPtr)native.ApplicationName,
                ApplicationNameLength = native.ApplicationNameLength,
                Set = IGCLMediaDtoBool.ToBool(native.bSet),
                ValueType = native.ValueType,
                Value = native.Value,
                CustomValueSize = native.CustomValueSize,
                CustomValue = (IntPtr)native.pCustomValue,
                ReservedFields = native.ReservedFields
            };
        }

        public ctl_video_processing_feature_getset_t ToNative()
        {
            return new ctl_video_processing_feature_getset_t
            {
                Size = Size,
                Version = Version,
                FeatureType = FeatureType,
                ApplicationName = (sbyte*)ApplicationName,
                ApplicationNameLength = ApplicationNameLength,
                bSet = IGCLMediaDtoBool.ToByte(Set),
                ValueType = ValueType,
                Value = Value,
                CustomValueSize = CustomValueSize,
                pCustomValue = (void*)CustomValue,
                ReservedFields = ReservedFields
            };
        }
    }
}
