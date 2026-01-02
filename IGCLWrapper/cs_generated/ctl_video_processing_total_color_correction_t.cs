using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_video_processing_total_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_t"]/*' />
    public partial struct ctl_video_processing_total_color_correction_t
    {
        /// <include file='ctl_video_processing_total_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_video_processing_total_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_video_processing_total_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_t.total_color_correction_enable"]/*' />
        [NativeTypeName("bool")]
        public byte total_color_correction_enable;

        /// <include file='ctl_video_processing_total_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_t.red"]/*' />
        [NativeTypeName("uint32_t")]
        public uint red;

        /// <include file='ctl_video_processing_total_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_t.green"]/*' />
        [NativeTypeName("uint32_t")]
        public uint green;

        /// <include file='ctl_video_processing_total_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_t.blue"]/*' />
        [NativeTypeName("uint32_t")]
        public uint blue;

        /// <include file='ctl_video_processing_total_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_t.yellow"]/*' />
        [NativeTypeName("uint32_t")]
        public uint yellow;

        /// <include file='ctl_video_processing_total_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_t.cyan"]/*' />
        [NativeTypeName("uint32_t")]
        public uint cyan;

        /// <include file='ctl_video_processing_total_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_t.magenta"]/*' />
        [NativeTypeName("uint32_t")]
        public uint magenta;

        /// <include file='ctl_video_processing_total_color_correction_t.xml' path='doc/member[@name="ctl_video_processing_total_color_correction_t.ReservedFields"]/*' />
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
