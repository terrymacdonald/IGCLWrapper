using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    /// <include file='ctl_property_t.xml' path='doc/member[@name="ctl_property_t"]/*' />
    [StructLayout(LayoutKind.Explicit)]
    public partial struct ctl_property_t
    {
        /// <include file='ctl_property_t.xml' path='doc/member[@name="ctl_property_t.BoolType"]/*' />
        [FieldOffset(0)]
        public ctl_property_boolean_t BoolType;

        /// <include file='ctl_property_t.xml' path='doc/member[@name="ctl_property_t.FloatType"]/*' />
        [FieldOffset(0)]
        public ctl_property_float_t FloatType;

        /// <include file='ctl_property_t.xml' path='doc/member[@name="ctl_property_t.IntType"]/*' />
        [FieldOffset(0)]
        public ctl_property_int_t IntType;

        /// <include file='ctl_property_t.xml' path='doc/member[@name="ctl_property_t.EnumType"]/*' />
        [FieldOffset(0)]
        public ctl_property_enum_t EnumType;

        /// <include file='ctl_property_t.xml' path='doc/member[@name="ctl_property_t.UIntType"]/*' />
        [FieldOffset(0)]
        public ctl_property_uint_t UIntType;
    }
}
