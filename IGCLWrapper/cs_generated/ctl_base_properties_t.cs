namespace IGCLWrapper
{
    /// <include file='ctl_base_properties_t.xml' path='doc/member[@name="ctl_base_properties_t"]/*' />
    public partial struct ctl_base_properties_t
    {
        /// <include file='ctl_base_properties_t.xml' path='doc/member[@name="ctl_base_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_base_properties_t.xml' path='doc/member[@name="ctl_base_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;
    }
}
