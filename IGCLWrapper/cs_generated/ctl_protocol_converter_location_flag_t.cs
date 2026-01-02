namespace IGCLWrapper
{
    /// <include file='ctl_protocol_converter_location_flag_t.xml' path='doc/member[@name="ctl_protocol_converter_location_flag_t"]/*' />
    public enum ctl_protocol_converter_location_flag_t
    {
        /// <include file='ctl_protocol_converter_location_flag_t.xml' path='doc/member[@name="ctl_protocol_converter_location_flag_t.CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_ONBOARD"]/*' />
        CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_ONBOARD = (1 << 0),

        /// <include file='ctl_protocol_converter_location_flag_t.xml' path='doc/member[@name="ctl_protocol_converter_location_flag_t.CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_EXTERNAL"]/*' />
        CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_EXTERNAL = (1 << 1),

        /// <include file='ctl_protocol_converter_location_flag_t.xml' path='doc/member[@name="ctl_protocol_converter_location_flag_t.CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_MAX"]/*' />
        CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_MAX = unchecked((int)(0x80000000)),
    }
}
