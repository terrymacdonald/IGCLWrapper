namespace IGCLWrapper
{
    /// <include file='ctl_adaptivesync_getset_t.xml' path='doc/member[@name="ctl_adaptivesync_getset_t"]/*' />
    public partial struct ctl_adaptivesync_getset_t
    {
        /// <include file='ctl_adaptivesync_getset_t.xml' path='doc/member[@name="ctl_adaptivesync_getset_t.AdaptiveSync"]/*' />
        [NativeTypeName("bool")]
        public byte AdaptiveSync;

        /// <include file='ctl_adaptivesync_getset_t.xml' path='doc/member[@name="ctl_adaptivesync_getset_t.AdaptiveBalance"]/*' />
        [NativeTypeName("bool")]
        public byte AdaptiveBalance;

        /// <include file='ctl_adaptivesync_getset_t.xml' path='doc/member[@name="ctl_adaptivesync_getset_t.AllowAsyncForHighFPS"]/*' />
        [NativeTypeName("bool")]
        public byte AllowAsyncForHighFPS;

        /// <include file='ctl_adaptivesync_getset_t.xml' path='doc/member[@name="ctl_adaptivesync_getset_t.AdaptiveBalanceStrength"]/*' />
        public float AdaptiveBalanceStrength;
    }
}
