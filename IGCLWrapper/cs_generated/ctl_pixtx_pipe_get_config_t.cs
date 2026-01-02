namespace IGCLWrapper
{
    /// <include file='ctl_pixtx_pipe_get_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_get_config_t"]/*' />
    public unsafe partial struct ctl_pixtx_pipe_get_config_t
    {
        /// <include file='ctl_pixtx_pipe_get_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_get_config_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_pixtx_pipe_get_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_get_config_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_pixtx_pipe_get_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_get_config_t.QueryType"]/*' />
        public ctl_pixtx_config_query_type_t QueryType;

        /// <include file='ctl_pixtx_pipe_get_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_get_config_t.InputPixelFormat"]/*' />
        public ctl_pixtx_pixel_format_t InputPixelFormat;

        /// <include file='ctl_pixtx_pipe_get_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_get_config_t.OutputPixelFormat"]/*' />
        public ctl_pixtx_pixel_format_t OutputPixelFormat;

        /// <include file='ctl_pixtx_pipe_get_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_get_config_t.NumBlocks"]/*' />
        [NativeTypeName("uint32_t")]
        public uint NumBlocks;

        /// <include file='ctl_pixtx_pipe_get_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_get_config_t.pBlockConfigs"]/*' />
        public ctl_pixtx_block_config_t* pBlockConfigs;
    }
}
