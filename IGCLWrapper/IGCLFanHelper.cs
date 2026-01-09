using System;
using System.Collections.Generic;

namespace IGCLWrapper
{
    /// <summary>
    /// Fan helper: enumerate fans, query properties, and set modes.
    /// </summary>
    public sealed class IGCLFanHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLFanHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe IReadOnlyList<IntPtr> EnumFans()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        public unsafe ctl_fan_properties_t FanGetPropertiesNative(IntPtr fanHandle)
        {
            ThrowIfDisposed();
            var props = CreateFanProperties();
            var result = IGCL.ctlFanGetProperties((_ctl_fan_handle_t*)fanHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get fan properties");
            return props;
        }

        public FanPropertiesDto FanGetProperties(IntPtr fanHandle)
        {
            var native = FanGetPropertiesNative(fanHandle);
            return FanPropertiesDto.FromNative(native);
        }

        public unsafe ctl_fan_config_t FanGetConfig(IntPtr fanHandle)
        {
            ThrowIfDisposed();
            var config = CreateFanConfig();
            var result = IGCL.ctlFanGetConfig((_ctl_fan_handle_t*)fanHandle, &config);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get fan config");
            return config;
        }

        public unsafe void FanSetDefaultMode(IntPtr fanHandle)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlFanSetDefaultMode((_ctl_fan_handle_t*)fanHandle);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set fan default mode");
        }

        public unsafe void FanSetFixedSpeedMode(IntPtr fanHandle, ctl_fan_speed_t speed)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlFanSetFixedSpeedMode((_ctl_fan_handle_t*)fanHandle, &speed);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set fan fixed speed");
        }

        public unsafe void FanSetSpeedTableMode(IntPtr fanHandle, ctl_fan_speed_table_t table)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlFanSetSpeedTableMode((_ctl_fan_handle_t*)fanHandle, &table);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set fan speed table");
        }

        public unsafe int FanGetState(IntPtr fanHandle, ctl_fan_speed_units_t units)
        {
            ThrowIfDisposed();
            int speed = 0;
            var result = IGCL.ctlFanGetState((_ctl_fan_handle_t*)fanHandle, units, &speed);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get fan state");
            return speed;
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumFans(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get fan count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumFans(adapter, &count, (_ctl_fan_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate fans");
            }
            return handles;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLFanHelper));
        }

        private static unsafe ctl_fan_properties_t CreateFanProperties() => new ctl_fan_properties_t { Size = (uint)sizeof(ctl_fan_properties_t), Version = 0 };
        private static unsafe ctl_fan_config_t CreateFanConfig() => new ctl_fan_config_t { Size = (uint)sizeof(ctl_fan_config_t), Version = 0 };
        public static unsafe ctl_fan_speed_t CreateFanSpeed() => new ctl_fan_speed_t { Size = (uint)sizeof(ctl_fan_speed_t), Version = 0 };
        public static unsafe ctl_fan_speed_table_t CreateFanSpeedTable() => new ctl_fan_speed_table_t { Size = (uint)sizeof(ctl_fan_speed_table_t), Version = 0 };

        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCLFanDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    public struct FanPropertiesDto
    {
        public uint Size;
        public byte Version;
        public bool CanControl;
        public uint SupportedModes;
        public uint SupportedUnits;
        public int MaxRpm;
        public int MaxPoints;

        public static FanPropertiesDto FromNative(ctl_fan_properties_t native)
        {
            return new FanPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                CanControl = IGCLFanDtoBool.ToBool(native.canControl),
                SupportedModes = native.supportedModes,
                SupportedUnits = native.supportedUnits,
                MaxRpm = native.maxRPM,
                MaxPoints = native.maxPoints
            };
        }

        public ctl_fan_properties_t ToNative()
        {
            return new ctl_fan_properties_t
            {
                Size = Size,
                Version = Version,
                canControl = IGCLFanDtoBool.ToByte(CanControl),
                supportedModes = SupportedModes,
                supportedUnits = SupportedUnits,
                maxRPM = MaxRpm,
                maxPoints = MaxPoints
            };
        }
    }
}
