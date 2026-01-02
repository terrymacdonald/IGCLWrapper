namespace IGCLWrapper
{
    /// <include file='ctl_adapter_properties_flag_t.xml' path='doc/member[@name="ctl_adapter_properties_flag_t"]/*' />
    public enum ctl_adapter_properties_flag_t
    {
        /// <include file='ctl_adapter_properties_flag_t.xml' path='doc/member[@name="ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_INTEGRATED"]/*' />
        CTL_ADAPTER_PROPERTIES_FLAG_INTEGRATED = (1 << 0),

        /// <include file='ctl_adapter_properties_flag_t.xml' path='doc/member[@name="ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_LDA_PRIMARY"]/*' />
        CTL_ADAPTER_PROPERTIES_FLAG_LDA_PRIMARY = (1 << 1),

        /// <include file='ctl_adapter_properties_flag_t.xml' path='doc/member[@name="ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_LDA_SECONDARY"]/*' />
        CTL_ADAPTER_PROPERTIES_FLAG_LDA_SECONDARY = (1 << 2),

        /// <include file='ctl_adapter_properties_flag_t.xml' path='doc/member[@name="ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_MAX"]/*' />
        CTL_ADAPTER_PROPERTIES_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
