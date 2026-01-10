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

        /// <summary>
        /// Get supported video processing feature capabilities for the adapter.
        /// </summary>
        /// <returns>Video processing feature capabilities struct.</returns>
        public unsafe ctl_video_processing_feature_caps_t GetSupportedVideoProcessingCapabilities()
        {
            ThrowIfDisposed();
            var caps = CreateVideoProcessingCaps();
            var result = IGCL.ctlGetSupportedVideoProcessingCapabilities((_ctl_device_adapter_handle_t*)_adapter, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get video processing capabilities");
            return caps;
        }

        /// <summary>
        /// Call the native get/set video processing feature API using the provided struct.
        /// </summary>
        /// <param name="featureGetSet">Video processing feature get/set struct.</param>
        /// <returns>Updated video processing feature get/set struct.</returns>
        public unsafe ctl_video_processing_feature_getset_t GetSetVideoProcessingFeatureNative(ctl_video_processing_feature_getset_t featureGetSet)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlGetSetVideoProcessingFeature((_ctl_device_adapter_handle_t*)_adapter, &featureGetSet);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to get/set video processing feature {featureGetSet.FeatureType}");
            return featureGetSet;
        }

        /// <summary>
        /// Get a video processing feature using a DTO request.
        /// </summary>
        /// <param name="featureGetSet">Video processing feature DTO.</param>
        /// <returns>Updated video processing feature DTO.</returns>
        public VideoProcessingFeatureGetSetDto GetVideoProcessingFeature(VideoProcessingFeatureGetSetDto featureGetSet)
        {
            var request = featureGetSet;
            request.Set = false;
            var native = GetSetVideoProcessingFeatureNative(request.ToNative());
            return VideoProcessingFeatureGetSetDto.FromNative(native);
        }

        /// <summary>
        /// Set a video processing feature using a DTO request.
        /// </summary>
        /// <param name="featureGetSet">Video processing feature DTO.</param>
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
        /// <summary>
        /// Create a video processing feature get/set struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized video processing feature get/set struct.</returns>
        public static unsafe ctl_video_processing_feature_getset_t CreateVideoProcessingFeatureGetSet() => new ctl_video_processing_feature_getset_t { Size = (uint)sizeof(ctl_video_processing_feature_getset_t), Version = 0 };

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
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

    /// <summary>
    /// DTO for video processing feature get/set operations.
    /// </summary>
    public unsafe struct VideoProcessingFeatureGetSetDto
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
        public ctl_video_processing_feature_t FeatureType;
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
        /// Reserved fields from the native struct.
        /// </summary>
        public ctl_video_processing_feature_getset_t._ReservedFields_e__FixedBuffer ReservedFields;

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Video processing feature DTO.</returns>
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

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Video processing feature get/set struct.</returns>
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
