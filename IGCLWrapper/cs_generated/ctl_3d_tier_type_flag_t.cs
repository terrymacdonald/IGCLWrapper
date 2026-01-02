namespace IGCLWrapper
{
    /// <include file='ctl_3d_tier_type_flag_t.xml' path='doc/member[@name="ctl_3d_tier_type_flag_t"]/*' />
    public enum ctl_3d_tier_type_flag_t
    {
        /// <include file='ctl_3d_tier_type_flag_t.xml' path='doc/member[@name="ctl_3d_tier_type_flag_t.CTL_3D_TIER_TYPE_FLAG_COMPATIBILITY"]/*' />
        CTL_3D_TIER_TYPE_FLAG_COMPATIBILITY = (1 << 0),

        /// <include file='ctl_3d_tier_type_flag_t.xml' path='doc/member[@name="ctl_3d_tier_type_flag_t.CTL_3D_TIER_TYPE_FLAG_PERFORMANCE"]/*' />
        CTL_3D_TIER_TYPE_FLAG_PERFORMANCE = (1 << 1),

        /// <include file='ctl_3d_tier_type_flag_t.xml' path='doc/member[@name="ctl_3d_tier_type_flag_t.CTL_3D_TIER_TYPE_FLAG_MAX"]/*' />
        CTL_3D_TIER_TYPE_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
