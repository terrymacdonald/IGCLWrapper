using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_child_display_target_mode_t.xml' path='doc/member[@name="ctl_child_display_target_mode_t"]/*' />
    public partial struct ctl_child_display_target_mode_t
    {
        /// <include file='ctl_child_display_target_mode_t.xml' path='doc/member[@name="ctl_child_display_target_mode_t.Width"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Width;

        /// <include file='ctl_child_display_target_mode_t.xml' path='doc/member[@name="ctl_child_display_target_mode_t.Height"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Height;

        /// <include file='ctl_child_display_target_mode_t.xml' path='doc/member[@name="ctl_child_display_target_mode_t.RefreshRate"]/*' />
        public float RefreshRate;

        /// <include file='ctl_child_display_target_mode_t.xml' path='doc/member[@name="ctl_child_display_target_mode_t.ReservedFields"]/*' />
        [NativeTypeName("uint32_t[4]")]
        public _ReservedFields_e__FixedBuffer ReservedFields;

        /// <include file='_ReservedFields_e__FixedBuffer.xml' path='doc/member[@name="_ReservedFields_e__FixedBuffer"]/*' />
        [InlineArray(4)]
        public partial struct _ReservedFields_e__FixedBuffer
        {
            public uint e0;
        }
    }
}
