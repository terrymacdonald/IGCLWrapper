namespace IGCLWrapper
{
    /// <include file='ctl_panel_descriptor_access_args_t.xml' path='doc/member[@name="ctl_panel_descriptor_access_args_t"]/*' />
    public unsafe partial struct ctl_panel_descriptor_access_args_t
    {
        /// <include file='ctl_panel_descriptor_access_args_t.xml' path='doc/member[@name="ctl_panel_descriptor_access_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_panel_descriptor_access_args_t.xml' path='doc/member[@name="ctl_panel_descriptor_access_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_panel_descriptor_access_args_t.xml' path='doc/member[@name="ctl_panel_descriptor_access_args_t.OpType"]/*' />
        public ctl_operation_type_t OpType;

        /// <include file='ctl_panel_descriptor_access_args_t.xml' path='doc/member[@name="ctl_panel_descriptor_access_args_t.BlockNumber"]/*' />
        [NativeTypeName("uint32_t")]
        public uint BlockNumber;

        /// <include file='ctl_panel_descriptor_access_args_t.xml' path='doc/member[@name="ctl_panel_descriptor_access_args_t.DescriptorDataSize"]/*' />
        [NativeTypeName("uint32_t")]
        public uint DescriptorDataSize;

        /// <include file='ctl_panel_descriptor_access_args_t.xml' path='doc/member[@name="ctl_panel_descriptor_access_args_t.pDescriptorData"]/*' />
        [NativeTypeName("uint8_t *")]
        public byte* pDescriptorData;
    }
}
