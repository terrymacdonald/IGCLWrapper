namespace IGCLWrapper
{
    /// <include file='ctl_power_properties_t.xml' path='doc/member[@name="ctl_power_properties_t"]/*' />
    public partial struct ctl_power_properties_t
    {
        /// <include file='ctl_power_properties_t.xml' path='doc/member[@name="ctl_power_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_power_properties_t.xml' path='doc/member[@name="ctl_power_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_power_properties_t.xml' path='doc/member[@name="ctl_power_properties_t.canControl"]/*' />
        [NativeTypeName("bool")]
        public byte canControl;

        /// <include file='ctl_power_properties_t.xml' path='doc/member[@name="ctl_power_properties_t.defaultLimit"]/*' />
        [NativeTypeName("int32_t")]
        public int defaultLimit;

        /// <include file='ctl_power_properties_t.xml' path='doc/member[@name="ctl_power_properties_t.minLimit"]/*' />
        [NativeTypeName("int32_t")]
        public int minLimit;

        /// <include file='ctl_power_properties_t.xml' path='doc/member[@name="ctl_power_properties_t.maxLimit"]/*' />
        [NativeTypeName("int32_t")]
        public int maxLimit;
    }
}
