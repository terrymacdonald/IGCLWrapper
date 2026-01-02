namespace IGCLWrapper
{
    /// <include file='ctl_combined_display_args_t.xml' path='doc/member[@name="ctl_combined_display_args_t"]/*' />
    public unsafe partial struct ctl_combined_display_args_t
    {
        /// <include file='ctl_combined_display_args_t.xml' path='doc/member[@name="ctl_combined_display_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_combined_display_args_t.xml' path='doc/member[@name="ctl_combined_display_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_combined_display_args_t.xml' path='doc/member[@name="ctl_combined_display_args_t.OpType"]/*' />
        public ctl_combined_display_optype_t OpType;

        /// <include file='ctl_combined_display_args_t.xml' path='doc/member[@name="ctl_combined_display_args_t.IsSupported"]/*' />
        [NativeTypeName("bool")]
        public byte IsSupported;

        /// <include file='ctl_combined_display_args_t.xml' path='doc/member[@name="ctl_combined_display_args_t.NumOutputs"]/*' />
        [NativeTypeName("uint8_t")]
        public byte NumOutputs;

        /// <include file='ctl_combined_display_args_t.xml' path='doc/member[@name="ctl_combined_display_args_t.CombinedDesktopWidth"]/*' />
        [NativeTypeName("uint32_t")]
        public uint CombinedDesktopWidth;

        /// <include file='ctl_combined_display_args_t.xml' path='doc/member[@name="ctl_combined_display_args_t.CombinedDesktopHeight"]/*' />
        [NativeTypeName("uint32_t")]
        public uint CombinedDesktopHeight;

        /// <include file='ctl_combined_display_args_t.xml' path='doc/member[@name="ctl_combined_display_args_t.pChildInfo"]/*' />
        public ctl_combined_display_child_info_t* pChildInfo;

        /// <include file='ctl_combined_display_args_t.xml' path='doc/member[@name="ctl_combined_display_args_t.hCombinedDisplayOutput"]/*' />
        [NativeTypeName("ctl_display_output_handle_t")]
        public _ctl_display_output_handle_t* hCombinedDisplayOutput;
    }
}
