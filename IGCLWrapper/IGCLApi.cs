using System;
using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    /// <summary>
    /// Exception thrown when an IGCL API call fails
    /// </summary>
    public class IGCLException : Exception
    {
        public _ctl_result_t Result { get; }

        public IGCLException(_ctl_result_t result, string message = null)
            : base(message ?? $"IGCL API error: {result}")
        {
            Result = result;
        }
    }

    /// <summary>
    /// Main IGCL API wrapper providing safe access to Intel Graphics Control Library
    /// </summary>
    public sealed class IGCLApi : IDisposable
    {
        private unsafe _ctl_api_handle_t* _hApi;
        private bool _disposed;

        private unsafe IGCLApi(_ctl_api_handle_t* hApi)
        {
            _hApi = hApi;
        }

        /// <summary>
        /// Initialize the IGCL API with default settings
        /// </summary>
        public static IGCLApi Initialize()
        {
            unsafe
            {
                // Create initialization arguments
                var initArgs = new _ctl_init_args_t
                {
                    Size = (uint)sizeof(_ctl_init_args_t),
                    Version = 0,
                    AppVersion = MakeVersion(1, 0),
                    flags = (uint)(_ctl_init_flag_t)(1 << 0), // CTL_INIT_FLAG_USE_LEVEL_ZERO
                    SupportedVersion = GetImplVersion()
                };

                // Initialize the API
                _ctl_api_handle_t* hApi;
                var result = IGCL.ctlInit(&initArgs, &hApi);
                
                if (result != _ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    throw new IGCLException(result, $"Failed to initialize IGCL API");
                }

                return new IGCLApi(hApi);
            }
        }

        /// <summary>
        /// Enumerate all GPU adapters in the system
        /// </summary>
        public unsafe _ctl_device_adapter_handle_t*[] EnumerateAdapters()
        {
            ThrowIfDisposed();

            // Get adapter count
            uint adapterCount = 0;
            var result = IGCL.ctlEnumerateDevices(_hApi, &adapterCount, null);
            
            if (result != _ctl_result_t.CTL_RESULT_SUCCESS)
            {
                throw new IGCLException(result, "Failed to get adapter count");
            }

            if (adapterCount == 0)
            {
                return new _ctl_device_adapter_handle_t*[0];
            }

            // Get adapters
            var adapters = new _ctl_device_adapter_handle_t*[adapterCount];
            fixed (_ctl_device_adapter_handle_t** pAdapters = adapters)
            {
                result = IGCL.ctlEnumerateDevices(_hApi, &adapterCount, pAdapters);
                
                if (result != _ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    throw new IGCLException(result, "Failed to enumerate adapters");
                }
            }

            return adapters;
        }

        /// <summary>
        /// Enumerate display outputs for a given adapter
        /// </summary>
        public unsafe _ctl_display_output_handle_t*[] EnumerateDisplays(_ctl_device_adapter_handle_t* hAdapter)
        {
            ThrowIfDisposed();

            // Get display count
            uint displayCount = 0;
            var result = IGCL.ctlEnumerateDisplayOutputs(hAdapter, &displayCount, null);
            
            if (result != _ctl_result_t.CTL_RESULT_SUCCESS)
            {
                throw new IGCLException(result, "Failed to get display count");
            }

            if (displayCount == 0)
            {
                return new _ctl_display_output_handle_t*[0];
            }

            // Get displays
            var displays = new _ctl_display_output_handle_t*[displayCount];
            fixed (_ctl_display_output_handle_t** pDisplays = displays)
            {
                result = IGCL.ctlEnumerateDisplayOutputs(hAdapter, &displayCount, pDisplays);
                
                if (result != _ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    throw new IGCLException(result, "Failed to enumerate displays");
                }
            }

            return displays;
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            unsafe
            {
                if (_hApi != null)
                {
                    IGCL.ctlClose(_hApi);
                    _hApi = null;
                }
            }

            _disposed = true;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLApi));
        }

        #region Helper Methods for Version Macros

        /// <summary>
        /// Create a version number from major and minor components
        /// </summary>
        public static uint MakeVersion(uint major, uint minor)
        {
            return (major << 16) | (minor & 0x0000ffff);
        }

        /// <summary>
        /// Extract major version from version number
        /// </summary>
        public static uint GetMajorVersion(uint version)
        {
            return version >> 16;
        }

        /// <summary>
        /// Extract minor version from version number
        /// </summary>
        public static uint GetMinorVersion(uint version)
        {
            return version & 0x0000ffff;
        }

        /// <summary>
        /// Get the IGCL implementation version
        /// </summary>
        public static uint GetImplVersion()
        {
            // CTL_IMPL_VERSION = (CTL_IMPL_MAJOR_VERSION << 16) | CTL_IMPL_MINOR_VERSION
            // These are typically 1.0
            return MakeVersion(1, 0);
        }

        #endregion
    }
}
