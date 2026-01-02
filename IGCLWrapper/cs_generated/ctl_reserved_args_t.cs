namespace IGCLWrapper
{
    /// <include file='ctl_reserved_args_t.xml' path='doc/member[@name="ctl_reserved_args_t"]/*' />
    public unsafe partial struct ctl_reserved_args_t
    {
        /// <include file='ctl_reserved_args_t.xml' path='doc/member[@name="ctl_reserved_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_reserved_args_t.xml' path='doc/member[@name="ctl_reserved_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_reserved_args_t.xml' path='doc/member[@name="ctl_reserved_args_t.pSpecialArg"]/*' />
        public void* pSpecialArg;

        /// <include file='ctl_reserved_args_t.xml' path='doc/member[@name="ctl_reserved_args_t.ArgSize"]/*' />
        [NativeTypeName("uint32_t")]
        public uint ArgSize;
    }
}
