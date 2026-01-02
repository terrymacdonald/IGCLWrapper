namespace IGCLWrapper
{
    /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t"]/*' />
    public partial struct ctl_display_timing_t
    {
        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.PixelClock"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong PixelClock;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.HActive"]/*' />
        [NativeTypeName("uint32_t")]
        public uint HActive;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.VActive"]/*' />
        [NativeTypeName("uint32_t")]
        public uint VActive;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.HTotal"]/*' />
        [NativeTypeName("uint32_t")]
        public uint HTotal;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.VTotal"]/*' />
        [NativeTypeName("uint32_t")]
        public uint VTotal;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.HBlank"]/*' />
        [NativeTypeName("uint32_t")]
        public uint HBlank;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.VBlank"]/*' />
        [NativeTypeName("uint32_t")]
        public uint VBlank;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.HSync"]/*' />
        [NativeTypeName("uint32_t")]
        public uint HSync;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.VSync"]/*' />
        [NativeTypeName("uint32_t")]
        public uint VSync;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.RefreshRate"]/*' />
        public float RefreshRate;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.SignalStandard"]/*' />
        public ctl_signal_standard_type_t SignalStandard;

        /// <include file='ctl_display_timing_t.xml' path='doc/member[@name="ctl_display_timing_t.VicId"]/*' />
        [NativeTypeName("uint8_t")]
        public byte VicId;
    }
}
