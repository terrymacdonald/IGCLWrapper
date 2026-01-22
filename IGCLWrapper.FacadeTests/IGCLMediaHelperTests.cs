using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
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
            Assert.Equal(16, dto.ReservedFields!.Length);
            Assert.True(dto.Equals(dto));
            _ = dto.GetHashCode();
        }
    }
}
