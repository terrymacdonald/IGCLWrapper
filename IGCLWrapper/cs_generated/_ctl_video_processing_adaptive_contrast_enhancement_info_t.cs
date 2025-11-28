namespace IGCLWrapper
{
    public unsafe partial struct _ctl_video_processing_adaptive_contrast_enhancement_info_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("ctl_property_info_uint_t")]
        public _ctl_property_info_uint_t adaptive_contrast_enhancement;

        [NativeTypeName("bool")]
        public byte adaptive_contrast_enhancement_coexistence_supported;

        [NativeTypeName("ctl_property_info_boolean_t")]
        public _ctl_property_info_boolean_t adaptive_contrast_enhancement_coexistence;

        [NativeTypeName("uint32_t[16]")]
        public fixed uint ReservedFields[16];
    }
}
