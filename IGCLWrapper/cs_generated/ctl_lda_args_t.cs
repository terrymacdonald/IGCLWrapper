using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_lda_args_t.xml' path='doc/member[@name="ctl_lda_args_t"]/*' />
    public unsafe partial struct ctl_lda_args_t
    {
        /// <include file='ctl_lda_args_t.xml' path='doc/member[@name="ctl_lda_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_lda_args_t.xml' path='doc/member[@name="ctl_lda_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_lda_args_t.xml' path='doc/member[@name="ctl_lda_args_t.NumAdapters"]/*' />
        [NativeTypeName("uint8_t")]
        public byte NumAdapters;

        /// <include file='ctl_lda_args_t.xml' path='doc/member[@name="ctl_lda_args_t.hLinkedAdapters"]/*' />
        [NativeTypeName("ctl_device_adapter_handle_t *")]
        public _ctl_device_adapter_handle_t** hLinkedAdapters;

        /// <include file='ctl_lda_args_t.xml' path='doc/member[@name="ctl_lda_args_t.Reserved"]/*' />
        [NativeTypeName("uint64_t[4]")]
        public _Reserved_e__FixedBuffer Reserved;

        /// <include file='_Reserved_e__FixedBuffer.xml' path='doc/member[@name="_Reserved_e__FixedBuffer"]/*' />
        [InlineArray(4)]
        public partial struct _Reserved_e__FixedBuffer
        {
            public ulong e0;
        }
    }
}
