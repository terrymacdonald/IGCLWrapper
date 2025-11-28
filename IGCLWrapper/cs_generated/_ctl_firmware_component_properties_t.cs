namespace IGCLWrapper
{
    public unsafe partial struct _ctl_firmware_component_properties_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("char[64]")]
        public fixed sbyte name[64];

        [NativeTypeName("char[64]")]
        public fixed sbyte version[64];

        [NativeTypeName("char[20]")]
        public fixed sbyte reserved[20];
    }
}
