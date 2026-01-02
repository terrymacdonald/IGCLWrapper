using System;
using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="ctl_video_processing_feature_caps_t" /> struct.</summary>
    public static unsafe partial class ctl_video_processing_feature_caps_tTests
    {
        /// <summary>Validates that the <see cref="ctl_video_processing_feature_caps_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(ctl_video_processing_feature_caps_t), Marshal.SizeOf<ctl_video_processing_feature_caps_t>());
        }

        /// <summary>Validates that the <see cref="ctl_video_processing_feature_caps_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutSequentialTest()
        {
            Assert.True(typeof(ctl_video_processing_feature_caps_t).IsLayoutSequential);
        }

        /// <summary>Validates that the <see cref="ctl_video_processing_feature_caps_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.Equal(88, sizeof(ctl_video_processing_feature_caps_t));
            }
            else
            {
                Assert.Equal(80, sizeof(ctl_video_processing_feature_caps_t));
            }
        }
    }
}
