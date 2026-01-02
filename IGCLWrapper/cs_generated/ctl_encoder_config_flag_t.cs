namespace IGCLWrapper
{
    /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t"]/*' />
    public enum ctl_encoder_config_flag_t
    {
        /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_INTERNAL_DISPLAY"]/*' />
        CTL_ENCODER_CONFIG_FLAG_INTERNAL_DISPLAY = (1 << 0),

        /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_VESA_TILED_DISPLAY"]/*' />
        CTL_ENCODER_CONFIG_FLAG_VESA_TILED_DISPLAY = (1 << 1),

        /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TYPEC_CAPABLE"]/*' />
        CTL_ENCODER_CONFIG_FLAG_TYPEC_CAPABLE = (1 << 2),

        /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TBT_CAPABLE"]/*' />
        CTL_ENCODER_CONFIG_FLAG_TBT_CAPABLE = (1 << 3),

        /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_DITHERING_SUPPORTED"]/*' />
        CTL_ENCODER_CONFIG_FLAG_DITHERING_SUPPORTED = (1 << 4),

        /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_VIRTUAL_DISPLAY"]/*' />
        CTL_ENCODER_CONFIG_FLAG_VIRTUAL_DISPLAY = (1 << 5),

        /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_HIDDEN_DISPLAY"]/*' />
        CTL_ENCODER_CONFIG_FLAG_HIDDEN_DISPLAY = (1 << 6),

        /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COLLAGE_DISPLAY"]/*' />
        CTL_ENCODER_CONFIG_FLAG_COLLAGE_DISPLAY = (1 << 7),

        /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_SPLIT_DISPLAY"]/*' />
        CTL_ENCODER_CONFIG_FLAG_SPLIT_DISPLAY = (1 << 8),

        /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COMPANION_DISPLAY"]/*' />
        CTL_ENCODER_CONFIG_FLAG_COMPANION_DISPLAY = (1 << 9),

        /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_MGPU_COLLAGE_DISPLAY"]/*' />
        CTL_ENCODER_CONFIG_FLAG_MGPU_COLLAGE_DISPLAY = (1 << 10),

        /// <include file='ctl_encoder_config_flag_t.xml' path='doc/member[@name="ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_MAX"]/*' />
        CTL_ENCODER_CONFIG_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
