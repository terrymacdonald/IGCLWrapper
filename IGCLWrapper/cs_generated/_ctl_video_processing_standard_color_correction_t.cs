namespace IGCLWrapper
{
    public unsafe partial struct _ctl_video_processing_standard_color_correction_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("bool")]
        public byte standard_color_correction_enable;

        public float brightness;

        public float contrast;

        public float hue;

        public float saturation;

        [NativeTypeName("uint32_t[16]")]
        public fixed uint ReservedFields[16];
    }
}
