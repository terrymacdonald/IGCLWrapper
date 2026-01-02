namespace IGCLWrapper
{
    /// <include file='ctl_oc_control_info_t.xml' path='doc/member[@name="ctl_oc_control_info_t"]/*' />
    public partial struct ctl_oc_control_info_t
    {
        /// <include file='ctl_oc_control_info_t.xml' path='doc/member[@name="ctl_oc_control_info_t.bSupported"]/*' />
        [NativeTypeName("bool")]
        public byte bSupported;

        /// <include file='ctl_oc_control_info_t.xml' path='doc/member[@name="ctl_oc_control_info_t.bRelative"]/*' />
        [NativeTypeName("bool")]
        public byte bRelative;

        /// <include file='ctl_oc_control_info_t.xml' path='doc/member[@name="ctl_oc_control_info_t.bReference"]/*' />
        [NativeTypeName("bool")]
        public byte bReference;

        /// <include file='ctl_oc_control_info_t.xml' path='doc/member[@name="ctl_oc_control_info_t.units"]/*' />
        public ctl_units_t units;

        /// <include file='ctl_oc_control_info_t.xml' path='doc/member[@name="ctl_oc_control_info_t.min"]/*' />
        public double min;

        /// <include file='ctl_oc_control_info_t.xml' path='doc/member[@name="ctl_oc_control_info_t.max"]/*' />
        public double max;

        /// <include file='ctl_oc_control_info_t.xml' path='doc/member[@name="ctl_oc_control_info_t.step"]/*' />
        public double step;

        /// <include file='ctl_oc_control_info_t.xml' path='doc/member[@name="ctl_oc_control_info_t.Default"]/*' />
        public double Default;

        /// <include file='ctl_oc_control_info_t.xml' path='doc/member[@name="ctl_oc_control_info_t.reference"]/*' />
        public double reference;
    }
}
