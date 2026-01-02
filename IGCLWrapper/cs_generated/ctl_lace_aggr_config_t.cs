using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    /// <include file='ctl_lace_aggr_config_t.xml' path='doc/member[@name="ctl_lace_aggr_config_t"]/*' />
    [StructLayout(LayoutKind.Explicit)]
    public partial struct ctl_lace_aggr_config_t
    {
        /// <include file='ctl_lace_aggr_config_t.xml' path='doc/member[@name="ctl_lace_aggr_config_t.FixedAggressivenessLevelPercent"]/*' />
        [FieldOffset(0)]
        [NativeTypeName("uint8_t")]
        public byte FixedAggressivenessLevelPercent;

        /// <include file='ctl_lace_aggr_config_t.xml' path='doc/member[@name="ctl_lace_aggr_config_t.AggrLevelMap"]/*' />
        [FieldOffset(0)]
        public ctl_lace_lux_aggr_map_t AggrLevelMap;
    }
}
