using System;
using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="_ctl_pixtx_1dlut_config_t" /> struct.</summary>
    public static unsafe partial class _ctl_pixtx_1dlut_config_tTests
    {
        /// <summary>Validates that the <see cref="_ctl_pixtx_1dlut_config_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(_ctl_pixtx_1dlut_config_t), Marshal.SizeOf<_ctl_pixtx_1dlut_config_t>());
        }

        /// <summary>Validates that the <see cref="_ctl_pixtx_1dlut_config_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutSequentialTest()
        {
            Assert.True(typeof(_ctl_pixtx_1dlut_config_t).IsLayoutSequential);
        }

        /// <summary>Validates that the <see cref="_ctl_pixtx_1dlut_config_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.Equal(40, sizeof(_ctl_pixtx_1dlut_config_t));
            }
            else
            {
                Assert.Equal(28, sizeof(_ctl_pixtx_1dlut_config_t));
            }
        }
    }
}
