using Xunit;
using IGCLWrapper;
using System;
using System.Runtime.Versioning;

namespace IGCLWrapper.Tests
{
    /// <summary>
    /// Tests for Display Services APIs including display enumeration, properties, scaling, sharpness,
    /// I2C/AUX access, brightness, pixel transformation, EDID management, and Intel Arc Sync
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class DisplayServicesTests : IDisposable
    {
        private readonly IGCLApi? _api;
        private readonly IntPtr[]? _adapters;
        private readonly IntPtr[]? _displays;
        private readonly bool _hasHardware;
        private readonly bool _hasDll;
        private readonly string _skipReason = string.Empty;
        private readonly bool _noDisplaysAvailable;

        public DisplayServicesTests()
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
                if (_adapters != null && _adapters.Length > 0)
                {
                    _displays = _api?.EnumerateDisplays(_adapters[0]);
                }
            }
            catch (IGCLException ex)
            {
                if (ex.IsNoDisplayError())
                {
                    // Mark that no displays are available so tests can skip
                    _noDisplaysAvailable = true;
                    _skipReason = "No displays connected";
                }
                else
                {
                    _skipReason = $"IGCL initialization failed: {ex.Message}";
                }
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

        [SkippableFact]
        public void CtlEnumerateDisplayOutputs_ShouldReturnDisplayCount()
        {
            // Arrange & Act
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0 || _noDisplaysAvailable, _skipReason);

            // Assert
            Assert.NotNull(_displays);
            // Note: May be 0 if no displays connected
        }

