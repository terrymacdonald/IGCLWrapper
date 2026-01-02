using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="ctl_child_display_target_mode_t" /> struct.</summary>
    public static unsafe partial class ctl_child_display_target_mode_tTests
    {
        /// <summary>Validates that the <see cref="ctl_child_display_target_mode_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(ctl_child_display_target_mode_t), Marshal.SizeOf<ctl_child_display_target_mode_t>());
        }

        /// <summary>Validates that the <see cref="ctl_child_display_target_mode_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutSequentialTest()
        {
            Assert.True(typeof(ctl_child_display_target_mode_t).IsLayoutSequential);
        }

        /// <summary>Validates that the <see cref="ctl_child_display_target_mode_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            Assert.Equal(28, sizeof(ctl_child_display_target_mode_t));
        }
    }
}
