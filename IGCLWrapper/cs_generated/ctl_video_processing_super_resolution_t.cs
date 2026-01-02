using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_video_processing_super_resolution_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_t"]/*' />
    public partial struct ctl_video_processing_super_resolution_t
    {
        /// <include file='ctl_video_processing_super_resolution_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_video_processing_super_resolution_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_video_processing_super_resolution_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_t.super_resolution_flag"]/*' />
        [NativeTypeName("ctl_video_processing_super_resolution_flags_t")]
        public uint super_resolution_flag;

        /// <include file='ctl_video_processing_super_resolution_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_t.super_resolution_max_in_enabled"]/*' />
        [NativeTypeName("bool")]
        public byte super_resolution_max_in_enabled;

        /// <include file='ctl_video_processing_super_resolution_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_t.super_resolution_max_in_width"]/*' />
        [NativeTypeName("uint32_t")]
        public uint super_resolution_max_in_width;

        /// <include file='ctl_video_processing_super_resolution_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_t.super_resolution_max_in_height"]/*' />
        [NativeTypeName("uint32_t")]
        public uint super_resolution_max_in_height;

        /// <include file='ctl_video_processing_super_resolution_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_t.super_resolution_reboot_reset"]/*' />
        [NativeTypeName("bool")]
        public byte super_resolution_reboot_reset;

        /// <include file='ctl_video_processing_super_resolution_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_t.ReservedFields"]/*' />
        [NativeTypeName("uint32_t[15]")]
        public _ReservedFields_e__FixedBuffer ReservedFields;

        /// <include file='ctl_video_processing_super_resolution_t.xml' path='doc/member[@name="ctl_video_processing_super_resolution_t.ReservedBytes"]/*' />
        [NativeTypeName("char[3]")]
        public _ReservedBytes_e__FixedBuffer ReservedBytes;

        /// <include file='_ReservedFields_e__FixedBuffer.xml' path='doc/member[@name="_ReservedFields_e__FixedBuffer"]/*' />
        [InlineArray(15)]
        public partial struct _ReservedFields_e__FixedBuffer
        {
            public uint e0;
        }

        /// <include file='_ReservedBytes_e__FixedBuffer.xml' path='doc/member[@name="_ReservedBytes_e__FixedBuffer"]/*' />
        [InlineArray(3)]
        public partial struct _ReservedBytes_e__FixedBuffer
        {
            public sbyte e0;
        }
    }
}
