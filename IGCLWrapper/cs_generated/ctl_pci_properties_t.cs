namespace IGCLWrapper
{
    /// <include file='ctl_pci_properties_t.xml' path='doc/member[@name="ctl_pci_properties_t"]/*' />
    public partial struct ctl_pci_properties_t
    {
        /// <include file='ctl_pci_properties_t.xml' path='doc/member[@name="ctl_pci_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_pci_properties_t.xml' path='doc/member[@name="ctl_pci_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_pci_properties_t.xml' path='doc/member[@name="ctl_pci_properties_t.address"]/*' />
        public ctl_pci_address_t address;

        /// <include file='ctl_pci_properties_t.xml' path='doc/member[@name="ctl_pci_properties_t.maxSpeed"]/*' />
        public ctl_pci_speed_t maxSpeed;

        /// <include file='ctl_pci_properties_t.xml' path='doc/member[@name="ctl_pci_properties_t.resizable_bar_supported"]/*' />
        [NativeTypeName("bool")]
        public byte resizable_bar_supported;

        /// <include file='ctl_pci_properties_t.xml' path='doc/member[@name="ctl_pci_properties_t.resizable_bar_enabled"]/*' />
        [NativeTypeName("bool")]
        public byte resizable_bar_enabled;
    }
}
