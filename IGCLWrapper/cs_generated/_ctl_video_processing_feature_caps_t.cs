namespace IGCLWrapper
{
    public unsafe partial struct _ctl_video_processing_feature_caps_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("uint32_t")]
        public uint NumSupportedFeatures;

        [NativeTypeName("ctl_video_processing_feature_details_t *")]
        public _ctl_video_processing_feature_details_t* pFeatureDetails;

        [NativeTypeName("uint32_t[16]")]
        public fixed uint ReservedFields[16];
    }
}
