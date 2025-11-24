namespace IGCLWrapper
{
    public unsafe partial struct _ctl_aux_access_args_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("ctl_operation_type_t")]
        public _ctl_operation_type_t OpType;

        [NativeTypeName("ctl_aux_flags_t")]
        public uint Flags;

        [NativeTypeName("uint32_t")]
        public uint Address;

        [NativeTypeName("uint64_t")]
        public ulong RAD;

        [NativeTypeName("uint32_t")]
        public uint PortID;

        [NativeTypeName("uint32_t")]
        public uint DataSize;

        [NativeTypeName("uint8_t[132]")]
        public fixed byte Data[132];
    }
}
