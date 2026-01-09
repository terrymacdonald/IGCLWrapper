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

        public unsafe ctl_pci_properties_t PciGetPropertiesNative()
        {
            ThrowIfDisposed();
            var props = new ctl_pci_properties_t { Size = (uint)sizeof(ctl_pci_properties_t), Version = 0 };
            var result = IGCL.ctlPciGetProperties((_ctl_device_adapter_handle_t*)_adapter, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get PCI properties");
            return props;
        }

        public PciPropertiesDto PciGetProperties()
        {
            var native = PciGetPropertiesNative();
            return PciPropertiesDto.FromNative(native);
        }

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

    public struct PciPropertiesDto
    {
        public uint Size;
        public byte Version;
        public ctl_pci_address_t Address;
        public ctl_pci_speed_t MaxSpeed;
        public bool ResizableBarSupported;
        public bool ResizableBarEnabled;

        public static PciPropertiesDto FromNative(ctl_pci_properties_t native)
        {
            return new PciPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                Address = native.address,
                MaxSpeed = native.maxSpeed,
                ResizableBarSupported = IGCLPciDtoBool.ToBool(native.resizable_bar_supported),
                ResizableBarEnabled = IGCLPciDtoBool.ToBool(native.resizable_bar_enabled)
            };
        }

        public ctl_pci_properties_t ToNative()
        {
            return new ctl_pci_properties_t
            {
                Size = Size,
                Version = Version,
                address = Address,
                maxSpeed = MaxSpeed,
                resizable_bar_supported = IGCLPciDtoBool.ToByte(ResizableBarSupported),
                resizable_bar_enabled = IGCLPciDtoBool.ToByte(ResizableBarEnabled)
            };
        }
    }
}
