using System;
using System.Collections.Generic;

namespace IGCLWrapper
{
    /// <summary>
    /// Frequency helper: enumerate domains, query properties/state, and set ranges.
    /// </summary>
    public sealed class IGCLFrequencyHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLFrequencyHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe IReadOnlyList<IntPtr> EnumerateDomains()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        public unsafe ctl_freq_properties_t GetProperties(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            var props = IGCLApiHelper.CreateFrequencyProperties();
            var result = IGCL.ctlFrequencyGetProperties((_ctl_freq_handle_t*)freqHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get frequency properties");
            return props;
        }

        public unsafe double[] GetAvailableClocks(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            uint count = 0;
            var result = IGCL.ctlFrequencyGetAvailableClocks((_ctl_freq_handle_t*)freqHandle, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get available clocks count");
            if (count == 0)
                return Array.Empty<double>();
            var freqs = new double[count];
            fixed (double* pFreqs = freqs)
            {
                result = IGCL.ctlFrequencyGetAvailableClocks((_ctl_freq_handle_t*)freqHandle, &count, pFreqs);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get available clocks");
            }
            return freqs;
        }

        public unsafe ctl_freq_range_t GetRange(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            var range = IGCLApiHelper.CreateFrequencyRange();
            var result = IGCL.ctlFrequencyGetRange((_ctl_freq_handle_t*)freqHandle, &range);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get frequency range");
            return range;
        }

        public unsafe void SetRange(IntPtr freqHandle, ctl_freq_range_t range)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlFrequencySetRange((_ctl_freq_handle_t*)freqHandle, &range);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set frequency range");
        }

        public unsafe ctl_freq_state_t GetState(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            var state = IGCLApiHelper.CreateFrequencyState();
            var result = IGCL.ctlFrequencyGetState((_ctl_freq_handle_t*)freqHandle, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get frequency state");
            return state;
        }

        public unsafe ctl_freq_throttle_time_t GetThrottleTime(IntPtr freqHandle)
        {
            ThrowIfDisposed();
            var tt = IGCLApiHelper.CreateFrequencyThrottleTime();
            var result = IGCL.ctlFrequencyGetThrottleTime((_ctl_freq_handle_t*)freqHandle, &tt);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get throttle time");
            return tt;
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumFrequencyDomains(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get frequency domain count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumFrequencyDomains(adapter, &count, (_ctl_freq_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate frequency domains");
            }
            return handles;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLFrequencyHelper));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
