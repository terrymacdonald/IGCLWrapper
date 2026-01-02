namespace IGCLWrapper
{
    /// <include file='ctl_lace_lux_aggr_map_t.xml' path='doc/member[@name="ctl_lace_lux_aggr_map_t"]/*' />
    public unsafe partial struct ctl_lace_lux_aggr_map_t
    {
        /// <include file='ctl_lace_lux_aggr_map_t.xml' path='doc/member[@name="ctl_lace_lux_aggr_map_t.MaxNumEntries"]/*' />
        [NativeTypeName("uint32_t")]
        public uint MaxNumEntries;

        /// <include file='ctl_lace_lux_aggr_map_t.xml' path='doc/member[@name="ctl_lace_lux_aggr_map_t.NumEntries"]/*' />
        [NativeTypeName("uint32_t")]
        public uint NumEntries;

        /// <include file='ctl_lace_lux_aggr_map_t.xml' path='doc/member[@name="ctl_lace_lux_aggr_map_t.pLuxToAggrMappingTable"]/*' />
        public ctl_lace_lux_aggr_map_entry_t* pLuxToAggrMappingTable;
    }
}
