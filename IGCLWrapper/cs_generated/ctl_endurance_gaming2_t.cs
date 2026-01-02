using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_endurance_gaming2_t.xml' path='doc/member[@name="ctl_endurance_gaming2_t"]/*' />
    public partial struct ctl_endurance_gaming2_t
    {
        /// <include file='ctl_endurance_gaming2_t.xml' path='doc/member[@name="ctl_endurance_gaming2_t.EGControl"]/*' />
        public ctl_3d_endurance_gaming_control_t EGControl;

        /// <include file='ctl_endurance_gaming2_t.xml' path='doc/member[@name="ctl_endurance_gaming2_t.EGMode"]/*' />
        public ctl_3d_endurance_gaming_mode_t EGMode;

        /// <include file='ctl_endurance_gaming2_t.xml' path='doc/member[@name="ctl_endurance_gaming2_t.IsFPRequired"]/*' />
        [NativeTypeName("bool")]
        public byte IsFPRequired;

        /// <include file='ctl_endurance_gaming2_t.xml' path='doc/member[@name="ctl_endurance_gaming2_t.TargetFPS"]/*' />
        public double TargetFPS;

        /// <include file='ctl_endurance_gaming2_t.xml' path='doc/member[@name="ctl_endurance_gaming2_t.RefreshRate"]/*' />
        public double RefreshRate;

        /// <include file='ctl_endurance_gaming2_t.xml' path='doc/member[@name="ctl_endurance_gaming2_t.Reserved"]/*' />
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
