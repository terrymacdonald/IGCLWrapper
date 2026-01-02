namespace IGCLWrapper
{
    /// <include file='ctl_voltage_frequency_point_t.xml' path='doc/member[@name="ctl_voltage_frequency_point_t"]/*' />
    public partial struct ctl_voltage_frequency_point_t
    {
        /// <include file='ctl_voltage_frequency_point_t.xml' path='doc/member[@name="ctl_voltage_frequency_point_t.Voltage"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Voltage;

        /// <include file='ctl_voltage_frequency_point_t.xml' path='doc/member[@name="ctl_voltage_frequency_point_t.Frequency"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Frequency;
    }
}
