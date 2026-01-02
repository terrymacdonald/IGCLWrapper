namespace IGCLWrapper
{
    /// <include file='ctl_intel_arc_sync_profile_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_profile_params_t"]/*' />
    public partial struct ctl_intel_arc_sync_profile_params_t
    {
        /// <include file='ctl_intel_arc_sync_profile_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_profile_params_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_intel_arc_sync_profile_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_profile_params_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_intel_arc_sync_profile_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_profile_params_t.IntelArcSyncProfile"]/*' />
        public ctl_intel_arc_sync_profile_t IntelArcSyncProfile;

        /// <include file='ctl_intel_arc_sync_profile_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_profile_params_t.MaxRefreshRateInHz"]/*' />
        public float MaxRefreshRateInHz;

        /// <include file='ctl_intel_arc_sync_profile_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_profile_params_t.MinRefreshRateInHz"]/*' />
        public float MinRefreshRateInHz;

        /// <include file='ctl_intel_arc_sync_profile_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_profile_params_t.MaxFrameTimeIncreaseInUs"]/*' />
        [NativeTypeName("uint32_t")]
        public uint MaxFrameTimeIncreaseInUs;

        /// <include file='ctl_intel_arc_sync_profile_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_profile_params_t.MaxFrameTimeDecreaseInUs"]/*' />
        [NativeTypeName("uint32_t")]
        public uint MaxFrameTimeDecreaseInUs;
    }
}
