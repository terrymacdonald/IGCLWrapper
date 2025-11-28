namespace IGCLWrapper
{
    public unsafe partial struct _ctl_get_brightness_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("uint32_t")]
        public uint TargetBrightness;

        [NativeTypeName("uint32_t")]
        public uint CurrentBrightness;

        [NativeTypeName("uint32_t[4]")]
        public fixed uint ReservedFields[4];
    }
}
