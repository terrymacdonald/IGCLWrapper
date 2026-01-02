using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_firmware_properties_t.xml' path='doc/member[@name="ctl_firmware_properties_t"]/*' />
    public partial struct ctl_firmware_properties_t
    {
        /// <include file='ctl_firmware_properties_t.xml' path='doc/member[@name="ctl_firmware_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_firmware_properties_t.xml' path='doc/member[@name="ctl_firmware_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_firmware_properties_t.xml' path='doc/member[@name="ctl_firmware_properties_t.name"]/*' />
        [NativeTypeName("char[64]")]
        public _name_e__FixedBuffer name;

        /// <include file='ctl_firmware_properties_t.xml' path='doc/member[@name="ctl_firmware_properties_t.version"]/*' />
        [NativeTypeName("char[64]")]
        public _version_e__FixedBuffer version;

        /// <include file='ctl_firmware_properties_t.xml' path='doc/member[@name="ctl_firmware_properties_t.FirmwareConfig"]/*' />
        [NativeTypeName("ctl_firmware_config_flags_t")]
        public uint FirmwareConfig;

        /// <include file='ctl_firmware_properties_t.xml' path='doc/member[@name="ctl_firmware_properties_t.reserved"]/*' />
        [NativeTypeName("char[16]")]
        public _reserved_e__FixedBuffer reserved;

        /// <include file='_name_e__FixedBuffer.xml' path='doc/member[@name="_name_e__FixedBuffer"]/*' />
        [InlineArray(64)]
        public partial struct _name_e__FixedBuffer
        {
            public sbyte e0;
        }

        /// <include file='_version_e__FixedBuffer.xml' path='doc/member[@name="_version_e__FixedBuffer"]/*' />
        [InlineArray(64)]
        public partial struct _version_e__FixedBuffer
        {
            public sbyte e0;
        }

        /// <include file='_reserved_e__FixedBuffer.xml' path='doc/member[@name="_reserved_e__FixedBuffer"]/*' />
        [InlineArray(16)]
        public partial struct _reserved_e__FixedBuffer
        {
            public sbyte e0;
        }
    }
}
