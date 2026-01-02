namespace IGCLWrapper
{
    /// <include file='ctl_power_optimization_flag_t.xml' path='doc/member[@name="ctl_power_optimization_flag_t"]/*' />
    public enum ctl_power_optimization_flag_t
    {
        /// <include file='ctl_power_optimization_flag_t.xml' path='doc/member[@name="ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_FBC"]/*' />
        CTL_POWER_OPTIMIZATION_FLAG_FBC = (1 << 0),

        /// <include file='ctl_power_optimization_flag_t.xml' path='doc/member[@name="ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_PSR"]/*' />
        CTL_POWER_OPTIMIZATION_FLAG_PSR = (1 << 1),

        /// <include file='ctl_power_optimization_flag_t.xml' path='doc/member[@name="ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_DPST"]/*' />
        CTL_POWER_OPTIMIZATION_FLAG_DPST = (1 << 2),

        /// <include file='ctl_power_optimization_flag_t.xml' path='doc/member[@name="ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LRR"]/*' />
        CTL_POWER_OPTIMIZATION_FLAG_LRR = (1 << 3),

        /// <include file='ctl_power_optimization_flag_t.xml' path='doc/member[@name="ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LACE"]/*' />
        CTL_POWER_OPTIMIZATION_FLAG_LACE = (1 << 4),

        /// <include file='ctl_power_optimization_flag_t.xml' path='doc/member[@name="ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_MAX"]/*' />
        CTL_POWER_OPTIMIZATION_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
