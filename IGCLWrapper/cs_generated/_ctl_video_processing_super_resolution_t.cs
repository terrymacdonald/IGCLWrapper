namespace IGCLWrapper
{
    public unsafe partial struct _ctl_video_processing_super_resolution_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("ctl_video_processing_super_resolution_flags_t")]
        public uint super_resolution_flag;

        [NativeTypeName("bool")]
        public byte super_resolution_max_in_enabled;

        [NativeTypeName("uint32_t")]
        public uint super_resolution_max_in_width;

        [NativeTypeName("uint32_t")]
        public uint super_resolution_max_in_height;

        [NativeTypeName("bool")]
        public byte super_resolution_reboot_reset;

        [NativeTypeName("uint32_t[15]")]
        public fixed uint ReservedFields[15];

        [NativeTypeName("char[3]")]
        public fixed sbyte ReservedBytes[3];
    }
}
