namespace IGCLWrapper
{
    /// <include file='ctl_generic_void_datatype_t.xml' path='doc/member[@name="ctl_generic_void_datatype_t"]/*' />
    public unsafe partial struct ctl_generic_void_datatype_t
    {
        /// <include file='ctl_generic_void_datatype_t.xml' path='doc/member[@name="ctl_generic_void_datatype_t.pData"]/*' />
        public void* pData;

        /// <include file='ctl_generic_void_datatype_t.xml' path='doc/member[@name="ctl_generic_void_datatype_t.size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint size;
    }
}
