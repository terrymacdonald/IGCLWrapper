namespace IGCLWrapper
{
    /// <include file='ctl_property_info_uint_t.xml' path='doc/member[@name="ctl_property_info_uint_t"]/*' />
    public partial struct ctl_property_info_uint_t
    {
        /// <include file='ctl_property_info_uint_t.xml' path='doc/member[@name="ctl_property_info_uint_t.DefaultEnable"]/*' />
        [NativeTypeName("bool")]
        public byte DefaultEnable;

        /// <include file='ctl_property_info_uint_t.xml' path='doc/member[@name="ctl_property_info_uint_t.RangeInfo"]/*' />
        public ctl_property_range_info_uint_t RangeInfo;
    }
}
