using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    /// <include file='ctl_data_value_t.xml' path='doc/member[@name="ctl_data_value_t"]/*' />
    [StructLayout(LayoutKind.Explicit)]
    public partial struct ctl_data_value_t
    {
        /// <include file='ctl_data_value_t.xml' path='doc/member[@name="ctl_data_value_t.data8"]/*' />
        [FieldOffset(0)]
        [NativeTypeName("int8_t")]
        public sbyte data8;

        /// <include file='ctl_data_value_t.xml' path='doc/member[@name="ctl_data_value_t.datau8"]/*' />
        [FieldOffset(0)]
        [NativeTypeName("uint8_t")]
        public byte datau8;

        /// <include file='ctl_data_value_t.xml' path='doc/member[@name="ctl_data_value_t.data16"]/*' />
        [FieldOffset(0)]
        [NativeTypeName("int16_t")]
        public short data16;

        /// <include file='ctl_data_value_t.xml' path='doc/member[@name="ctl_data_value_t.datau16"]/*' />
        [FieldOffset(0)]
        [NativeTypeName("uint16_t")]
        public ushort datau16;

        /// <include file='ctl_data_value_t.xml' path='doc/member[@name="ctl_data_value_t.data32"]/*' />
        [FieldOffset(0)]
        [NativeTypeName("int32_t")]
        public int data32;

        /// <include file='ctl_data_value_t.xml' path='doc/member[@name="ctl_data_value_t.datau32"]/*' />
        [FieldOffset(0)]
        [NativeTypeName("uint32_t")]
        public uint datau32;

        /// <include file='ctl_data_value_t.xml' path='doc/member[@name="ctl_data_value_t.data64"]/*' />
        [FieldOffset(0)]
        [NativeTypeName("int64_t")]
        public long data64;

        /// <include file='ctl_data_value_t.xml' path='doc/member[@name="ctl_data_value_t.datau64"]/*' />
        [FieldOffset(0)]
        [NativeTypeName("uint64_t")]
        public ulong datau64;

        /// <include file='ctl_data_value_t.xml' path='doc/member[@name="ctl_data_value_t.datafloat"]/*' />
        [FieldOffset(0)]
        public float datafloat;

        /// <include file='ctl_data_value_t.xml' path='doc/member[@name="ctl_data_value_t.datadouble"]/*' />
        [FieldOffset(0)]
        public double datadouble;
    }
}
