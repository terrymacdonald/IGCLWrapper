namespace IGCLWrapper
{
    /// <include file='ctl_engine_properties_t.xml' path='doc/member[@name="ctl_engine_properties_t"]/*' />
    public partial struct ctl_engine_properties_t
    {
        /// <include file='ctl_engine_properties_t.xml' path='doc/member[@name="ctl_engine_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_engine_properties_t.xml' path='doc/member[@name="ctl_engine_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_engine_properties_t.xml' path='doc/member[@name="ctl_engine_properties_t.type"]/*' />
        public ctl_engine_group_t type;
    }
}
