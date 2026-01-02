namespace IGCLWrapper
{
    /// <include file='ctl_init_args_t.xml' path='doc/member[@name="ctl_init_args_t"]/*' />
    public partial struct ctl_init_args_t
    {
        /// <include file='ctl_init_args_t.xml' path='doc/member[@name="ctl_init_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_init_args_t.xml' path='doc/member[@name="ctl_init_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_init_args_t.xml' path='doc/member[@name="ctl_init_args_t.AppVersion"]/*' />
        [NativeTypeName("ctl_version_info_t")]
        public uint AppVersion;

        /// <include file='ctl_init_args_t.xml' path='doc/member[@name="ctl_init_args_t.flags"]/*' />
        [NativeTypeName("ctl_init_flags_t")]
        public uint flags;

        /// <include file='ctl_init_args_t.xml' path='doc/member[@name="ctl_init_args_t.SupportedVersion"]/*' />
        [NativeTypeName("ctl_version_info_t")]
        public uint SupportedVersion;

        /// <include file='ctl_init_args_t.xml' path='doc/member[@name="ctl_init_args_t.ApplicationUID"]/*' />
        public ctl_application_id_t ApplicationUID;
    }
}
