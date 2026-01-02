namespace IGCLWrapper
{
    /// <include file='ctl_property_info_enum_t.xml' path='doc/member[@name="ctl_property_info_enum_t"]/*' />
    public partial struct ctl_property_info_enum_t
    {
        /// <include file='ctl_property_info_enum_t.xml' path='doc/member[@name="ctl_property_info_enum_t.SupportedTypes"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong SupportedTypes;

        /// <include file='ctl_property_info_enum_t.xml' path='doc/member[@name="ctl_property_info_enum_t.DefaultType"]/*' />
        [NativeTypeName("uint32_t")]
        public uint DefaultType;
    }
}
