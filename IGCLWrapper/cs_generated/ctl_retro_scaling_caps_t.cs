namespace IGCLWrapper
{
    /// <include file='ctl_retro_scaling_caps_t.xml' path='doc/member[@name="ctl_retro_scaling_caps_t"]/*' />
    public partial struct ctl_retro_scaling_caps_t
    {
        /// <include file='ctl_retro_scaling_caps_t.xml' path='doc/member[@name="ctl_retro_scaling_caps_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_retro_scaling_caps_t.xml' path='doc/member[@name="ctl_retro_scaling_caps_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_retro_scaling_caps_t.xml' path='doc/member[@name="ctl_retro_scaling_caps_t.SupportedRetroScaling"]/*' />
        [NativeTypeName("ctl_retro_scaling_type_flags_t")]
        public uint SupportedRetroScaling;
    }
}
