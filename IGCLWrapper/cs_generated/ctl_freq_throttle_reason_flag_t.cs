namespace IGCLWrapper
{
    /// <include file='ctl_freq_throttle_reason_flag_t.xml' path='doc/member[@name="ctl_freq_throttle_reason_flag_t"]/*' />
    public enum ctl_freq_throttle_reason_flag_t
    {
        /// <include file='ctl_freq_throttle_reason_flag_t.xml' path='doc/member[@name="ctl_freq_throttle_reason_flag_t.CTL_FREQ_THROTTLE_REASON_FLAG_AVE_PWR_CAP"]/*' />
        CTL_FREQ_THROTTLE_REASON_FLAG_AVE_PWR_CAP = (1 << 0),

        /// <include file='ctl_freq_throttle_reason_flag_t.xml' path='doc/member[@name="ctl_freq_throttle_reason_flag_t.CTL_FREQ_THROTTLE_REASON_FLAG_BURST_PWR_CAP"]/*' />
        CTL_FREQ_THROTTLE_REASON_FLAG_BURST_PWR_CAP = (1 << 1),

        /// <include file='ctl_freq_throttle_reason_flag_t.xml' path='doc/member[@name="ctl_freq_throttle_reason_flag_t.CTL_FREQ_THROTTLE_REASON_FLAG_CURRENT_LIMIT"]/*' />
        CTL_FREQ_THROTTLE_REASON_FLAG_CURRENT_LIMIT = (1 << 2),

        /// <include file='ctl_freq_throttle_reason_flag_t.xml' path='doc/member[@name="ctl_freq_throttle_reason_flag_t.CTL_FREQ_THROTTLE_REASON_FLAG_THERMAL_LIMIT"]/*' />
        CTL_FREQ_THROTTLE_REASON_FLAG_THERMAL_LIMIT = (1 << 3),

        /// <include file='ctl_freq_throttle_reason_flag_t.xml' path='doc/member[@name="ctl_freq_throttle_reason_flag_t.CTL_FREQ_THROTTLE_REASON_FLAG_PSU_ALERT"]/*' />
        CTL_FREQ_THROTTLE_REASON_FLAG_PSU_ALERT = (1 << 4),

        /// <include file='ctl_freq_throttle_reason_flag_t.xml' path='doc/member[@name="ctl_freq_throttle_reason_flag_t.CTL_FREQ_THROTTLE_REASON_FLAG_SW_RANGE"]/*' />
        CTL_FREQ_THROTTLE_REASON_FLAG_SW_RANGE = (1 << 5),

        /// <include file='ctl_freq_throttle_reason_flag_t.xml' path='doc/member[@name="ctl_freq_throttle_reason_flag_t.CTL_FREQ_THROTTLE_REASON_FLAG_HW_RANGE"]/*' />
        CTL_FREQ_THROTTLE_REASON_FLAG_HW_RANGE = (1 << 6),

        /// <include file='ctl_freq_throttle_reason_flag_t.xml' path='doc/member[@name="ctl_freq_throttle_reason_flag_t.CTL_FREQ_THROTTLE_REASON_FLAG_MAX"]/*' />
        CTL_FREQ_THROTTLE_REASON_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
