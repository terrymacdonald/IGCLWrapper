using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_i2c_access_pinpair_args_t.xml' path='doc/member[@name="ctl_i2c_access_pinpair_args_t"]/*' />
    public partial struct ctl_i2c_access_pinpair_args_t
    {
        /// <include file='ctl_i2c_access_pinpair_args_t.xml' path='doc/member[@name="ctl_i2c_access_pinpair_args_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_i2c_access_pinpair_args_t.xml' path='doc/member[@name="ctl_i2c_access_pinpair_args_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_i2c_access_pinpair_args_t.xml' path='doc/member[@name="ctl_i2c_access_pinpair_args_t.DataSize"]/*' />
        [NativeTypeName("uint32_t")]
        public uint DataSize;

        /// <include file='ctl_i2c_access_pinpair_args_t.xml' path='doc/member[@name="ctl_i2c_access_pinpair_args_t.Address"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Address;

        /// <include file='ctl_i2c_access_pinpair_args_t.xml' path='doc/member[@name="ctl_i2c_access_pinpair_args_t.OpType"]/*' />
        public ctl_operation_type_t OpType;

        /// <include file='ctl_i2c_access_pinpair_args_t.xml' path='doc/member[@name="ctl_i2c_access_pinpair_args_t.Offset"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Offset;

        /// <include file='ctl_i2c_access_pinpair_args_t.xml' path='doc/member[@name="ctl_i2c_access_pinpair_args_t.Flags"]/*' />
        [NativeTypeName("ctl_i2c_pinpair_flags_t")]
        public uint Flags;

        /// <include file='ctl_i2c_access_pinpair_args_t.xml' path='doc/member[@name="ctl_i2c_access_pinpair_args_t.Data"]/*' />
        [NativeTypeName("uint8_t[128]")]
        public _Data_e__FixedBuffer Data;

        /// <include file='ctl_i2c_access_pinpair_args_t.xml' path='doc/member[@name="ctl_i2c_access_pinpair_args_t.ReservedFields"]/*' />
        [NativeTypeName("uint32_t[4]")]
        public _ReservedFields_e__FixedBuffer ReservedFields;

        /// <include file='_Data_e__FixedBuffer.xml' path='doc/member[@name="_Data_e__FixedBuffer"]/*' />
        [InlineArray(128)]
        public partial struct _Data_e__FixedBuffer
        {
            public byte e0;
        }

        /// <include file='_ReservedFields_e__FixedBuffer.xml' path='doc/member[@name="_ReservedFields_e__FixedBuffer"]/*' />
        [InlineArray(4)]
        public partial struct _ReservedFields_e__FixedBuffer
        {
            public uint e0;
        }
    }
}
