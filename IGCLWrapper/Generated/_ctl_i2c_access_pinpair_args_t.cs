namespace IGCLWrapper
{
    public unsafe partial struct _ctl_i2c_access_pinpair_args_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("uint32_t")]
        public uint DataSize;

        [NativeTypeName("uint32_t")]
        public uint Address;

        [NativeTypeName("ctl_operation_type_t")]
        public _ctl_operation_type_t OpType;

        [NativeTypeName("uint32_t")]
        public uint Offset;

        [NativeTypeName("ctl_i2c_pinpair_flags_t")]
        public uint Flags;

        [NativeTypeName("uint8_t[128]")]
        public fixed byte Data[128];

        [NativeTypeName("uint32_t[4]")]
        public fixed uint ReservedFields[4];
    }
}
