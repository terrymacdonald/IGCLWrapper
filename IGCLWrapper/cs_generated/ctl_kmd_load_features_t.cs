using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_kmd_load_features_t.xml' path='doc/member[@name="ctl_kmd_load_features_t"]/*' />
    public unsafe partial struct ctl_kmd_load_features_t
    {
        /// <include file='ctl_kmd_load_features_t.xml' path='doc/member[@name="ctl_kmd_load_features_t.ReservedFuncID"]/*' />
        public ctl_application_id_t ReservedFuncID;

        /// <include file='ctl_kmd_load_features_t.xml' path='doc/member[@name="ctl_kmd_load_features_t.bLoad"]/*' />
        [NativeTypeName("bool")]
        public byte bLoad;

        /// <include file='ctl_kmd_load_features_t.xml' path='doc/member[@name="ctl_kmd_load_features_t.SubsetFeatureMask"]/*' />
        [NativeTypeName("int64_t")]
        public long SubsetFeatureMask;

        /// <include file='ctl_kmd_load_features_t.xml' path='doc/member[@name="ctl_kmd_load_features_t.ApplicationName"]/*' />
        [NativeTypeName("char *")]
        public sbyte* ApplicationName;

        /// <include file='ctl_kmd_load_features_t.xml' path='doc/member[@name="ctl_kmd_load_features_t.ApplicationNameLength"]/*' />
        [NativeTypeName("int8_t")]
        public sbyte ApplicationNameLength;

        /// <include file='ctl_kmd_load_features_t.xml' path='doc/member[@name="ctl_kmd_load_features_t.CallerComponent"]/*' />
        [NativeTypeName("int8_t")]
        public sbyte CallerComponent;

        /// <include file='ctl_kmd_load_features_t.xml' path='doc/member[@name="ctl_kmd_load_features_t.Reserved"]/*' />
        [NativeTypeName("int64_t[4]")]
        public _Reserved_e__FixedBuffer Reserved;

        /// <include file='_Reserved_e__FixedBuffer.xml' path='doc/member[@name="_Reserved_e__FixedBuffer"]/*' />
        [InlineArray(4)]
        public partial struct _Reserved_e__FixedBuffer
        {
            public long e0;
        }
    }
}
