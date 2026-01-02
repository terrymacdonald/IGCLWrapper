namespace IGCLWrapper
{
    /// <include file='ctl_property_type_flag_t.xml' path='doc/member[@name="ctl_property_type_flag_t"]/*' />
    public enum ctl_property_type_flag_t
    {
        /// <include file='ctl_property_type_flag_t.xml' path='doc/member[@name="ctl_property_type_flag_t.CTL_PROPERTY_TYPE_FLAG_DISPLAY"]/*' />
        CTL_PROPERTY_TYPE_FLAG_DISPLAY = (1 << 0),

        /// <include file='ctl_property_type_flag_t.xml' path='doc/member[@name="ctl_property_type_flag_t.CTL_PROPERTY_TYPE_FLAG_3D"]/*' />
        CTL_PROPERTY_TYPE_FLAG_3D = (1 << 1),

        /// <include file='ctl_property_type_flag_t.xml' path='doc/member[@name="ctl_property_type_flag_t.CTL_PROPERTY_TYPE_FLAG_MEDIA"]/*' />
        CTL_PROPERTY_TYPE_FLAG_MEDIA = (1 << 2),

        /// <include file='ctl_property_type_flag_t.xml' path='doc/member[@name="ctl_property_type_flag_t.CTL_PROPERTY_TYPE_FLAG_CORE"]/*' />
        CTL_PROPERTY_TYPE_FLAG_CORE = (1 << 3),

        /// <include file='ctl_property_type_flag_t.xml' path='doc/member[@name="ctl_property_type_flag_t.CTL_PROPERTY_TYPE_FLAG_MAX"]/*' />
        CTL_PROPERTY_TYPE_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
