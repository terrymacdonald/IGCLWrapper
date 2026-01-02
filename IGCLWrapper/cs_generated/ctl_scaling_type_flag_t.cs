namespace IGCLWrapper
{
    /// <include file='ctl_scaling_type_flag_t.xml' path='doc/member[@name="ctl_scaling_type_flag_t"]/*' />
    public enum ctl_scaling_type_flag_t
    {
        /// <include file='ctl_scaling_type_flag_t.xml' path='doc/member[@name="ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_IDENTITY"]/*' />
        CTL_SCALING_TYPE_FLAG_IDENTITY = (1 << 0),

        /// <include file='ctl_scaling_type_flag_t.xml' path='doc/member[@name="ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_CENTERED"]/*' />
        CTL_SCALING_TYPE_FLAG_CENTERED = (1 << 1),

        /// <include file='ctl_scaling_type_flag_t.xml' path='doc/member[@name="ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_STRETCHED"]/*' />
        CTL_SCALING_TYPE_FLAG_STRETCHED = (1 << 2),

        /// <include file='ctl_scaling_type_flag_t.xml' path='doc/member[@name="ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_ASPECT_RATIO_CENTERED_MAX"]/*' />
        CTL_SCALING_TYPE_FLAG_ASPECT_RATIO_CENTERED_MAX = (1 << 3),

        /// <include file='ctl_scaling_type_flag_t.xml' path='doc/member[@name="ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_CUSTOM"]/*' />
        CTL_SCALING_TYPE_FLAG_CUSTOM = (1 << 4),

        /// <include file='ctl_scaling_type_flag_t.xml' path='doc/member[@name="ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_MAX"]/*' />
        CTL_SCALING_TYPE_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
