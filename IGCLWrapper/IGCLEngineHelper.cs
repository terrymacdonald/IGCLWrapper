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

        public unsafe IReadOnlyList<IntPtr> EnumEngineGroups()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        public unsafe ctl_engine_properties_t EngineGetProperties(IntPtr engineHandle)
        {
            ThrowIfDisposed();
            var props = CreateEngineProperties();
            var result = IGCL.ctlEngineGetProperties((_ctl_engine_handle_t*)engineHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get engine properties");
            return props;
        }

        public unsafe ctl_engine_stats_t EngineGetActivity(IntPtr engineHandle)
        {
            ThrowIfDisposed();
            var stats = CreateEngineStats();
            var result = IGCL.ctlEngineGetActivity((_ctl_engine_handle_t*)engineHandle, &stats);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get engine activity");
            return stats;
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumEngineGroups(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get engine count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumEngineGroups(adapter, &count, (_ctl_engine_handle_t**)pHandles);
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

        private static unsafe ctl_engine_properties_t CreateEngineProperties() => new ctl_engine_properties_t { Size = (uint)sizeof(ctl_engine_properties_t), Version = 0 };
        private static unsafe ctl_engine_stats_t CreateEngineStats() => new ctl_engine_stats_t { Size = (uint)sizeof(ctl_engine_stats_t), Version = 0 };

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
