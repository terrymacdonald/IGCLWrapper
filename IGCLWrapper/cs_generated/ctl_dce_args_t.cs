namespace IGCLWrapper
{
    /// <include file='ctl_dce_args_t.xml' path='doc/member[@name="ctl_dce_args_t"]/*' />
    public unsafe partial struct ctl_dce_args_t
    {
        /// <include file='ctl_dce_args_t.xml' path='doc/member[@name="ctl_dce_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_dce_args_t.xml' path='doc/member[@name="ctl_dce_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_dce_args_t.xml' path='doc/member[@name="ctl_dce_args_t.Set"]/*' />
        [NativeTypeName("bool")]
        public byte Set;

        /// <include file='ctl_dce_args_t.xml' path='doc/member[@name="ctl_dce_args_t.TargetBrightnessPercent"]/*' />
        [NativeTypeName("uint32_t")]
        public uint TargetBrightnessPercent;

        /// <include file='ctl_dce_args_t.xml' path='doc/member[@name="ctl_dce_args_t.PhaseinSpeedMultiplier"]/*' />
        public double PhaseinSpeedMultiplier;

        /// <include file='ctl_dce_args_t.xml' path='doc/member[@name="ctl_dce_args_t.NumBins"]/*' />
        [NativeTypeName("uint32_t")]
        public uint NumBins;

        /// <include file='ctl_dce_args_t.xml' path='doc/member[@name="ctl_dce_args_t.Enable"]/*' />
        [NativeTypeName("bool")]
        public byte Enable;

        /// <include file='ctl_dce_args_t.xml' path='doc/member[@name="ctl_dce_args_t.IsSupported"]/*' />
        [NativeTypeName("bool")]
        public byte IsSupported;

        /// <include file='ctl_dce_args_t.xml' path='doc/member[@name="ctl_dce_args_t.pHistogram"]/*' />
        [NativeTypeName("uint32_t *")]
        public uint* pHistogram;
    }
}
