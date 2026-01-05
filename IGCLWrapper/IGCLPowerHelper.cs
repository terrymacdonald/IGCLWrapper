using System;
using System.Collections.Generic;

namespace IGCLWrapper
{
    /// <summary>
    /// Power helper: enumerate power domains, properties, energy counters, limits.
    /// </summary>
    public sealed class IGCLPowerHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLPowerHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe IReadOnlyList<IntPtr> EnumPowerDomains()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        public unsafe ctl_power_properties_t PowerGetProperties(IntPtr powerHandle)
        {
            ThrowIfDisposed();
            var props = CreatePowerProperties();
            var result = IGCL.ctlPowerGetProperties((_ctl_pwr_handle_t*)powerHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power properties");
            return props;
        }

        public unsafe ctl_power_energy_counter_t PowerGetEnergyCounter(IntPtr powerHandle)
        {
            ThrowIfDisposed();
            var counter = CreatePowerEnergyCounter();
            var result = IGCL.ctlPowerGetEnergyCounter((_ctl_pwr_handle_t*)powerHandle, &counter);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power energy counter");
            return counter;
        }

        public unsafe ctl_power_limits_t PowerGetLimits(IntPtr powerHandle)
        {
            ThrowIfDisposed();
            var limits = CreatePowerLimits();
            var result = IGCL.ctlPowerGetLimits((_ctl_pwr_handle_t*)powerHandle, &limits);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power limits");
            return limits;
        }

        public unsafe void PowerSetLimits(IntPtr powerHandle, ctl_power_limits_t limits)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlPowerSetLimits((_ctl_pwr_handle_t*)powerHandle, &limits);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set power limits");
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumPowerDomains(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get power domain count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumPowerDomains(adapter, &count, (_ctl_pwr_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate power domains");
            }
            return handles;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLPowerHelper));
        }

        private static unsafe ctl_power_properties_t CreatePowerProperties() => new ctl_power_properties_t { Size = (uint)sizeof(ctl_power_properties_t), Version = 0 };
        private static unsafe ctl_power_energy_counter_t CreatePowerEnergyCounter() => new ctl_power_energy_counter_t { Size = (uint)sizeof(ctl_power_energy_counter_t), Version = 0 };
        private static unsafe ctl_power_limits_t CreatePowerLimits() => new ctl_power_limits_t { Size = (uint)sizeof(ctl_power_limits_t), Version = 0 };

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
