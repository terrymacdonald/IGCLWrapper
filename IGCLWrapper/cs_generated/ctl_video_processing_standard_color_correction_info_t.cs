using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_video_processing_standard_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_info_t"]/*' />
    public partial struct ctl_video_processing_standard_color_correction_info_t
    {
        /// <include file='ctl_video_processing_standard_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_info_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_video_processing_standard_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_info_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_video_processing_standard_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_info_t.standard_color_correction_default_enable"]/*' />
        [NativeTypeName("bool")]
        public byte standard_color_correction_default_enable;

        /// <include file='ctl_video_processing_standard_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_info_t.brightness"]/*' />
        public ctl_property_info_float_t brightness;

        /// <include file='ctl_video_processing_standard_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_info_t.contrast"]/*' />
        public ctl_property_info_float_t contrast;

        /// <include file='ctl_video_processing_standard_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_info_t.hue"]/*' />
        public ctl_property_info_float_t hue;

        /// <include file='ctl_video_processing_standard_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_info_t.saturation"]/*' />
        public ctl_property_info_float_t saturation;

        /// <include file='ctl_video_processing_standard_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_info_t.ReservedFields"]/*' />
        [NativeTypeName("uint32_t[16]")]
        public _ReservedFields_e__FixedBuffer ReservedFields;

        /// <include file='_ReservedFields_e__FixedBuffer.xml' path='doc/member[@name="_ReservedFields_e__FixedBuffer"]/*' />
        [InlineArray(16)]
        public partial struct _ReservedFields_e__FixedBuffer
        {
            public uint e0;
        }
    }
}
