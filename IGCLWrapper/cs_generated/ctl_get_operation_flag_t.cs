namespace IGCLWrapper
{
    /// <include file='ctl_get_operation_flag_t.xml' path='doc/member[@name="ctl_get_operation_flag_t"]/*' />
    public enum ctl_get_operation_flag_t
    {
        /// <include file='ctl_get_operation_flag_t.xml' path='doc/member[@name="ctl_get_operation_flag_t.CTL_GET_OPERATION_FLAG_CURRENT"]/*' />
        CTL_GET_OPERATION_FLAG_CURRENT = (1 << 0),

        /// <include file='ctl_get_operation_flag_t.xml' path='doc/member[@name="ctl_get_operation_flag_t.CTL_GET_OPERATION_FLAG_DEFAULT"]/*' />
        CTL_GET_OPERATION_FLAG_DEFAULT = (1 << 1),

        /// <include file='ctl_get_operation_flag_t.xml' path='doc/member[@name="ctl_get_operation_flag_t.CTL_GET_OPERATION_FLAG_CAPABILITY"]/*' />
        CTL_GET_OPERATION_FLAG_CAPABILITY = (1 << 2),

        /// <include file='ctl_get_operation_flag_t.xml' path='doc/member[@name="ctl_get_operation_flag_t.CTL_GET_OPERATION_FLAG_MAX"]/*' />
        CTL_GET_OPERATION_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
