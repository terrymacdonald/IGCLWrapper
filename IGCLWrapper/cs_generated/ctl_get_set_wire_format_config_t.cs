using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_get_set_wire_format_config_t.xml' path='doc/member[@name="ctl_get_set_wire_format_config_t"]/*' />
    public partial struct ctl_get_set_wire_format_config_t
    {
        /// <include file='ctl_get_set_wire_format_config_t.xml' path='doc/member[@name="ctl_get_set_wire_format_config_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_get_set_wire_format_config_t.xml' path='doc/member[@name="ctl_get_set_wire_format_config_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_get_set_wire_format_config_t.xml' path='doc/member[@name="ctl_get_set_wire_format_config_t.Operation"]/*' />
        public ctl_wire_format_operation_type_t Operation;

        /// <include file='ctl_get_set_wire_format_config_t.xml' path='doc/member[@name="ctl_get_set_wire_format_config_t.SupportedWireFormat"]/*' />
        [NativeTypeName("ctl_wire_format_t[4]")]
        public _SupportedWireFormat_e__FixedBuffer SupportedWireFormat;

        /// <include file='ctl_get_set_wire_format_config_t.xml' path='doc/member[@name="ctl_get_set_wire_format_config_t.WireFormat"]/*' />
        public ctl_wire_format_t WireFormat;

        /// <include file='_SupportedWireFormat_e__FixedBuffer.xml' path='doc/member[@name="_SupportedWireFormat_e__FixedBuffer"]/*' />
        [InlineArray(4)]
        public partial struct _SupportedWireFormat_e__FixedBuffer
        {
            public ctl_wire_format_t e0;
        }
    }
}
