using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="ctl_pixtx_config_t" /> struct.</summary>
    public static unsafe partial class ctl_pixtx_config_tTests
    {
        /// <summary>Validates that the <see cref="ctl_pixtx_config_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(ctl_pixtx_config_t), Marshal.SizeOf<ctl_pixtx_config_t>());
        }

        /// <summary>Validates that the <see cref="ctl_pixtx_config_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutExplicitTest()
        {
            Assert.True(typeof(ctl_pixtx_config_t).IsExplicitLayout);
        }

        /// <summary>Validates that the <see cref="ctl_pixtx_config_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            Assert.Equal(128, sizeof(ctl_pixtx_config_t));
        }
    }
}
