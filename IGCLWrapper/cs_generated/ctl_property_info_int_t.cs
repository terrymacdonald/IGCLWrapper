namespace IGCLWrapper
{
    /// <include file='ctl_property_info_int_t.xml' path='doc/member[@name="ctl_property_info_int_t"]/*' />
    public partial struct ctl_property_info_int_t
    {
        /// <include file='ctl_property_info_int_t.xml' path='doc/member[@name="ctl_property_info_int_t.DefaultEnable"]/*' />
        [NativeTypeName("bool")]
        public byte DefaultEnable;

        /// <include file='ctl_property_info_int_t.xml' path='doc/member[@name="ctl_property_info_int_t.RangeInfo"]/*' />
        public ctl_property_range_info_int_t RangeInfo;
    }
}
