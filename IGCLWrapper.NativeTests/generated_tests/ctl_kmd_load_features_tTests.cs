using System;
using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="ctl_kmd_load_features_t" /> struct.</summary>
    public static unsafe partial class ctl_kmd_load_features_tTests
    {
        /// <summary>Validates that the <see cref="ctl_kmd_load_features_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(ctl_kmd_load_features_t), Marshal.SizeOf<ctl_kmd_load_features_t>());
        }

        /// <summary>Validates that the <see cref="ctl_kmd_load_features_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutSequentialTest()
        {
            Assert.True(typeof(ctl_kmd_load_features_t).IsLayoutSequential);
        }

        /// <summary>Validates that the <see cref="ctl_kmd_load_features_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.Equal(80, sizeof(ctl_kmd_load_features_t));
            }
            else
            {
                Assert.Equal(72, sizeof(ctl_kmd_load_features_t));
            }
        }
    }
}
