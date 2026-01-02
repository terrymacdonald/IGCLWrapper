namespace IGCLWrapper
{
    /// <include file='ctl_pci_speed_t.xml' path='doc/member[@name="ctl_pci_speed_t"]/*' />
    public partial struct ctl_pci_speed_t
    {
        /// <include file='ctl_pci_speed_t.xml' path='doc/member[@name="ctl_pci_speed_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_pci_speed_t.xml' path='doc/member[@name="ctl_pci_speed_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_pci_speed_t.xml' path='doc/member[@name="ctl_pci_speed_t.gen"]/*' />
        [NativeTypeName("int32_t")]
        public int gen;

        /// <include file='ctl_pci_speed_t.xml' path='doc/member[@name="ctl_pci_speed_t.width"]/*' />
        [NativeTypeName("int32_t")]
        public int width;

        /// <include file='ctl_pci_speed_t.xml' path='doc/member[@name="ctl_pci_speed_t.maxBandwidth"]/*' />
        [NativeTypeName("int64_t")]
        public long maxBandwidth;
    }
}
