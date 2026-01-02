namespace IGCLWrapper
{
    /// <include file='ctl_scaling_settings_t.xml' path='doc/member[@name="ctl_scaling_settings_t"]/*' />
    public partial struct ctl_scaling_settings_t
    {
        /// <include file='ctl_scaling_settings_t.xml' path='doc/member[@name="ctl_scaling_settings_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_scaling_settings_t.xml' path='doc/member[@name="ctl_scaling_settings_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_scaling_settings_t.xml' path='doc/member[@name="ctl_scaling_settings_t.Enable"]/*' />
        [NativeTypeName("bool")]
        public byte Enable;

        /// <include file='ctl_scaling_settings_t.xml' path='doc/member[@name="ctl_scaling_settings_t.ScalingType"]/*' />
        [NativeTypeName("ctl_scaling_type_flags_t")]
        public uint ScalingType;

        /// <include file='ctl_scaling_settings_t.xml' path='doc/member[@name="ctl_scaling_settings_t.CustomScalingX"]/*' />
        [NativeTypeName("uint32_t")]
        public uint CustomScalingX;

        /// <include file='ctl_scaling_settings_t.xml' path='doc/member[@name="ctl_scaling_settings_t.CustomScalingY"]/*' />
        [NativeTypeName("uint32_t")]
        public uint CustomScalingY;

        /// <include file='ctl_scaling_settings_t.xml' path='doc/member[@name="ctl_scaling_settings_t.HardwareModeSet"]/*' />
        [NativeTypeName("bool")]
        public byte HardwareModeSet;

        /// <include file='ctl_scaling_settings_t.xml' path='doc/member[@name="ctl_scaling_settings_t.PreferredScalingType"]/*' />
        [NativeTypeName("ctl_scaling_type_flags_t")]
        public uint PreferredScalingType;
    }
}
