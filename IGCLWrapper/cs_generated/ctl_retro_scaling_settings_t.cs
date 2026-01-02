namespace IGCLWrapper
{
    /// <include file='ctl_retro_scaling_settings_t.xml' path='doc/member[@name="ctl_retro_scaling_settings_t"]/*' />
    public partial struct ctl_retro_scaling_settings_t
    {
        /// <include file='ctl_retro_scaling_settings_t.xml' path='doc/member[@name="ctl_retro_scaling_settings_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_retro_scaling_settings_t.xml' path='doc/member[@name="ctl_retro_scaling_settings_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_retro_scaling_settings_t.xml' path='doc/member[@name="ctl_retro_scaling_settings_t.Get"]/*' />
        [NativeTypeName("bool")]
        public byte Get;

        /// <include file='ctl_retro_scaling_settings_t.xml' path='doc/member[@name="ctl_retro_scaling_settings_t.Enable"]/*' />
        [NativeTypeName("bool")]
        public byte Enable;

        /// <include file='ctl_retro_scaling_settings_t.xml' path='doc/member[@name="ctl_retro_scaling_settings_t.RetroScalingType"]/*' />
        [NativeTypeName("ctl_retro_scaling_type_flags_t")]
        public uint RetroScalingType;
    }
}
