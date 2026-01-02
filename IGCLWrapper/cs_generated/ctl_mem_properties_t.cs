namespace IGCLWrapper
{
    /// <include file='ctl_mem_properties_t.xml' path='doc/member[@name="ctl_mem_properties_t"]/*' />
    public partial struct ctl_mem_properties_t
    {
        /// <include file='ctl_mem_properties_t.xml' path='doc/member[@name="ctl_mem_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_mem_properties_t.xml' path='doc/member[@name="ctl_mem_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_mem_properties_t.xml' path='doc/member[@name="ctl_mem_properties_t.type"]/*' />
        public ctl_mem_type_t type;

        /// <include file='ctl_mem_properties_t.xml' path='doc/member[@name="ctl_mem_properties_t.location"]/*' />
        public ctl_mem_loc_t location;

        /// <include file='ctl_mem_properties_t.xml' path='doc/member[@name="ctl_mem_properties_t.physicalSize"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong physicalSize;

        /// <include file='ctl_mem_properties_t.xml' path='doc/member[@name="ctl_mem_properties_t.busWidth"]/*' />
        [NativeTypeName("int32_t")]
        public int busWidth;

        /// <include file='ctl_mem_properties_t.xml' path='doc/member[@name="ctl_mem_properties_t.numChannels"]/*' />
        [NativeTypeName("int32_t")]
        public int numChannels;
    }
}
