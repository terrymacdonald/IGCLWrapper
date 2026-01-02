namespace IGCLWrapper
{
    /// <include file='ctl_oc_telemetry_item_t.xml' path='doc/member[@name="ctl_oc_telemetry_item_t"]/*' />
    public partial struct ctl_oc_telemetry_item_t
    {
        /// <include file='ctl_oc_telemetry_item_t.xml' path='doc/member[@name="ctl_oc_telemetry_item_t.bSupported"]/*' />
        [NativeTypeName("bool")]
        public byte bSupported;

        /// <include file='ctl_oc_telemetry_item_t.xml' path='doc/member[@name="ctl_oc_telemetry_item_t.units"]/*' />
        public ctl_units_t units;

        /// <include file='ctl_oc_telemetry_item_t.xml' path='doc/member[@name="ctl_oc_telemetry_item_t.type"]/*' />
        public ctl_data_type_t type;

        /// <include file='ctl_oc_telemetry_item_t.xml' path='doc/member[@name="ctl_oc_telemetry_item_t.value"]/*' />
        public ctl_data_value_t value;
    }
}
