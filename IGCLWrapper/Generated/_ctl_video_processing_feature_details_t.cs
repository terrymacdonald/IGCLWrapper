namespace IGCLWrapper
{
    public unsafe partial struct _ctl_video_processing_feature_details_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("ctl_video_processing_feature_t")]
        public _ctl_video_processing_feature_t FeatureType;

        [NativeTypeName("ctl_property_value_type_t")]
        public _ctl_property_value_type_t ValueType;

        [NativeTypeName("ctl_property_info_t")]
        public _ctl_property_info_t Value;

        [NativeTypeName("int32_t")]
        public int CustomValueSize;

        public void* pCustomValue;

        [NativeTypeName("uint32_t[16]")]
        public fixed uint ReservedFields[16];
    }
}
