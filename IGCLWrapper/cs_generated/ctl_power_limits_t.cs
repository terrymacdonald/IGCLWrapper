namespace IGCLWrapper
{
    /// <include file='ctl_power_limits_t.xml' path='doc/member[@name="ctl_power_limits_t"]/*' />
    public partial struct ctl_power_limits_t
    {
        /// <include file='ctl_power_limits_t.xml' path='doc/member[@name="ctl_power_limits_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_power_limits_t.xml' path='doc/member[@name="ctl_power_limits_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_power_limits_t.xml' path='doc/member[@name="ctl_power_limits_t.sustainedPowerLimit"]/*' />
        public ctl_power_sustained_limit_t sustainedPowerLimit;

        /// <include file='ctl_power_limits_t.xml' path='doc/member[@name="ctl_power_limits_t.burstPowerLimit"]/*' />
        public ctl_power_burst_limit_t burstPowerLimit;

        /// <include file='ctl_power_limits_t.xml' path='doc/member[@name="ctl_power_limits_t.peakPowerLimits"]/*' />
        public ctl_power_peak_limit_t peakPowerLimits;
    }
}
