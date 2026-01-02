namespace IGCLWrapper
{
    /// <include file='ctl_power_optimization_settings_t.xml' path='doc/member[@name="ctl_power_optimization_settings_t"]/*' />
    public partial struct ctl_power_optimization_settings_t
    {
        /// <include file='ctl_power_optimization_settings_t.xml' path='doc/member[@name="ctl_power_optimization_settings_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_power_optimization_settings_t.xml' path='doc/member[@name="ctl_power_optimization_settings_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_power_optimization_settings_t.xml' path='doc/member[@name="ctl_power_optimization_settings_t.PowerOptimizationPlan"]/*' />
        public ctl_power_optimization_plan_t PowerOptimizationPlan;

        /// <include file='ctl_power_optimization_settings_t.xml' path='doc/member[@name="ctl_power_optimization_settings_t.PowerOptimizationFeature"]/*' />
        [NativeTypeName("ctl_power_optimization_flags_t")]
        public uint PowerOptimizationFeature;

        /// <include file='ctl_power_optimization_settings_t.xml' path='doc/member[@name="ctl_power_optimization_settings_t.Enable"]/*' />
        [NativeTypeName("bool")]
        public byte Enable;

        /// <include file='ctl_power_optimization_settings_t.xml' path='doc/member[@name="ctl_power_optimization_settings_t.FeatureSpecificData"]/*' />
        public ctl_power_optimization_feature_specific_info_t FeatureSpecificData;

        /// <include file='ctl_power_optimization_settings_t.xml' path='doc/member[@name="ctl_power_optimization_settings_t.PowerSource"]/*' />
        public ctl_power_source_t PowerSource;
    }
}
