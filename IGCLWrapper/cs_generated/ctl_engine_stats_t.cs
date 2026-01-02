namespace IGCLWrapper
{
    /// <include file='ctl_engine_stats_t.xml' path='doc/member[@name="ctl_engine_stats_t"]/*' />
    public partial struct ctl_engine_stats_t
    {
        /// <include file='ctl_engine_stats_t.xml' path='doc/member[@name="ctl_engine_stats_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_engine_stats_t.xml' path='doc/member[@name="ctl_engine_stats_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_engine_stats_t.xml' path='doc/member[@name="ctl_engine_stats_t.activeTime"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong activeTime;

        /// <include file='ctl_engine_stats_t.xml' path='doc/member[@name="ctl_engine_stats_t.timestamp"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong timestamp;
    }
}
