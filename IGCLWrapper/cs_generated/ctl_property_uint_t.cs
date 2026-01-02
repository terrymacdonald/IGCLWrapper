namespace IGCLWrapper
{
    /// <include file='ctl_property_uint_t.xml' path='doc/member[@name="ctl_property_uint_t"]/*' />
    public partial struct ctl_property_uint_t
    {
        /// <include file='ctl_property_uint_t.xml' path='doc/member[@name="ctl_property_uint_t.Enable"]/*' />
        [NativeTypeName("bool")]
        public byte Enable;

        /// <include file='ctl_property_uint_t.xml' path='doc/member[@name="ctl_property_uint_t.Value"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Value;
    }
}
