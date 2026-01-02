namespace IGCLWrapper
{
    /// <include file='ctl_edid_management_out_flag_t.xml' path='doc/member[@name="ctl_edid_management_out_flag_t"]/*' />
    public enum ctl_edid_management_out_flag_t
    {
        /// <include file='ctl_edid_management_out_flag_t.xml' path='doc/member[@name="ctl_edid_management_out_flag_t.CTL_EDID_MANAGEMENT_OUT_FLAG_OS_CONN_NOTIFICATION"]/*' />
        CTL_EDID_MANAGEMENT_OUT_FLAG_OS_CONN_NOTIFICATION = (1 << 0),

        /// <include file='ctl_edid_management_out_flag_t.xml' path='doc/member[@name="ctl_edid_management_out_flag_t.CTL_EDID_MANAGEMENT_OUT_FLAG_SUPPLIED_EDID"]/*' />
        CTL_EDID_MANAGEMENT_OUT_FLAG_SUPPLIED_EDID = (1 << 1),

        /// <include file='ctl_edid_management_out_flag_t.xml' path='doc/member[@name="ctl_edid_management_out_flag_t.CTL_EDID_MANAGEMENT_OUT_FLAG_MONITOR_EDID"]/*' />
        CTL_EDID_MANAGEMENT_OUT_FLAG_MONITOR_EDID = (1 << 2),

        /// <include file='ctl_edid_management_out_flag_t.xml' path='doc/member[@name="ctl_edid_management_out_flag_t.CTL_EDID_MANAGEMENT_OUT_FLAG_DISPLAY_CONNECTED"]/*' />
        CTL_EDID_MANAGEMENT_OUT_FLAG_DISPLAY_CONNECTED = (1 << 3),

        /// <include file='ctl_edid_management_out_flag_t.xml' path='doc/member[@name="ctl_edid_management_out_flag_t.CTL_EDID_MANAGEMENT_OUT_FLAG_MAX"]/*' />
        CTL_EDID_MANAGEMENT_OUT_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
