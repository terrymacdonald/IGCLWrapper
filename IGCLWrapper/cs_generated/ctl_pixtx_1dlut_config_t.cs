namespace IGCLWrapper
{
    /// <include file='ctl_pixtx_1dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_1dlut_config_t"]/*' />
    public unsafe partial struct ctl_pixtx_1dlut_config_t
    {
        /// <include file='ctl_pixtx_1dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_1dlut_config_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_pixtx_1dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_1dlut_config_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_pixtx_1dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_1dlut_config_t.SamplingType"]/*' />
        public ctl_pixtx_lut_sampling_type_t SamplingType;

        /// <include file='ctl_pixtx_1dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_1dlut_config_t.NumSamplesPerChannel"]/*' />
        [NativeTypeName("uint32_t")]
        public uint NumSamplesPerChannel;

        /// <include file='ctl_pixtx_1dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_1dlut_config_t.NumChannels"]/*' />
        [NativeTypeName("uint32_t")]
        public uint NumChannels;

        /// <include file='ctl_pixtx_1dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_1dlut_config_t.pSampleValues"]/*' />
        public double* pSampleValues;

        /// <include file='ctl_pixtx_1dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_1dlut_config_t.pSamplePositions"]/*' />
        public double* pSamplePositions;
    }
}
