namespace IGCLWrapper
{
    /// <include file='ctl_sharpness_caps_t.xml' path='doc/member[@name="ctl_sharpness_caps_t"]/*' />
    public unsafe partial struct ctl_sharpness_caps_t
    {
        /// <include file='ctl_sharpness_caps_t.xml' path='doc/member[@name="ctl_sharpness_caps_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_sharpness_caps_t.xml' path='doc/member[@name="ctl_sharpness_caps_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_sharpness_caps_t.xml' path='doc/member[@name="ctl_sharpness_caps_t.SupportedFilterFlags"]/*' />
        [NativeTypeName("ctl_sharpness_filter_type_flags_t")]
        public uint SupportedFilterFlags;

        /// <include file='ctl_sharpness_caps_t.xml' path='doc/member[@name="ctl_sharpness_caps_t.NumFilterTypes"]/*' />
        [NativeTypeName("uint8_t")]
        public byte NumFilterTypes;

        /// <include file='ctl_sharpness_caps_t.xml' path='doc/member[@name="ctl_sharpness_caps_t.pFilterProperty"]/*' />
        public ctl_sharpness_filter_properties_t* pFilterProperty;
    }
}
