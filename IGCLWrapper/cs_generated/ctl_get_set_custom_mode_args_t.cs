namespace IGCLWrapper
{
    /// <include file='ctl_get_set_custom_mode_args_t.xml' path='doc/member[@name="ctl_get_set_custom_mode_args_t"]/*' />
    public unsafe partial struct ctl_get_set_custom_mode_args_t
    {
        /// <include file='ctl_get_set_custom_mode_args_t.xml' path='doc/member[@name="ctl_get_set_custom_mode_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_get_set_custom_mode_args_t.xml' path='doc/member[@name="ctl_get_set_custom_mode_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_get_set_custom_mode_args_t.xml' path='doc/member[@name="ctl_get_set_custom_mode_args_t.CustomModeOpType"]/*' />
        public ctl_custom_mode_operation_types_t CustomModeOpType;

        /// <include file='ctl_get_set_custom_mode_args_t.xml' path='doc/member[@name="ctl_get_set_custom_mode_args_t.NumOfModes"]/*' />
        [NativeTypeName("uint32_t")]
        public uint NumOfModes;

        /// <include file='ctl_get_set_custom_mode_args_t.xml' path='doc/member[@name="ctl_get_set_custom_mode_args_t.pCustomSrcModeList"]/*' />
        public ctl_custom_src_mode_t* pCustomSrcModeList;
    }
}
