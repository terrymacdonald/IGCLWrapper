namespace IGCLWrapper
{
    /// <include file='ctl_sharpness_filter_properties_t.xml' path='doc/member[@name="ctl_sharpness_filter_properties_t"]/*' />
    public partial struct ctl_sharpness_filter_properties_t
    {
        /// <include file='ctl_sharpness_filter_properties_t.xml' path='doc/member[@name="ctl_sharpness_filter_properties_t.FilterType"]/*' />
        [NativeTypeName("ctl_sharpness_filter_type_flags_t")]
        public uint FilterType;

        /// <include file='ctl_sharpness_filter_properties_t.xml' path='doc/member[@name="ctl_sharpness_filter_properties_t.FilterDetails"]/*' />
        public ctl_property_range_info_t FilterDetails;
    }
}
