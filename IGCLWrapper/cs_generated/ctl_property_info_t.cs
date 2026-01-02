using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    /// <include file='ctl_property_info_t.xml' path='doc/member[@name="ctl_property_info_t"]/*' />
    [StructLayout(LayoutKind.Explicit)]
    public partial struct ctl_property_info_t
    {
        /// <include file='ctl_property_info_t.xml' path='doc/member[@name="ctl_property_info_t.BoolType"]/*' />
        [FieldOffset(0)]
        public ctl_property_info_boolean_t BoolType;

        /// <include file='ctl_property_info_t.xml' path='doc/member[@name="ctl_property_info_t.FloatType"]/*' />
        [FieldOffset(0)]
        public ctl_property_info_float_t FloatType;

        /// <include file='ctl_property_info_t.xml' path='doc/member[@name="ctl_property_info_t.IntType"]/*' />
        [FieldOffset(0)]
        public ctl_property_info_int_t IntType;

        /// <include file='ctl_property_info_t.xml' path='doc/member[@name="ctl_property_info_t.EnumType"]/*' />
        [FieldOffset(0)]
        public ctl_property_info_enum_t EnumType;

        /// <include file='ctl_property_info_t.xml' path='doc/member[@name="ctl_property_info_t.UIntType"]/*' />
        [FieldOffset(0)]
        public ctl_property_info_uint_t UIntType;
    }
}
