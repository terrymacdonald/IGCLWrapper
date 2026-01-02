namespace IGCLWrapper
{
    /// <include file='ctl_lace_trigger_flag_t.xml' path='doc/member[@name="ctl_lace_trigger_flag_t"]/*' />
    public enum ctl_lace_trigger_flag_t
    {
        /// <include file='ctl_lace_trigger_flag_t.xml' path='doc/member[@name="ctl_lace_trigger_flag_t.CTL_LACE_TRIGGER_FLAG_AMBIENT_LIGHT"]/*' />
        CTL_LACE_TRIGGER_FLAG_AMBIENT_LIGHT = (1 << 0),

        /// <include file='ctl_lace_trigger_flag_t.xml' path='doc/member[@name="ctl_lace_trigger_flag_t.CTL_LACE_TRIGGER_FLAG_FIXED_AGGRESSIVENESS"]/*' />
        CTL_LACE_TRIGGER_FLAG_FIXED_AGGRESSIVENESS = (1 << 1),

        /// <include file='ctl_lace_trigger_flag_t.xml' path='doc/member[@name="ctl_lace_trigger_flag_t.CTL_LACE_TRIGGER_FLAG_MAX"]/*' />
        CTL_LACE_TRIGGER_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
