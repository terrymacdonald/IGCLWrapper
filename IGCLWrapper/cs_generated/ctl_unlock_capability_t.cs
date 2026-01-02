namespace IGCLWrapper
{
    /// <include file='ctl_unlock_capability_t.xml' path='doc/member[@name="ctl_unlock_capability_t"]/*' />
    public partial struct ctl_unlock_capability_t
    {
        /// <include file='ctl_unlock_capability_t.xml' path='doc/member[@name="ctl_unlock_capability_t.ReservedFuncID"]/*' />
        public ctl_application_id_t ReservedFuncID;

        /// <include file='ctl_unlock_capability_t.xml' path='doc/member[@name="ctl_unlock_capability_t.UnlockCapsID"]/*' />
        public ctl_application_id_t UnlockCapsID;
    }
}
