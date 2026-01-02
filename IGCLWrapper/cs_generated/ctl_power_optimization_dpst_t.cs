namespace IGCLWrapper
{
    /// <include file='ctl_power_optimization_dpst_t.xml' path='doc/member[@name="ctl_power_optimization_dpst_t"]/*' />
    public partial struct ctl_power_optimization_dpst_t
    {
        /// <include file='ctl_power_optimization_dpst_t.xml' path='doc/member[@name="ctl_power_optimization_dpst_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_power_optimization_dpst_t.xml' path='doc/member[@name="ctl_power_optimization_dpst_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_power_optimization_dpst_t.xml' path='doc/member[@name="ctl_power_optimization_dpst_t.MinLevel"]/*' />
        [NativeTypeName("uint8_t")]
        public byte MinLevel;

        /// <include file='ctl_power_optimization_dpst_t.xml' path='doc/member[@name="ctl_power_optimization_dpst_t.MaxLevel"]/*' />
        [NativeTypeName("uint8_t")]
        public byte MaxLevel;

        /// <include file='ctl_power_optimization_dpst_t.xml' path='doc/member[@name="ctl_power_optimization_dpst_t.Level"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Level;

        /// <include file='ctl_power_optimization_dpst_t.xml' path='doc/member[@name="ctl_power_optimization_dpst_t.SupportedFeatures"]/*' />
        [NativeTypeName("ctl_power_optimization_dpst_flags_t")]
        public uint SupportedFeatures;

        /// <include file='ctl_power_optimization_dpst_t.xml' path='doc/member[@name="ctl_power_optimization_dpst_t.EnabledFeatures"]/*' />
        [NativeTypeName("ctl_power_optimization_dpst_flags_t")]
        public uint EnabledFeatures;
    }
}
