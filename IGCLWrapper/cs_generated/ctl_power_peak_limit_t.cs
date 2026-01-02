namespace IGCLWrapper
{
    /// <include file='ctl_power_peak_limit_t.xml' path='doc/member[@name="ctl_power_peak_limit_t"]/*' />
    public partial struct ctl_power_peak_limit_t
    {
        /// <include file='ctl_power_peak_limit_t.xml' path='doc/member[@name="ctl_power_peak_limit_t.powerAC"]/*' />
        [NativeTypeName("int32_t")]
        public int powerAC;

        /// <include file='ctl_power_peak_limit_t.xml' path='doc/member[@name="ctl_power_peak_limit_t.powerDC"]/*' />
        [NativeTypeName("int32_t")]
        public int powerDC;
    }
}
