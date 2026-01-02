using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    /// <include file='ctl_power_optimization_feature_specific_info_t.xml' path='doc/member[@name="ctl_power_optimization_feature_specific_info_t"]/*' />
    [StructLayout(LayoutKind.Explicit)]
    public partial struct ctl_power_optimization_feature_specific_info_t
    {
        /// <include file='ctl_power_optimization_feature_specific_info_t.xml' path='doc/member[@name="ctl_power_optimization_feature_specific_info_t.LRRInfo"]/*' />
        [FieldOffset(0)]
        public ctl_power_optimization_lrr_t LRRInfo;

        /// <include file='ctl_power_optimization_feature_specific_info_t.xml' path='doc/member[@name="ctl_power_optimization_feature_specific_info_t.PSRInfo"]/*' />
        [FieldOffset(0)]
        public ctl_power_optimization_psr_t PSRInfo;

        /// <include file='ctl_power_optimization_feature_specific_info_t.xml' path='doc/member[@name="ctl_power_optimization_feature_specific_info_t.DPSTInfo"]/*' />
        [FieldOffset(0)]
        public ctl_power_optimization_dpst_t DPSTInfo;
    }
}
