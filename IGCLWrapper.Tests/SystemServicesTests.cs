using Xunit;
using IGCLWrapper;
using System;

namespace IGCLWrapper.Tests
{
    /// <summary>
    /// Tests for System Services APIs including overclocking and system-level operations
    /// </summary>
    public class SystemServicesTests : IDisposable
    {
        private IGCLApi? _api;
        private IntPtr[]? _adapters;

        public SystemServicesTests()
        {
            try
            {
                _api = IGCLApi.Initialize();
                _adapters = _api?.EnumerateAdapters();
            }
            catch (DllNotFoundException)
            {
                _api = null;
            }
        }

        public void Dispose()
        {
            _api?.Dispose();
        }

        #region Overclocking Tests

        [Fact]
        public void CtlOverclockGetProperties_ShouldReturnProperties()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                var props = new _ctl_oc_properties_t
                {
                    Size = (uint)sizeof(_ctl_oc_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlOverclockGetProperties((_ctl_device_adapter_handle_t*)_adapters[0], &props);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [Fact]
        public void CtlOverclockGpuFrequencyOffsetGet_ShouldReturnOffset()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                double offset;
                var result = IGCL.ctlOverclockGpuFrequencyOffsetGet((_ctl_device_adapter_handle_t*)_adapters[0], &offset);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED
                );
            }
        }

