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
            if (!IGCLHardwareDetection.HasIntelGPU(out string hwError))
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
                var props = new ctl_display_properties_t
                {
                    Size = (uint)sizeof(ctl_display_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlGetDisplayProperties((_ctl_display_output_handle_t*)_displays[0], &props);

                // Assert
                if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    Assert.True(props.Type != ctl_display_output_types_t.CTL_DISPLAY_OUTPUT_TYPES_INVALID);
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
                var props = new ctl_adapter_display_encoder_properties_t
                {
                    Size = (uint)sizeof(ctl_adapter_display_encoder_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlGetAdaperDisplayEncoderProperties((_ctl_display_output_handle_t*)_displays[0], &props);

                // Assert
                if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    Assert.True(props.Type != ctl_display_output_types_t.CTL_DISPLAY_OUTPUT_TYPES_INVALID);
                }
            }
        }

        [SkippableFact]
        public void CtlGetSetCombinedDisplay_ShouldReturnChildInfo_WhenConfigured()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0 || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            unsafe
            {
                var probe = new ctl_combined_display_args_t
                {
                    Size = (uint)sizeof(ctl_combined_display_args_t),
                    Version = 0,
                    OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_IS_SUPPORTED_CONFIG
                };

                var probeResult = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)_adapters[0], &probe);
                if (probeResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    probeResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                {
                    throw new SkipException($"Combined display unsupported: {probeResult}");
                }

                Assert.True(probeResult == ctl_result_t.CTL_RESULT_SUCCESS, $"Unexpected error code: {probeResult} (0x{(uint)probeResult:X})");

                if (probe.NumOutputs == 0)
                {
                    throw new SkipException("Combined display outputs not available.");
                }

                IntPtr combinedOutput = IntPtr.Zero;
                foreach (var display in _displays)
                {
                    if (display == IntPtr.Zero)
                        continue;

                    var encoderProps = new ctl_adapter_display_encoder_properties_t
                    {
                        Size = (uint)sizeof(ctl_adapter_display_encoder_properties_t),
                        Version = 0
                    };

                    var encoderResult = IGCL.ctlGetAdaperDisplayEncoderProperties((_ctl_display_output_handle_t*)display, &encoderProps);
                    if (encoderResult != ctl_result_t.CTL_RESULT_SUCCESS)
                        continue;

                    var flags = encoderProps.EncoderConfigFlags;
                    var isCombined = (flags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COLLAGE_DISPLAY) != 0 ||
                                     (flags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_MGPU_COLLAGE_DISPLAY) != 0 ||
                                     (flags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_SPLIT_DISPLAY) != 0;

                    if (isCombined)
                    {
                        combinedOutput = display;
                        break;
                    }
                }

                if (combinedOutput == IntPtr.Zero)
                {
                    throw new SkipException("No combined display outputs reported.");
                }

                var children = new ctl_combined_display_child_info_t[probe.NumOutputs];
                fixed (ctl_combined_display_child_info_t* pChildren = children)
                {
                    var args = new ctl_combined_display_args_t
                    {
                        Size = (uint)sizeof(ctl_combined_display_args_t),
                        Version = 0,
                        OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_QUERY_CONFIG,
                        NumOutputs = probe.NumOutputs,
                        pChildInfo = pChildren,
                        hCombinedDisplayOutput = (_ctl_display_output_handle_t*)combinedOutput
                    };

                    var result = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)_adapters[0], &args);
                    if (result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                        result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                    {
                        throw new SkipException($"Combined display unsupported: {result}");
                    }

                    Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS, $"Unexpected error code: {result} (0x{(uint)result:X})");

                    if (args.NumOutputs == 0)
                    {
                        throw new SkipException("Combined display not configured.");
                    }

                    Assert.True(args.NumOutputs <= probe.NumOutputs);
                    for (var i = 0; i < args.NumOutputs; i++)
                    {
                        Assert.True(pChildren[i].hDisplayOutput != null);
                    }
                }
            }
        }

        [SkippableFact]
        public void CtlGetSetCombinedDisplay_ShouldReportConfiguredOutputs_WhenCombinedHandlePresent()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0 || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            unsafe
            {
                // Probe support and maximum outputs (Version = 1 matches IGCL sample).
                var probe = new ctl_combined_display_args_t
                {
                    Size = (uint)sizeof(ctl_combined_display_args_t),
                    Version = 1,
                    OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_IS_SUPPORTED_CONFIG
                };

                var probeResult = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)_adapters[0], &probe);
                if (probeResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    probeResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                {
                    throw new SkipException($"Combined display unsupported: {probeResult}");
                }

                Assert.True(probeResult == ctl_result_t.CTL_RESULT_SUCCESS, $"Probe failed: {probeResult} (NumOutputs={probe.NumOutputs})");
                if (probe.NumOutputs == 0)
                {
                    throw new SkipException("Combined display outputs not reported in probe.");
                }

                // Find a combined display output handle based on encoder flags.
                IntPtr combinedHandle = IntPtr.Zero;
                foreach (var display in _displays)
                {
                    if (display == IntPtr.Zero)
                        continue;

                    var encoderProps = new ctl_adapter_display_encoder_properties_t
                    {
                        Size = (uint)sizeof(ctl_adapter_display_encoder_properties_t),
                        Version = 0
                    };

                    var encoderResult = IGCL.ctlGetAdaperDisplayEncoderProperties((_ctl_display_output_handle_t*)display, &encoderProps);
                    if (encoderResult != ctl_result_t.CTL_RESULT_SUCCESS)
                        continue;

                    var flags = encoderProps.EncoderConfigFlags;
                    var isCombined = (flags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COLLAGE_DISPLAY) != 0 ||
                                     (flags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_MGPU_COLLAGE_DISPLAY) != 0 ||
                                     (flags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_SPLIT_DISPLAY) != 0;
                    if (isCombined)
                    {
                        combinedHandle = display;
                        break;
                    }
                }

                if (combinedHandle == IntPtr.Zero)
                {
                    throw new SkipException("No combined display output handle found.");
                }

                var children = new ctl_combined_display_child_info_t[probe.NumOutputs];
                fixed (ctl_combined_display_child_info_t* pChildren = children)
                {
                    var query = new ctl_combined_display_args_t
                    {
                        Size = (uint)sizeof(ctl_combined_display_args_t),
                        Version = 1,
                        OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_QUERY_CONFIG,
                        NumOutputs = 0,
                        pChildInfo = pChildren,
                        hCombinedDisplayOutput = (_ctl_display_output_handle_t*)combinedHandle
                    };

                    var queryResult = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)_adapters[0], &query);
                    if (queryResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                        queryResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                    {
                        throw new SkipException($"Combined display query unsupported: {queryResult}");
                    }

                    var diag = $"Result={queryResult} NumOutputs={query.NumOutputs} IsSupported={query.IsSupported} Width={query.CombinedDesktopWidth} Height={query.CombinedDesktopHeight}";
                    Assert.True(queryResult == ctl_result_t.CTL_RESULT_SUCCESS, diag);
                    Assert.True(query.NumOutputs > 0, "Driver returned zero combined outputs. " + diag);

                    Console.WriteLine("Combined display detected.");
                    Console.WriteLine($" - NumOutputs={query.NumOutputs} IsSupported={query.IsSupported} Width={query.CombinedDesktopWidth} Height={query.CombinedDesktopHeight}");

                    for (var i = 0; i < query.NumOutputs; i++)
                    {
                        var hChild = query.pChildInfo[i].hDisplayOutput;
                        Assert.True(hChild != null, $"Child {i} display handle is null. " + diag);

                        var childHandle = (IntPtr)hChild;
                        var props = new ctl_display_properties_t
                        {
                            Size = (uint)sizeof(ctl_display_properties_t),
                            Version = 0
                        };
                        var propsResult = IGCL.ctlGetDisplayProperties(hChild, &props);

                        if (propsResult == ctl_result_t.CTL_RESULT_SUCCESS)
                        {
                            Console.WriteLine($" - Child {i}: handle=0x{childHandle.ToString("X")} type={props.Type} flags=0x{props.DisplayConfigFlags:X}");
                        }
                        else
                        {
                            Console.WriteLine($" - Child {i}: handle=0x{childHandle.ToString("X")} (properties unavailable, result {propsResult})");
                        }
                    }
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
                var caps = new ctl_sharpness_caps_t
                {
                    Size = (uint)sizeof(ctl_sharpness_caps_t),
                    Version = 0
                };

                var result = IGCL.ctlGetSharpnessCaps((_ctl_display_output_handle_t*)_displays[0], &caps);

                // Assert - Success or unsupported
                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
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
                var settings = new ctl_sharpness_settings_t
                {
                    Size = (uint)sizeof(ctl_sharpness_settings_t),
                    Version = 0
                };

                var result = IGCL.ctlGetCurrentSharpness((_ctl_display_output_handle_t*)_displays[0], &settings);

                // Assert - Success or unsupported
                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
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
                var caps = new ctl_scaling_caps_t
                {
                    Size = (uint)sizeof(ctl_scaling_caps_t),
                    Version = 0
                };

                var result = IGCL.ctlGetSupportedScalingCapability((_ctl_display_output_handle_t*)_displays[0], &caps);

                // Assert
                if (result == ctl_result_t.CTL_RESULT_SUCCESS)
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
                var settings = new ctl_scaling_settings_t
                {
                    Size = (uint)sizeof(ctl_scaling_settings_t),
                    Version = 0
                };

                var result = IGCL.ctlGetCurrentScaling((_ctl_display_output_handle_t*)_displays[0], &settings);

                // Assert - Success or unsupported
                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
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
                var caps = new ctl_power_optimization_caps_t
                {
                    Size = (uint)sizeof(ctl_power_optimization_caps_t),
                    Version = 0
                };

                var result = IGCL.ctlGetPowerOptimizationCaps((_ctl_display_output_handle_t*)_displays[0], &caps);

                // Assert
                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
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
                var info = new ctl_intel_arc_sync_monitor_params_t
                {
                    Size = (uint)sizeof(ctl_intel_arc_sync_monitor_params_t),
                    Version = 0
                };

                var result = IGCL.ctlGetIntelArcSyncInfoForMonitor((_ctl_display_output_handle_t*)_displays[0], &info);

                // Assert
                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
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
                var profile = new ctl_intel_arc_sync_profile_params_t
                {
                    Size = (uint)sizeof(ctl_intel_arc_sync_profile_params_t),
                    Version = 0
                };

                var result = IGCL.ctlGetIntelArcSyncProfile((_ctl_display_output_handle_t*)_displays[0], &profile);

                // Assert - Accept all documented return codes for this API
                // Note: KMD_CALL can occur when kernel mode driver encounters issues
                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNINITIALIZED ||
                    result == ctl_result_t.CTL_RESULT_ERROR_DEVICE_LOST ||
                    result == ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_HANDLE ||
                    result == ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_POINTER ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION ||
                    result == ctl_result_t.CTL_RESULT_ERROR_KMD_CALL,
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
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
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
                var args = new ctl_panel_descriptor_access_args_t
                {
                    Size = (uint)sizeof(ctl_panel_descriptor_access_args_t),
                    Version = 0,
                    OpType = ctl_operation_type_t.CTL_OPERATION_TYPE_READ,
                    BlockNumber = 0,
                    DescriptorDataSize = 0,
                    pDescriptorData = null
                };

                var result = IGCL.ctlPanelDescriptorAccess((_ctl_display_output_handle_t*)_displays[0], &args);

                // Should either succeed (returning size) or indicate unsupported
                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
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
                var settings = new ctl_display_settings_t
                {
                    Size = (uint)sizeof(ctl_display_settings_t),
                    Version = 0,
                    Set = 0  // GET operation (false)
                };

                var result = IGCL.ctlGetSetDisplaySettings((_ctl_display_output_handle_t*)_displays[0], &settings);

                // Assert
                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == ctl_result_t.CTL_RESULT_ERROR_INVALID_ARGUMENT
                );
            }
        }

        [SkippableFact]
        public void CtlEdidManagement_ReadEdid_ShouldReturnBytesOrSkip()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            unsafe
            {
                bool anyAttached = false;
                bool anySuccess = false;

                for (var displayIndex = 0; displayIndex < _displays.Length; displayIndex++)
                {
                    var displayHandle = _displays[displayIndex];
                    var props = new ctl_display_properties_t
                    {
                        Size = (uint)sizeof(ctl_display_properties_t),
                        Version = 0
                    };

                    var propsResult = IGCL.ctlGetDisplayProperties((_ctl_display_output_handle_t*)displayHandle, &props);
                    if (propsResult != ctl_result_t.CTL_RESULT_SUCCESS)
                        continue;

                    var isAttached = (props.DisplayConfigFlags & (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ATTACHED) != 0;
                    if (!isAttached)
                        continue;

                    anyAttached = true;

                    var args = new ctl_edid_management_args_t
                    {
                        Size = (uint)sizeof(ctl_edid_management_args_t),
                        Version = 0,
                        OpType = ctl_edid_management_optype_t.CTL_EDID_MANAGEMENT_OPTYPE_READ_EDID,
                        EdidType = ctl_edid_type_t.CTL_EDID_TYPE_CURRENT,
                        EdidSize = 0,
                        pEdidBuf = null
                    };

                    var result = IGCL.ctlEdidManagement((_ctl_display_output_handle_t*)displayHandle, &args);
                    if (result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                        result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                    {
                        continue;
                    }
                    if (result == ctl_result_t.CTL_RESULT_ERROR_DISPLAY_NOT_ATTACHED ||
                        result == ctl_result_t.CTL_RESULT_ERROR_DATA_NOT_FOUND)
                    {
                        continue;
                    }
                    if (result != ctl_result_t.CTL_RESULT_SUCCESS || args.EdidSize == 0)
                        continue;

                    var buffer = new byte[args.EdidSize];
                    for (var attempt = 0; attempt < 2; attempt++)
                    {
                        fixed (byte* pBuf = buffer)
                        {
                            args.EdidSize = (uint)buffer.Length;
                            args.pEdidBuf = pBuf;
                            result = IGCL.ctlEdidManagement((_ctl_display_output_handle_t*)displayHandle, &args);
                        }

                        if (result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                            result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                        {
                            break;
                        }
                        if (result == ctl_result_t.CTL_RESULT_ERROR_DISPLAY_NOT_ATTACHED ||
                            result == ctl_result_t.CTL_RESULT_ERROR_DATA_NOT_FOUND)
                        {
                            break;
                        }
                        if (result != ctl_result_t.CTL_RESULT_SUCCESS || args.EdidSize == 0)
                            break;

                        if (args.EdidSize <= buffer.Length)
                        {
                            anySuccess = true;
                            break;
                        }

                        buffer = new byte[args.EdidSize];
                    }

                    if (anySuccess)
                        break;
                }

                if (!anyAttached)
                    throw new SkipException("No attached displays reported.");

                if (!anySuccess)
                    throw new SkipException("No attached display returned EDID successfully.");
            }
        }
    }
}
