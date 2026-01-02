namespace IGCLWrapper
{
    /// <include file='ctl_lace_config_t.xml' path='doc/member[@name="ctl_lace_config_t"]/*' />
    public partial struct ctl_lace_config_t
    {
        /// <include file='ctl_lace_config_t.xml' path='doc/member[@name="ctl_lace_config_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_lace_config_t.xml' path='doc/member[@name="ctl_lace_config_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_lace_config_t.xml' path='doc/member[@name="ctl_lace_config_t.Enabled"]/*' />
        [NativeTypeName("bool")]
        public byte Enabled;

        /// <include file='ctl_lace_config_t.xml' path='doc/member[@name="ctl_lace_config_t.OpTypeGet"]/*' />
        [NativeTypeName("ctl_get_operation_flags_t")]
        public uint OpTypeGet;

        /// <include file='ctl_lace_config_t.xml' path='doc/member[@name="ctl_lace_config_t.OpTypeSet"]/*' />
        public ctl_set_operation_t OpTypeSet;

        /// <include file='ctl_lace_config_t.xml' path='doc/member[@name="ctl_lace_config_t.Trigger"]/*' />
        [NativeTypeName("ctl_lace_trigger_flags_t")]
        public uint Trigger;

        /// <include file='ctl_lace_config_t.xml' path='doc/member[@name="ctl_lace_config_t.LaceConfig"]/*' />
        public ctl_lace_aggr_config_t LaceConfig;
    }
}
