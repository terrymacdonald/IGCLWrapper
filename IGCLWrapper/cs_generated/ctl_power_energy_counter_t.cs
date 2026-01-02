namespace IGCLWrapper
{
    /// <include file='ctl_power_energy_counter_t.xml' path='doc/member[@name="ctl_power_energy_counter_t"]/*' />
    public partial struct ctl_power_energy_counter_t
    {
        /// <include file='ctl_power_energy_counter_t.xml' path='doc/member[@name="ctl_power_energy_counter_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_power_energy_counter_t.xml' path='doc/member[@name="ctl_power_energy_counter_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_power_energy_counter_t.xml' path='doc/member[@name="ctl_power_energy_counter_t.energy"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong energy;

        /// <include file='ctl_power_energy_counter_t.xml' path='doc/member[@name="ctl_power_energy_counter_t.timestamp"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong timestamp;
    }
}
