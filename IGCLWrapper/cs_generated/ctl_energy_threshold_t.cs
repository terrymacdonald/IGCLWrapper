namespace IGCLWrapper
{
    /// <include file='ctl_energy_threshold_t.xml' path='doc/member[@name="ctl_energy_threshold_t"]/*' />
    public partial struct ctl_energy_threshold_t
    {
        /// <include file='ctl_energy_threshold_t.xml' path='doc/member[@name="ctl_energy_threshold_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_energy_threshold_t.xml' path='doc/member[@name="ctl_energy_threshold_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_energy_threshold_t.xml' path='doc/member[@name="ctl_energy_threshold_t.enable"]/*' />
        [NativeTypeName("bool")]
        public byte enable;

        /// <include file='ctl_energy_threshold_t.xml' path='doc/member[@name="ctl_energy_threshold_t.threshold"]/*' />
        public double threshold;

        /// <include file='ctl_energy_threshold_t.xml' path='doc/member[@name="ctl_energy_threshold_t.processId"]/*' />
        [NativeTypeName("uint32_t")]
        public uint processId;
    }
}
