namespace IGCLWrapper
{
    /// <include file='ctl_sharpness_settings_t.xml' path='doc/member[@name="ctl_sharpness_settings_t"]/*' />
    public partial struct ctl_sharpness_settings_t
    {
        /// <include file='ctl_sharpness_settings_t.xml' path='doc/member[@name="ctl_sharpness_settings_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_sharpness_settings_t.xml' path='doc/member[@name="ctl_sharpness_settings_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_sharpness_settings_t.xml' path='doc/member[@name="ctl_sharpness_settings_t.Enable"]/*' />
        [NativeTypeName("bool")]
        public byte Enable;

        /// <include file='ctl_sharpness_settings_t.xml' path='doc/member[@name="ctl_sharpness_settings_t.FilterType"]/*' />
        [NativeTypeName("ctl_sharpness_filter_type_flags_t")]
        public uint FilterType;

        /// <include file='ctl_sharpness_settings_t.xml' path='doc/member[@name="ctl_sharpness_settings_t.Intensity"]/*' />
        public float Intensity;
    }
}
