namespace IGCLWrapper
{
    /// <include file='ctl_3d_feature_getset_t.xml' path='doc/member[@name="ctl_3d_feature_getset_t"]/*' />
    public unsafe partial struct ctl_3d_feature_getset_t
    {
        /// <include file='ctl_3d_feature_getset_t.xml' path='doc/member[@name="ctl_3d_feature_getset_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_3d_feature_getset_t.xml' path='doc/member[@name="ctl_3d_feature_getset_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_3d_feature_getset_t.xml' path='doc/member[@name="ctl_3d_feature_getset_t.FeatureType"]/*' />
        public ctl_3d_feature_t FeatureType;

        /// <include file='ctl_3d_feature_getset_t.xml' path='doc/member[@name="ctl_3d_feature_getset_t.ApplicationName"]/*' />
        [NativeTypeName("char *")]
        public sbyte* ApplicationName;

        /// <include file='ctl_3d_feature_getset_t.xml' path='doc/member[@name="ctl_3d_feature_getset_t.ApplicationNameLength"]/*' />
        [NativeTypeName("int8_t")]
        public sbyte ApplicationNameLength;

        /// <include file='ctl_3d_feature_getset_t.xml' path='doc/member[@name="ctl_3d_feature_getset_t.bSet"]/*' />
        [NativeTypeName("bool")]
        public byte bSet;

        /// <include file='ctl_3d_feature_getset_t.xml' path='doc/member[@name="ctl_3d_feature_getset_t.ValueType"]/*' />
        public ctl_property_value_type_t ValueType;

        /// <include file='ctl_3d_feature_getset_t.xml' path='doc/member[@name="ctl_3d_feature_getset_t.Value"]/*' />
        public ctl_property_t Value;

        /// <include file='ctl_3d_feature_getset_t.xml' path='doc/member[@name="ctl_3d_feature_getset_t.CustomValueSize"]/*' />
        [NativeTypeName("int32_t")]
        public int CustomValueSize;

        /// <include file='ctl_3d_feature_getset_t.xml' path='doc/member[@name="ctl_3d_feature_getset_t.pCustomValue"]/*' />
        public void* pCustomValue;
    }
}
