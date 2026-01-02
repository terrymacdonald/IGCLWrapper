namespace IGCLWrapper
{
    /// <include file='ctl_runtime_path_args_t.xml' path='doc/member[@name="ctl_runtime_path_args_t"]/*' />
    public unsafe partial struct ctl_runtime_path_args_t
    {
        /// <include file='ctl_runtime_path_args_t.xml' path='doc/member[@name="ctl_runtime_path_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_runtime_path_args_t.xml' path='doc/member[@name="ctl_runtime_path_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_runtime_path_args_t.xml' path='doc/member[@name="ctl_runtime_path_args_t.UnlockID"]/*' />
        public ctl_application_id_t UnlockID;

        /// <include file='ctl_runtime_path_args_t.xml' path='doc/member[@name="ctl_runtime_path_args_t.pRuntimePath"]/*' />
        [NativeTypeName("wchar_t *")]
        public ushort* pRuntimePath;

        /// <include file='ctl_runtime_path_args_t.xml' path='doc/member[@name="ctl_runtime_path_args_t.DeviceID"]/*' />
        [NativeTypeName("uint16_t")]
        public ushort DeviceID;

        /// <include file='ctl_runtime_path_args_t.xml' path='doc/member[@name="ctl_runtime_path_args_t.RevID"]/*' />
        [NativeTypeName("uint8_t")]
        public byte RevID;
    }
}
