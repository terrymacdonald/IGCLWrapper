using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_3d_live_state_t.xml' path='doc/member[@name="ctl_3d_live_state_t"]/*' />
    public partial struct ctl_3d_live_state_t
    {
        /// <include file='ctl_3d_live_state_t.xml' path='doc/member[@name="ctl_3d_live_state_t.GfxApi"]/*' />
        [NativeTypeName("ctl_3d_feature_misc_flags_t")]
        public uint GfxApi;

        /// <include file='ctl_3d_live_state_t.xml' path='doc/member[@name="ctl_3d_live_state_t.TargetFPS"]/*' />
        [NativeTypeName("uint32_t")]
        public uint TargetFPS;

        /// <include file='ctl_3d_live_state_t.xml' path='doc/member[@name="ctl_3d_live_state_t.FramePacingStatus"]/*' />
        public ctl_3d_live_state_frame_pacing_types_t FramePacingStatus;

        /// <include file='ctl_3d_live_state_t.xml' path='doc/member[@name="ctl_3d_live_state_t.Reserved"]/*' />
        [NativeTypeName("uint32_t[4]")]
        public _Reserved_e__FixedBuffer Reserved;

        /// <include file='_Reserved_e__FixedBuffer.xml' path='doc/member[@name="_Reserved_e__FixedBuffer"]/*' />
        [InlineArray(4)]
        public partial struct _Reserved_e__FixedBuffer
        {
            public uint e0;
        }
    }
}
