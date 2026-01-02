using System;

namespace IGCLWrapper
{
    /// <summary>
    /// 3D feature helper: capabilities and get/set operations.
    /// </summary>
    public sealed class IGCL3DHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCL3DHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        public unsafe ctl_3d_feature_caps_t GetCapabilities()
        {
            ThrowIfDisposed();
            var caps = IGCLApiHelper.Create3DFeatureCaps();
            var result = IGCL.ctlGetSupported3DCapabilities((_ctl_device_adapter_handle_t*)_adapter, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get 3D capabilities");
            return caps;
        }

        public unsafe ctl_3d_feature_getset_t GetFeature(ctl_3d_feature_t featureType)
        {
            ThrowIfDisposed();
            var feature = IGCLApiHelper.Create3DFeatureGetSet(featureType);
            var result = IGCL.ctlGetSet3DFeature((_ctl_device_adapter_handle_t*)_adapter, &feature);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to get 3D feature {featureType}");
            return feature;
        }

        public unsafe void SetFeature(ctl_3d_feature_getset_t feature)
        {
            ThrowIfDisposed();
            feature.bSet = true;
            var result = IGCL.ctlGetSet3DFeature((_ctl_device_adapter_handle_t*)_adapter, &feature);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to set 3D feature {feature.FeatureType}");
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCL3DHelper));
        }

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
