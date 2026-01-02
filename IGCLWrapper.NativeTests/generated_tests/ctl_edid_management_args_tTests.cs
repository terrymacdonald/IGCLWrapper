using System;
using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="ctl_edid_management_args_t" /> struct.</summary>
    public static unsafe partial class ctl_edid_management_args_tTests
    {
        /// <summary>Validates that the <see cref="ctl_edid_management_args_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(ctl_edid_management_args_t), Marshal.SizeOf<ctl_edid_management_args_t>());
        }

        /// <summary>Validates that the <see cref="ctl_edid_management_args_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutSequentialTest()
        {
            Assert.True(typeof(ctl_edid_management_args_t).IsLayoutSequential);
        }

        /// <summary>Validates that the <see cref="ctl_edid_management_args_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.Equal(40, sizeof(ctl_edid_management_args_t));
            }
            else
            {
                Assert.Equal(28, sizeof(ctl_edid_management_args_t));
            }
        }
    }
}
