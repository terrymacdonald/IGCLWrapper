namespace IGCLWrapper
{
    /// <include file='ctl_led_color_t.xml' path='doc/member[@name="ctl_led_color_t"]/*' />
    public partial struct ctl_led_color_t
    {
        /// <include file='ctl_led_color_t.xml' path='doc/member[@name="ctl_led_color_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_led_color_t.xml' path='doc/member[@name="ctl_led_color_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_led_color_t.xml' path='doc/member[@name="ctl_led_color_t.red"]/*' />
        public double red;

        /// <include file='ctl_led_color_t.xml' path='doc/member[@name="ctl_led_color_t.green"]/*' />
        public double green;

        /// <include file='ctl_led_color_t.xml' path='doc/member[@name="ctl_led_color_t.blue"]/*' />
        public double blue;
    }
}
