namespace IGCLWrapper
{
    /// <include file='ctl_pixtx_block_config_t.xml' path='doc/member[@name="ctl_pixtx_block_config_t"]/*' />
    public partial struct ctl_pixtx_block_config_t
    {
        /// <include file='ctl_pixtx_block_config_t.xml' path='doc/member[@name="ctl_pixtx_block_config_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_pixtx_block_config_t.xml' path='doc/member[@name="ctl_pixtx_block_config_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_pixtx_block_config_t.xml' path='doc/member[@name="ctl_pixtx_block_config_t.BlockId"]/*' />
        [NativeTypeName("uint32_t")]
        public uint BlockId;

        /// <include file='ctl_pixtx_block_config_t.xml' path='doc/member[@name="ctl_pixtx_block_config_t.BlockType"]/*' />
        public ctl_pixtx_block_type_t BlockType;

        /// <include file='ctl_pixtx_block_config_t.xml' path='doc/member[@name="ctl_pixtx_block_config_t.Config"]/*' />
        public ctl_pixtx_config_t Config;
    }
}
