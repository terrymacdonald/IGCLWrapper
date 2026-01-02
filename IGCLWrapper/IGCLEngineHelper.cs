using System;
using System.Collections.Generic;

namespace IGCLWrapper
{
    /// <summary>
    /// Engine helper: enumerate engine groups and query properties/activity.
    /// </summary>
    public sealed class IGCLEngineHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLEngineHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe IReadOnlyList<IntPtr> EnumerateEngines()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter, IGCL.ctlEnumEngineGroups);
        }

        public unsafe ctl_engine_properties_t GetProperties(IntPtr engineHandle)
        {
            ThrowIfDisposed();
            var props = IGCLApiHelper.CreateEngineProperties();
            var result = IGCL.ctlEngineGetProperties((_ctl_engine_handle_t*)engineHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get engine properties");
            return props;
        }

        public unsafe ctl_engine_stats_t GetActivity(IntPtr engineHandle)
        {
            ThrowIfDisposed();
            var stats = IGCLApiHelper.CreateEngineStats();
            var result = IGCL.ctlEngineGetActivity((_ctl_engine_handle_t*)engineHandle, &stats);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get engine activity");
            return stats;
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter, delegate* unmanaged[Cdecl]< _ctl_device_adapter_handle_t*, uint*, _ctl_engine_handle_t**, ctl_result_t> enumerateFn)
        {
            uint count = 0;
            var result = enumerateFn(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get engine count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = enumerateFn(adapter, &count, (_ctl_engine_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate engines");
            }
            return handles;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLEngineHelper));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
