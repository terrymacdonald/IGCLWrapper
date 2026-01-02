namespace IGCLWrapper
{
    /// <include file='ctl_3d_feature_caps_t.xml' path='doc/member[@name="ctl_3d_feature_caps_t"]/*' />
    public unsafe partial struct ctl_3d_feature_caps_t
    {
        /// <include file='ctl_3d_feature_caps_t.xml' path='doc/member[@name="ctl_3d_feature_caps_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_3d_feature_caps_t.xml' path='doc/member[@name="ctl_3d_feature_caps_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_3d_feature_caps_t.xml' path='doc/member[@name="ctl_3d_feature_caps_t.NumSupportedFeatures"]/*' />
        [NativeTypeName("uint32_t")]
        public uint NumSupportedFeatures;

        /// <include file='ctl_3d_feature_caps_t.xml' path='doc/member[@name="ctl_3d_feature_caps_t.pFeatureDetails"]/*' />
        public ctl_3d_feature_details_t* pFeatureDetails;
    }
}
