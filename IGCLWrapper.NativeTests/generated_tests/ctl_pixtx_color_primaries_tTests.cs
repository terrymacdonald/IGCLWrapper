using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="ctl_pixtx_color_primaries_t" /> struct.</summary>
    public static unsafe partial class ctl_pixtx_color_primaries_tTests
    {
        /// <summary>Validates that the <see cref="ctl_pixtx_color_primaries_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(ctl_pixtx_color_primaries_t), Marshal.SizeOf<ctl_pixtx_color_primaries_t>());
        }

        /// <summary>Validates that the <see cref="ctl_pixtx_color_primaries_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutSequentialTest()
        {
            Assert.True(typeof(ctl_pixtx_color_primaries_t).IsLayoutSequential);
        }

        /// <summary>Validates that the <see cref="ctl_pixtx_color_primaries_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            Assert.Equal(72, sizeof(ctl_pixtx_color_primaries_t));
        }
    }
}
