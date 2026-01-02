namespace IGCLWrapper
{
    /// <include file='ctl_wait_property_change_args_t.xml' path='doc/member[@name="ctl_wait_property_change_args_t"]/*' />
    public unsafe partial struct ctl_wait_property_change_args_t
    {
        /// <include file='ctl_wait_property_change_args_t.xml' path='doc/member[@name="ctl_wait_property_change_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_wait_property_change_args_t.xml' path='doc/member[@name="ctl_wait_property_change_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_wait_property_change_args_t.xml' path='doc/member[@name="ctl_wait_property_change_args_t.PropertyType"]/*' />
        [NativeTypeName("ctl_property_type_flags_t")]
        public uint PropertyType;

        /// <include file='ctl_wait_property_change_args_t.xml' path='doc/member[@name="ctl_wait_property_change_args_t.TimeOutMilliSec"]/*' />
        [NativeTypeName("uint32_t")]
        public uint TimeOutMilliSec;

        /// <include file='ctl_wait_property_change_args_t.xml' path='doc/member[@name="ctl_wait_property_change_args_t.EventMiscFlags"]/*' />
        [NativeTypeName("uint32_t")]
        public uint EventMiscFlags;

        /// <include file='ctl_wait_property_change_args_t.xml' path='doc/member[@name="ctl_wait_property_change_args_t.pReserved"]/*' />
        public void* pReserved;

        /// <include file='ctl_wait_property_change_args_t.xml' path='doc/member[@name="ctl_wait_property_change_args_t.ReservedOutFlags"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong ReservedOutFlags;
    }
}
