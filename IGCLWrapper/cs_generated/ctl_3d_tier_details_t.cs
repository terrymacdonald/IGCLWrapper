using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_3d_tier_details_t.xml' path='doc/member[@name="ctl_3d_tier_details_t"]/*' />
    public partial struct ctl_3d_tier_details_t
    {
        /// <include file='ctl_3d_tier_details_t.xml' path='doc/member[@name="ctl_3d_tier_details_t.TierType"]/*' />
        public ctl_3d_tier_type_flag_t TierType;

        /// <include file='ctl_3d_tier_details_t.xml' path='doc/member[@name="ctl_3d_tier_details_t.TierProfile"]/*' />
        public ctl_3d_tier_profile_flag_t TierProfile;

        /// <include file='ctl_3d_tier_details_t.xml' path='doc/member[@name="ctl_3d_tier_details_t.Reserved"]/*' />
        [NativeTypeName("uint64_t[4]")]
        public _Reserved_e__FixedBuffer Reserved;

        /// <include file='_Reserved_e__FixedBuffer.xml' path='doc/member[@name="_Reserved_e__FixedBuffer"]/*' />
        [InlineArray(4)]
        public partial struct _Reserved_e__FixedBuffer
        {
            public ulong e0;
        }
    }
}
