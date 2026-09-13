namespace IGCLWrapper
{
    /// <include file='ctl_dev_prop_properties_t.xml' path='doc/member[@name="ctl_dev_prop_properties_t"]/*' />
    public partial struct ctl_dev_prop_properties_t
    {
        /// <include file='ctl_dev_prop_properties_t.xml' path='doc/member[@name="ctl_dev_prop_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_dev_prop_properties_t.xml' path='doc/member[@name="ctl_dev_prop_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_dev_prop_properties_t.xml' path='doc/member[@name="ctl_dev_prop_properties_t.isWorkstation"]/*' />
        [NativeTypeName("bool")]
        public byte isWorkstation;
    }
}
