namespace IGCLWrapper
{
    public unsafe partial struct _ctl_video_processing_noise_reduction_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("ctl_property_uint_t")]
        public _ctl_property_uint_t noise_reduction;

        [NativeTypeName("ctl_property_boolean_t")]
        public _ctl_property_boolean_t noise_reduction_auto_detect;

        [NativeTypeName("uint32_t[16]")]
        public fixed uint ReservedFields[16];
    }
}
