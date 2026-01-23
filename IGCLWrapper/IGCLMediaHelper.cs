using System;
using System.Runtime.CompilerServices;

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
        /// Compare video processing feature capabilities while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left capabilities struct.</param>
        /// <param name="right">Right capabilities struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreVideoProcessingFeatureCapsEqual(ctl_video_processing_feature_caps_t left, ctl_video_processing_feature_caps_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.NumSupportedFeatures == right.NumSupportedFeatures;
        }

        /// <summary>
        /// Compare video processing feature get/set data while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left get/set struct.</param>
        /// <param name="right">Right get/set struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreVideoProcessingFeatureGetSetEqual(ctl_video_processing_feature_getset_t left, ctl_video_processing_feature_getset_t right)
        {
            return VideoProcessingFeatureGetSetDto.FromNative(left).Equals(VideoProcessingFeatureGetSetDto.FromNative(right));
        }

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
    public unsafe struct VideoProcessingFeatureGetSetDto : IEquatable<VideoProcessingFeatureGetSetDto>
    {
        private const int ReservedFieldCount = 16;
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
        public uint[]? ReservedFields;

        /// <summary>
        /// Compare video processing feature get/set args while ignoring pointer and reserved fields.
        /// </summary>
        /// <param name="other">Other args instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(VideoProcessingFeatureGetSetDto other)
        {
            // ApplicationName and CustomValue are pointers; ReservedFields are native-only.
            return Size == other.Size &&
                   Version == other.Version &&
                   FeatureType == other.FeatureType &&
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
        public override bool Equals(object? obj) => obj is VideoProcessingFeatureGetSetDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
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
                ReservedFields = ReadReservedFields(native.ReservedFields)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Video processing feature get/set struct.</returns>
        public ctl_video_processing_feature_getset_t ToNative()
        {
            var native = new ctl_video_processing_feature_getset_t
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
                pCustomValue = (void*)CustomValue
            };
            WriteReservedFields(ReservedFields, ref native.ReservedFields);
            return native;
        }

        private static unsafe uint[] ReadReservedFields(ctl_video_processing_feature_getset_t._ReservedFields_e__FixedBuffer buffer)
        {
            var values = new uint[ReservedFieldCount];
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedFieldCount; i++)
                values[i] = pValues[i];
            return values;
        }

        private static unsafe void WriteReservedFields(uint[]? values, ref ctl_video_processing_feature_getset_t._ReservedFields_e__FixedBuffer buffer)
        {
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedFieldCount; i++)
                pValues[i] = 0;

            if (values == null || values.Length == 0)
                return;

            var count = Math.Min(values.Length, ReservedFieldCount);
            for (var i = 0; i < count; i++)
                pValues[i] = values[i];
        }
    }
}

