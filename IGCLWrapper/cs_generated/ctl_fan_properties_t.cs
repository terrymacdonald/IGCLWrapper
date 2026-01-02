namespace IGCLWrapper
{
    /// <include file='ctl_fan_properties_t.xml' path='doc/member[@name="ctl_fan_properties_t"]/*' />
    public partial struct ctl_fan_properties_t
    {
        /// <include file='ctl_fan_properties_t.xml' path='doc/member[@name="ctl_fan_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_fan_properties_t.xml' path='doc/member[@name="ctl_fan_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_fan_properties_t.xml' path='doc/member[@name="ctl_fan_properties_t.canControl"]/*' />
        [NativeTypeName("bool")]
        public byte canControl;

        /// <include file='ctl_fan_properties_t.xml' path='doc/member[@name="ctl_fan_properties_t.supportedModes"]/*' />
        [NativeTypeName("uint32_t")]
        public uint supportedModes;

        /// <include file='ctl_fan_properties_t.xml' path='doc/member[@name="ctl_fan_properties_t.supportedUnits"]/*' />
        [NativeTypeName("uint32_t")]
        public uint supportedUnits;

        /// <include file='ctl_fan_properties_t.xml' path='doc/member[@name="ctl_fan_properties_t.maxRPM"]/*' />
        [NativeTypeName("int32_t")]
        public int maxRPM;

        /// <include file='ctl_fan_properties_t.xml' path='doc/member[@name="ctl_fan_properties_t.maxPoints"]/*' />
        [NativeTypeName("int32_t")]
        public int maxPoints;
    }
}
