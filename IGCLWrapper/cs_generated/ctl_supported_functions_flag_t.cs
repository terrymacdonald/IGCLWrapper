namespace IGCLWrapper
{
    /// <include file='ctl_supported_functions_flag_t.xml' path='doc/member[@name="ctl_supported_functions_flag_t"]/*' />
    public enum ctl_supported_functions_flag_t
    {
        /// <include file='ctl_supported_functions_flag_t.xml' path='doc/member[@name="ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_DISPLAY"]/*' />
        CTL_SUPPORTED_FUNCTIONS_FLAG_DISPLAY = (1 << 0),

        /// <include file='ctl_supported_functions_flag_t.xml' path='doc/member[@name="ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_3D"]/*' />
        CTL_SUPPORTED_FUNCTIONS_FLAG_3D = (1 << 1),

        /// <include file='ctl_supported_functions_flag_t.xml' path='doc/member[@name="ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_MEDIA"]/*' />
        CTL_SUPPORTED_FUNCTIONS_FLAG_MEDIA = (1 << 2),

        /// <include file='ctl_supported_functions_flag_t.xml' path='doc/member[@name="ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_MAX"]/*' />
        CTL_SUPPORTED_FUNCTIONS_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
