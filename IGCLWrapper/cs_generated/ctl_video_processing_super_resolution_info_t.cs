using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_video_processing_super_resolution_info_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_info_t"]/*' />
    public partial struct ctl_video_processing_super_resolution_info_t
    {
        /// <include file='ctl_video_processing_super_resolution_info_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_info_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_video_processing_super_resolution_info_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_info_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_video_processing_super_resolution_info_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_info_t.super_resolution_flag"]/*' />
        [NativeTypeName("ctl_video_processing_super_resolution_flags_t")]
        public uint super_resolution_flag;

        /// <include file='ctl_video_processing_super_resolution_info_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_info_t.super_resolution_range_in_width"]/*' />
        public ctl_property_info_uint_t super_resolution_range_in_width;

        /// <include file='ctl_video_processing_super_resolution_info_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_info_t.super_resolution_range_in_height"]/*' />
        public ctl_property_info_uint_t super_resolution_range_in_height;

        /// <include file='ctl_video_processing_super_resolution_info_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_info_t.ReservedFields"]/*' />
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
