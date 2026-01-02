namespace IGCLWrapper
{
    /// <include file='ctl_mux_properties_t.xml' path='doc/member[@name="ctl_mux_properties_t"]/*' />
    public unsafe partial struct ctl_mux_properties_t
    {
        /// <include file='ctl_mux_properties_t.xml' path='doc/member[@name="ctl_mux_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_mux_properties_t.xml' path='doc/member[@name="ctl_mux_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_mux_properties_t.xml' path='doc/member[@name="ctl_mux_properties_t.MuxId"]/*' />
        [NativeTypeName("uint8_t")]
        public byte MuxId;

        /// <include file='ctl_mux_properties_t.xml' path='doc/member[@name="ctl_mux_properties_t.Count"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Count;

        /// <include file='ctl_mux_properties_t.xml' path='doc/member[@name="ctl_mux_properties_t.phDisplayOutputs"]/*' />
        [NativeTypeName("ctl_display_output_handle_t *")]
        public _ctl_display_output_handle_t** phDisplayOutputs;

        /// <include file='ctl_mux_properties_t.xml' path='doc/member[@name="ctl_mux_properties_t.IndexOfDisplayOutputOwningMux"]/*' />
        [NativeTypeName("uint8_t")]
        public byte IndexOfDisplayOutputOwningMux;
    }
}
