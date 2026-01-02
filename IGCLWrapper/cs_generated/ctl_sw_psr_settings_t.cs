namespace IGCLWrapper
{
    /// <include file='ctl_sw_psr_settings_t.xml' path='doc/member[@name="ctl_sw_psr_settings_t"]/*' />
    public partial struct ctl_sw_psr_settings_t
    {
        /// <include file='ctl_sw_psr_settings_t.xml' path='doc/member[@name="ctl_sw_psr_settings_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_sw_psr_settings_t.xml' path='doc/member[@name="ctl_sw_psr_settings_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_sw_psr_settings_t.xml' path='doc/member[@name="ctl_sw_psr_settings_t.Set"]/*' />
        [NativeTypeName("bool")]
        public byte Set;

        /// <include file='ctl_sw_psr_settings_t.xml' path='doc/member[@name="ctl_sw_psr_settings_t.Supported"]/*' />
        [NativeTypeName("bool")]
        public byte Supported;

        /// <include file='ctl_sw_psr_settings_t.xml' path='doc/member[@name="ctl_sw_psr_settings_t.Enable"]/*' />
        [NativeTypeName("bool")]
        public byte Enable;
    }
}
