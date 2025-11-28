namespace IGCLWrapper
{
    public unsafe partial struct _ctl_application_id_t
    {
        [NativeTypeName("uint32_t")]
        public uint Data1;

        [NativeTypeName("uint16_t")]
        public ushort Data2;

        [NativeTypeName("uint16_t")]
        public ushort Data3;

        [NativeTypeName("uint8_t[8]")]
        public fixed byte Data4[8];
    }
}
