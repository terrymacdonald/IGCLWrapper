namespace IGCLWrapper
{
    public partial struct _ctl_get_set_wire_format_config_t
    {
        [NativeTypeName("uint32_t")]
        public uint Size;

        [NativeTypeName("uint8_t")]
        public byte Version;

        [NativeTypeName("ctl_wire_format_operation_type_t")]
        public _ctl_wire_format_operation_type_t Operation;

        [NativeTypeName("ctl_wire_format_t[4]")]
        public _SupportedWireFormat_e__FixedBuffer SupportedWireFormat;

        [NativeTypeName("ctl_wire_format_t")]
        public _ctl_wire_format_t WireFormat;

        public partial struct _SupportedWireFormat_e__FixedBuffer
        {
            public _ctl_wire_format_t e0;
            public _ctl_wire_format_t e1;
            public _ctl_wire_format_t e2;
            public _ctl_wire_format_t e3;

            public unsafe ref _ctl_wire_format_t this[int index]
            {
                get
                {
                    fixed (_ctl_wire_format_t* pThis = &e0)
                    {
                        return ref pThis[index];
                    }
                }
            }
        }
    }
}
