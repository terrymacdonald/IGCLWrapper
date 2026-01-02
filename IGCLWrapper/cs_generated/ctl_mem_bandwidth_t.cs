namespace IGCLWrapper
{
    /// <include file='ctl_mem_bandwidth_t.xml' path='doc/member[@name="ctl_mem_bandwidth_t"]/*' />
    public partial struct ctl_mem_bandwidth_t
    {
        /// <include file='ctl_mem_bandwidth_t.xml' path='doc/member[@name="ctl_mem_bandwidth_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_mem_bandwidth_t.xml' path='doc/member[@name="ctl_mem_bandwidth_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_mem_bandwidth_t.xml' path='doc/member[@name="ctl_mem_bandwidth_t.maxBandwidth"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong maxBandwidth;

        /// <include file='ctl_mem_bandwidth_t.xml' path='doc/member[@name="ctl_mem_bandwidth_t.timestamp"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong timestamp;

        /// <include file='ctl_mem_bandwidth_t.xml' path='doc/member[@name="ctl_mem_bandwidth_t.readCounter"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong readCounter;

        /// <include file='ctl_mem_bandwidth_t.xml' path='doc/member[@name="ctl_mem_bandwidth_t.writeCounter"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong writeCounter;
    }
}
