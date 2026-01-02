using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="ctl_video_processing_standard_color_correction_t" /> struct.</summary>
    public static unsafe partial class ctl_video_processing_standard_color_correction_tTests
    {
        /// <summary>Validates that the <see cref="ctl_video_processing_standard_color_correction_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(ctl_video_processing_standard_color_correction_t), Marshal.SizeOf<ctl_video_processing_standard_color_correction_t>());
        }

        /// <summary>Validates that the <see cref="ctl_video_processing_standard_color_correction_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutSequentialTest()
        {
            Assert.True(typeof(ctl_video_processing_standard_color_correction_t).IsLayoutSequential);
        }

        /// <summary>Validates that the <see cref="ctl_video_processing_standard_color_correction_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            Assert.Equal(88, sizeof(ctl_video_processing_standard_color_correction_t));
        }
    }
}
