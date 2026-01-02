namespace IGCLWrapper
{
    /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t"]/*' />
    public partial struct ctl_oc_properties_t
    {
        /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t.bSupported"]/*' />
        [NativeTypeName("bool")]
        public byte bSupported;

        /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t.gpuFrequencyOffset"]/*' />
        public ctl_oc_control_info_t gpuFrequencyOffset;

        /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t.gpuVoltageOffset"]/*' />
        public ctl_oc_control_info_t gpuVoltageOffset;

        /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t.vramFrequencyOffset"]/*' />
        public ctl_oc_control_info_t vramFrequencyOffset;

        /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t.vramVoltageOffset"]/*' />
        public ctl_oc_control_info_t vramVoltageOffset;

        /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t.powerLimit"]/*' />
        public ctl_oc_control_info_t powerLimit;

        /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t.temperatureLimit"]/*' />
        public ctl_oc_control_info_t temperatureLimit;

        /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t.vramMemSpeedLimit"]/*' />
        public ctl_oc_control_info_t vramMemSpeedLimit;

        /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t.gpuVFCurveVoltageLimit"]/*' />
        public ctl_oc_control_info_t gpuVFCurveVoltageLimit;

        /// <include file='ctl_oc_properties_t.xml' path='doc/member[@name="ctl_oc_properties_t.gpuVFCurveFrequencyLimit"]/*' />
        public ctl_oc_control_info_t gpuVFCurveFrequencyLimit;
    }
}
