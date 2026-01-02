namespace IGCLWrapper
{
    /// <include file='ctl_power_burst_limit_t.xml' path='doc/member[@name="ctl_power_burst_limit_t"]/*' />
    public partial struct ctl_power_burst_limit_t
    {
        /// <include file='ctl_power_burst_limit_t.xml' path='doc/member[@name="ctl_power_burst_limit_t.enabled"]/*' />
        [NativeTypeName("bool")]
        public byte enabled;

        /// <include file='ctl_power_burst_limit_t.xml' path='doc/member[@name="ctl_power_burst_limit_t.power"]/*' />
        [NativeTypeName("int32_t")]
        public int power;
    }
}
