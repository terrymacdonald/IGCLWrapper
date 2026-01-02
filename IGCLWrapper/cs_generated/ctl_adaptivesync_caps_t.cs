namespace IGCLWrapper
{
    /// <include file='ctl_adaptivesync_caps_t.xml' path='doc/member[@name="ctl_adaptivesync_caps_t"]/*' />
    public partial struct ctl_adaptivesync_caps_t
    {
        /// <include file='ctl_adaptivesync_caps_t.xml' path='doc/member[@name="ctl_adaptivesync_caps_t.AdaptiveBalanceSupported"]/*' />
        [NativeTypeName("bool")]
        public byte AdaptiveBalanceSupported;

        /// <include file='ctl_adaptivesync_caps_t.xml' path='doc/member[@name="ctl_adaptivesync_caps_t.AdaptiveBalanceStrengthCaps"]/*' />
        public ctl_property_info_float_t AdaptiveBalanceStrengthCaps;
    }
}
