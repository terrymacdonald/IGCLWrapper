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

        public unsafe ctl_pci_properties_t PciGetProperties()
        {
            ThrowIfDisposed();
            var props = new ctl_pci_properties_t { Size = (uint)sizeof(ctl_pci_properties_t), Version = 0 };
            var result = IGCL.ctlPciGetProperties((_ctl_device_adapter_handle_t*)_adapter, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get PCI properties");
            return props;
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
}
