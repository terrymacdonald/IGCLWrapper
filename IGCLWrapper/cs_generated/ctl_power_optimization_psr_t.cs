namespace IGCLWrapper
{
    /// <include file='ctl_power_optimization_psr_t.xml' path='doc/member[@name="ctl_power_optimization_psr_t"]/*' />
    public partial struct ctl_power_optimization_psr_t
    {
        /// <include file='ctl_power_optimization_psr_t.xml' path='doc/member[@name="ctl_power_optimization_psr_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_power_optimization_psr_t.xml' path='doc/member[@name="ctl_power_optimization_psr_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_power_optimization_psr_t.xml' path='doc/member[@name="ctl_power_optimization_psr_t.PSRVersion"]/*' />
        [NativeTypeName("uint8_t")]
        public byte PSRVersion;

        /// <include file='ctl_power_optimization_psr_t.xml' path='doc/member[@name="ctl_power_optimization_psr_t.FullFetchUpdate"]/*' />
        [NativeTypeName("bool")]
        public byte FullFetchUpdate;
    }
}
