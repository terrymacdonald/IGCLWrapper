namespace IGCLWrapper
{
    /// <include file='ctl_display_feature_reset_t.xml' path='doc/member[@name="ctl_display_feature_reset_t"]/*' />
    public partial struct ctl_display_feature_reset_t
    {
        /// <include file='ctl_display_feature_reset_t.xml' path='doc/member[@name="ctl_display_feature_reset_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_display_feature_reset_t.xml' path='doc/member[@name="ctl_display_feature_reset_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_display_feature_reset_t.xml' path='doc/member[@name="ctl_display_feature_reset_t.ResetFeature"]/*' />
        [NativeTypeName("ctl_display_feature_reset_flags_t")]
        public uint ResetFeature;

        /// <include file='ctl_display_feature_reset_t.xml' path='doc/member[@name="ctl_display_feature_reset_t.ResetFeatureStatus"]/*' />
        [NativeTypeName("ctl_display_feature_reset_flags_t")]
        public uint ResetFeatureStatus;
    }
}
