namespace IGCLWrapper
{
    public unsafe partial struct _ctl_pixtx_matrix_config_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("double[3]")]
        public fixed double PreOffsets[3];

        [NativeTypeName("double[3]")]
        public fixed double PostOffsets[3];

        [NativeTypeName("double[3][3]")]
        public fixed double Matrix[3 * 3];
    }
}
