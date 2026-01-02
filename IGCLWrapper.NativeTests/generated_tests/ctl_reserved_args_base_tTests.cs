using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="ctl_reserved_args_base_t" /> struct.</summary>
    public static unsafe partial class ctl_reserved_args_base_tTests
    {
        /// <summary>Validates that the <see cref="ctl_reserved_args_base_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(ctl_reserved_args_base_t), Marshal.SizeOf<ctl_reserved_args_base_t>());
        }

        /// <summary>Validates that the <see cref="ctl_reserved_args_base_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutSequentialTest()
        {
            Assert.True(typeof(ctl_reserved_args_base_t).IsLayoutSequential);
        }

        /// <summary>Validates that the <see cref="ctl_reserved_args_base_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            Assert.Equal(16, sizeof(ctl_reserved_args_base_t));
        }
    }
}
