namespace IGCLWrapper
{
    /// <include file='ctl_display_setting_flag_t.xml' path='doc/member[@name="ctl_display_setting_flag_t"]/*' />
    public enum ctl_display_setting_flag_t
    {
        /// <include file='ctl_display_setting_flag_t.xml' path='doc/member[@name="ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY"]/*' />
        CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY = (1 << 0),

        /// <include file='ctl_display_setting_flag_t.xml' path='doc/member[@name="ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_SOURCE_TM"]/*' />
        CTL_DISPLAY_SETTING_FLAG_SOURCE_TM = (1 << 1),

        /// <include file='ctl_display_setting_flag_t.xml' path='doc/member[@name="ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_CONTENT_TYPE"]/*' />
        CTL_DISPLAY_SETTING_FLAG_CONTENT_TYPE = (1 << 2),

        /// <include file='ctl_display_setting_flag_t.xml' path='doc/member[@name="ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_QUANTIZATION_RANGE"]/*' />
        CTL_DISPLAY_SETTING_FLAG_QUANTIZATION_RANGE = (1 << 3),

        /// <include file='ctl_display_setting_flag_t.xml' path='doc/member[@name="ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_PICTURE_AR"]/*' />
        CTL_DISPLAY_SETTING_FLAG_PICTURE_AR = (1 << 4),

        /// <include file='ctl_display_setting_flag_t.xml' path='doc/member[@name="ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO"]/*' />
        CTL_DISPLAY_SETTING_FLAG_AUDIO = (1 << 5),

        /// <include file='ctl_display_setting_flag_t.xml' path='doc/member[@name="ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_MAX"]/*' />
        CTL_DISPLAY_SETTING_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
