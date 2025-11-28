namespace IGCLWrapper
{
    public unsafe partial struct _ctl_3d_tier_details_t
    {
        [NativeTypeName("ctl_3d_tier_type_flag_t")]
        public _ctl_3d_tier_type_flag_t TierType;

        [NativeTypeName("ctl_3d_tier_profile_flag_t")]
        public _ctl_3d_tier_profile_flag_t TierProfile;

        [NativeTypeName("uint64_t[4]")]
        public fixed ulong Reserved[4];
    }
}
