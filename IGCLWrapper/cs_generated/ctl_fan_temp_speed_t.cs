namespace IGCLWrapper
{
    /// <include file='ctl_fan_temp_speed_t.xml' path='doc/member[@name="ctl_fan_temp_speed_t"]/*' />
    public partial struct ctl_fan_temp_speed_t
    {
        /// <include file='ctl_fan_temp_speed_t.xml' path='doc/member[@name="ctl_fan_temp_speed_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_fan_temp_speed_t.xml' path='doc/member[@name="ctl_fan_temp_speed_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_fan_temp_speed_t.xml' path='doc/member[@name="ctl_fan_temp_speed_t.temperature"]/*' />
        [NativeTypeName("uint32_t")]
        public uint temperature;

        /// <include file='ctl_fan_temp_speed_t.xml' path='doc/member[@name="ctl_fan_temp_speed_t.speed"]/*' />
        public ctl_fan_speed_t speed;
    }
}
