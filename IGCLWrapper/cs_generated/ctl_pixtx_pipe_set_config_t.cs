namespace IGCLWrapper
{
    /// <include file='ctl_pixtx_pipe_set_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_set_config_t"]/*' />
    public unsafe partial struct ctl_pixtx_pipe_set_config_t
    {
        /// <include file='ctl_pixtx_pipe_set_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_set_config_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_pixtx_pipe_set_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_set_config_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_pixtx_pipe_set_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_set_config_t.OpertaionType"]/*' />
        public ctl_pixtx_config_opertaion_type_t OpertaionType;

        /// <include file='ctl_pixtx_pipe_set_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_set_config_t.Flags"]/*' />
        [NativeTypeName("ctl_pixtx_pipe_set_config_flags_t")]
        public uint Flags;

        /// <include file='ctl_pixtx_pipe_set_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_set_config_t.NumBlocks"]/*' />
        [NativeTypeName("uint32_t")]
        public uint NumBlocks;

        /// <include file='ctl_pixtx_pipe_set_config_t.xml' path='doc/member[@name="ctl_pixtx_pipe_set_config_t.pBlockConfigs"]/*' />
        public ctl_pixtx_block_config_t* pBlockConfigs;
    }
}
