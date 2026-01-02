namespace IGCLWrapper
{
    /// <include file='ctl_mem_state_t.xml' path='doc/member[@name="ctl_mem_state_t"]/*' />
    public partial struct ctl_mem_state_t
    {
        /// <include file='ctl_mem_state_t.xml' path='doc/member[@name="ctl_mem_state_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_mem_state_t.xml' path='doc/member[@name="ctl_mem_state_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_mem_state_t.xml' path='doc/member[@name="ctl_mem_state_t.free"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong free;

        /// <include file='ctl_mem_state_t.xml' path='doc/member[@name="ctl_mem_state_t.size"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong size;
    }
}
