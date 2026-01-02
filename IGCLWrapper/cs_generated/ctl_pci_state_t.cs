namespace IGCLWrapper
{
    /// <include file='ctl_pci_state_t.xml' path='doc/member[@name="ctl_pci_state_t"]/*' />
    public partial struct ctl_pci_state_t
    {
        /// <include file='ctl_pci_state_t.xml' path='doc/member[@name="ctl_pci_state_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_pci_state_t.xml' path='doc/member[@name="ctl_pci_state_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_pci_state_t.xml' path='doc/member[@name="ctl_pci_state_t.speed"]/*' />
        public ctl_pci_speed_t speed;
    }
}
