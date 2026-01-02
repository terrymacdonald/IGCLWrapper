namespace IGCLWrapper
{
    /// <include file='ctl_combined_display_child_info_t.xml' path='doc/member[@name="ctl_combined_display_child_info_t"]/*' />
    public unsafe partial struct ctl_combined_display_child_info_t
    {
        /// <include file='ctl_combined_display_child_info_t.xml' path='doc/member[@name="ctl_combined_display_child_info_t.hDisplayOutput"]/*' />
        [NativeTypeName("ctl_display_output_handle_t")]
        public _ctl_display_output_handle_t* hDisplayOutput;

        /// <include file='ctl_combined_display_child_info_t.xml' path='doc/member[@name="ctl_combined_display_child_info_t.FbSrc"]/*' />
        public ctl_rect_t FbSrc;

        /// <include file='ctl_combined_display_child_info_t.xml' path='doc/member[@name="ctl_combined_display_child_info_t.FbPos"]/*' />
        public ctl_rect_t FbPos;

        /// <include file='ctl_combined_display_child_info_t.xml' path='doc/member[@name="ctl_combined_display_child_info_t.DisplayOrientation"]/*' />
        public ctl_display_orientation_t DisplayOrientation;

        /// <include file='ctl_combined_display_child_info_t.xml' path='doc/member[@name="ctl_combined_display_child_info_t.TargetMode"]/*' />
        public ctl_child_display_target_mode_t TargetMode;
    }
}
