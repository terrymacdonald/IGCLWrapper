namespace IGCLWrapper
{
    /// <include file='ctl_fan_config_t.xml' path='doc/member[@name="ctl_fan_config_t"]/*' />
    public partial struct ctl_fan_config_t
    {
        /// <include file='ctl_fan_config_t.xml' path='doc/member[@name="ctl_fan_config_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_fan_config_t.xml' path='doc/member[@name="ctl_fan_config_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_fan_config_t.xml' path='doc/member[@name="ctl_fan_config_t.mode"]/*' />
        public ctl_fan_speed_mode_t mode;

        /// <include file='ctl_fan_config_t.xml' path='doc/member[@name="ctl_fan_config_t.speedFixed"]/*' />
        public ctl_fan_speed_t speedFixed;

        /// <include file='ctl_fan_config_t.xml' path='doc/member[@name="ctl_fan_config_t.speedTable"]/*' />
        public ctl_fan_speed_table_t speedTable;
    }
}
