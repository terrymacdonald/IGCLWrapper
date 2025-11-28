namespace IGCLWrapper
{
    public unsafe partial struct _ctl_video_processing_total_color_correction_info_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("bool")]
        public byte total_color_correction_default_enable;

        [NativeTypeName("ctl_property_info_uint_t")]
        public _ctl_property_info_uint_t red;

        [NativeTypeName("ctl_property_info_uint_t")]
        public _ctl_property_info_uint_t green;

        [NativeTypeName("ctl_property_info_uint_t")]
        public _ctl_property_info_uint_t blue;

        [NativeTypeName("ctl_property_info_uint_t")]
        public _ctl_property_info_uint_t yellow;

        [NativeTypeName("ctl_property_info_uint_t")]
        public _ctl_property_info_uint_t cyan;

        [NativeTypeName("ctl_property_info_uint_t")]
        public _ctl_property_info_uint_t magenta;

        [NativeTypeName("uint32_t[16]")]
        public fixed uint ReservedFields[16];
    }
}
