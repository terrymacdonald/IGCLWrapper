namespace IGCLWrapper
{
    /// <include file='ctl_pixtx_3dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_3dlut_config_t"]/*' />
    public unsafe partial struct ctl_pixtx_3dlut_config_t
    {
        /// <include file='ctl_pixtx_3dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_3dlut_config_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_pixtx_3dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_3dlut_config_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_pixtx_3dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_3dlut_config_t.NumSamplesPerChannel"]/*' />
        [NativeTypeName("uint32_t")]
        public uint NumSamplesPerChannel;

        /// <include file='ctl_pixtx_3dlut_config_t.xml' path='doc/member[@name="ctl_pixtx_3dlut_config_t.pSampleValues"]/*' />
        public ctl_pixtx_3dlut_sample_t* pSampleValues;
    }
}
