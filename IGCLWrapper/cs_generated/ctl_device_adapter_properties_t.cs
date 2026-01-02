using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t"]/*' />
    public unsafe partial struct ctl_device_adapter_properties_t
    {
        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.pDeviceID"]/*' />
        public void* pDeviceID;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.device_id_size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint device_id_size;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.device_type"]/*' />
        public ctl_device_type_t device_type;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.supported_subfunction_flags"]/*' />
        [NativeTypeName("ctl_supported_functions_flags_t")]
        public uint supported_subfunction_flags;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.driver_version"]/*' />
        [NativeTypeName("uint64_t")]
        public ulong driver_version;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.firmware_version"]/*' />
        public ctl_firmware_version_t firmware_version;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.pci_vendor_id"]/*' />
        [NativeTypeName("uint32_t")]
        public uint pci_vendor_id;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.pci_device_id"]/*' />
        [NativeTypeName("uint32_t")]
        public uint pci_device_id;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.rev_id"]/*' />
        [NativeTypeName("uint32_t")]
        public uint rev_id;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.num_eus_per_sub_slice"]/*' />
        [NativeTypeName("uint32_t")]
        public uint num_eus_per_sub_slice;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.num_sub_slices_per_slice"]/*' />
        [NativeTypeName("uint32_t")]
        public uint num_sub_slices_per_slice;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.num_slices"]/*' />
        [NativeTypeName("uint32_t")]
        public uint num_slices;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.name"]/*' />
        [NativeTypeName("char[100]")]
        public _name_e__FixedBuffer name;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.graphics_adapter_properties"]/*' />
        [NativeTypeName("ctl_adapter_properties_flags_t")]
        public uint graphics_adapter_properties;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.Frequency"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Frequency;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.pci_subsys_id"]/*' />
        [NativeTypeName("uint16_t")]
        public ushort pci_subsys_id;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.pci_subsys_vendor_id"]/*' />
        [NativeTypeName("uint16_t")]
        public ushort pci_subsys_vendor_id;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.adapter_bdf"]/*' />
        public ctl_adapter_bdf_t adapter_bdf;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.num_xe_cores"]/*' />
        [NativeTypeName("uint32_t")]
        public uint num_xe_cores;

        /// <include file='ctl_device_adapter_properties_t.xml' path='doc/member[@name="ctl_device_adapter_properties_t.reserved"]/*' />
        [NativeTypeName("char[108]")]
        public _reserved_e__FixedBuffer reserved;

        /// <include file='_name_e__FixedBuffer.xml' path='doc/member[@name="_name_e__FixedBuffer"]/*' />
        [InlineArray(100)]
        public partial struct _name_e__FixedBuffer
        {
            public sbyte e0;
        }

        /// <include file='_reserved_e__FixedBuffer.xml' path='doc/member[@name="_reserved_e__FixedBuffer"]/*' />
        [InlineArray(108)]
        public partial struct _reserved_e__FixedBuffer
        {
            public sbyte e0;
        }
    }
}
