namespace IGCLWrapper
{
    /// <include file='ctl_genlock_args_t.xml' path='doc/member[@name="ctl_genlock_args_t"]/*' />
    public partial struct ctl_genlock_args_t
    {
        /// <include file='ctl_genlock_args_t.xml' path='doc/member[@name="ctl_genlock_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_genlock_args_t.xml' path='doc/member[@name="ctl_genlock_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_genlock_args_t.xml' path='doc/member[@name="ctl_genlock_args_t.Operation"]/*' />
        public ctl_genlock_operation_t Operation;

        /// <include file='ctl_genlock_args_t.xml' path='doc/member[@name="ctl_genlock_args_t.GenlockTopology"]/*' />
        public ctl_genlock_topology_t GenlockTopology;

        /// <include file='ctl_genlock_args_t.xml' path='doc/member[@name="ctl_genlock_args_t.IsGenlockEnabled"]/*' />
        [NativeTypeName("bool")]
        public byte IsGenlockEnabled;

        /// <include file='ctl_genlock_args_t.xml' path='doc/member[@name="ctl_genlock_args_t.IsGenlockPossible"]/*' />
        [NativeTypeName("bool")]
        public byte IsGenlockPossible;
    }
}
