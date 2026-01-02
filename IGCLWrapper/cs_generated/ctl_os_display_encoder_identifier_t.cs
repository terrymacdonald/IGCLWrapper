using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    /// <include file='ctl_os_display_encoder_identifier_t.xml' path='doc/member[@name="ctl_os_display_encoder_identifier_t"]/*' />
    [StructLayout(LayoutKind.Explicit)]
    public partial struct ctl_os_display_encoder_identifier_t
    {
        /// <include file='ctl_os_display_encoder_identifier_t.xml' path='doc/member[@name="ctl_os_display_encoder_identifier_t.WindowsDisplayEncoderID"]/*' />
        [FieldOffset(0)]
        [NativeTypeName("uint32_t")]
        public uint WindowsDisplayEncoderID;

        /// <include file='ctl_os_display_encoder_identifier_t.xml' path='doc/member[@name="ctl_os_display_encoder_identifier_t.DisplayEncoderID"]/*' />
        [FieldOffset(0)]
        public ctl_generic_void_datatype_t DisplayEncoderID;
    }
}
