namespace IGCLWrapper
{
    /// <include file='ctl_revision_datatype_t.xml' path='doc/member[@name="ctl_revision_datatype_t"]/*' />
    public partial struct ctl_revision_datatype_t
    {
        /// <include file='ctl_revision_datatype_t.xml' path='doc/member[@name="ctl_revision_datatype_t.major_version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte major_version;

        /// <include file='ctl_revision_datatype_t.xml' path='doc/member[@name="ctl_revision_datatype_t.minor_version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte minor_version;

        /// <include file='ctl_revision_datatype_t.xml' path='doc/member[@name="ctl_revision_datatype_t.revision_version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte revision_version;
    }
}
