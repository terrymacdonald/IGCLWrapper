using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="_ctl_property_info_t" /> struct.</summary>
    public static unsafe partial class _ctl_property_info_tTests
    {
        /// <summary>Validates that the <see cref="_ctl_property_info_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(_ctl_property_info_t), Marshal.SizeOf<_ctl_property_info_t>());
        }

        /// <summary>Validates that the <see cref="_ctl_property_info_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutExplicitTest()
        {
            Assert.True(typeof(_ctl_property_info_t).IsExplicitLayout);
        }

        /// <summary>Validates that the <see cref="_ctl_property_info_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            Assert.Equal(24, sizeof(_ctl_property_info_t));
        }
    }
}
