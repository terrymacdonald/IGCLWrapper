namespace IGCLWrapper
{
    /// <include file='ctl_display_feature_reset_flag_t.xml' path='doc/member[@name="ctl_display_feature_reset_flag_t"]/*' />
    public enum ctl_display_feature_reset_flag_t
    {
        /// <include file='ctl_display_feature_reset_flag_t.xml' path='doc/member[@name="ctl_display_feature_reset_flag_t.CTL_DISPLAY_FEATURE_RESET_FLAG_SCALING"]/*' />
        CTL_DISPLAY_FEATURE_RESET_FLAG_SCALING = (1 << 0),

        /// <include file='ctl_display_feature_reset_flag_t.xml' path='doc/member[@name="ctl_display_feature_reset_flag_t.CTL_DISPLAY_FEATURE_RESET_FLAG_WIRE_FORMAT"]/*' />
        CTL_DISPLAY_FEATURE_RESET_FLAG_WIRE_FORMAT = (1 << 1),

        /// <include file='ctl_display_feature_reset_flag_t.xml' path='doc/member[@name="ctl_display_feature_reset_flag_t.CTL_DISPLAY_FEATURE_RESET_FLAG_LACE"]/*' />
        CTL_DISPLAY_FEATURE_RESET_FLAG_LACE = (1 << 2),

        /// <include file='ctl_display_feature_reset_flag_t.xml' path='doc/member[@name="ctl_display_feature_reset_flag_t.CTL_DISPLAY_FEATURE_RESET_FLAG_COLOR"]/*' />
        CTL_DISPLAY_FEATURE_RESET_FLAG_COLOR = (1 << 3),

        /// <include file='ctl_display_feature_reset_flag_t.xml' path='doc/member[@name="ctl_display_feature_reset_flag_t.CTL_DISPLAY_FEATURE_RESET_FLAG_VRR"]/*' />
        CTL_DISPLAY_FEATURE_RESET_FLAG_VRR = (1 << 4),

        /// <include file='ctl_display_feature_reset_flag_t.xml' path='doc/member[@name="ctl_display_feature_reset_flag_t.CTL_DISPLAY_FEATURE_RESET_FLAG_QUANTIZATION_RANGE"]/*' />
        CTL_DISPLAY_FEATURE_RESET_FLAG_QUANTIZATION_RANGE = (1 << 5),

        /// <include file='ctl_display_feature_reset_flag_t.xml' path='doc/member[@name="ctl_display_feature_reset_flag_t.CTL_DISPLAY_FEATURE_RESET_FLAG_CONTENT_TYPE"]/*' />
        CTL_DISPLAY_FEATURE_RESET_FLAG_CONTENT_TYPE = (1 << 6),

        /// <include file='ctl_display_feature_reset_flag_t.xml' path='doc/member[@name="ctl_display_feature_reset_flag_t.CTL_DISPLAY_FEATURE_RESET_FLAG_PSR"]/*' />
        CTL_DISPLAY_FEATURE_RESET_FLAG_PSR = (1 << 7),

        /// <include file='ctl_display_feature_reset_flag_t.xml' path='doc/member[@name="ctl_display_feature_reset_flag_t.CTL_DISPLAY_FEATURE_RESET_FLAG_AUDIO"]/*' />
        CTL_DISPLAY_FEATURE_RESET_FLAG_AUDIO = (1 << 8),

        /// <include file='ctl_display_feature_reset_flag_t.xml' path='doc/member[@name="ctl_display_feature_reset_flag_t.CTL_DISPLAY_FEATURE_RESET_FLAG_ALL"]/*' />
        CTL_DISPLAY_FEATURE_RESET_FLAG_ALL = (1 << 30),

        /// <include file='ctl_display_feature_reset_flag_t.xml' path='doc/member[@name="ctl_display_feature_reset_flag_t.CTL_DISPLAY_FEATURE_RESET_FLAG_MAX"]/*' />
        CTL_DISPLAY_FEATURE_RESET_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
