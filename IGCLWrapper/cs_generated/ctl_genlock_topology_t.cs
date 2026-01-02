namespace IGCLWrapper
{
    /// <include file='ctl_genlock_topology_t.xml' path='doc/member[@name="ctl_genlock_topology_t"]/*' />
    public unsafe partial struct ctl_genlock_topology_t
    {
        /// <include file='ctl_genlock_topology_t.xml' path='doc/member[@name="ctl_genlock_topology_t.NumGenlockDisplays"]/*' />
        [NativeTypeName("uint8_t")]
        public byte NumGenlockDisplays;

        /// <include file='ctl_genlock_topology_t.xml' path='doc/member[@name="ctl_genlock_topology_t.IsPrimaryGenlockSystem"]/*' />
        [NativeTypeName("bool")]
        public byte IsPrimaryGenlockSystem;

        /// <include file='ctl_genlock_topology_t.xml' path='doc/member[@name="ctl_genlock_topology_t.CommonTargetMode"]/*' />
        public ctl_display_timing_t CommonTargetMode;

        /// <include file='ctl_genlock_topology_t.xml' path='doc/member[@name="ctl_genlock_topology_t.pGenlockDisplayInfo"]/*' />
        public ctl_genlock_display_info_t* pGenlockDisplayInfo;

        /// <include file='ctl_genlock_topology_t.xml' path='doc/member[@name="ctl_genlock_topology_t.pGenlockModeList"]/*' />
        public ctl_genlock_target_mode_list_t* pGenlockModeList;
    }
}
