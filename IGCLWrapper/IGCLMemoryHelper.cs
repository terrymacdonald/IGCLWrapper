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

        /// <summary>
        /// Enumerate memory module handles for the adapter.
        /// </summary>
        /// <returns>Read-only list of memory module handles.</returns>
        public unsafe IReadOnlyList<IntPtr> EnumMemoryModules()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        /// <summary>
        /// Get memory module properties.
        /// </summary>
        /// <param name="memoryHandle">Memory module handle.</param>
        /// <returns>Memory properties struct.</returns>
        public unsafe ctl_mem_properties_t MemoryGetProperties(IntPtr memoryHandle)
        {
            ThrowIfDisposed();
            var props = CreateMemoryProperties();
            var result = IGCL.ctlMemoryGetProperties((_ctl_mem_handle_t*)memoryHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get memory properties");
            return props;
        }

        /// <summary>
        /// Get current memory module state.
        /// </summary>
        /// <param name="memoryHandle">Memory module handle.</param>
        /// <returns>Memory state struct.</returns>
        public unsafe ctl_mem_state_t MemoryGetState(IntPtr memoryHandle)
        {
            ThrowIfDisposed();
            var state = CreateMemoryState();
            var result = IGCL.ctlMemoryGetState((_ctl_mem_handle_t*)memoryHandle, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get memory state");
            return state;
        }

        /// <summary>
        /// Get memory bandwidth information.
        /// </summary>
        /// <param name="memoryHandle">Memory module handle.</param>
        /// <returns>Memory bandwidth struct.</returns>
        public unsafe ctl_mem_bandwidth_t MemoryGetBandwidth(IntPtr memoryHandle)
        {
            ThrowIfDisposed();
            var bw = CreateMemoryBandwidth();
            var result = IGCL.ctlMemoryGetBandwidth((_ctl_mem_handle_t*)memoryHandle, &bw);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to get memory bandwidth: {result}");
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

        private static unsafe ctl_mem_properties_t CreateMemoryProperties() => new ctl_mem_properties_t { Size = (uint)sizeof(ctl_mem_properties_t), Version = 0 };
        private static unsafe ctl_mem_state_t CreateMemoryState() => new ctl_mem_state_t { Size = (uint)sizeof(ctl_mem_state_t), Version = 0 };
        private static unsafe ctl_mem_bandwidth_t CreateMemoryBandwidth() => new ctl_mem_bandwidth_t { Size = (uint)sizeof(ctl_mem_bandwidth_t), Version = 0 };

        /// <summary>
        /// Compare memory properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreMemoryPropertiesEqual(ctl_mem_properties_t left, ctl_mem_properties_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.type == right.type &&
                   left.location == right.location &&
                   left.physicalSize == right.physicalSize &&
                   left.busWidth == right.busWidth &&
                   left.numChannels == right.numChannels;
        }

        /// <summary>
        /// Compare memory state while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left state struct.</param>
        /// <param name="right">Right state struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreMemoryStatesEqual(ctl_mem_state_t left, ctl_mem_state_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.free == right.free &&
                   left.size == right.size;
        }

        /// <summary>
        /// Compare memory bandwidth while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left bandwidth struct.</param>
        /// <param name="right">Right bandwidth struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreMemoryBandwidthEqual(ctl_mem_bandwidth_t left, ctl_mem_bandwidth_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.maxBandwidth == right.maxBandwidth &&
                   left.timestamp == right.timestamp &&
                   left.readCounter == right.readCounter &&
                   left.writeCounter == right.writeCounter;
        }

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }
}

