namespace IGCLWrapper
{
    /// <include file='ctl_freq_range_t.xml' path='doc/member[@name="ctl_freq_range_t"]/*' />
    public partial struct ctl_freq_range_t
    {
        /// <include file='ctl_freq_range_t.xml' path='doc/member[@name="ctl_freq_range_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_freq_range_t.xml' path='doc/member[@name="ctl_freq_range_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_freq_range_t.xml' path='doc/member[@name="ctl_freq_range_t.min"]/*' />
        public double min;

        /// <include file='ctl_freq_range_t.xml' path='doc/member[@name="ctl_freq_range_t.max"]/*' />
        public double max;
    }
}
