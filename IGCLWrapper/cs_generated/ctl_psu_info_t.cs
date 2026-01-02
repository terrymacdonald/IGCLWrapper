namespace IGCLWrapper
{
    /// <include file='ctl_psu_info_t.xml' path='doc/member[@name="ctl_psu_info_t"]/*' />
    public partial struct ctl_psu_info_t
    {
        /// <include file='ctl_psu_info_t.xml' path='doc/member[@name="ctl_psu_info_t.bSupported"]/*' />
        [NativeTypeName("bool")]
        public byte bSupported;

        /// <include file='ctl_psu_info_t.xml' path='doc/member[@name="ctl_psu_info_t.psuType"]/*' />
        public ctl_psu_type_t psuType;

        /// <include file='ctl_psu_info_t.xml' path='doc/member[@name="ctl_psu_info_t.energyCounter"]/*' />
        public ctl_oc_telemetry_item_t energyCounter;

        /// <include file='ctl_psu_info_t.xml' path='doc/member[@name="ctl_psu_info_t.voltage"]/*' />
        public ctl_oc_telemetry_item_t voltage;
    }
}
