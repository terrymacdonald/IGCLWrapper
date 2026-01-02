namespace IGCLWrapper
{
    /// <include file='ctl_intel_arc_sync_monitor_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_monitor_params_t"]/*' />
    public partial struct ctl_intel_arc_sync_monitor_params_t
    {
        /// <include file='ctl_intel_arc_sync_monitor_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_monitor_params_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_intel_arc_sync_monitor_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_monitor_params_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_intel_arc_sync_monitor_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_monitor_params_t.IsIntelArcSyncSupported"]/*' />
        [NativeTypeName("bool")]
        public byte IsIntelArcSyncSupported;

        /// <include file='ctl_intel_arc_sync_monitor_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_monitor_params_t.MinimumRefreshRateInHz"]/*' />
        public float MinimumRefreshRateInHz;

        /// <include file='ctl_intel_arc_sync_monitor_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_monitor_params_t.MaximumRefreshRateInHz"]/*' />
        public float MaximumRefreshRateInHz;

        /// <include file='ctl_intel_arc_sync_monitor_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_monitor_params_t.MaxFrameTimeIncreaseInUs"]/*' />
        [NativeTypeName("uint32_t")]
        public uint MaxFrameTimeIncreaseInUs;

        /// <include file='ctl_intel_arc_sync_monitor_params_t.xml' path='doc/member[@name="ctl_intel_arc_sync_monitor_params_t.MaxFrameTimeDecreaseInUs"]/*' />
        [NativeTypeName("uint32_t")]
        public uint MaxFrameTimeDecreaseInUs;
    }
}
