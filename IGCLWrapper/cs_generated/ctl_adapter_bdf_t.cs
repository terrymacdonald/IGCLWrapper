namespace IGCLWrapper
{
    /// <include file='ctl_adapter_bdf_t.xml' path='doc/member[@name="ctl_adapter_bdf_t"]/*' />
    public partial struct ctl_adapter_bdf_t
    {
        /// <include file='ctl_adapter_bdf_t.xml' path='doc/member[@name="ctl_adapter_bdf_t.bus"]/*' />
        [NativeTypeName("uint8_t")]
        public byte bus;

        /// <include file='ctl_adapter_bdf_t.xml' path='doc/member[@name="ctl_adapter_bdf_t.device"]/*' />
        [NativeTypeName("uint8_t")]
        public byte device;

        /// <include file='ctl_adapter_bdf_t.xml' path='doc/member[@name="ctl_adapter_bdf_t.function"]/*' />
        [NativeTypeName("uint8_t")]
        public byte function;
    }
}
