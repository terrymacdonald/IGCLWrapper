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

        public unsafe ctl_3d_feature_caps_t GetSupported3DCapabilities()
        {
            ThrowIfDisposed();
            var caps = Create3DFeatureCaps();
            var result = IGCL.ctlGetSupported3DCapabilities((_ctl_device_adapter_handle_t*)_adapter, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get 3D capabilities");
            return caps;
        }

        public unsafe ctl_3d_feature_getset_t GetSet3DFeature(ctl_3d_feature_getset_t feature)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlGetSet3DFeature((_ctl_device_adapter_handle_t*)_adapter, &feature);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to get/set 3D feature {feature.FeatureType}");
            return feature;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCL3DHelper));
        }

        private static unsafe ctl_3d_feature_caps_t Create3DFeatureCaps() => new ctl_3d_feature_caps_t { Size = (uint)sizeof(ctl_3d_feature_caps_t), Version = 0 };
        public static unsafe ctl_3d_feature_getset_t Create3DFeatureGetSet() => new ctl_3d_feature_getset_t { Size = (uint)sizeof(ctl_3d_feature_getset_t), Version = 0 };

        public void Dispose()
        {
            _disposed = true;
        }
    }
}
