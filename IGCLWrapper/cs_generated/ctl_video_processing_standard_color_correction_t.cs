using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_video_processing_standard_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_t"]/*' />
    public partial struct ctl_video_processing_standard_color_correction_t
    {
        /// <include file='ctl_video_processing_standard_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_video_processing_standard_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_video_processing_standard_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_t.standard_color_correction_enable"]/*' />
        [NativeTypeName("bool")]
        public byte standard_color_correction_enable;

        /// <include file='ctl_video_processing_standard_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_t.brightness"]/*' />
        public float brightness;

        /// <include file='ctl_video_processing_standard_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_t.contrast"]/*' />
        public float contrast;

        /// <include file='ctl_video_processing_standard_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_t.hue"]/*' />
        public float hue;

        /// <include file='ctl_video_processing_standard_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_t.saturation"]/*' />
        public float saturation;

        /// <include file='ctl_video_processing_standard_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_standard_color_correction_t.ReservedFields"]/*' />
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
