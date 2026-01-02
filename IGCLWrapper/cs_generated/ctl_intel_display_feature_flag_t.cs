namespace IGCLWrapper
{
    /// <include file='ctl_intel_display_feature_flag_t.xml' path='doc/member[@name="ctl_intel_display_feature_flag_t"]/*' />
    public enum ctl_intel_display_feature_flag_t
    {
        /// <include file='ctl_intel_display_feature_flag_t.xml' path='doc/member[@name="ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DPST"]/*' />
        CTL_INTEL_DISPLAY_FEATURE_FLAG_DPST = (1 << 0),

        /// <include file='ctl_intel_display_feature_flag_t.xml' path='doc/member[@name="ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_LACE"]/*' />
        CTL_INTEL_DISPLAY_FEATURE_FLAG_LACE = (1 << 1),

        /// <include file='ctl_intel_display_feature_flag_t.xml' path='doc/member[@name="ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DRRS"]/*' />
        CTL_INTEL_DISPLAY_FEATURE_FLAG_DRRS = (1 << 2),

        /// <include file='ctl_intel_display_feature_flag_t.xml' path='doc/member[@name="ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_ARC_ADAPTIVE_SYNC_CERTIFIED"]/*' />
        CTL_INTEL_DISPLAY_FEATURE_FLAG_ARC_ADAPTIVE_SYNC_CERTIFIED = (1 << 3),

        /// <include file='ctl_intel_display_feature_flag_t.xml' path='doc/member[@name="ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_MAX"]/*' />
        CTL_INTEL_DISPLAY_FEATURE_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
