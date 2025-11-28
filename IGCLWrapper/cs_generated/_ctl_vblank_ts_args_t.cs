namespace IGCLWrapper
{
    public unsafe partial struct _ctl_vblank_ts_args_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("uint8_t")]
        public byte NumOfTargets;

        [NativeTypeName("uint64_t[16]")]
        public fixed ulong VblankTS[16];
    }
}
