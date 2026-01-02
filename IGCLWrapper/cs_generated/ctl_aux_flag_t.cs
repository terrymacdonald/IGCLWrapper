namespace IGCLWrapper
{
    /// <include file='ctl_aux_flag_t.xml' path='doc/member[@name="ctl_aux_flag_t"]/*' />
    public enum ctl_aux_flag_t
    {
        /// <include file='ctl_aux_flag_t.xml' path='doc/member[@name="ctl_aux_flag_t.CTL_AUX_FLAG_NATIVE_AUX"]/*' />
        CTL_AUX_FLAG_NATIVE_AUX = (1 << 0),

        /// <include file='ctl_aux_flag_t.xml' path='doc/member[@name="ctl_aux_flag_t.CTL_AUX_FLAG_I2C_AUX"]/*' />
        CTL_AUX_FLAG_I2C_AUX = (1 << 1),

        /// <include file='ctl_aux_flag_t.xml' path='doc/member[@name="ctl_aux_flag_t.CTL_AUX_FLAG_I2C_AUX_MOT"]/*' />
        CTL_AUX_FLAG_I2C_AUX_MOT = (1 << 2),

        /// <include file='ctl_aux_flag_t.xml' path='doc/member[@name="ctl_aux_flag_t.CTL_AUX_FLAG_MAX"]/*' />
        CTL_AUX_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
