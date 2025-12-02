using System;
using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="_ctl_genlock_topology_t" /> struct.</summary>
    public static unsafe partial class _ctl_genlock_topology_tTests
    {
        /// <summary>Validates that the <see cref="_ctl_genlock_topology_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(_ctl_genlock_topology_t), Marshal.SizeOf<_ctl_genlock_topology_t>());
        }

        /// <summary>Validates that the <see cref="_ctl_genlock_topology_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutSequentialTest()
        {
            Assert.True(typeof(_ctl_genlock_topology_t).IsLayoutSequential);
        }

        /// <summary>Validates that the <see cref="_ctl_genlock_topology_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.Equal(88, sizeof(_ctl_genlock_topology_t));
            }
            else
            {
                Assert.Equal(80, sizeof(_ctl_genlock_topology_t));
            }
        }
    }
}
