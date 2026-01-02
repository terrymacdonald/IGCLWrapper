using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_set_brightness_t.xml' path='doc/member[@name="ctl_set_brightness_t"]/*' />
    public partial struct ctl_set_brightness_t
    {
        /// <include file='ctl_set_brightness_t.xml' path='doc/member[@name="ctl_set_brightness_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_set_brightness_t.xml' path='doc/member[@name="ctl_set_brightness_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_set_brightness_t.xml' path='doc/member[@name="ctl_set_brightness_t.TargetBrightness"]/*' />
        [NativeTypeName("uint32_t")]
        public uint TargetBrightness;

        /// <include file='ctl_set_brightness_t.xml' path='doc/member[@name="ctl_set_brightness_t.SmoothTransitionTimeInMs"]/*' />
        [NativeTypeName("uint32_t")]
        public uint SmoothTransitionTimeInMs;

        /// <include file='ctl_set_brightness_t.xml' path='doc/member[@name="ctl_set_brightness_t.ReservedFields"]/*' />
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
