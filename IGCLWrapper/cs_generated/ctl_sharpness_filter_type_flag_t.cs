namespace IGCLWrapper
{
    /// <include file='ctl_sharpness_filter_type_flag_t.xml' path='doc/member[@name="ctl_sharpness_filter_type_flag_t"]/*' />
    public enum ctl_sharpness_filter_type_flag_t
    {
        /// <include file='ctl_sharpness_filter_type_flag_t.xml' path='doc/member[@name="ctl_sharpness_filter_type_flag_t.CTL_SHARPNESS_FILTER_TYPE_FLAG_NON_ADAPTIVE"]/*' />
        CTL_SHARPNESS_FILTER_TYPE_FLAG_NON_ADAPTIVE = (1 << 0),

        /// <include file='ctl_sharpness_filter_type_flag_t.xml' path='doc/member[@name="ctl_sharpness_filter_type_flag_t.CTL_SHARPNESS_FILTER_TYPE_FLAG_ADAPTIVE"]/*' />
        CTL_SHARPNESS_FILTER_TYPE_FLAG_ADAPTIVE = (1 << 1),

        /// <include file='ctl_sharpness_filter_type_flag_t.xml' path='doc/member[@name="ctl_sharpness_filter_type_flag_t.CTL_SHARPNESS_FILTER_TYPE_FLAG_MAX"]/*' />
        CTL_SHARPNESS_FILTER_TYPE_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
