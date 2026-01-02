using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_video_processing_adaptive_contrast_enhancement_t.xml' path='doc/member[@name="ctl_video_processing_adaptive_contrast_enhancement_t"]/*' />
    public partial struct ctl_video_processing_adaptive_contrast_enhancement_t
    {
        /// <include file='ctl_video_processing_adaptive_contrast_enhancement_t.xml' path='doc/member[@name="ctl_video_processing_adaptive_contrast_enhancement_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_video_processing_adaptive_contrast_enhancement_t.xml' path='doc/member[@name="ctl_video_processing_adaptive_contrast_enhancement_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_video_processing_adaptive_contrast_enhancement_t.xml' path='doc/member[@name="ctl_video_processing_adaptive_contrast_enhancement_t.adaptive_contrast_enhancement"]/*' />
        public ctl_property_uint_t adaptive_contrast_enhancement;

        /// <include file='ctl_video_processing_adaptive_contrast_enhancement_t.xml' path='doc/member[@name="ctl_video_processing_adaptive_contrast_enhancement_t.adaptive_contrast_enhancement_coexistence"]/*' />
        public ctl_property_boolean_t adaptive_contrast_enhancement_coexistence;

        /// <include file='ctl_video_processing_adaptive_contrast_enhancement_t.xml' path='doc/member[@name="ctl_video_processing_adaptive_contrast_enhancement_t.ReservedFields"]/*' />
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
