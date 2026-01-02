namespace IGCLWrapper
{
    /// <include file='ctl_temp_properties_t.xml' path='doc/member[@name="ctl_temp_properties_t"]/*' />
    public partial struct ctl_temp_properties_t
    {
        /// <include file='ctl_temp_properties_t.xml' path='doc/member[@name="ctl_temp_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_temp_properties_t.xml' path='doc/member[@name="ctl_temp_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_temp_properties_t.xml' path='doc/member[@name="ctl_temp_properties_t.type"]/*' />
        public ctl_temp_sensors_t type;

        /// <include file='ctl_temp_properties_t.xml' path='doc/member[@name="ctl_temp_properties_t.maxTemperature"]/*' />
        public double maxTemperature;
    }
}
