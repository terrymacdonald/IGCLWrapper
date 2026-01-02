namespace IGCLWrapper
{
    /// <include file='ctl_freq_throttle_time_t.xml' path='doc/member[@name="ctl_freq_throttle_time_t"]/*' />
    public partial struct ctl_freq_throttle_time_t
    {
        /// <include file='ctl_freq_throttle_time_t.xml' path='doc/member[@name="ctl_freq_throttle_time_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_freq_throttle_time_t.xml' path='doc/member[@name="ctl_freq_throttle_time_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_freq_throttle_time_t.xml' path='doc/member[@name="ctl_freq_throttle_time_t.throttleTime"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong throttleTime;

        /// <include file='ctl_freq_throttle_time_t.xml' path='doc/member[@name="ctl_freq_throttle_time_t.timestamp"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong timestamp;
    }
}
