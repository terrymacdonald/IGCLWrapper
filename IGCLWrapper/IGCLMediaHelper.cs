using System;

namespace IGCLWrapper
{
    /// <summary>
    /// Media helper: video processing capabilities and get/set.
    /// </summary>
    public sealed class IGCLMediaHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLMediaHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe ctl_video_processing_feature_caps_t GetSupportedVideoProcessingCapabilities()
        {
            ThrowIfDisposed();
            var caps = IGCLApiHelper.CreateVideoProcessingCaps();
            var result = IGCL.ctlGetSupportedVideoProcessingCapabilities((_ctl_device_adapter_handle_t*)_adapter, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get video processing capabilities");
            return caps;
        }

        public unsafe ctl_video_processing_feature_getset_t GetSetVideoProcessingFeature(ctl_video_processing_feature_getset_t featureGetSet)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlGetSetVideoProcessingFeature((_ctl_device_adapter_handle_t*)_adapter, &featureGetSet);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to get/set video processing feature {featureGetSet.FeatureType}");
            return featureGetSet;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLMediaHelper));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
