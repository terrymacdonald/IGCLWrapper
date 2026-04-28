using System;

namespace IGCLWrapper
{
    /// <summary>
    /// PCI helper: properties and current state.
    /// </summary>
    public sealed class IGCLPciHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLPciHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        /// <summary>
        /// Get PCI properties using the native struct.
        /// </summary>
        /// <returns>PCI properties struct.</returns>
        public unsafe ctl_pci_properties_t PciGetPropertiesNative()
        {
            ThrowIfDisposed();
            var props = new ctl_pci_properties_t { Size = (uint)sizeof(ctl_pci_properties_t), Version = 0 };
            var result = IGCL.ctlPciGetProperties((_ctl_device_adapter_handle_t*)_adapter, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get PCI properties");
            return props;
        }

        /// <summary>
        /// Get PCI properties as a DTO.
        /// </summary>
        /// <returns>PCI properties DTO.</returns>
        public PciPropertiesDto PciGetProperties()
        {
            var native = PciGetPropertiesNative();
            return PciPropertiesDto.FromNative(native);
        }

        /// <summary>
        /// Get PCI state.
        /// </summary>
        /// <returns>PCI state struct.</returns>
        public unsafe ctl_pci_state_t PciGetState()
        {
            ThrowIfDisposed();
            var state = new ctl_pci_state_t { Size = (uint)sizeof(ctl_pci_state_t), Version = 0 };
            var result = IGCL.ctlPciGetState((_ctl_device_adapter_handle_t*)_adapter, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get PCI state");
            return state;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLPciHelper));
        }

        /// <summary>
        /// Compare PCI properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool ArePciPropertiesEqual(ctl_pci_properties_t left, ctl_pci_properties_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   ArePciAddressEqual(left.address, right.address) &&
                   ArePciSpeedEqual(left.maxSpeed, right.maxSpeed) &&
                   left.resizable_bar_supported == right.resizable_bar_supported &&
                   left.resizable_bar_enabled == right.resizable_bar_enabled;
        }

        /// <summary>
        /// Compare PCI state while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left state struct.</param>
        /// <param name="right">Right state struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool ArePciStateEqual(ctl_pci_state_t left, ctl_pci_state_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   ArePciSpeedEqual(left.speed, right.speed);
        }

        private static bool ArePciAddressEqual(ctl_pci_address_t left, ctl_pci_address_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.domain == right.domain &&
                   left.bus == right.bus &&
                   left.device == right.device &&
                   left.function == right.function;
        }

        private static bool ArePciSpeedEqual(ctl_pci_speed_t left, ctl_pci_speed_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.gen == right.gen &&
                   left.width == right.width &&
                   left.maxBandwidth == right.maxBandwidth;
        }

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCLPciDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    /// <summary>
    /// DTO for PCI address values.
    /// </summary>
    public struct PciAddressDto : IEquatable<PciAddressDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// PCI domain.
        /// </summary>
        public uint Domain;
        /// <summary>
        /// PCI bus.
        /// </summary>
        public uint Bus;
        /// <summary>
        /// PCI device.
        /// </summary>
        public uint Device;
        /// <summary>
        /// PCI function.
        /// </summary>
        public uint Function;

        public bool Equals(PciAddressDto other)
        {
            return Domain == other.Domain &&
                   Bus == other.Bus &&
                   Device == other.Device &&
                   Function == other.Function;
        }

        public override bool Equals(object? obj) => obj is PciAddressDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Domain);
            hash.Add(Bus);
            hash.Add(Device);
            hash.Add(Function);
            return hash.ToHashCode();
        }

        public static PciAddressDto FromNative(ctl_pci_address_t native)
        {
            return new PciAddressDto
            {
                Size = native.Size,
                Version = native.Version,
                Domain = native.domain,
                Bus = native.bus,
                Device = native.device,
                Function = native.function
            };
        }

        public unsafe ctl_pci_address_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_pci_address_t);

            return new ctl_pci_address_t
            {
                Size = size,
                Version = Version,
                domain = Domain,
                bus = Bus,
                device = Device,
                function = Function
            };
        }
    }

    /// <summary>
    /// DTO for PCI link speed values.
    /// </summary>
    public struct PciSpeedDto : IEquatable<PciSpeedDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// PCIe generation.
        /// </summary>
        public int Generation;
        /// <summary>
        /// PCIe lane width.
        /// </summary>
        public int Width;
        /// <summary>
        /// Maximum bandwidth.
        /// </summary>
        public long MaxBandwidth;

        public bool Equals(PciSpeedDto other)
        {
            return Generation == other.Generation &&
                   Width == other.Width &&
                   MaxBandwidth == other.MaxBandwidth;
        }

        public override bool Equals(object? obj) => obj is PciSpeedDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Generation);
            hash.Add(Width);
            hash.Add(MaxBandwidth);
            return hash.ToHashCode();
        }

        public static PciSpeedDto FromNative(ctl_pci_speed_t native)
        {
            return new PciSpeedDto
            {
                Size = native.Size,
                Version = native.Version,
                Generation = native.gen,
                Width = native.width,
                MaxBandwidth = native.maxBandwidth
            };
        }

        public unsafe ctl_pci_speed_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_pci_speed_t);

            return new ctl_pci_speed_t
            {
                Size = size,
                Version = Version,
                gen = Generation,
                width = Width,
                maxBandwidth = MaxBandwidth
            };
        }
    }

    /// <summary>
    /// DTO for PCI properties.
    /// </summary>
    public struct PciPropertiesDto : IEquatable<PciPropertiesDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// PCI address.
        /// </summary>
        public PciAddressDto Address;
        /// <summary>
        /// Maximum PCIe speed.
        /// </summary>
        public PciSpeedDto MaxSpeed;
        /// <summary>
        /// Indicates whether resizable BAR is supported.
        /// </summary>
        public bool ResizableBarSupported;
        /// <summary>
        /// Indicates whether resizable BAR is enabled.
        /// </summary>
        public bool ResizableBarEnabled;

        /// <summary>
        /// Compare PCI properties.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(PciPropertiesDto other)
        {
                 return Address.Equals(other.Address) &&
                   MaxSpeed.Equals(other.MaxSpeed) &&
                   ResizableBarSupported == other.ResizableBarSupported &&
                   ResizableBarEnabled == other.ResizableBarEnabled;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is PciPropertiesDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Address);
            hash.Add(MaxSpeed);
            hash.Add(ResizableBarSupported);
            hash.Add(ResizableBarEnabled);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>PCI properties DTO.</returns>
        public static PciPropertiesDto FromNative(ctl_pci_properties_t native)
        {
            return new PciPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                Address = PciAddressDto.FromNative(native.address),
                MaxSpeed = PciSpeedDto.FromNative(native.maxSpeed),
                ResizableBarSupported = IGCLPciDtoBool.ToBool(native.resizable_bar_supported),
                ResizableBarEnabled = IGCLPciDtoBool.ToBool(native.resizable_bar_enabled)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>PCI properties struct.</returns>
        public unsafe ctl_pci_properties_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_pci_properties_t);

            return new ctl_pci_properties_t
            {
                Size = size,
                Version = Version,
                address = Address.ToNative(),
                maxSpeed = MaxSpeed.ToNative(),
                resizable_bar_supported = IGCLPciDtoBool.ToByte(ResizableBarSupported),
                resizable_bar_enabled = IGCLPciDtoBool.ToByte(ResizableBarEnabled)
            };
        }
    }
}

