namespace IGCLWrapper
{
    /// <include file='ctl_oc_vf_pair_t.xml' path='doc/member[@name="ctl_oc_vf_pair_t"]/*' />
    public partial struct ctl_oc_vf_pair_t
    {
        /// <include file='ctl_oc_vf_pair_t.xml' path='doc/member[@name="ctl_oc_vf_pair_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_oc_vf_pair_t.xml' path='doc/member[@name="ctl_oc_vf_pair_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_oc_vf_pair_t.xml' path='doc/member[@name="ctl_oc_vf_pair_t.Voltage"]/*' />
        public double Voltage;

        /// <include file='ctl_oc_vf_pair_t.xml' path='doc/member[@name="ctl_oc_vf_pair_t.Frequency"]/*' />
        public double Frequency;
    }
}
