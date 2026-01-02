namespace IGCLWrapper
{
    /// <include file='ctl_power_optimization_lrr_t.xml' path='doc/member[@name="ctl_power_optimization_lrr_t"]/*' />
    public partial struct ctl_power_optimization_lrr_t
    {
        /// <include file='ctl_power_optimization_lrr_t.xml' path='doc/member[@name="ctl_power_optimization_lrr_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_power_optimization_lrr_t.xml' path='doc/member[@name="ctl_power_optimization_lrr_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_power_optimization_lrr_t.xml' path='doc/member[@name="ctl_power_optimization_lrr_t.SupportedLRRTypes"]/*' />
        [NativeTypeName("ctl_power_optimization_lrr_flags_t")]
        public uint SupportedLRRTypes;

        /// <include file='ctl_power_optimization_lrr_t.xml' path='doc/member[@name="ctl_power_optimization_lrr_t.CurrentLRRTypes"]/*' />
        [NativeTypeName("ctl_power_optimization_lrr_flags_t")]
        public uint CurrentLRRTypes;

        /// <include file='ctl_power_optimization_lrr_t.xml' path='doc/member[@name="ctl_power_optimization_lrr_t.bRequirePSRDisable"]/*' />
        [NativeTypeName("bool")]
        public byte bRequirePSRDisable;

        /// <include file='ctl_power_optimization_lrr_t.xml' path='doc/member[@name="ctl_power_optimization_lrr_t.LowRR"]/*' />
        [NativeTypeName("uint16_t")]
        public ushort LowRR;
    }
}
