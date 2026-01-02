using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_aux_access_args_t.xml' path='doc/member[@name="ctl_aux_access_args_t"]/*' />
    public partial struct ctl_aux_access_args_t
    {
        /// <include file='ctl_aux_access_args_t.xml' path='doc/member[@name="ctl_aux_access_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_aux_access_args_t.xml' path='doc/member[@name="ctl_aux_access_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_aux_access_args_t.xml' path='doc/member[@name="ctl_aux_access_args_t.OpType"]/*' />
        public ctl_operation_type_t OpType;

        /// <include file='ctl_aux_access_args_t.xml' path='doc/member[@name="ctl_aux_access_args_t.Flags"]/*' />
        [NativeTypeName("ctl_aux_flags_t")]
        public uint Flags;

        /// <include file='ctl_aux_access_args_t.xml' path='doc/member[@name="ctl_aux_access_args_t.Address"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Address;

        /// <include file='ctl_aux_access_args_t.xml' path='doc/member[@name="ctl_aux_access_args_t.RAD"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong RAD;

        /// <include file='ctl_aux_access_args_t.xml' path='doc/member[@name="ctl_aux_access_args_t.PortID"]/*' />
        [NativeTypeName("uint32_t")]
        public uint PortID;

        /// <include file='ctl_aux_access_args_t.xml' path='doc/member[@name="ctl_aux_access_args_t.DataSize"]/*' />
        [NativeTypeName("uint32_t")]
        public uint DataSize;

        /// <include file='ctl_aux_access_args_t.xml' path='doc/member[@name="ctl_aux_access_args_t.Data"]/*' />
        [NativeTypeName("uint8_t[132]")]
        public _Data_e__FixedBuffer Data;

        /// <include file='_Data_e__FixedBuffer.xml' path='doc/member[@name="_Data_e__FixedBuffer"]/*' />
        [InlineArray(132)]
        public partial struct _Data_e__FixedBuffer
        {
            public byte e0;
        }
    }
}
