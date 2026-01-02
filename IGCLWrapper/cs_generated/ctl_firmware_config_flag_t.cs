namespace IGCLWrapper
{
    /// <include file='ctl_firmware_config_flag_t.xml' path='doc/member[@name="ctl_firmware_config_flag_t"]/*' />
    public enum ctl_firmware_config_flag_t
    {
        /// <include file='ctl_firmware_config_flag_t.xml' path='doc/member[@name="ctl_firmware_config_flag_t.CTL_FIRMWARE_CONFIG_FLAG_IS_DEVICE_LINK_SPEED_DOWNGRADE_CAPABLE"]/*' />
        CTL_FIRMWARE_CONFIG_FLAG_IS_DEVICE_LINK_SPEED_DOWNGRADE_CAPABLE = (1 << 0),

        /// <include file='ctl_firmware_config_flag_t.xml' path='doc/member[@name="ctl_firmware_config_flag_t.CTL_FIRMWARE_CONFIG_FLAG_IS_DEVICE_LINK_SPEED_DOWNGRADE_ACTIVE"]/*' />
        CTL_FIRMWARE_CONFIG_FLAG_IS_DEVICE_LINK_SPEED_DOWNGRADE_ACTIVE = (1 << 1),

        /// <include file='ctl_firmware_config_flag_t.xml' path='doc/member[@name="ctl_firmware_config_flag_t.CTL_FIRMWARE_CONFIG_FLAG_MAX"]/*' />
        CTL_FIRMWARE_CONFIG_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
