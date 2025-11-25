using Xunit;
using IGCLWrapper;
using System;

namespace IGCLWrapper.Tests
{
    /// <summary>
    /// Tests for Display Services APIs including display enumeration, properties, scaling, sharpness,
    /// I2C/AUX access, brightness, pixel transformation, EDID management, and Intel Arc Sync
    /// </summary>
    public class DisplayServicesTests : IDisposable
    {
        private IGCLApi? _api;
        private IntPtr[]? _adapters;
        private IntPtr[]? _displays;

        public DisplayServicesTests()
        {
            try
            {
                _api = IGCLApi.Initialize();
                _adapters = _api?.EnumerateAdapters();
                if (_adapters != null && _adapters.Length > 0)
                {
                    unsafe
                    {
                        uint count = 0;
                        IGCL.ctlEnumerateDisplayOutputs(_adapters[0], &count, null);
                        if (count > 0)
                        {
                            _displays = new IntPtr[count];
                            fixed (IntPtr* pDisplays = _displays)
                            {
                                IGCL.ctlEnumerateDisplayOutputs(_adapters[0], &count, pDisplays);
                            }
                        }
                    }
                }
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

        [Fact]
        public void CtlEnumerateDisplayOutputs_ShouldReturnDisplayCount()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return; // Skip test
            }

            // Assert
            Assert.NotNull(_displays);
            // Note: May be 0 if no displays connected
        }

        [Fact]
        public void CtlGetDisplayProperties_ShouldReturnValidProperties()
        {
            // Arrange
            if (_api == null || _displays == null || _displays.Length == 0)
            {
                return; // Skip test
            }

            // Act
            unsafe
            {
                var props = new _ctl_display_properties_t
                {
                    Size = (uint)sizeof(_ctl_display_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlGetDisplayProperties(_displays[0], &props);

                // Assert
                if (result == _ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    Assert.True(props.Type != _ctl_display_output_types_t.CTL_DISPLAY_OUTPUT_TYPES_INVALID);
                }
            }
        }

        [Fact]
        public void CtlGetAdaperDisplayEncoderProperties_ShouldReturnValidProperties()
        {
            // Arrange
            if (_api == null || _displays == null || _displays.Length == 0)
            {
                return; // Skip test
            }

            // Act
            unsafe
            {
                var props = new _ctl_adapter_display_encoder_properties_t
                {
                    Size = (uint)sizeof(_ctl_adapter_display_encoder_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlGetAdaperDisplayEncoderProperties(_displays[0], &props);

                // Assert
                if (result == _ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    Assert.True(props.Type != _ctl_display_output_types_t.CTL_DISPLAY_OUTPUT_TYPES_INVALID);
                }
            }
        }

        [Fact]
        public void CtlGetSharpnessCaps_ShouldReturnCapabilities()
        {
            // Arrange
            if (_api == null || _displays == null || _displays.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var caps = new _ctl_sharpness_caps_t
                {
                    Size = (uint)sizeof(_ctl_sharpness_caps_t),
                    Version = 0
                };

                var result = IGCL.ctlGetSharpnessCaps(_displays[0], &caps);

                // Assert - Success or unsupported
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [Fact]
        public void CtlGetCurrentSharpness_ShouldReturnSettings()
        {
            // Arrange
            if (_api == null || _displays == null || _displays.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var settings = new _ctl_sharpness_settings_t
                {
                    Size = (uint)sizeof(_ctl_sharpness_settings_t),
                    Version = 0
                };

                var result = IGCL.ctlGetCurrentSharpness(_displays[0], &settings);

                // Assert - Success or unsupported
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [Fact]
        public void CtlGetSupportedScalingCapability_ShouldReturnCapabilities()
        {
            // Arrange
            if (_api == null || _displays == null || _displays.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var caps = new _ctl_scaling_caps_t
                {
                    Size = (uint)sizeof(_ctl_scaling_caps_t),
                    Version = 0
                };

                var result = IGCL.ctlGetSupportedScalingCapability(_displays[0], &caps);

                // Assert
                if (result == _ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    Assert.NotEqual(0u, caps.SupportedScaling);
                }
            }
        }

        [Fact]
        public void CtlGetCurrentScaling_ShouldReturnSettings()
        {
            // Arrange
            if (_api == null || _displays == null || _displays.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var settings = new _ctl_scaling_settings_t
                {
                    Size = (uint)sizeof(_ctl_scaling_settings_t),
                    Version = 0
                };

                var result = IGCL.ctlGetCurrentScaling(_displays[0], &settings);

                // Assert - Success or unsupported
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [Fact]
        public void CtlGetPowerOptimizationCaps_ShouldReturnCapabilities()
        {
            // Arrange
            if (_api == null || _displays == null || _displays.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var caps = new _ctl_power_optimization_caps_t
                {
                    Size = (uint)sizeof(_ctl_power_optimization_caps_t),
                    Version = 0
                };

                var result = IGCL.ctlGetPowerOptimizationCaps(_displays[0], &caps);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [Fact]
        public void CtlGetIntelArcSyncInfoForMonitor_ShouldReturnInfo()
        {
            // Arrange
            if (_api == null || _displays == null || _displays.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var info = new _ctl_intel_arc_sync_monitor_params_t
                {
                    Size = (uint)sizeof(_ctl_intel_arc_sync_monitor_params_t),
                    Version = 0
                };

                var result = IGCL.ctlGetIntelArcSyncInfoForMonitor(_displays[0], &info);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [Fact]
        public void CtlGetIntelArcSyncProfile_ShouldReturnProfile()
        {
            // Arrange
            if (_api == null || _displays == null || _displays.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var profile = new _ctl_intel_arc_sync_profile_params_t
                {
                    Size = (uint)sizeof(_ctl_intel_arc_sync_profile_params_t),
                    Version = 0
                };

                var result = IGCL.ctlGetIntelArcSyncProfile(_displays[0], &profile);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [Fact]
        public void CtlEnumerateI2CPinPairs_ShouldReturnCount()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumerateI2CPinPairs(_adapters[0], &count, null);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
                // Count may be 0 if no I2C pin pairs available
            }
        }

        [Fact]
        public void CtlPanelDescriptorAccess_WithInvalidArgs_ShouldReturnError()
        {
            // Arrange
            if (_api == null || _displays == null || _displays.Length == 0)
            {
                return;
            }

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

                var result = IGCL.ctlPanelDescriptorAccess(_displays[0], &args);

                // Should either succeed (returning size) or indicate unsupported
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [Fact]
        public void CtlEnumerateMuxDevices_ShouldReturnCount()
        {
            // Arrange & Act
            if (_api == null)
            {
                return;
            }

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumerateMuxDevices(_api.Handle, &count, null);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
                // Count may be 0 if no MUX devices present
            }
        }

        [Fact]
        public void CtlGetSetDisplaySettings_ShouldReadSettings()
        {
            // Arrange
            if (_api == null || _displays == null || _displays.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var settings = new _ctl_display_settings_t
                {
                    Size = (uint)sizeof(_ctl_display_settings_t),
                    Version = 0,
                    Set = false  // GET operation
                };

                var result = IGCL.ctlGetSetDisplaySettings(_displays[0], &settings);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }
    }
}
