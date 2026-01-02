namespace IGCLWrapper
{
    /// <include file='ctl_freq_properties_t.xml' path='doc/member[@name="ctl_freq_properties_t"]/*' />
    public partial struct ctl_freq_properties_t
    {
        /// <include file='ctl_freq_properties_t.xml' path='doc/member[@name="ctl_freq_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_freq_properties_t.xml' path='doc/member[@name="ctl_freq_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_freq_properties_t.xml' path='doc/member[@name="ctl_freq_properties_t.type"]/*' />
        public ctl_freq_domain_t type;

        /// <include file='ctl_freq_properties_t.xml' path='doc/member[@name="ctl_freq_properties_t.canControl"]/*' />
        [NativeTypeName("bool")]
        public byte canControl;

        /// <include file='ctl_freq_properties_t.xml' path='doc/member[@name="ctl_freq_properties_t.min"]/*' />
        public double min;

        /// <include file='ctl_freq_properties_t.xml' path='doc/member[@name="ctl_freq_properties_t.max"]/*' />
        public double max;
    }
}
