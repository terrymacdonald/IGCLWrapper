namespace IGCLWrapper
{
    /// <include file='ctl_firmware_version_t.xml' path='doc/member[@name="ctl_firmware_version_t"]/*' />
    public partial struct ctl_firmware_version_t
    {
        /// <include file='ctl_firmware_version_t.xml' path='doc/member[@name="ctl_firmware_version_t.major_version"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong major_version;

        /// <include file='ctl_firmware_version_t.xml' path='doc/member[@name="ctl_firmware_version_t.minor_version"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong minor_version;

        /// <include file='ctl_firmware_version_t.xml' path='doc/member[@name="ctl_firmware_version_t.build_number"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong build_number;
    }
}
