using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t"]/*' />
    public partial struct ctl_display_properties_t
    {
        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.Os_display_encoder_handle"]/*' />
        public ctl_os_display_encoder_identifier_t Os_display_encoder_handle;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.Type"]/*' />
        public ctl_display_output_types_t Type;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.AttachedDisplayMuxType"]/*' />
        public ctl_attached_display_mux_type_t AttachedDisplayMuxType;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.ProtocolConverterOutput"]/*' />
        public ctl_display_output_types_t ProtocolConverterOutput;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.SupportedSpec"]/*' />
        public ctl_revision_datatype_t SupportedSpec;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.SupportedOutputBPCFlags"]/*' />
        [NativeTypeName("ctl_output_bpc_flags_t")]
        public uint SupportedOutputBPCFlags;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.ProtocolConverterType"]/*' />
        [NativeTypeName("ctl_protocol_converter_location_flags_t")]
        public uint ProtocolConverterType;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.DisplayConfigFlags"]/*' />
        [NativeTypeName("ctl_display_config_flags_t")]
        public uint DisplayConfigFlags;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.FeatureEnabledFlags"]/*' />
        [NativeTypeName("ctl_std_display_feature_flags_t")]
        public uint FeatureEnabledFlags;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.FeatureSupportedFlags"]/*' />
        [NativeTypeName("ctl_std_display_feature_flags_t")]
        public uint FeatureSupportedFlags;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.AdvancedFeatureEnabledFlags"]/*' />
        [NativeTypeName("ctl_intel_display_feature_flags_t")]
        public uint AdvancedFeatureEnabledFlags;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.AdvancedFeatureSupportedFlags"]/*' />
        [NativeTypeName("ctl_intel_display_feature_flags_t")]
        public uint AdvancedFeatureSupportedFlags;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.Display_Timing_Info"]/*' />
        public ctl_display_timing_t Display_Timing_Info;

        /// <include file='ctl_display_properties_t.xml' path='doc/member[@name="ctl_display_properties_t.ReservedFields"]/*' />
        [NativeTypeName("uint32_t[16]")]
        public _ReservedFields_e__FixedBuffer ReservedFields;

        /// <include file='_ReservedFields_e__FixedBuffer.xml' path='doc/member[@name="_ReservedFields_e__FixedBuffer"]/*' />
        [InlineArray(16)]
        public partial struct _ReservedFields_e__FixedBuffer
        {
            public uint e0;
        }
    }
}
