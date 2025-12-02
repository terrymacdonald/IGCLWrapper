using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="_ctl_power_energy_counter_t" /> struct.</summary>
    public static unsafe partial class _ctl_power_energy_counter_tTests
    {
        /// <summary>Validates that the <see cref="_ctl_power_energy_counter_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(_ctl_power_energy_counter_t), Marshal.SizeOf<_ctl_power_energy_counter_t>());
        }

        /// <summary>Validates that the <see cref="_ctl_power_energy_counter_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutSequentialTest()
        {
            Assert.True(typeof(_ctl_power_energy_counter_t).IsLayoutSequential);
        }

        /// <summary>Validates that the <see cref="_ctl_power_energy_counter_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            Assert.Equal(24, sizeof(_ctl_power_energy_counter_t));
        }
    }
}
