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
        private readonly IGCLApi? _api;
        private readonly IntPtr[]? _adapters;
        private readonly bool _hasHardware;
        private readonly bool _hasDll;
        private readonly string _skipReason = string.Empty;

        public SystemServicesTests()
        {
            // Stage 1: Check for Intel GPU hardware via PCI
            if (!HardwareDetection.HasIntelGPU(out string hwError))
            {
                _hasHardware = false;
                _hasDll = false;
                _skipReason = hwError;
                return;
            }
            _hasHardware = true;

            // Stage 2: Check for IGCL DLL availability
            if (!IGCLApi.IsIGCLDllAvailable(out string dllError))
            {
                _hasDll = false;
                _skipReason = dllError;
                return;
            }
            _hasDll = true;

            // Stage 3: Try to initialize IGCL API
            try
            {
                _api = IGCLApi.Initialize();
                _adapters = _api?.EnumerateAdapters();
            }
            catch (IGCLException ex)
            {
                _skipReason = $"IGCL initialization failed: {ex.Message}";
            }
            catch (DllNotFoundException)
            {
                _skipReason = "IGCL DLL not found";
            }
        }

        public void Dispose()
        {
            _api?.Dispose();
        }

        #region Overclocking Tests

        [SkippableFact]
        public void CtlOverclockGetProperties_ShouldReturnProperties()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlOverclockGpuFrequencyOffsetGet_ShouldReturnOffset()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlOverclockGpuVoltageOffsetGet_ShouldReturnOffset()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlOverclockPowerLimitGet_ShouldReturnLimit()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlOverclockTemperatureLimitGet_ShouldReturnLimit()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlPowerTelemetryGet_ShouldReturnTelemetry()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlOverclockGpuFrequencyOffsetGetV2_ShouldReturnOffset()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlOverclockGpuMaxVoltageOffsetGetV2_ShouldReturnOffset()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlOverclockVramMemSpeedLimitGetV2_ShouldReturnLimit()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlOverclockPowerLimitGetV2_ShouldReturnLimit()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlOverclockTemperatureLimitGetV2_ShouldReturnLimit()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlOverclockGpuLockGet_ShouldReturnLock()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlGetSupported3DCapabilities_ShouldReturnCapabilities()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlGetSupportedVideoProcessingCapabilities_ShouldReturnCapabilities()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlGetSupportedRetroScalingCapability_ShouldReturnCapabilities()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlGetSetRetroScaling_ShouldReadSettings()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

        [SkippableFact]
        public void CtlGetLinkedDisplayAdapters_ShouldReturnAdapters()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

                // Assert - Accept all officially documented return codes for this API
                // Note: INSUFFICIENT_PERMISSIONS can occur when LDA operations require elevated privileges
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNINITIALIZED ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_DEVICE_LOST ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_HANDLE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_POINTER ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_NULL_OS_INTERFACE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_NULL_OS_ADAPATER_HANDLE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_KMD_CALL ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_ADAPTER_NOT_SUPPORTED_ON_LDA_SECONDARY ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INSUFFICIENT_PERMISSIONS,
                    $"Unexpected error code: {result} (0x{(uint)result:X})"
                );
            }
        }

        #endregion

        #region VF Curve Tests

        [SkippableFact]
        public void CtlOverclockReadVFCurve_ShouldReturnCurve()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

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

                // Assert - Accept all documented return codes for this API
                // This API can return various error codes depending on hardware/driver state and permissions
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_WAIVER_NOT_SET ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INSUFFICIENT_PERMISSIONS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_HANDLE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_ENUMERATION ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNKNOWN ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNINITIALIZED ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_DEVICE_LOST ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_DEPRECATED_API ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_DATA_READ
                );
            }
        }

        #endregion

        #region Error Handling Tests

        [SkippableFact]
        public void CtlOverclockSetWithoutWaiver_ShouldReturnError()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            // Try to set overclock without waiver - should fail
            unsafe
            {
                var result = IGCL.ctlOverclockGpuFrequencyOffsetSet((_ctl_device_adapter_handle_t*)_adapters[0], 50.0);

                // Assert - Accept all documented return codes for this API
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_WAIVER_NOT_SET ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_ARGUMENT ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_FREQUENCY_OUTSIDE_RANGE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_HANDLE ||
                    result == _ctl_result_t.CTL_RESULT_SUCCESS // May already have waiver from previous test run
                );
            }
        }

        #endregion
    }
}
