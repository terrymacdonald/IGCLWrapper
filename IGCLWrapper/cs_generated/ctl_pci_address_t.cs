namespace IGCLWrapper
{
    /// <include file='ctl_pci_address_t.xml' path='doc/member[@name="ctl_pci_address_t"]/*' />
    public partial struct ctl_pci_address_t
    {
        /// <include file='ctl_pci_address_t.xml' path='doc/member[@name="ctl_pci_address_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_pci_address_t.xml' path='doc/member[@name="ctl_pci_address_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_pci_address_t.xml' path='doc/member[@name="ctl_pci_address_t.domain"]/*' />
        [NativeTypeName("uint32_t")]
        public uint domain;

        /// <include file='ctl_pci_address_t.xml' path='doc/member[@name="ctl_pci_address_t.bus"]/*' />
        [NativeTypeName("uint32_t")]
        public uint bus;

        /// <include file='ctl_pci_address_t.xml' path='doc/member[@name="ctl_pci_address_t.device"]/*' />
        [NativeTypeName("uint32_t")]
        public uint device;

        /// <include file='ctl_pci_address_t.xml' path='doc/member[@name="ctl_pci_address_t.function"]/*' />
        [NativeTypeName("uint32_t")]
        public uint function;
    }
}
