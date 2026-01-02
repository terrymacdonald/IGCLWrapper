namespace IGCLWrapper
{
    /// <include file='ctl_wire_format_t.xml' path='doc/member[@name="ctl_wire_format_t"]/*' />
    public partial struct ctl_wire_format_t
    {
        /// <include file='ctl_wire_format_t.xml' path='doc/member[@name="ctl_wire_format_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_wire_format_t.xml' path='doc/member[@name="ctl_wire_format_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_wire_format_t.xml' path='doc/member[@name="ctl_wire_format_t.ColorModel"]/*' />
        public ctl_wire_format_color_model_t ColorModel;

        /// <include file='ctl_wire_format_t.xml' path='doc/member[@name="ctl_wire_format_t.ColorDepth"]/*' />
        [NativeTypeName("ctl_output_bpc_flags_t")]
        public uint ColorDepth;
    }
}
