namespace IGCLWrapper
{
    /// <include file='ctl_pixtx_pixel_format_t.xml' path='doc/member[@name="ctl_pixtx_pixel_format_t"]/*' />
    public partial struct ctl_pixtx_pixel_format_t
    {
        /// <include file='ctl_pixtx_pixel_format_t.xml' path='doc/member[@name="ctl_pixtx_pixel_format_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_pixtx_pixel_format_t.xml' path='doc/member[@name="ctl_pixtx_pixel_format_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_pixtx_pixel_format_t.xml' path='doc/member[@name="ctl_pixtx_pixel_format_t.BitsPerColor"]/*' />
        [NativeTypeName("uint32_t")]
        public uint BitsPerColor;

        /// <include file='ctl_pixtx_pixel_format_t.xml' path='doc/member[@name="ctl_pixtx_pixel_format_t.IsFloat"]/*' />
        [NativeTypeName("bool")]
        public byte IsFloat;

        /// <include file='ctl_pixtx_pixel_format_t.xml' path='doc/member[@name="ctl_pixtx_pixel_format_t.EncodingType"]/*' />
        public ctl_pixtx_gamma_encoding_type_t EncodingType;

        /// <include file='ctl_pixtx_pixel_format_t.xml' path='doc/member[@name="ctl_pixtx_pixel_format_t.ColorSpace"]/*' />
        public ctl_pixtx_color_space_t ColorSpace;

        /// <include file='ctl_pixtx_pixel_format_t.xml' path='doc/member[@name="ctl_pixtx_pixel_format_t.ColorModel"]/*' />
        public ctl_pixtx_color_model_t ColorModel;

        /// <include file='ctl_pixtx_pixel_format_t.xml' path='doc/member[@name="ctl_pixtx_pixel_format_t.ColorPrimaries"]/*' />
        public ctl_pixtx_color_primaries_t ColorPrimaries;

        /// <include file='ctl_pixtx_pixel_format_t.xml' path='doc/member[@name="ctl_pixtx_pixel_format_t.MaxBrightness"]/*' />
        public double MaxBrightness;

        /// <include file='ctl_pixtx_pixel_format_t.xml' path='doc/member[@name="ctl_pixtx_pixel_format_t.MinBrightness"]/*' />
        public double MinBrightness;
    }
}
