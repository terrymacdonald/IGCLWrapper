namespace IGCLWrapper
{
    /// <include file='ctl_edid_management_args_t.xml' path='doc/member[@name="ctl_edid_management_args_t"]/*' />
    public unsafe partial struct ctl_edid_management_args_t
    {
        /// <include file='ctl_edid_management_args_t.xml' path='doc/member[@name="ctl_edid_management_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_edid_management_args_t.xml' path='doc/member[@name="ctl_edid_management_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_edid_management_args_t.xml' path='doc/member[@name="ctl_edid_management_args_t.OpType"]/*' />
        public ctl_edid_management_optype_t OpType;

        /// <include file='ctl_edid_management_args_t.xml' path='doc/member[@name="ctl_edid_management_args_t.EdidType"]/*' />
        public ctl_edid_type_t EdidType;

        /// <include file='ctl_edid_management_args_t.xml' path='doc/member[@name="ctl_edid_management_args_t.EdidSize"]/*' />
        [NativeTypeName("uint32_t")]
        public uint EdidSize;

        /// <include file='ctl_edid_management_args_t.xml' path='doc/member[@name="ctl_edid_management_args_t.pEdidBuf"]/*' />
        [NativeTypeName("uint8_t *")]
        public byte* pEdidBuf;

        /// <include file='ctl_edid_management_args_t.xml' path='doc/member[@name="ctl_edid_management_args_t.OutFlags"]/*' />
        [NativeTypeName("ctl_edid_management_out_flags_t")]
        public uint OutFlags;
    }
}
