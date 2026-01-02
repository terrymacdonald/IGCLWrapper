namespace IGCLWrapper
{
    /// <include file='ctl_3d_tier_profile_flag_t.xml' path='doc/member[@name="ctl_3d_tier_profile_flag_t"]/*' />
    public enum ctl_3d_tier_profile_flag_t
    {
        /// <include file='ctl_3d_tier_profile_flag_t.xml' path='doc/member[@name="ctl_3d_tier_profile_flag_t.CTL_3D_TIER_PROFILE_FLAG_TIER_1"]/*' />
        CTL_3D_TIER_PROFILE_FLAG_TIER_1 = (1 << 0),

        /// <include file='ctl_3d_tier_profile_flag_t.xml' path='doc/member[@name="ctl_3d_tier_profile_flag_t.CTL_3D_TIER_PROFILE_FLAG_TIER_2"]/*' />
        CTL_3D_TIER_PROFILE_FLAG_TIER_2 = (1 << 1),

        /// <include file='ctl_3d_tier_profile_flag_t.xml' path='doc/member[@name="ctl_3d_tier_profile_flag_t.CTL_3D_TIER_PROFILE_FLAG_TIER_RECOMMENDED"]/*' />
        CTL_3D_TIER_PROFILE_FLAG_TIER_RECOMMENDED = (1 << 30),

        /// <include file='ctl_3d_tier_profile_flag_t.xml' path='doc/member[@name="ctl_3d_tier_profile_flag_t.CTL_3D_TIER_PROFILE_FLAG_MAX"]/*' />
        CTL_3D_TIER_PROFILE_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
