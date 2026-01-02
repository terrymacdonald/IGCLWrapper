namespace IGCLWrapper
{
    /// <include file='ctl_3d_app_profiles_caps_t.xml' path='doc/member[@name="ctl_3d_app_profiles_caps_t"]/*' />
    public partial struct ctl_3d_app_profiles_caps_t
    {
        /// <include file='ctl_3d_app_profiles_caps_t.xml' path='doc/member[@name="ctl_3d_app_profiles_caps_t.SupportedTierTypes"]/*' />
        [NativeTypeName("ctl_3d_tier_type_flags_t")]
        public uint SupportedTierTypes;

        /// <include file='ctl_3d_app_profiles_caps_t.xml' path='doc/member[@name="ctl_3d_app_profiles_caps_t.Reserved"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong Reserved;
    }
}
