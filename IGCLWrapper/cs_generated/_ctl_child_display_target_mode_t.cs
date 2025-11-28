namespace IGCLWrapper
{
    public unsafe partial struct _ctl_child_display_target_mode_t
    {
        [NativeTypeName("uint32_t")]
        public uint Width;

        [NativeTypeName("uint32_t")]
        public uint Height;

        public float RefreshRate;

        [NativeTypeName("uint32_t[4]")]
        public fixed uint ReservedFields[4];
    }
}
