namespace IGCLWrapper
{
    /// <include file='ctl_scaling_caps_t.xml' path='doc/member[@name="ctl_scaling_caps_t"]/*' />
    public partial struct ctl_scaling_caps_t
    {
        /// <include file='ctl_scaling_caps_t.xml' path='doc/member[@name="ctl_scaling_caps_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_scaling_caps_t.xml' path='doc/member[@name="ctl_scaling_caps_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_scaling_caps_t.xml' path='doc/member[@name="ctl_scaling_caps_t.SupportedScaling"]/*' />
        [NativeTypeName("ctl_scaling_type_flags_t")]
        public uint SupportedScaling;
    }
}
