using System;

namespace IGCLWrapper
{
    /// <summary>
    /// Display helper facade for IGCL display handles.
    /// </summary>
    public sealed class IGCLDisplayHelper : IDisposable
    {
        private readonly object _lock = new();
        private ctl_display_properties_t? _properties;
        private bool _disposed;
        internal IGCLApiHelper Api { get; }
        internal IntPtr DisplayHandle { get; }

        internal IGCLDisplayHelper(IGCLApiHelper api, IntPtr displayHandle)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            DisplayHandle = displayHandle;
        }

        public unsafe ctl_display_properties_t GetProperties()
        {
            ThrowIfDisposed();
            lock (_lock)
            {
                if (_properties.HasValue)
                {
                    return _properties.Value;
                }

                var props = IGCLApiHelper.CreateDisplayProperties();
                var result = IGCL.ctlGetDisplayProperties((_ctl_display_output_handle_t*)DisplayHandle, &props);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    throw new IGCLException(result, "Failed to get display properties");
                }

                _properties = props;
                return props;
            }
        }

        public ctl_display_timing_t GetTiming()
        {
            var props = GetProperties();
            return props.Display_Timing_Info;
        }

        public bool IsActive()
        {
            var timing = GetTiming();
            return timing.HActive > 0 && timing.VActive > 0;
        }

        public (uint width, uint height) GetResolution()
        {
            var timing = GetTiming();
            return (timing.HActive, timing.VActive);
        }

        public double GetRefreshRateHz()
        {
            var timing = GetTiming();
            return timing.RefreshRate / 1000.0;
        }

        public string Name => $"Display-{DisplayHandle.ToInt64():X}";

        internal void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLDisplayHelper));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
