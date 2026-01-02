namespace IGCLWrapper
{
    /// <include file='ctl_genlock_target_mode_list_t.xml' path='doc/member[@name="ctl_genlock_target_mode_list_t"]/*' />
    public unsafe partial struct ctl_genlock_target_mode_list_t
    {
        /// <include file='ctl_genlock_target_mode_list_t.xml' path='doc/member[@name="ctl_genlock_target_mode_list_t.hDisplayOutput"]/*' />
        [NativeTypeName("ctl_display_output_handle_t")]
        public _ctl_display_output_handle_t* hDisplayOutput;

        /// <include file='ctl_genlock_target_mode_list_t.xml' path='doc/member[@name="ctl_genlock_target_mode_list_t.NumModes"]/*' />
        [NativeTypeName("uint32_t")]
        public uint NumModes;

        /// <include file='ctl_genlock_target_mode_list_t.xml' path='doc/member[@name="ctl_genlock_target_mode_list_t.pTargetModes"]/*' />
        public ctl_display_timing_t* pTargetModes;
    }
}