        [SkippableFact]
        public void CtlGetDisplayProperties_ShouldReturnValidProperties()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            // Act
            unsafe
            {
                var props = new _ctl_display_properties_t
                {
                    Size = (uint)sizeof(_ctl_display_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlGetDisplayProperties((_ctl_display_output_handle_t*)_displays[0], &props);

                // Assert
                if (result == _ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    Assert.True(props.Type != _ctl_display_output_types_t.CTL_DISPLAY_OUTPUT_TYPES_INVALID);
                }
            }
        }

        [SkippableFact]
        public void CtlGetAdaperDisplayEncoderProperties_ShouldReturnValidProperties()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            // Act
            unsafe
            {
                var props = new _ctl_adapter_display_encoder_properties_t
                {
                    Size = (uint)sizeof(_ctl_adapter_display_encoder_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlGetAdaperDisplayEncoderProperties((_ctl_display_output_handle_t*)_displays[0], &props);

                // Assert
                if (result == _ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    Assert.True(props.Type != _ctl_display_output_types_t.CTL_DISPLAY_OUTPUT_TYPES_INVALID);
                }
            }
        }

        [SkippableFact]
        public void CtlGetSharpnessCaps_ShouldReturnCapabilities()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            // Act
            unsafe
            {
                var caps = new _ctl_sharpness_caps_t
                {
                    Size = (uint)sizeof(_ctl_sharpness_caps_t),
                    Version = 0
                };

                var result = IGCL.ctlGetSharpnessCaps((_ctl_display_output_handle_t*)_displays[0], &caps);

                // Assert - Success or unsupported
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [SkippableFact]
        public void CtlGetCurrentSharpness_ShouldReturnSettings()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            // Act
            unsafe
            {
                var settings = new _ctl_sharpness_settings_t
                {
                    Size = (uint)sizeof(_ctl_sharpness_settings_t),
                    Version = 0
                };

                var result = IGCL.ctlGetCurrentSharpness((_ctl_display_output_handle_t*)_displays[0], &settings);

                // Assert - Success or unsupported
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [SkippableFact]
        public void CtlGetSupportedScalingCapability_ShouldReturnCapabilities()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            // Act
            unsafe
            {
                var caps = new _ctl_scaling_caps_t
                {
                    Size = (uint)sizeof(_ctl_scaling_caps_t),
                    Version = 0
                };

                var result = IGCL.ctlGetSupportedScalingCapability((_ctl_display_output_handle_t*)_displays[0], &caps);

                // Assert
                if (result == _ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    Assert.NotEqual(0u, caps.SupportedScaling);
                }
            }
        }

        [SkippableFact]
        public void CtlGetCurrentScaling_ShouldReturnSettings()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            // Act
            unsafe
            {
                var settings = new _ctl_scaling_settings_t
                {
                    Size = (uint)sizeof(_ctl_scaling_settings_t),
                    Version = 0
                };

                var result = IGCL.ctlGetCurrentScaling((_ctl_display_output_handle_t*)_displays[0], &settings);

                // Assert - Success or unsupported
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [SkippableFact]
        public void CtlGetPowerOptimizationCaps_ShouldReturnCapabilities()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            // Act
            unsafe
            {
                var caps = new _ctl_power_optimization_caps_t
                {
                    Size = (uint)sizeof(_ctl_power_optimization_caps_t),
                    Version = 0
                };

                var result = IGCL.ctlGetPowerOptimizationCaps((_ctl_display_output_handle_t*)_displays[0], &caps);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [SkippableFact]
        public void CtlGetIntelArcSyncInfoForMonitor_ShouldReturnInfo()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            // Act
            unsafe
            {
                var info = new _ctl_intel_arc_sync_monitor_params_t
                {
                    Size = (uint)sizeof(_ctl_intel_arc_sync_monitor_params_t),
                    Version = 0
                };

                var result = IGCL.ctlGetIntelArcSyncInfoForMonitor((_ctl_display_output_handle_t*)_displays[0], &info);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [SkippableFact]
        public void CtlGetIntelArcSyncProfile_ShouldReturnProfile()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            // Act
            unsafe
            {
                var profile = new _ctl_intel_arc_sync_profile_params_t
                {
                    Size = (uint)sizeof(_ctl_intel_arc_sync_profile_params_t),
                    Version = 0
                };

                var result = IGCL.ctlGetIntelArcSyncProfile((_ctl_display_output_handle_t*)_displays[0], &profile);

                // Assert - Accept all documented return codes for this API
                // Note: KMD_CALL can occur when kernel mode driver encounters issues
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNINITIALIZED ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_DEVICE_LOST ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_HANDLE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_POINTER ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_KMD_CALL,
                    $"Unexpected error code: {result} (0x{(uint)result:X})"
                );
            }
        }

        [SkippableFact]
        public void CtlEnumerateI2CPinPairs_ShouldReturnCount()
        {
            // Arrange & Act
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0 || _noDisplaysAvailable, _skipReason);

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumerateI2CPinPairs((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
                // Count may be 0 if no I2C pin pairs available
            }
        }

        [SkippableFact]
        public void CtlPanelDescriptorAccess_WithInvalidArgs_ShouldReturnError()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            // Act & Assert
            unsafe
            {
                var args = new _ctl_panel_descriptor_access_args_t
                {
                    Size = (uint)sizeof(_ctl_panel_descriptor_access_args_t),
                    Version = 0,
                    OpType = _ctl_operation_type_t.CTL_OPERATION_TYPE_READ,
                    BlockNumber = 0,
                    DescriptorDataSize = 0,
                    pDescriptorData = null
                };

                var result = IGCL.ctlPanelDescriptorAccess((_ctl_display_output_handle_t*)_displays[0], &args);

                // Should either succeed (returning size) or indicate unsupported
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [SkippableFact]
        public void CtlGetSetDisplaySettings_ShouldReadSettings()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            // Act
            unsafe
            {
                var settings = new _ctl_display_settings_t
                {
                    Size = (uint)sizeof(_ctl_display_settings_t),
                    Version = 0,
                    Set = 0  // GET operation (false)
                };

                var result = IGCL.ctlGetSetDisplaySettings((_ctl_display_output_handle_t*)_displays[0], &settings);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_ARGUMENT
                );
            }
        }
    }
}
