namespace IGCLWrapper
{
    /// <include file='ctl_3d_app_profiles_t.xml' path='doc/member[@name="ctl_3d_app_profiles_t"]/*' />
    public partial struct ctl_3d_app_profiles_t
    {
        /// <include file='ctl_3d_app_profiles_t.xml' path='doc/member[@name="ctl_3d_app_profiles_t.TierType"]/*' />
        public ctl_3d_tier_type_flag_t TierType;

        /// <include file='ctl_3d_app_profiles_t.xml' path='doc/member[@name="ctl_3d_app_profiles_t.SupportedTierProfiles"]/*' />
        [NativeTypeName("ctl_3d_tier_profile_flags_t")]
        public uint SupportedTierProfiles;

        /// <include file='ctl_3d_app_profiles_t.xml' path='doc/member[@name="ctl_3d_app_profiles_t.DefaultEnabledTierProfiles"]/*' />
        [NativeTypeName("ctl_3d_tier_profile_flags_t")]
        public uint DefaultEnabledTierProfiles;

        /// <include file='ctl_3d_app_profiles_t.xml' path='doc/member[@name="ctl_3d_app_profiles_t.CustomizationSupportedTierProfiles"]/*' />
        [NativeTypeName("ctl_3d_tier_profile_flags_t")]
        public uint CustomizationSupportedTierProfiles;

        /// <include file='ctl_3d_app_profiles_t.xml' path='doc/member[@name="ctl_3d_app_profiles_t.EnabledTierProfiles"]/*' />
        [NativeTypeName("ctl_3d_tier_profile_flags_t")]
        public uint EnabledTierProfiles;

        /// <include file='ctl_3d_app_profiles_t.xml' path='doc/member[@name="ctl_3d_app_profiles_t.CustomizationEnabledTierProfiles"]/*' />
        [NativeTypeName("ctl_3d_tier_profile_flags_t")]
        public uint CustomizationEnabledTierProfiles;

        /// <include file='ctl_3d_app_profiles_t.xml' path='doc/member[@name="ctl_3d_app_profiles_t.Reserved"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong Reserved;
    }
}
