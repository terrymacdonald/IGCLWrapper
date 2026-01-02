namespace IGCLWrapper
{
    /// <include file='ctl_i2c_pinpair_flag_t.xml' path='doc/member[@name="ctl_i2c_pinpair_flag_t"]/*' />
    public enum ctl_i2c_pinpair_flag_t
    {
        /// <include file='ctl_i2c_pinpair_flag_t.xml' path='doc/member[@name="ctl_i2c_pinpair_flag_t.CTL_I2C_PINPAIR_FLAG_ATOMICI2C"]/*' />
        CTL_I2C_PINPAIR_FLAG_ATOMICI2C = (1 << 0),

        /// <include file='ctl_i2c_pinpair_flag_t.xml' path='doc/member[@name="ctl_i2c_pinpair_flag_t.CTL_I2C_PINPAIR_FLAG_1BYTE_INDEX"]/*' />
        CTL_I2C_PINPAIR_FLAG_1BYTE_INDEX = (1 << 1),

        /// <include file='ctl_i2c_pinpair_flag_t.xml' path='doc/member[@name="ctl_i2c_pinpair_flag_t.CTL_I2C_PINPAIR_FLAG_2BYTE_INDEX"]/*' />
        CTL_I2C_PINPAIR_FLAG_2BYTE_INDEX = (1 << 2),

        /// <include file='ctl_i2c_pinpair_flag_t.xml' path='doc/member[@name="ctl_i2c_pinpair_flag_t.CTL_I2C_PINPAIR_FLAG_4BYTE_INDEX"]/*' />
        CTL_I2C_PINPAIR_FLAG_4BYTE_INDEX = (1 << 3),

        /// <include file='ctl_i2c_pinpair_flag_t.xml' path='doc/member[@name="ctl_i2c_pinpair_flag_t.CTL_I2C_PINPAIR_FLAG_SPEED_SLOW"]/*' />
        CTL_I2C_PINPAIR_FLAG_SPEED_SLOW = (1 << 4),

        /// <include file='ctl_i2c_pinpair_flag_t.xml' path='doc/member[@name="ctl_i2c_pinpair_flag_t.CTL_I2C_PINPAIR_FLAG_SPEED_FAST"]/*' />
        CTL_I2C_PINPAIR_FLAG_SPEED_FAST = (1 << 5),

        /// <include file='ctl_i2c_pinpair_flag_t.xml' path='doc/member[@name="ctl_i2c_pinpair_flag_t.CTL_I2C_PINPAIR_FLAG_SPEED_BIT_BASH"]/*' />
        CTL_I2C_PINPAIR_FLAG_SPEED_BIT_BASH = (1 << 6),

        /// <include file='ctl_i2c_pinpair_flag_t.xml' path='doc/member[@name="ctl_i2c_pinpair_flag_t.CTL_I2C_PINPAIR_FLAG_MAX"]/*' />
        CTL_I2C_PINPAIR_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
