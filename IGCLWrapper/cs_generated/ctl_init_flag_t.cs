namespace IGCLWrapper
{
    /// <include file='ctl_init_flag_t.xml' path='doc/member[@name="ctl_init_flag_t"]/*' />
    public enum ctl_init_flag_t
    {
        /// <include file='ctl_init_flag_t.xml' path='doc/member[@name="ctl_init_flag_t.CTL_INIT_FLAG_USE_LEVEL_ZERO"]/*' />
        CTL_INIT_FLAG_USE_LEVEL_ZERO = (1 << 0),

        /// <include file='ctl_init_flag_t.xml' path='doc/member[@name="ctl_init_flag_t.CTL_INIT_FLAG_MAX"]/*' />
        CTL_INIT_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
