using System;
using System.Collections.Generic;

namespace IGCLWrapper
{
    /// <summary>
    /// Memory helper: enumerate memory modules and query properties/state/bandwidth.
    /// </summary>
    public sealed class IGCLMemoryHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLMemoryHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe IReadOnlyList<IntPtr> EnumMemoryModules()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        public unsafe ctl_mem_properties_t MemoryGetProperties(IntPtr memoryHandle)
        {
            ThrowIfDisposed();
            var props = IGCLApiHelper.CreateMemoryProperties();
            var result = IGCL.ctlMemoryGetProperties((_ctl_mem_handle_t*)memoryHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get memory properties");
            return props;
        }

        public unsafe ctl_mem_state_t MemoryGetState(IntPtr memoryHandle)
        {
            ThrowIfDisposed();
            var state = IGCLApiHelper.CreateMemoryState();
            var result = IGCL.ctlMemoryGetState((_ctl_mem_handle_t*)memoryHandle, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get memory state");
            return state;
        }

        public unsafe ctl_mem_bandwidth_t MemoryGetBandwidth(IntPtr memoryHandle)
        {
            ThrowIfDisposed();
            var bw = IGCLApiHelper.CreateMemoryBandwidth();
            var result = IGCL.ctlMemoryGetBandwidth((_ctl_mem_handle_t*)memoryHandle, &bw);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get memory bandwidth");
            return bw;
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumMemoryModules(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get memory module count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumMemoryModules(adapter, &count, (_ctl_mem_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate memory modules");
            }
            return handles;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLMemoryHelper));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
