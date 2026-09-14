using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <returns>Video processing feature capabilities DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe VideoProcessingFeatureCapsDto? GetSupportedVideoProcessingCapabilities()
        {
            ThrowIfDisposed();
            var caps = CreateVideoProcessingCaps();
            var result = IGCL.ctlGetSupportedVideoProcessingCapabilities((_ctl_device_adapter_handle_t*)_adapter, &caps);
            if (IsUnsupportedResult(result))
                return null;
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get video processing capabilities");

            if (caps.NumSupportedFeatures == 0)
                return VideoProcessingFeatureCapsDto.FromNative(caps);

            var nativeDetails = new ctl_video_processing_feature_details_t[caps.NumSupportedFeatures];
            for (var i = 0; i < nativeDetails.Length; i++)
                nativeDetails[i].Size = (uint)sizeof(ctl_video_processing_feature_details_t);

            fixed (ctl_video_processing_feature_details_t* pDetails = nativeDetails)
            {
                caps.pFeatureDetails = pDetails;
                result = IGCL.ctlGetSupportedVideoProcessingCapabilities((_ctl_device_adapter_handle_t*)_adapter, &caps);
            }
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to retrieve video processing feature details");

            var dto = VideoProcessingFeatureCapsDto.FromNative(caps);
            dto.Features = nativeDetails.Select(VideoProcessingFeatureDetailDto.FromNative).ToList();
            return dto;
        }

        /// <summary>
        /// Get a video processing feature using a DTO request.
        /// </summary>
        /// <param name="featureGetSet">Video processing feature DTO.</param>
        /// <returns>Updated video processing feature DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public VideoProcessingFeatureGetSetDto? GetVideoProcessingFeature(VideoProcessingFeatureGetSetDto featureGetSet)
        {
            ThrowIfDisposed();
            try
            {
                var request = featureGetSet;
                request.Set = false;
                return ExecuteGetSetVideoProcessingFeature(request);
            }
            catch (IGCLException ex) when (IsUnsupportedResult(ex.Result))
            {
                return null;
            }
        }

        /// <summary>
        /// Gets the adapter-wide standard colour-correction settings.
        /// </summary>
        /// <returns>The colour-correction settings, or <c>null</c> when unsupported.</returns>
        public unsafe StandardColorCorrectionDto? GetStandardColorCorrection()
        {
            ThrowIfDisposed();
            var request = CreateVideoProcessingFeatureGetSet();
            var nativeSettings = StandardColorCorrectionDto.CreateNative();
            request.FeatureType = ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_STANDARD_COLOR_CORRECTION;
            request.ValueType = ctl_property_value_type_t.CTL_PROPERTY_VALUE_TYPE_CUSTOM;
            request.CustomValueSize = sizeof(ctl_video_processing_standard_color_correction_t);
            request.pCustomValue = &nativeSettings;

            var result = IGCL.ctlGetSetVideoProcessingFeature((_ctl_device_adapter_handle_t*)_adapter, &request);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return StandardColorCorrectionDto.FromNative(nativeSettings);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, "Failed to get standard colour correction");
        }

        /// <summary>
        /// Sets the adapter-wide standard colour-correction settings.
        /// </summary>
        /// <param name="settings">The colour-correction settings to apply.</param>
        /// <returns><c>true</c> when applied; <c>false</c> when unsupported.</returns>
        public unsafe bool SetStandardColorCorrection(StandardColorCorrectionDto settings)
        {
            ThrowIfDisposed();
            var request = CreateVideoProcessingFeatureGetSet();
            var nativeSettings = settings.ToNative();
            request.FeatureType = ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_STANDARD_COLOR_CORRECTION;
            request.bSet = 1;
            request.ValueType = ctl_property_value_type_t.CTL_PROPERTY_VALUE_TYPE_CUSTOM;
            request.CustomValueSize = sizeof(ctl_video_processing_standard_color_correction_t);
            request.pCustomValue = &nativeSettings;

            var result = IGCL.ctlGetSetVideoProcessingFeature((_ctl_device_adapter_handle_t*)_adapter, &request);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return true;
            if (IsUnsupportedResult(result))
                return false;
            throw new IGCLException(result, "Failed to set standard colour correction");
        }

        /// <summary>
        /// Set a video processing feature using a DTO request.
        /// </summary>
        /// <param name="featureGetSet">Video processing feature DTO.</param>
        /// <returns><c>true</c> if the setting was applied successfully; <c>false</c> if the feature is not supported on this hardware or driver.</returns>
        public bool SetVideoProcessingFeature(VideoProcessingFeatureGetSetDto featureGetSet)
        {
            ThrowIfDisposed();
            try
            {
                var request = featureGetSet;
                request.Set = true;
                ValidateSetVideoProcessingFeatureRequest(request);
                _ = ExecuteGetSetVideoProcessingFeature(request);
                return true;
            }
            catch (IGCLException ex) when (IsUnsupportedResult(ex.Result))
            {
                return false;
            }
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
            var customValueSize = customValue?.Count ?? 0;
            if (customValueSize == 0 && request.ValueType == ctl_property_value_type_t.CTL_PROPERTY_VALUE_TYPE_CUSTOM)
                customValueSize = GetCustomValueSize(request.FeatureType);

            if (customValueSize > 0)
            {
                unsafe
                {
                    byte* pCustomValue = stackalloc byte[customValueSize];
                    for (var i = 0; i < customValueSize; i++)
                        pCustomValue[i] = customValue != null && i < customValue.Count ? customValue[i] : (byte)0;
                    native.pCustomValue = pCustomValue;
                    native.CustomValueSize = customValueSize;
                }
            }

            var result2 = IGCL.ctlGetSetVideoProcessingFeature((_ctl_device_adapter_handle_t*)_adapter, &native);
            if (result2 != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result2, $"Failed to get/set video processing feature {native.FeatureType}");
            return VideoProcessingFeatureGetSetDto.FromNative(native);
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

        private static unsafe int GetCustomValueSize(ctl_video_processing_feature_t featureType)
        {
            return featureType switch
            {
                ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_NOISE_REDUCTION => sizeof(ctl_video_processing_noise_reduction_t),
                ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_ADAPTIVE_CONTRAST_ENHANCEMENT => sizeof(ctl_video_processing_adaptive_contrast_enhancement_t),
                ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_SUPER_RESOLUTION => sizeof(ctl_video_processing_super_resolution_t),
                ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_STANDARD_COLOR_CORRECTION => sizeof(ctl_video_processing_standard_color_correction_t),
                ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_TOTAL_COLOR_CORRECTION => sizeof(ctl_video_processing_total_color_correction_t),
                _ => 0
            };
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
            if (FeatureType != other.FeatureType ||
                Set != other.Set ||
                ValueType != other.ValueType ||
                !string.Equals(ApplicationName, other.ApplicationName, StringComparison.Ordinal))
            {
                return false;
            }

            // IGCL documents Value as invalid for custom media features; only the custom payload is meaningful.
            if (ValueType == ctl_property_value_type_t.CTL_PROPERTY_VALUE_TYPE_CUSTOM)
                return AreCustomValuesEqual(FeatureType, CustomValue, other.CustomValue);

            return Value.Equals(other.Value) && AreByteListsEqual(CustomValue, other.CustomValue);
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

        private static unsafe bool AreCustomValuesEqual(ctl_video_processing_feature_t featureType, List<byte>? left, List<byte>? right)
        {
            if (left == null || right == null)
                return AreByteListsEqual(left, right);

            var leftBytes = left.ToArray();
            var rightBytes = right.ToArray();
            fixed (byte* pLeft = leftBytes)
            fixed (byte* pRight = rightBytes)
            {
                switch (featureType)
                {
                    case ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_NOISE_REDUCTION:
                        if (leftBytes.Length >= sizeof(ctl_video_processing_noise_reduction_t) && rightBytes.Length >= sizeof(ctl_video_processing_noise_reduction_t))
                        {
                            var a = (ctl_video_processing_noise_reduction_t*)pLeft;
                            var b = (ctl_video_processing_noise_reduction_t*)pRight;
                            return a->noise_reduction.Enable == b->noise_reduction.Enable && a->noise_reduction.Value == b->noise_reduction.Value && a->noise_reduction_auto_detect.Enable == b->noise_reduction_auto_detect.Enable;
                        }
                        break;
                    case ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_ADAPTIVE_CONTRAST_ENHANCEMENT:
                        if (leftBytes.Length >= sizeof(ctl_video_processing_adaptive_contrast_enhancement_t) && rightBytes.Length >= sizeof(ctl_video_processing_adaptive_contrast_enhancement_t))
                        {
                            var a = (ctl_video_processing_adaptive_contrast_enhancement_t*)pLeft;
                            var b = (ctl_video_processing_adaptive_contrast_enhancement_t*)pRight;
                            return a->adaptive_contrast_enhancement.Enable == b->adaptive_contrast_enhancement.Enable && a->adaptive_contrast_enhancement.Value == b->adaptive_contrast_enhancement.Value && a->adaptive_contrast_enhancement_coexistence.Enable == b->adaptive_contrast_enhancement_coexistence.Enable;
                        }
                        break;
                    case ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_SUPER_RESOLUTION:
                        if (leftBytes.Length >= sizeof(ctl_video_processing_super_resolution_t) && rightBytes.Length >= sizeof(ctl_video_processing_super_resolution_t))
                        {
                            var a = (ctl_video_processing_super_resolution_t*)pLeft;
                            var b = (ctl_video_processing_super_resolution_t*)pRight;
                            return a->super_resolution_flag == b->super_resolution_flag && a->super_resolution_max_in_enabled == b->super_resolution_max_in_enabled && a->super_resolution_max_in_width == b->super_resolution_max_in_width && a->super_resolution_max_in_height == b->super_resolution_max_in_height && a->super_resolution_reboot_reset == b->super_resolution_reboot_reset;
                        }
                        break;
                    case ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_TOTAL_COLOR_CORRECTION:
                        if (leftBytes.Length >= sizeof(ctl_video_processing_total_color_correction_t) && rightBytes.Length >= sizeof(ctl_video_processing_total_color_correction_t))
                        {
                            var a = (ctl_video_processing_total_color_correction_t*)pLeft;
                            var b = (ctl_video_processing_total_color_correction_t*)pRight;
                            return a->total_color_correction_enable == b->total_color_correction_enable && a->red == b->red && a->green == b->green && a->blue == b->blue && a->yellow == b->yellow && a->cyan == b->cyan && a->magenta == b->magenta;
                        }
                        break;
                }
            }
            return AreByteListsEqual(left, right);
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
        /// <summary>Details for the media features reported by the driver.</summary>
        public List<VideoProcessingFeatureDetailDto> Features = new();
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

    /// <summary>
    /// Adapter-wide hue, saturation, contrast, and brightness settings.
    /// </summary>
    public struct StandardColorCorrectionDto : IEquatable<StandardColorCorrectionDto>
    {
        private const float ComparisonTolerance = 0.0001f;
        /// <summary>Whether standard colour correction is enabled.</summary>
        public bool Enable;
        /// <summary>Brightness adjustment.</summary>
        public float Brightness;
        /// <summary>Contrast adjustment.</summary>
        public float Contrast;
        /// <summary>Hue adjustment.</summary>
        public float Hue;
        /// <summary>Saturation adjustment.</summary>
        public float Saturation;

        public bool Equals(StandardColorCorrectionDto other)
        {
            return Enable == other.Enable &&
                NearlyEqual(Brightness, other.Brightness) &&
                NearlyEqual(Contrast, other.Contrast) &&
                NearlyEqual(Hue, other.Hue) &&
                NearlyEqual(Saturation, other.Saturation);
        }

        private static bool NearlyEqual(float left, float right) => MathF.Abs(left - right) <= ComparisonTolerance;

        public override bool Equals(object? obj) => obj is StandardColorCorrectionDto other && Equals(other);

        public override int GetHashCode() => (Enable, Brightness, Contrast, Hue, Saturation).GetHashCode();

        public static unsafe StandardColorCorrectionDto FromNative(ctl_video_processing_standard_color_correction_t native)
        {
            return new StandardColorCorrectionDto
            {
                Enable = native.standard_color_correction_enable != 0,
                Brightness = native.brightness,
                Contrast = native.contrast,
                Hue = native.hue,
                Saturation = native.saturation
            };
        }

        internal static unsafe ctl_video_processing_standard_color_correction_t CreateNative()
        {
            return new ctl_video_processing_standard_color_correction_t
            {
                Size = (uint)sizeof(ctl_video_processing_standard_color_correction_t),
                Version = 0
            };
        }

        internal unsafe ctl_video_processing_standard_color_correction_t ToNative()
        {
            var native = CreateNative();
            native.standard_color_correction_enable = Enable ? (byte)1 : (byte)0;
            native.brightness = Brightness;
            native.contrast = Contrast;
            native.hue = Hue;
            native.saturation = Saturation;
            return native;
        }
    }

    /// <summary>Driver-reported media feature type and required value type.</summary>
    public struct VideoProcessingFeatureDetailDto
    {
        public ctl_video_processing_feature_t FeatureType;
        public ctl_property_value_type_t ValueType;
        public int CustomValueSize;

        internal static VideoProcessingFeatureDetailDto FromNative(ctl_video_processing_feature_details_t native)
        {
            return new VideoProcessingFeatureDetailDto
            {
                FeatureType = native.FeatureType,
                ValueType = native.ValueType,
                CustomValueSize = native.CustomValueSize
            };
        }
    }
}

