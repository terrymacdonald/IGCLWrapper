using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
    public class IGCLEccHelperTests
    {
        [SkippableFact]
        public void EccGetters_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.GetEccHelper(adapter);
                var props = FacadeTestUtils.InvokeOrSkip(() => helper.EccGetProperties(), "ECC unsupported");
                if (props.Size == 0) throw new SkipException("ECC unsupported (empty props).");
                FacadeTestUtils.InvokeOrSkip(() => helper.EccGetState(), "ECC state unsupported");
            }
        }

        [Fact]
        public void EccStateDescDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_ecc_state_desc_t
            {
                Size = 24u,
                Version = 1,
                currentEccState = ctl_ecc_state_t.CTL_ECC_STATE_ECC_ENABLED_STATE,
                pendingEccState = ctl_ecc_state_t.CTL_ECC_STATE_ECC_DISABLED_STATE
            };

            var dto = EccStateDescDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.currentEccState, dto.CurrentEccState);
            Assert.Equal(native.pendingEccState, dto.PendingEccState);

            var roundtrip = dto.ToNative();
            Assert.Equal(native.currentEccState, roundtrip.currentEccState);
            Assert.Equal(native.pendingEccState, roundtrip.pendingEccState);
        }
    }
}
