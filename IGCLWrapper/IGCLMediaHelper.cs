using System;
using System.Collections.Generic;
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
        /// <returns>Video processing feature capabilities DTO.</returns>
        public unsafe VideoProcessingFeatureCapsDto GetSupportedVideoProcessingCapabilities()
        {
            ThrowIfDisposed();
            var caps = CreateVideoProcessingCaps();
            var result = IGCL.ctlGetSupportedVideoProcessingCapabilities((_ctl_device_adapter_handle_t*)_adapter, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get video processing capabilities");
            return VideoProcessingFeatureCapsDto.FromNative(caps);
        }

        /// <summary>
        /// Get a video processing feature using a DTO request.
        /// </summary>
        /// <param name="featureGetSet">Video processing feature DTO.</param>
        /// <returns>Updated video processing feature DTO.</returns>
        public VideoProcessingFeatureGetSetDto GetVideoProcessingFeature(VideoProcessingFeatureGetSetDto featureGetSet)
        {
            ThrowIfDisposed();
            var request = featureGetSet;
            request.Set = false;
            return ExecuteGetSetVideoProcessingFeature(request);
        }

        /// <summary>
        /// Set a video processing feature using a DTO request.
        /// </summary>
        /// <param name="featureGetSet">Video processing feature DTO.</param>
        public void SetVideoProcessingFeature(VideoProcessingFeatureGetSetDto featureGetSet)
        {
            ThrowIfDisposed();
            var request = featureGetSet;
            request.Set = true;
            ValidateSetVideoProcessingFeatureRequest(request);
            _ = ExecuteGetSetVideoProcessingFeature(request);
        }

        private unsafe VideoProcessingFeatureGetSetDto ExecuteGetSetVideoProcessingFeature(VideoProcessingFeatureGetSetDto request)
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

            var result2 = IGCL.ctlGetSetVideoProcessingFeature((_ctl_device_adapter_handle_t*)_adapter, &native);
            if (result2 != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result2, $"Failed to get/set video processing feature {native.FeatureType}");
            return VideoProcessingFeatureGetSetDto.FromNative(native);
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
        /// Create a DTO request for a video-processing get operation.
        /// </summary>
        /// <param name="featureType">Feature to query.</param>
        /// <param name="valueType">Expected value type.</param>
        /// <returns>Initialized get request DTO.</returns>
        public static VideoProcessingFeatureGetSetDto CreateVideoProcessingFeatureGetRequest(ctl_video_processing_feature_t featureType, ctl_property_value_type_t valueType)
        {
            return new VideoProcessingFeatureGetSetDto
            {
                FeatureType = featureType,
                ValueType = valueType,
                Set = false
            };
        }

        /// <summary>
        /// Create a DTO request for a video-processing set operation.
        /// </summary>
        /// <param name="featureType">Feature to set.</param>
        /// <param name="valueType">Value type for the feature.</param>
        /// <param name="value">Feature value payload.</param>
        /// <param name="applicationName">Optional application name.</param>
        /// <param name="customValue">Optional custom payload bytes.</param>
        /// <returns>Initialized set request DTO.</returns>
        public static VideoProcessingFeatureGetSetDto CreateVideoProcessingFeatureSetRequest(
            ctl_video_processing_feature_t featureType,
            ctl_property_value_type_t valueType,
            PropertyDto value,
            string? applicationName = null,
            List<byte>? customValue = null)
        {
            return new VideoProcessingFeatureGetSetDto
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
        /// Validate a video-processing set request to catch accidental default DTO usage.
        /// </summary>
        /// <param name="request">Request DTO.</param>
        /// <exception cref="ArgumentException">Thrown when request appears to be an accidental default payload.</exception>
        public static void ValidateSetVideoProcessingFeatureRequest(VideoProcessingFeatureGetSetDto request)
        {
            if (request.Equals(default))
            {
                throw new ArgumentException(
                    "Video processing set request cannot be default. Use CreateVideoProcessingFeatureSetRequest and provide explicit feature/value fields.",
                    nameof(request));
            }
        }

        /// <summary>
        /// Compare video processing feature capabilities while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left capabilities struct.</param>
        /// <param name="right">Right capabilities struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreVideoProcessingFeatureCapsEqual(ctl_video_processing_feature_caps_t left, ctl_video_processing_feature_caps_t right)
        {
            return VideoProcessingFeatureCapsDto.FromNative(left).Equals(VideoProcessingFeatureCapsDto.FromNative(right));
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
        public VideoProcessingFeatureGetSetDto() {}
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
        public List<byte> CustomValue = new();
        /// <summary>
        /// Reserved fields.
        /// </summary>
        public List<uint> ReservedFields = new();

        /// <summary>
        /// Compare video processing feature get/set args while ignoring pointer and reserved fields.
        /// </summary>
        /// <param name="other">Other args instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(VideoProcessingFeatureGetSetDto other)
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
        public override bool Equals(object? obj) => obj is VideoProcessingFeatureGetSetDto other && Equals(other);

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
        /// <returns>Video processing feature DTO.</returns>
        public static VideoProcessingFeatureGetSetDto FromNative(ctl_video_processing_feature_getset_t native)
        {
            return new VideoProcessingFeatureGetSetDto
            {
                Size = native.Size,
                Version = native.Version,
                FeatureType = native.FeatureType,
                ApplicationName = ReadAsciiString(native.ApplicationName, native.ApplicationNameLength),
                Set = IGCLMediaDtoBool.ToBool(native.bSet),
                ValueType = native.ValueType,
                Value = PropertyDto.FromNative(native.Value),
                CustomValue = ReadCustomValue(native.pCustomValue, native.CustomValueSize) ?? new List<byte>(),
                ReservedFields = ReadReservedFields(native.ReservedFields)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct (pointers are null; pin at call site).
        /// </summary>
        /// <returns>Video processing feature get/set struct.</returns>
        public ctl_video_processing_feature_getset_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_video_processing_feature_getset_t);

            var native = new ctl_video_processing_feature_getset_t
            {
                Size = size,
                Version = Version,
                FeatureType = FeatureType,
                ApplicationName = null,
                ApplicationNameLength = string.IsNullOrEmpty(ApplicationName) ? (sbyte)0 : (sbyte)Math.Min(ApplicationName.Length, sbyte.MaxValue),
                bSet = IGCLMediaDtoBool.ToByte(Set),
                ValueType = ValueType,
                Value = Value.ToNative(),
                CustomValueSize = CustomValue == null ? 0 : CustomValue.Count,
                pCustomValue = null
            };
            WriteReservedFields(ReservedFields, ref native.ReservedFields);
            return native;
        }

        private static unsafe string ReadAsciiString(sbyte* pValue, sbyte length)
        {
            if (pValue == null || length <= 0)
                return string.Empty;

            return new string(pValue, 0, length, System.Text.Encoding.ASCII);
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

        private static unsafe List<uint> ReadReservedFields(ctl_video_processing_feature_getset_t._ReservedFields_e__FixedBuffer buffer)
        {
            var values = new List<uint>(ReservedFieldCount);
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedFieldCount; i++)
                values.Add(pValues[i]);
            return values;
        }

        private static unsafe void WriteReservedFields(List<uint>? values, ref ctl_video_processing_feature_getset_t._ReservedFields_e__FixedBuffer buffer)
        {
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedFieldCount; i++)
                pValues[i] = 0;

            if (values == null || values.Count == 0)
                return;

            var count = Math.Min(values.Count, ReservedFieldCount);
            for (var i = 0; i < count; i++)
                pValues[i] = values[i];
        }
    }

    /// <summary>
    /// DTO for video processing feature capabilities.
    /// </summary>
    public unsafe struct VideoProcessingFeatureCapsDto : IEquatable<VideoProcessingFeatureCapsDto>
    {
        public VideoProcessingFeatureCapsDto() {}
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
        /// Number of supported features.
        /// </summary>
        public uint NumSupportedFeatures;
        /// <summary>
        /// Reserved fields.
        /// </summary>
        public List<uint> ReservedFields = new();

        public bool Equals(VideoProcessingFeatureCapsDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   NumSupportedFeatures == other.NumSupportedFeatures;
        }

        public override bool Equals(object? obj) => obj is VideoProcessingFeatureCapsDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(NumSupportedFeatures);
            return hash.ToHashCode();
        }

        public static VideoProcessingFeatureCapsDto FromNative(ctl_video_processing_feature_caps_t native)
        {
            return new VideoProcessingFeatureCapsDto
            {
                Size = native.Size,
                Version = native.Version,
                NumSupportedFeatures = native.NumSupportedFeatures,
                ReservedFields = ReadReservedFields(native.ReservedFields)
            };
        }

        public unsafe ctl_video_processing_feature_caps_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_video_processing_feature_caps_t);
            var native = new ctl_video_processing_feature_caps_t
            {
                Size = size,
                Version = Version,
                NumSupportedFeatures = NumSupportedFeatures
            };
            WriteReservedFields(ReservedFields, ref native.ReservedFields);
            return native;
        }

        private static unsafe List<uint> ReadReservedFields(ctl_video_processing_feature_caps_t._ReservedFields_e__FixedBuffer buffer)
        {
            var values = new List<uint>(ReservedFieldCount);
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedFieldCount; i++)
                values.Add(pValues[i]);
            return values;
        }

        private static unsafe void WriteReservedFields(List<uint>? values, ref ctl_video_processing_feature_caps_t._ReservedFields_e__FixedBuffer buffer)
        {
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedFieldCount; i++)
                pValues[i] = 0;

            if (values == null || values.Count == 0)
                return;

            var count = Math.Min(values.Count, ReservedFieldCount);
            for (var i = 0; i < count; i++)
                pValues[i] = values[i];
        }
    }
}

