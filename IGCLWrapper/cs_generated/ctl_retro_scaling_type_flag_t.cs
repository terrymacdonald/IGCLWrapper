namespace IGCLWrapper
{
    /// <include file='ctl_retro_scaling_type_flag_t.xml' path='doc/member[@name="ctl_retro_scaling_type_flag_t"]/*' />
    public enum ctl_retro_scaling_type_flag_t
    {
        /// <include file='ctl_retro_scaling_type_flag_t.xml' path='doc/member[@name="ctl_retro_scaling_type_flag_t.CTL_RETRO_SCALING_TYPE_FLAG_INTEGER"]/*' />
        CTL_RETRO_SCALING_TYPE_FLAG_INTEGER = (1 << 0),

        /// <include file='ctl_retro_scaling_type_flag_t.xml' path='doc/member[@name="ctl_retro_scaling_type_flag_t.CTL_RETRO_SCALING_TYPE_FLAG_NEAREST_NEIGHBOUR"]/*' />
        CTL_RETRO_SCALING_TYPE_FLAG_NEAREST_NEIGHBOUR = (1 << 1),

        /// <include file='ctl_retro_scaling_type_flag_t.xml' path='doc/member[@name="ctl_retro_scaling_type_flag_t.CTL_RETRO_SCALING_TYPE_FLAG_MAX"]/*' />
        CTL_RETRO_SCALING_TYPE_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
