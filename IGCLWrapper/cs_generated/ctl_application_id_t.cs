using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_application_id_t.xml' path='doc/member[@name="ctl_application_id_t"]/*' />
    public partial struct ctl_application_id_t
    {
        /// <include file='ctl_application_id_t.xml' path='doc/member[@name="ctl_application_id_t.Data1"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Data1;

        /// <include file='ctl_application_id_t.xml' path='doc/member[@name="ctl_application_id_t.Data2"]/*' />
        [NativeTypeName("uint16_t")]
        public ushort Data2;

        /// <include file='ctl_application_id_t.xml' path='doc/member[@name="ctl_application_id_t.Data3"]/*' />
        [NativeTypeName("uint16_t")]
        public ushort Data3;

        /// <include file='ctl_application_id_t.xml' path='doc/member[@name="ctl_application_id_t.Data4"]/*' />
        [NativeTypeName("uint8_t[8]")]
        public _Data4_e__FixedBuffer Data4;

        /// <include file='_Data4_e__FixedBuffer.xml' path='doc/member[@name="_Data4_e__FixedBuffer"]/*' />
        [InlineArray(8)]
        public partial struct _Data4_e__FixedBuffer
        {
            public byte e0;
        }
    }
}
