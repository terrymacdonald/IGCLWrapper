using System;
using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
    public class IGCLMediaHelperTests
    {
        [SkippableFact]
        public void MediaGetCapabilities_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetMediaHelper(adapter);
                var caps = helper.GetSupportedVideoProcessingCapabilities();
                Assert.True(caps.Size > 0);
            }
        }

        [Fact]
        public void VideoProcessingFeatureDto_ShouldBeSafeToConsume()
        {
            var native = IGCLMediaHelper.CreateVideoProcessingFeatureGetSet();
            var dto = VideoProcessingFeatureGetSetDto.FromNative(native);
            Assert.NotNull(dto.ReservedFields);
            Assert.Equal(16, dto.ReservedFields!.Count);
            Assert.True(dto.Equals(dto));
            _ = dto.GetHashCode();
        }

        [Fact]
        public unsafe void VideoProcessingFeatureDto_ToNative_ShouldUseManagedFieldsAndNullPointers()
        {
            var dto = new VideoProcessingFeatureGetSetDto
            {
                FeatureType = ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_FILM_MODE_DETECTION,
                ApplicationName = "FacadeTests",
                Set = true,
                ValueType = ctl_property_value_type_t.CTL_PROPERTY_VALUE_TYPE_INT32,
                Value = new PropertyDto
                {
                    IntType = new PropertyIntDto { Enable = true, Value = 7 }
                },
                CustomValue = new System.Collections.Generic.List<byte> { 8, 9, 10 },
                ReservedFields = new System.Collections.Generic.List<uint> { 11, 12, 13 }
            };

            var native = dto.ToNative();

            Assert.Equal((sbyte)Math.Min(dto.ApplicationName!.Length, sbyte.MaxValue), native.ApplicationNameLength);
            Assert.Equal(dto.CustomValue!.Count, native.CustomValueSize);
            Assert.True(native.ApplicationName == null);
            Assert.True(native.pCustomValue == null);
            Assert.Equal((byte)1, native.bSet);

            var fromNative = VideoProcessingFeatureGetSetDto.FromNative(native);
            Assert.Equal(dto.FeatureType, fromNative.FeatureType);
            Assert.Equal(dto.ValueType, fromNative.ValueType);
            Assert.Empty(fromNative.CustomValue);
            Assert.NotNull(fromNative.ReservedFields);
            Assert.Equal(16, fromNative.ReservedFields!.Count);
        }

        [Fact]
        public void CreateVideoProcessingFeatureSetRequest_ShouldInitializeUsefulDefaults()
        {
            var value = new PropertyDto
            {
                BoolType = new PropertyBooleanDto { Enable = true }
            };

            var dto = IGCLMediaHelper.CreateVideoProcessingFeatureSetRequest(
                ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_FILM_MODE_DETECTION,
                ctl_property_value_type_t.CTL_PROPERTY_VALUE_TYPE_BOOL,
                value,
                "Player.exe");

            Assert.True(dto.Set);
            Assert.Equal(ctl_video_processing_feature_t.CTL_VIDEO_PROCESSING_FEATURE_FILM_MODE_DETECTION, dto.FeatureType);
            Assert.Equal(ctl_property_value_type_t.CTL_PROPERTY_VALUE_TYPE_BOOL, dto.ValueType);
            Assert.Equal("Player.exe", dto.ApplicationName);
            Assert.True(dto.Value.BoolType.Enable);
        }

        [Fact]
        public void ValidateSetVideoProcessingFeatureRequest_DefaultDto_ShouldThrow()
        {
            var ex = Assert.Throws<ArgumentException>(() => IGCLMediaHelper.ValidateSetVideoProcessingFeatureRequest(default));
            Assert.Contains("default", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void VideoProcessingFeatureCapsDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_video_processing_feature_caps_t
            {
                Size = 48u,
                Version = 2,
                NumSupportedFeatures = 3u
            };

            var dto = VideoProcessingFeatureCapsDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.NumSupportedFeatures, dto.NumSupportedFeatures);
            Assert.NotNull(dto.ReservedFields);
            Assert.Equal(16, dto.ReservedFields!.Count);

            var roundtrip = dto.ToNative();
            Assert.Equal(native.NumSupportedFeatures, roundtrip.NumSupportedFeatures);
        }
    }
}
