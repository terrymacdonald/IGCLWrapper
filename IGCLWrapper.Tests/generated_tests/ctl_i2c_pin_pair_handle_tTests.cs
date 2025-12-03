using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="ctl_i2c_pin_pair_handle_t" /> struct.</summary>
    public static unsafe partial class ctl_i2c_pin_pair_handle_tTests
    {
        /// <summary>Validates that the <see cref="ctl_i2c_pin_pair_handle_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(ctl_i2c_pin_pair_handle_t), Marshal.SizeOf<ctl_i2c_pin_pair_handle_t>());
        }

        /// <summary>Validates that the <see cref="ctl_i2c_pin_pair_handle_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutSequentialTest()
        {
            Assert.True(typeof(ctl_i2c_pin_pair_handle_t).IsLayoutSequential);
        }

        /// <summary>Validates that the <see cref="ctl_i2c_pin_pair_handle_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            Assert.Equal(1, sizeof(ctl_i2c_pin_pair_handle_t));
        }
    }
}
