namespace IGCLWrapper
{
    /// <include file='ctl_led_state_t.xml' path='doc/member[@name="ctl_led_state_t"]/*' />
    public partial struct ctl_led_state_t
    {
        /// <include file='ctl_led_state_t.xml' path='doc/member[@name="ctl_led_state_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_led_state_t.xml' path='doc/member[@name="ctl_led_state_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_led_state_t.xml' path='doc/member[@name="ctl_led_state_t.isOn"]/*' />
        [NativeTypeName("bool")]
        public byte isOn;

        /// <include file='ctl_led_state_t.xml' path='doc/member[@name="ctl_led_state_t.pwm"]/*' />
        public double pwm;

        /// <include file='ctl_led_state_t.xml' path='doc/member[@name="ctl_led_state_t.color"]/*' />
        public ctl_led_color_t color;
    }
}
