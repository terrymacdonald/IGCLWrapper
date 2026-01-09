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

        public unsafe ctl_ecc_properties_t EccGetPropertiesNative()
        {
            ThrowIfDisposed();
            var props = CreateEccProperties();
            var result = IGCL.ctlEccGetProperties((_ctl_device_adapter_handle_t*)_adapter, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get ECC properties");
            return props;
        }

        public EccPropertiesDto EccGetProperties()
        {
            var native = EccGetPropertiesNative();
            return EccPropertiesDto.FromNative(native);
        }

        public unsafe ctl_ecc_state_desc_t EccGetState()
        {
            ThrowIfDisposed();
            var state = CreateEccState();
            var result = IGCL.ctlEccGetState((_ctl_device_adapter_handle_t*)_adapter, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get ECC state");
            return state;
        }

        public unsafe void EccSetState(ctl_ecc_state_t desiredState)
        {
            ThrowIfDisposed();
            var state = CreateEccState();
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

        private static unsafe ctl_ecc_properties_t CreateEccProperties() => new ctl_ecc_properties_t { Size = (uint)sizeof(ctl_ecc_properties_t), Version = 0 };
        private static unsafe ctl_ecc_state_desc_t CreateEccState() => new ctl_ecc_state_desc_t { Size = (uint)sizeof(ctl_ecc_state_desc_t), Version = 0 };

        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCLEccDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    public struct EccPropertiesDto
    {
        public uint Size;
        public byte Version;
        public bool IsSupported;
        public bool CanControl;

        public static EccPropertiesDto FromNative(ctl_ecc_properties_t native)
        {
            return new EccPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                IsSupported = IGCLEccDtoBool.ToBool(native.isSupported),
                CanControl = IGCLEccDtoBool.ToBool(native.canControl)
            };
        }

        public ctl_ecc_properties_t ToNative()
        {
            return new ctl_ecc_properties_t
            {
                Size = Size,
                Version = Version,
                isSupported = IGCLEccDtoBool.ToByte(IsSupported),
                canControl = IGCLEccDtoBool.ToByte(CanControl)
            };
        }
    }
}
