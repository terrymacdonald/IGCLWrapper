using System;
using System.Runtime.InteropServices;
using Xunit;

namespace IGCLWrapper.UnitTests
{
    /// <summary>Provides validation of the <see cref="_ctl_os_display_encoder_identifier_t" /> struct.</summary>
    public static unsafe partial class _ctl_os_display_encoder_identifier_tTests
    {
        /// <summary>Validates that the <see cref="_ctl_os_display_encoder_identifier_t" /> struct is blittable.</summary>
        [Fact]
        public static void IsBlittableTest()
        {
            Assert.Equal(sizeof(_ctl_os_display_encoder_identifier_t), Marshal.SizeOf<_ctl_os_display_encoder_identifier_t>());
        }

        /// <summary>Validates that the <see cref="_ctl_os_display_encoder_identifier_t" /> struct has the right <see cref="LayoutKind" />.</summary>
        [Fact]
        public static void IsLayoutExplicitTest()
        {
            Assert.True(typeof(_ctl_os_display_encoder_identifier_t).IsExplicitLayout);
        }

        /// <summary>Validates that the <see cref="_ctl_os_display_encoder_identifier_t" /> struct has the correct size.</summary>
        [Fact]
        public static void SizeOfTest()
        {
            if (Environment.Is64BitProcess)
            {
                Assert.Equal(16, sizeof(_ctl_os_display_encoder_identifier_t));
            }
            else
            {
                Assert.Equal(8, sizeof(_ctl_os_display_encoder_identifier_t));
            }
        }
    }
}
