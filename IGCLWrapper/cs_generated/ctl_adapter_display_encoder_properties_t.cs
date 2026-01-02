using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_adapter_display_encoder_properties_t.xml' path='doc/member[@name="ctl_adapter_display_encoder_properties_t"]/*' />
    public partial struct ctl_adapter_display_encoder_properties_t
    {
        /// <include file='ctl_adapter_display_encoder_properties_t.xml' path='doc/member[@name="ctl_adapter_display_encoder_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_adapter_display_encoder_properties_t.xml' path='doc/member[@name="ctl_adapter_display_encoder_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_adapter_display_encoder_properties_t.xml' path='doc/member[@name="ctl_adapter_display_encoder_properties_t.Os_display_encoder_handle"]/*' />
        public ctl_os_display_encoder_identifier_t Os_display_encoder_handle;

        /// <include file='ctl_adapter_display_encoder_properties_t.xml' path='doc/member[@name="ctl_adapter_display_encoder_properties_t.Type"]/*' />
        public ctl_display_output_types_t Type;

        /// <include file='ctl_adapter_display_encoder_properties_t.xml' path='doc/member[@name="ctl_adapter_display_encoder_properties_t.IsOnBoardProtocolConverterOutputPresent"]/*' />
        [NativeTypeName("bool")]
        public byte IsOnBoardProtocolConverterOutputPresent;

        /// <include file='ctl_adapter_display_encoder_properties_t.xml' path='doc/member[@name="ctl_adapter_display_encoder_properties_t.SupportedSpec"]/*' />
        public ctl_revision_datatype_t SupportedSpec;

        /// <include file='ctl_adapter_display_encoder_properties_t.xml' path='doc/member[@name="ctl_adapter_display_encoder_properties_t.SupportedOutputBPCFlags"]/*' />
        [NativeTypeName("ctl_output_bpc_flags_t")]
        public uint SupportedOutputBPCFlags;

        /// <include file='ctl_adapter_display_encoder_properties_t.xml' path='doc/member[@name="ctl_adapter_display_encoder_properties_t.EncoderConfigFlags"]/*' />
        [NativeTypeName("ctl_encoder_config_flags_t")]
        public uint EncoderConfigFlags;

        /// <include file='ctl_adapter_display_encoder_properties_t.xml' path='doc/member[@name="ctl_adapter_display_encoder_properties_t.FeatureSupportedFlags"]/*' />
        [NativeTypeName("ctl_std_display_feature_flags_t")]
        public uint FeatureSupportedFlags;

        /// <include file='ctl_adapter_display_encoder_properties_t.xml' path='doc/member[@name="ctl_adapter_display_encoder_properties_t.AdvancedFeatureSupportedFlags"]/*' />
        [NativeTypeName("ctl_intel_display_feature_flags_t")]
        public uint AdvancedFeatureSupportedFlags;

        /// <include file='ctl_adapter_display_encoder_properties_t.xml' path='doc/member[@name="ctl_adapter_display_encoder_properties_t.ReservedFields"]/*' />
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
