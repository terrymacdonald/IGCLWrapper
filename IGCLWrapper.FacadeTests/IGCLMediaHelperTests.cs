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
            Assert.True(fromNative.CustomValue == null);
            Assert.NotNull(fromNative.ReservedFields);
            Assert.Equal(16, fromNative.ReservedFields!.Count);
        }
    }
}
