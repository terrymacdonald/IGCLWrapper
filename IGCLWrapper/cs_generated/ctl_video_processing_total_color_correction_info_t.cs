using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_video_processing_total_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_info_t"]/*' />
    public partial struct ctl_video_processing_total_color_correction_info_t
    {
        /// <include file='ctl_video_processing_total_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_info_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_video_processing_total_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_info_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_video_processing_total_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_info_t.total_color_correction_default_enable"]/*' />
        [NativeTypeName("bool")]
        public byte total_color_correction_default_enable;

        /// <include file='ctl_video_processing_total_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_info_t.red"]/*' />
        public ctl_property_info_uint_t red;

        /// <include file='ctl_video_processing_total_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_info_t.green"]/*' />
        public ctl_property_info_uint_t green;

        /// <include file='ctl_video_processing_total_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_info_t.blue"]/*' />
        public ctl_property_info_uint_t blue;

        /// <include file='ctl_video_processing_total_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_info_t.yellow"]/*' />
        public ctl_property_info_uint_t yellow;

        /// <include file='ctl_video_processing_total_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_info_t.cyan"]/*' />
        public ctl_property_info_uint_t cyan;

        /// <include file='ctl_video_processing_total_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_info_t.magenta"]/*' />
        public ctl_property_info_uint_t magenta;

        /// <include file='ctl_video_processing_total_color_correction_info_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_info_t.ReservedFields"]/*' />
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
