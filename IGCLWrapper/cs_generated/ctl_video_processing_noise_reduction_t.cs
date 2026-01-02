using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_video_processing_noise_reduction_t.xml' path='doc/member[@name="ctl_video_processing_noise_reduction_t"]/*' />
    public partial struct ctl_video_processing_noise_reduction_t
    {
        /// <include file='ctl_video_processing_noise_reduction_t.xml' path='doc/member[@name="ctl_video_processing_noise_reduction_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_video_processing_noise_reduction_t.xml' path='doc/member[@name="ctl_video_processing_noise_reduction_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_video_processing_noise_reduction_t.xml' path='doc/member[@name="ctl_video_processing_noise_reduction_t.noise_reduction"]/*' />
        public ctl_property_uint_t noise_reduction;

        /// <include file='ctl_video_processing_noise_reduction_t.xml' path='doc/member[@name="ctl_video_processing_noise_reduction_t.noise_reduction_auto_detect"]/*' />
        public ctl_property_boolean_t noise_reduction_auto_detect;

        /// <include file='ctl_video_processing_noise_reduction_t.xml' path='doc/member[@name="ctl_video_processing_noise_reduction_t.ReservedFields"]/*' />
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