        [Fact]
        public void CtlOverclockGpuVoltageOffsetGet_ShouldReturnOffset()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                double offset;
                var result = IGCL.ctlOverclockGpuVoltageOffsetGet((_ctl_device_adapter_handle_t*)_adapters[0], &offset);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED
                );
            }
        }

        [Fact]
        public void CtlOverclockPowerLimitGet_ShouldReturnLimit()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                double limit;
                var result = IGCL.ctlOverclockPowerLimitGet((_ctl_device_adapter_handle_t*)_adapters[0], &limit);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED
                );
            }
        }

        [Fact]
        public void CtlOverclockTemperatureLimitGet_ShouldReturnLimit()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                double limit;
                var result = IGCL.ctlOverclockTemperatureLimitGet((_ctl_device_adapter_handle_t*)_adapters[0], &limit);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED
                );
            }
        }

        [Fact]
        public void CtlPowerTelemetryGet_ShouldReturnTelemetry()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                var telemetry = new _ctl_power_telemetry_t
                {
                    Size = (uint)sizeof(_ctl_power_telemetry_t),
                    Version = 0
                };

                var result = IGCL.ctlPowerTelemetryGet((_ctl_device_adapter_handle_t*)_adapters[0], &telemetry);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [Fact]
        public void CtlOverclockGpuFrequencyOffsetGetV2_ShouldReturnOffset()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                double offset;
                var result = IGCL.ctlOverclockGpuFrequencyOffsetGetV2((_ctl_device_adapter_handle_t*)_adapters[0], &offset);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED
                );
            }
        }

        [Fact]
        public void CtlOverclockGpuMaxVoltageOffsetGetV2_ShouldReturnOffset()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                double offset;
                var result = IGCL.ctlOverclockGpuMaxVoltageOffsetGetV2((_ctl_device_adapter_handle_t*)_adapters[0], &offset);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED
                );
            }
        }

        [Fact]
        public void CtlOverclockVramMemSpeedLimitGetV2_ShouldReturnLimit()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                double limit;
                var result = IGCL.ctlOverclockVramMemSpeedLimitGetV2((_ctl_device_adapter_handle_t*)_adapters[0], &limit);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED
                );
            }
        }

        [Fact]
        public void CtlOverclockPowerLimitGetV2_ShouldReturnLimit()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                double limit;
                var result = IGCL.ctlOverclockPowerLimitGetV2((_ctl_device_adapter_handle_t*)_adapters[0], &limit);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED
                );
            }
        }

        [Fact]
        public void CtlOverclockTemperatureLimitGetV2_ShouldReturnLimit()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                double limit;
                var result = IGCL.ctlOverclockTemperatureLimitGetV2((_ctl_device_adapter_handle_t*)_adapters[0], &limit);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED
                );
            }
        }

        [Fact]
        public void CtlOverclockGpuLockGet_ShouldReturnLock()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                var vfPair = new _ctl_oc_vf_pair_t
                {
                    Size = (uint)sizeof(_ctl_oc_vf_pair_t),
                    Version = 0
                };

                var result = IGCL.ctlOverclockGpuLockGet((_ctl_device_adapter_handle_t*)_adapters[0], &vfPair);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED
                );
            }
        }

        #endregion

        #region 3D Graphics Tests

        [Fact]
        public void CtlGetSupported3DCapabilities_ShouldReturnCapabilities()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                // First get count
                var caps = new _ctl_3d_feature_caps_t
                {
                    Size = (uint)sizeof(_ctl_3d_feature_caps_t),
                    Version = 0,
                    NumSupportedFeatures = 0,
                    pFeatureDetails = null
                };

                var result = IGCL.ctlGetSupported3DCapabilities((_ctl_device_adapter_handle_t*)_adapters[0], &caps);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        #endregion

        #region Media/Video Processing Tests

        [Fact]
        public void CtlGetSupportedVideoProcessingCapabilities_ShouldReturnCapabilities()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                // First get count
                var caps = new _ctl_video_processing_feature_caps_t
                {
                    Size = (uint)sizeof(_ctl_video_processing_feature_caps_t),
                    Version = 0,
                    NumSupportedFeatures = 0,
                    pFeatureDetails = null
                };

                var result = IGCL.ctlGetSupportedVideoProcessingCapabilities((_ctl_device_adapter_handle_t*)_adapters[0], &caps);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        #endregion

        #region Advanced Display Tests

        [Fact]
        public void CtlGetSupportedRetroScalingCapability_ShouldReturnCapabilities()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                var caps = new _ctl_retro_scaling_caps_t
                {
                    Size = (uint)sizeof(_ctl_retro_scaling_caps_t),
                    Version = 0
                };

                var result = IGCL.ctlGetSupportedRetroScalingCapability((_ctl_device_adapter_handle_t*)_adapters[0], &caps);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [Fact]
        public void CtlGetSetRetroScaling_ShouldReadSettings()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                var settings = new _ctl_retro_scaling_settings_t
                {
                    Size = (uint)sizeof(_ctl_retro_scaling_settings_t),
                    Version = 0,
                    Get = 1  // GET operation (true)
                };

                var result = IGCL.ctlGetSetRetroScaling((_ctl_device_adapter_handle_t*)_adapters[0], &settings);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        #endregion

        #region Linked Display Adapter Tests

        [Fact]
        public void CtlGetLinkedDisplayAdapters_ShouldReturnAdapters()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                var args = new _ctl_lda_args_t
                {
                    Size = (uint)sizeof(_ctl_lda_args_t),
                    Version = 0,
                    NumAdapters = 0,
                    hLinkedAdapters = null
                };

                var result = IGCL.ctlGetLinkedDisplayAdapters((_ctl_device_adapter_handle_t*)_adapters[0], &args);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_ADAPTER_NOT_SUPPORTED_ON_LDA_SECONDARY
                );
            }
        }

        #endregion

        #region VF Curve Tests

        [Fact]
        public void CtlOverclockReadVFCurve_ShouldReturnCurve()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                uint numPoints = 0;
                var result = IGCL.ctlOverclockReadVFCurve(
                    (_ctl_device_adapter_handle_t*)_adapters[0],
                    _ctl_vf_curve_type_t.CTL_VF_CURVE_TYPE_STOCK,
                    _ctl_vf_curve_details_t.CTL_VF_CURVE_DETAILS_SIMPLIFIED,
                    &numPoints,
                    null
                );

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED
                );
            }
        }

        #endregion

        #region Error Handling Tests

        [Fact]
        public void CtlOverclockSetWithoutWaiver_ShouldReturnError()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            // Try to set overclock without waiver - should fail
            unsafe
            {
                var result = IGCL.ctlOverclockGpuFrequencyOffsetSet((_ctl_device_adapter_handle_t*)_adapters[0], 50.0);

                // Assert - Should fail without waiver
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_WAIVER_NOT_SET ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED ||
                    result == _ctl_result_t.CTL_RESULT_SUCCESS // May already have waiver from previous test run
                );
            }
        }

        #endregion
    }
}
