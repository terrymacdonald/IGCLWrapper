using System;

namespace IGCLWrapper
{
    /// <summary>
    /// ECC helper: properties and state management.
    /// </summary>
    public sealed class IGCLEccHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLEccHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe ctl_ecc_properties_t EccGetProperties()
        {
            ThrowIfDisposed();
            var props = IGCLApiHelper.CreateEccProperties();
            var result = IGCL.ctlEccGetProperties((_ctl_device_adapter_handle_t*)_adapter, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get ECC properties");
            return props;
        }

        public unsafe ctl_ecc_state_desc_t EccGetState()
        {
            ThrowIfDisposed();
            var state = IGCLApiHelper.CreateEccState();
            var result = IGCL.ctlEccGetState((_ctl_device_adapter_handle_t*)_adapter, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get ECC state");
            return state;
        }

        public unsafe void EccSetState(ctl_ecc_state_t desiredState)
        {
            ThrowIfDisposed();
            var state = IGCLApiHelper.CreateEccState();
            state.currentEccState = desiredState;
            var result = IGCL.ctlEccSetState((_ctl_device_adapter_handle_t*)_adapter, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to set ECC state to {desiredState}");
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLEccHelper));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
