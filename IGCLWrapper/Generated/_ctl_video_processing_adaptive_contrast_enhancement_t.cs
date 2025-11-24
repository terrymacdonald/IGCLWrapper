namespace IGCLWrapper
{
    public unsafe partial struct _ctl_video_processing_adaptive_contrast_enhancement_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("ctl_property_uint_t")]
        public _ctl_property_uint_t adaptive_contrast_enhancement;

        [NativeTypeName("ctl_property_boolean_t")]
        public _ctl_property_boolean_t adaptive_contrast_enhancement_coexistence;

        [NativeTypeName("uint32_t[16]")]
        public fixed uint ReservedFields[16];
    }
}
