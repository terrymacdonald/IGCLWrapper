namespace IGCLWrapper
{
    /// <include file='ctl_fan_speed_t.xml' path='doc/member[@name="ctl_fan_speed_t"]/*' />
    public partial struct ctl_fan_speed_t
    {
        /// <include file='ctl_fan_speed_t.xml' path='doc/member[@name="ctl_fan_speed_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_fan_speed_t.xml' path='doc/member[@name="ctl_fan_speed_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_fan_speed_t.xml' path='doc/member[@name="ctl_fan_speed_t.speed"]/*' />
        [NativeTypeName("int32_t")]
        public int speed;

        /// <include file='ctl_fan_speed_t.xml' path='doc/member[@name="ctl_fan_speed_t.units"]/*' />
        public ctl_fan_speed_units_t units;
    }
}
