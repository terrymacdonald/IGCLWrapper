namespace IGCLWrapper
{
    /// <include file='ctl_display_config_flag_t.xml' path='doc/member[@name="ctl_display_config_flag_t"]/*' />
    public enum ctl_display_config_flag_t
    {
        /// <include file='ctl_display_config_flag_t.xml' path='doc/member[@name="ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ACTIVE"]/*' />
        CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ACTIVE = (1 << 0),

        /// <include file='ctl_display_config_flag_t.xml' path='doc/member[@name="ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ATTACHED"]/*' />
        CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ATTACHED = (1 << 1),

        /// <include file='ctl_display_config_flag_t.xml' path='doc/member[@name="ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_IS_DONGLE_CONNECTED_TO_ENCODER"]/*' />
        CTL_DISPLAY_CONFIG_FLAG_IS_DONGLE_CONNECTED_TO_ENCODER = (1 << 2),

        /// <include file='ctl_display_config_flag_t.xml' path='doc/member[@name="ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DITHERING_ENABLED"]/*' />
        CTL_DISPLAY_CONFIG_FLAG_DITHERING_ENABLED = (1 << 3),

        /// <include file='ctl_display_config_flag_t.xml' path='doc/member[@name="ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_MAX"]/*' />
        CTL_DISPLAY_CONFIG_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
