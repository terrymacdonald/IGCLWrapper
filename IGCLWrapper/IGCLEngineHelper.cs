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

        /// <summary>
        /// Enumerate engine group handles for the adapter.
        /// </summary>
        /// <returns>Read-only list of engine handles.</returns>
        public unsafe IReadOnlyList<IntPtr> EnumEngineGroups()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        /// <summary>
        /// Get engine properties for a handle using the native struct.
        /// </summary>
        /// <param name="engineHandle">Engine handle.</param>
        /// <returns>Engine properties struct.</returns>
        public unsafe ctl_engine_properties_t EngineGetPropertiesNative(IntPtr engineHandle)
        {
            ThrowIfDisposed();
            var props = CreateEngineProperties();
            var result = IGCL.ctlEngineGetProperties((_ctl_engine_handle_t*)engineHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get engine properties");
            return props;
        }

        /// <summary>
        /// Get engine properties as a DTO.
        /// </summary>
        /// <param name="engineHandle">Engine handle.</param>
        /// <returns>Engine properties DTO.</returns>
        public EnginePropertiesDto EngineGetProperties(IntPtr engineHandle)
        {
            var native = EngineGetPropertiesNative(engineHandle);
            return EnginePropertiesDto.FromNative(native);
        }

        /// <summary>
        /// Get engine activity stats for a handle using the native struct.
        /// </summary>
        /// <param name="engineHandle">Engine handle.</param>
        /// <returns>Engine stats struct.</returns>
        public unsafe ctl_engine_stats_t EngineGetActivityNative(IntPtr engineHandle)
        {
            ThrowIfDisposed();
            var stats = CreateEngineStats();
            var result = IGCL.ctlEngineGetActivity((_ctl_engine_handle_t*)engineHandle, &stats);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get engine activity");
            return stats;
        }

        /// <summary>
        /// Get engine activity stats as a DTO.
        /// </summary>
        /// <param name="engineHandle">Engine handle.</param>
        /// <returns>Engine stats DTO.</returns>
        public EngineStatsDto EngineGetActivity(IntPtr engineHandle)
        {
            var native = EngineGetActivityNative(engineHandle);
            return EngineStatsDto.FromNative(native);
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

        /// <summary>
        /// Compare engine properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreEnginePropertiesEqual(ctl_engine_properties_t left, ctl_engine_properties_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.type == right.type;
        }

        /// <summary>
        /// Compare engine statistics while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left statistics struct.</param>
        /// <param name="right">Right statistics struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreEngineStatsEqual(ctl_engine_stats_t left, ctl_engine_stats_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.activeTime == right.activeTime &&
                   left.timestamp == right.timestamp;
        }

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }

    public struct EnginePropertiesDto : IEquatable<EnginePropertiesDto>
    {
        public uint Size;
        public byte Version;
        public ctl_engine_group_t Type;

        public bool Equals(EnginePropertiesDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Type == other.Type;
        }

        public override bool Equals(object? obj) => obj is EnginePropertiesDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Type);
            return hash.ToHashCode();
        }

        public static EnginePropertiesDto FromNative(ctl_engine_properties_t native)
        {
            return new EnginePropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                Type = native.type
            };
        }

        public unsafe ctl_engine_properties_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_engine_properties_t);
            return new ctl_engine_properties_t
            {
                Size = size,
                Version = Version,
                type = Type
            };
        }
    }

    public struct EngineStatsDto : IEquatable<EngineStatsDto>
    {
        public uint Size;
        public byte Version;
        public ulong ActiveTime;
        public ulong Timestamp;

        public bool Equals(EngineStatsDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   ActiveTime == other.ActiveTime &&
                   Timestamp == other.Timestamp;
        }

        public override bool Equals(object? obj) => obj is EngineStatsDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(ActiveTime);
            hash.Add(Timestamp);
            return hash.ToHashCode();
        }

        public static EngineStatsDto FromNative(ctl_engine_stats_t native)
        {
            return new EngineStatsDto
            {
                Size = native.Size,
                Version = native.Version,
                ActiveTime = native.activeTime,
                Timestamp = native.timestamp
            };
        }

        public unsafe ctl_engine_stats_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_engine_stats_t);
            return new ctl_engine_stats_t
            {
                Size = size,
                Version = Version,
                activeTime = ActiveTime,
                timestamp = Timestamp
            };
        }
    }
}

