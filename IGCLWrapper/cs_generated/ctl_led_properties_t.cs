namespace IGCLWrapper
{
    /// <include file='ctl_led_properties_t.xml' path='doc/member[@name="ctl_led_properties_t"]/*' />
    public partial struct ctl_led_properties_t
    {
        /// <include file='ctl_led_properties_t.xml' path='doc/member[@name="ctl_led_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_led_properties_t.xml' path='doc/member[@name="ctl_led_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_led_properties_t.xml' path='doc/member[@name="ctl_led_properties_t.canControl"]/*' />
        [NativeTypeName("bool")]
        public byte canControl;

        /// <include file='ctl_led_properties_t.xml' path='doc/member[@name="ctl_led_properties_t.isI2C"]/*' />
        [NativeTypeName("bool")]
        public byte isI2C;

        /// <include file='ctl_led_properties_t.xml' path='doc/member[@name="ctl_led_properties_t.isPWM"]/*' />
        [NativeTypeName("bool")]
        public byte isPWM;

        /// <include file='ctl_led_properties_t.xml' path='doc/member[@name="ctl_led_properties_t.haveRGB"]/*' />
        [NativeTypeName("bool")]
        public byte haveRGB;
    }
}
