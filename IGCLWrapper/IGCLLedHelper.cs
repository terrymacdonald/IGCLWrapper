using System;
using System.Collections.Generic;

namespace IGCLWrapper
{
    /// <summary>
    /// LED helper: enumerate LEDs and get/set state.
    /// </summary>
    public sealed class IGCLLedHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLLedHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe IReadOnlyList<IntPtr> EnumLeds()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        public unsafe ctl_led_properties_t LedGetProperties(IntPtr ledHandle)
        {
            ThrowIfDisposed();
            var props = IGCLApiHelper.CreateLedProperties();
            var result = IGCL.ctlLedGetProperties((_ctl_led_handle_t*)ledHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get LED properties");
            return props;
        }

        public unsafe ctl_led_state_t LedGetState(IntPtr ledHandle)
        {
            ThrowIfDisposed();
            var state = IGCLApiHelper.CreateLedState();
            var result = IGCL.ctlLedGetState((_ctl_led_handle_t*)ledHandle, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get LED state");
            return state;
        }

        public unsafe void LedSetState(IntPtr ledHandle, ctl_led_state_t state)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlLedSetState((_ctl_led_handle_t*)ledHandle, &state, (uint)sizeof(ctl_led_state_t));
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set LED state");
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumLeds(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get LED count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumLeds(adapter, &count, (_ctl_led_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate LEDs");
            }
            return handles;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLLedHelper));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
