namespace IGCLWrapper
{
    /// <include file='ctl_genlock_display_info_t.xml' path='doc/member[@name="ctl_genlock_display_info_t"]/*' />
    public unsafe partial struct ctl_genlock_display_info_t
    {
        /// <include file='ctl_genlock_display_info_t.xml' path='doc/member[@name="ctl_genlock_display_info_t.hDisplayOutput"]/*' />
        [NativeTypeName("ctl_display_output_handle_t")]
        public _ctl_display_output_handle_t* hDisplayOutput;

        /// <include file='ctl_genlock_display_info_t.xml' path='doc/member[@name="ctl_genlock_display_info_t.IsPrimary"]/*' />
        [NativeTypeName("bool")]
        public byte IsPrimary;
    }
}
