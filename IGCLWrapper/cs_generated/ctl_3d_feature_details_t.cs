namespace IGCLWrapper
{
    /// <include file='ctl_3d_feature_details_t.xml' path='doc/member[@name="ctl_3d_feature_details_t"]/*' />
    public unsafe partial struct ctl_3d_feature_details_t
    {
        /// <include file='ctl_3d_feature_details_t.xml' path='doc/member[@name="ctl_3d_feature_details_t.FeatureType"]/*' />
        public ctl_3d_feature_t FeatureType;

        /// <include file='ctl_3d_feature_details_t.xml' path='doc/member[@name="ctl_3d_feature_details_t.ValueType"]/*' />
        public ctl_property_value_type_t ValueType;

        /// <include file='ctl_3d_feature_details_t.xml' path='doc/member[@name="ctl_3d_feature_details_t.Value"]/*' />
        public ctl_property_info_t Value;

        /// <include file='ctl_3d_feature_details_t.xml' path='doc/member[@name="ctl_3d_feature_details_t.CustomValueSize"]/*' />
        [NativeTypeName("int32_t")]
        public int CustomValueSize;

        /// <include file='ctl_3d_feature_details_t.xml' path='doc/member[@name="ctl_3d_feature_details_t.pCustomValue"]/*' />
        public void* pCustomValue;

        /// <include file='ctl_3d_feature_details_t.xml' path='doc/member[@name="ctl_3d_feature_details_t.PerAppSupport"]/*' />
        [NativeTypeName("bool")]
        public byte PerAppSupport;

        /// <include file='ctl_3d_feature_details_t.xml' path='doc/member[@name="ctl_3d_feature_details_t.ConflictingFeatures"]/*' />
        [NativeTypeName("int64_t")]
        public long ConflictingFeatures;

        /// <include file='ctl_3d_feature_details_t.xml' path='doc/member[@name="ctl_3d_feature_details_t.FeatureMiscSupport"]/*' />
        [NativeTypeName("int16_t")]
        public short FeatureMiscSupport;

        /// <include file='ctl_3d_feature_details_t.xml' path='doc/member[@name="ctl_3d_feature_details_t.Reserved"]/*' />
        [NativeTypeName("int16_t")]
        public short Reserved;

        /// <include file='ctl_3d_feature_details_t.xml' path='doc/member[@name="ctl_3d_feature_details_t.Reserved1"]/*' />
        [NativeTypeName("int16_t")]
        public short Reserved1;

        /// <include file='ctl_3d_feature_details_t.xml' path='doc/member[@name="ctl_3d_feature_details_t.Reserved2"]/*' />
        [NativeTypeName("int16_t")]
        public short Reserved2;
    }
}
