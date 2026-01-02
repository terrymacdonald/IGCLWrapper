namespace IGCLWrapper
{
    /// <include file='ctl_ecc_state_desc_t.xml' path='doc/member[@name="ctl_ecc_state_desc_t"]/*' />
    public partial struct ctl_ecc_state_desc_t
    {
        /// <include file='ctl_ecc_state_desc_t.xml' path='doc/member[@name="ctl_ecc_state_desc_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_ecc_state_desc_t.xml' path='doc/member[@name="ctl_ecc_state_desc_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_ecc_state_desc_t.xml' path='doc/member[@name="ctl_ecc_state_desc_t.currentEccState"]/*' />
        public ctl_ecc_state_t currentEccState;

        /// <include file='ctl_ecc_state_desc_t.xml' path='doc/member[@name="ctl_ecc_state_desc_t.pendingEccState"]/*' />
        public ctl_ecc_state_t pendingEccState;
    }
}
