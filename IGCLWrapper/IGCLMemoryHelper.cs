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
        /// Get memory module properties as a DTO.
        /// </summary>
        /// <param name="memoryHandle">Memory module handle.</param>
        /// <returns>Memory properties DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe MemoryPropertiesDto? MemoryGetProperties(IntPtr memoryHandle)
        {
            ThrowIfDisposed();
            var props = CreateMemoryProperties();
            var result = IGCL.ctlMemoryGetProperties((_ctl_mem_handle_t*)memoryHandle, &props);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return MemoryPropertiesDto.FromNative(props);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, "Failed to get memory properties");
        }

        /// <summary>
        /// Get current memory module state as a DTO.
        /// </summary>
        /// <param name="memoryHandle">Memory module handle.</param>
        /// <returns>Memory state DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe MemoryStateDto? MemoryGetState(IntPtr memoryHandle)
        {
            ThrowIfDisposed();
            var state = CreateMemoryState();
            var result = IGCL.ctlMemoryGetState((_ctl_mem_handle_t*)memoryHandle, &state);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return MemoryStateDto.FromNative(state);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, "Failed to get memory state");
        }

        /// <summary>
        /// Get memory bandwidth information as a DTO.
        /// </summary>
        /// <param name="memoryHandle">Memory module handle.</param>
        /// <returns>Memory bandwidth DTO, or <c>null</c> if the feature is not supported on this hardware or driver.</returns>
        public unsafe MemoryBandwidthDto? MemoryGetBandwidth(IntPtr memoryHandle)
        {
            ThrowIfDisposed();
            var bw = CreateMemoryBandwidth();
            var result = IGCL.ctlMemoryGetBandwidth((_ctl_mem_handle_t*)memoryHandle, &bw);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                return MemoryBandwidthDto.FromNative(bw);
            if (IsUnsupportedResult(result))
                return null;
            throw new IGCLException(result, $"Failed to get memory bandwidth: {result}");
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

        /// <summary>
        /// Returns true when the result code indicates a feature is not available
        /// on the current hardware or driver, rather than a genuine API failure.
        /// </summary>
        private static bool IsUnsupportedResult(ctl_result_t result)
        {
            return result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                || result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION
                || result == ctl_result_t.CTL_RESULT_ERROR_INVALID_OPERATION_TYPE
                || result == ctl_result_t.CTL_RESULT_ERROR_INVALID_ARGUMENT;
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

    public struct MemoryPropertiesDto : IEquatable<MemoryPropertiesDto>
    {
        public uint Size;
        public byte Version;
        public ctl_mem_type_t Type;
        public ctl_mem_loc_t Location;
        public ulong PhysicalSize;
        public int BusWidth;
        public int NumChannels;

        public bool Equals(MemoryPropertiesDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Type == other.Type &&
                   Location == other.Location &&
                   PhysicalSize == other.PhysicalSize &&
                   BusWidth == other.BusWidth &&
                   NumChannels == other.NumChannels;
        }

        public override bool Equals(object? obj) => obj is MemoryPropertiesDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Type);
            hash.Add(Location);
            hash.Add(PhysicalSize);
            hash.Add(BusWidth);
            hash.Add(NumChannels);
            return hash.ToHashCode();
        }

        public static MemoryPropertiesDto FromNative(ctl_mem_properties_t native)
        {
            return new MemoryPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                Type = native.type,
                Location = native.location,
                PhysicalSize = native.physicalSize,
                BusWidth = native.busWidth,
                NumChannels = native.numChannels
            };
        }

        public unsafe ctl_mem_properties_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_mem_properties_t);
            return new ctl_mem_properties_t
            {
                Size = size,
                Version = Version,
                type = Type,
                location = Location,
                physicalSize = PhysicalSize,
                busWidth = BusWidth,
                numChannels = NumChannels
            };
        }
    }

    public struct MemoryStateDto : IEquatable<MemoryStateDto>
    {
        public uint Size;
        public byte Version;
        public ulong Free;
        public ulong TotalSize;

        public bool Equals(MemoryStateDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Free == other.Free &&
                   TotalSize == other.TotalSize;
        }

        public override bool Equals(object? obj) => obj is MemoryStateDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Free);
            hash.Add(TotalSize);
            return hash.ToHashCode();
        }

        public static MemoryStateDto FromNative(ctl_mem_state_t native)
        {
            return new MemoryStateDto
            {
                Size = native.Size,
                Version = native.Version,
                Free = native.free,
                TotalSize = native.size
            };
        }

        public unsafe ctl_mem_state_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_mem_state_t);
            return new ctl_mem_state_t
            {
                Size = size,
                Version = Version,
                free = Free,
                size = TotalSize
            };
        }
    }

    public struct MemoryBandwidthDto : IEquatable<MemoryBandwidthDto>
    {
        public uint Size;
        public byte Version;
        public ulong MaxBandwidth;
        public ulong Timestamp;
        public ulong ReadCounter;
        public ulong WriteCounter;

        public bool Equals(MemoryBandwidthDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   MaxBandwidth == other.MaxBandwidth &&
                   Timestamp == other.Timestamp &&
                   ReadCounter == other.ReadCounter &&
                   WriteCounter == other.WriteCounter;
        }

        public override bool Equals(object? obj) => obj is MemoryBandwidthDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(MaxBandwidth);
            hash.Add(Timestamp);
            hash.Add(ReadCounter);
            hash.Add(WriteCounter);
            return hash.ToHashCode();
        }

        public static MemoryBandwidthDto FromNative(ctl_mem_bandwidth_t native)
        {
            return new MemoryBandwidthDto
            {
                Size = native.Size,
                Version = native.Version,
                MaxBandwidth = native.maxBandwidth,
                Timestamp = native.timestamp,
                ReadCounter = native.readCounter,
                WriteCounter = native.writeCounter
            };
        }

        public unsafe ctl_mem_bandwidth_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_mem_bandwidth_t);
            return new ctl_mem_bandwidth_t
            {
                Size = size,
                Version = Version,
                maxBandwidth = MaxBandwidth,
                timestamp = Timestamp,
                readCounter = ReadCounter,
                writeCounter = WriteCounter
            };
        }
    }
}

