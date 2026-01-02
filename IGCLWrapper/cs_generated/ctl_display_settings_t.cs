using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t"]/*' />
    public partial struct ctl_display_settings_t
    {
        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.Set"]/*' />
        [NativeTypeName("bool")]
        public byte Set;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.SupportedFlags"]/*' />
        [NativeTypeName("ctl_display_setting_flags_t")]
        public uint SupportedFlags;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.ControllableFlags"]/*' />
        [NativeTypeName("ctl_display_setting_flags_t")]
        public uint ControllableFlags;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.ValidFlags"]/*' />
        [NativeTypeName("ctl_display_setting_flags_t")]
        public uint ValidFlags;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.LowLatency"]/*' />
        public ctl_display_setting_low_latency_t LowLatency;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.SourceTM"]/*' />
        public ctl_display_setting_sourcetm_t SourceTM;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.ContentType"]/*' />
        public ctl_display_setting_content_type_t ContentType;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.QuantizationRange"]/*' />
        public ctl_display_setting_quantization_range_t QuantizationRange;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.SupportedPictureAR"]/*' />
        [NativeTypeName("ctl_display_setting_picture_ar_flags_t")]
        public uint SupportedPictureAR;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.PictureAR"]/*' />
        public ctl_display_setting_picture_ar_flag_t PictureAR;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.AudioSettings"]/*' />
        public ctl_display_setting_audio_t AudioSettings;

        /// <include file='ctl_display_settings_t.xml' path='doc/member[@name="ctl_display_settings_t.Reserved"]/*' />
        [NativeTypeName("uint32_t[25]")]
        public _Reserved_e__FixedBuffer Reserved;

        /// <include file='_Reserved_e__FixedBuffer.xml' path='doc/member[@name="_Reserved_e__FixedBuffer"]/*' />
        [InlineArray(25)]
        public partial struct _Reserved_e__FixedBuffer
        {
            public uint e0;
        }
    }
}
