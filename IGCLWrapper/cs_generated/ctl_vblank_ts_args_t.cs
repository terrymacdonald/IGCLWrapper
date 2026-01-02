using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_vblank_ts_args_t.xml' path='doc/member[@name="ctl_vblank_ts_args_t"]/*' />
    public partial struct ctl_vblank_ts_args_t
    {
        /// <include file='ctl_vblank_ts_args_t.xml' path='doc/member[@name="ctl_vblank_ts_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_vblank_ts_args_t.xml' path='doc/member[@name="ctl_vblank_ts_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_vblank_ts_args_t.xml' path='doc/member[@name="ctl_vblank_ts_args_t.NumOfTargets"]/*' />
        [NativeTypeName("uint8_t")]
        public byte NumOfTargets;

        /// <include file='ctl_vblank_ts_args_t.xml' path='doc/member[@name="ctl_vblank_ts_args_t.VblankTS"]/*' />
        [NativeTypeName("uint64_t[16]")]
        public _VblankTS_e__FixedBuffer VblankTS;

        /// <include file='_VblankTS_e__FixedBuffer.xml' path='doc/member[@name="_VblankTS_e__FixedBuffer"]/*' />
        [InlineArray(16)]
        public partial struct _VblankTS_e__FixedBuffer
        {
            public ulong e0;
        }
    }
}
