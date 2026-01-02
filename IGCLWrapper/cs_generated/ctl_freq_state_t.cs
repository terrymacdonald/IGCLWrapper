namespace IGCLWrapper
{
    /// <include file='ctl_freq_state_t.xml' path='doc/member[@name="ctl_freq_state_t"]/*' />
    public partial struct ctl_freq_state_t
    {
        /// <include file='ctl_freq_state_t.xml' path='doc/member[@name="ctl_freq_state_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_freq_state_t.xml' path='doc/member[@name="ctl_freq_state_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_freq_state_t.xml' path='doc/member[@name="ctl_freq_state_t.currentVoltage"]/*' />
        public double currentVoltage;

        /// <include file='ctl_freq_state_t.xml' path='doc/member[@name="ctl_freq_state_t.request"]/*' />
        public double request;

        /// <include file='ctl_freq_state_t.xml' path='doc/member[@name="ctl_freq_state_t.tdp"]/*' />
        public double tdp;

        /// <include file='ctl_freq_state_t.xml' path='doc/member[@name="ctl_freq_state_t.efficient"]/*' />
        public double efficient;

        /// <include file='ctl_freq_state_t.xml' path='doc/member[@name="ctl_freq_state_t.actual"]/*' />
        public double actual;

        /// <include file='ctl_freq_state_t.xml' path='doc/member[@name="ctl_freq_state_t.throttleReasons"]/*' />
        [NativeTypeName("ctl_freq_throttle_reason_flags_t")]
        public uint throttleReasons;
    }
}
