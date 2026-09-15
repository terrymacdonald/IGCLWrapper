using IGCLWrapper;

namespace IGCLWrapper.Tests
{
    public class MediaDtoTests
    {
        [Fact]
        public void StandardColorCorrection_DisabledNativeValues_AreNormalised()
        {
            var result = StandardColorCorrectionDto.FromNative(new ctl_video_processing_standard_color_correction_t
            {
                standard_color_correction_enable = 0,
                brightness = 315.9961f,
                contrast = 0.000000000000000000000000000000000000000001012f,
                hue = 359.64355f,
                saturation = 0.000000000000000000000000000000000000000001012f
            });

            Assert.False(result.Enable);
            Assert.Equal(0f, result.Brightness);
            Assert.Equal(0f, result.Contrast);
            Assert.Equal(0f, result.Hue);
            Assert.Equal(0f, result.Saturation);
        }

        [Fact]
        public void StandardColorCorrection_DisabledValues_AreEqualAndShareHashCode()
        {
            var first = new StandardColorCorrectionDto
            {
                Enable = false,
                Brightness = 315.9961f,
                Contrast = 1.012e-42f,
                Hue = 359.64355f,
                Saturation = 1.012e-42f
            };
            var second = new StandardColorCorrectionDto
            {
                Enable = false,
                Brightness = 9.12e-42f,
                Contrast = 0f,
                Hue = -4.1728218e-23f,
                Saturation = 4.5916e-41f
            };

            Assert.Equal(first, second);
            Assert.Equal(first.GetHashCode(), second.GetHashCode());
        }

        [Fact]
        public void StandardColorCorrection_EnabledValues_RemainExact()
        {
            var expected = new StandardColorCorrectionDto
            {
                Enable = true,
                Brightness = 0f,
                Contrast = 1f,
                Hue = 0f,
                Saturation = 1f
            };
            var different = expected;
            different.Hue = 1f;

            Assert.NotEqual(expected, different);
            Assert.Equal(expected, StandardColorCorrectionDto.FromNative(new ctl_video_processing_standard_color_correction_t
            {
                standard_color_correction_enable = 1,
                brightness = 0f,
                contrast = 1f,
                hue = 0f,
                saturation = 1f
            }));
        }
    }
}
