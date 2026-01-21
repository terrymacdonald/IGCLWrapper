using Xunit;
using IGCLWrapper;
using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using System.Text;

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

        [Trait("Category", "Active")]
        [SkippableFact]
        public void CtlGetSetCombinedDisplay_EnableDisable_ShouldCreateAndRevert_WhenActiveTestsEnabled()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0 || _displays == null || _displays.Length == 0 || _noDisplaysAvailable, _skipReason);

            unsafe
            {
                const byte desiredOutputs = 2;
                const int targetWidth = 2560;
                const int targetHeight = 1440;
                const float targetRefreshRate = 60.0f;

                IntPtr FindCombinedDisplayOutput(IntPtr[] displayHandles)
                {
                    foreach (var display in displayHandles)
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
                            return display;
                        }
                    }

                    return IntPtr.Zero;
                }

                if (desiredOutputs < 2 || desiredOutputs > 4)
                {
                    throw new SkipException($"Combined display requires between 2 and 4 outputs; requested {desiredOutputs}.");
                }

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

                Assert.True(probeResult == ctl_result_t.CTL_RESULT_SUCCESS, $"Combined display probe failed: {probeResult} (NumOutputs={probe.NumOutputs})");
                if (probe.NumOutputs < desiredOutputs)
                {
                    throw new SkipException($"Combined display requires {desiredOutputs} outputs but driver reports {probe.NumOutputs}.");
                }

                var existingCombinedOutput = FindCombinedDisplayOutput(_displays);
                if (existingCombinedOutput != IntPtr.Zero)
                {
                    throw new SkipException("Combined display already active; refusing to modify current layout.");
                }

                uint combinedAllowedEncoderTypes =
                    (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TYPEC_CAPABLE |
                    (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TBT_CAPABLE |
                    (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_DITHERING_SUPPORTED |
                    (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_INTERNAL_DISPLAY;

                var activeOutputs = new List<(IntPtr Handle, int Width, int Height, float RefreshRate, uint DisplayConfigFlags, uint EncoderFlags, uint EncoderId)>();
                var matchingOutputs = new List<(IntPtr Handle, int Width, int Height, float RefreshRate, uint DisplayConfigFlags, uint EncoderFlags, uint EncoderId)>();

                foreach (var display in _displays)
                {
                    if (display == IntPtr.Zero)
                        continue;

                    var props = new ctl_display_properties_t
                    {
                        Size = (uint)sizeof(ctl_display_properties_t),
                        Version = 0
                    };

                    var propsResult = IGCL.ctlGetDisplayProperties((_ctl_display_output_handle_t*)display, &props);
                    if (propsResult != ctl_result_t.CTL_RESULT_SUCCESS)
                        continue;

                    var encoderProps = new ctl_adapter_display_encoder_properties_t
                    {
                        Size = (uint)sizeof(ctl_adapter_display_encoder_properties_t),
                        Version = 0
                    };

                    var encoderResult = IGCL.ctlGetAdaperDisplayEncoderProperties((_ctl_display_output_handle_t*)display, &encoderProps);
                    if (encoderResult != ctl_result_t.CTL_RESULT_SUCCESS)
                        continue;

                    var isDisplayActive = (props.DisplayConfigFlags & (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ACTIVE) != 0;
                    var isDisplayAttached = (props.DisplayConfigFlags & (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ATTACHED) != 0;
                    var encoderFlags = encoderProps.EncoderConfigFlags;
                    var isCombinedAvailable = encoderFlags == 0 || (encoderFlags & combinedAllowedEncoderTypes) != 0;

                    if (isDisplayActive && isDisplayAttached && isCombinedAvailable)
                    {
                        var width = (int)props.Display_Timing_Info.HActive;
                        var height = (int)props.Display_Timing_Info.VActive;
                        if (width <= 0 || height <= 0)
                        {
                            continue;
                        }

                        var refreshRate = props.Display_Timing_Info.RefreshRate;
                        var encoderId = encoderProps.Os_display_encoder_handle.WindowsDisplayEncoderID;
                        var entry = (display, width, height, refreshRate, props.DisplayConfigFlags, encoderFlags, encoderId);
                        activeOutputs.Add(entry);
                        if (width == targetWidth && height == targetHeight)
                        {
                            matchingOutputs.Add(entry);
                        }
                    }
                }

                if (activeOutputs.Count < desiredOutputs)
                {
                    throw new SkipException($"Combined display requires {desiredOutputs} active outputs but only {activeOutputs.Count} are available.");
                }

                if (matchingOutputs.Count > 4)
                {
                    matchingOutputs.RemoveRange(4, matchingOutputs.Count - 4);
                }

                if (matchingOutputs.Count < desiredOutputs)
                {
                    throw new SkipException($"Combined display requires {desiredOutputs} active outputs at {targetWidth}x{targetHeight} but only {matchingOutputs.Count} are available.");
                }

                var displayOrder = new[] { 1, 0 };
                if (displayOrder.Length != desiredOutputs)
                {
                    throw new SkipException($"Display order length {displayOrder.Length} does not match NumOutputs {desiredOutputs}.");
                }

                var firstOutput = matchingOutputs[displayOrder[0]];
                var secondOutput = matchingOutputs[displayOrder[1]];
                const int combinedWidth = targetWidth * 2;
                const int combinedHeight = targetHeight;

                static string DescribeOutput(string label, (IntPtr Handle, int Width, int Height, float RefreshRate, uint DisplayConfigFlags, uint EncoderFlags, uint EncoderId) output)
                {
                    return $"{label}: handle=0x{output.Handle.ToString("X")} size={output.Width}x{output.Height} refresh={output.RefreshRate:F3} displayFlags=0x{output.DisplayConfigFlags:X} encoderFlags=0x{output.EncoderFlags:X} encoderId={output.EncoderId}";
                }

                var diagBuilder = new StringBuilder();
                diagBuilder.AppendLine($"Combined layout (IGCL order): combined={combinedWidth}x{combinedHeight} target={targetWidth}x{targetHeight}@{targetRefreshRate:F1}");
                diagBuilder.AppendLine($"  {DescribeOutput("first", firstOutput)}");
                diagBuilder.AppendLine($"  {DescribeOutput("second", secondOutput)}");
                diagBuilder.AppendLine($"DisplayOrder: {string.Join(",", displayOrder)}");
                diagBuilder.AppendLine("Placement: left=first, right=second");
                var combinedDiag = diagBuilder.ToString().TrimEnd();
                Console.WriteLine(combinedDiag);

                var childInfos = new ctl_combined_display_child_info_t[desiredOutputs];
                childInfos[0] = new ctl_combined_display_child_info_t
                {
                    hDisplayOutput = (_ctl_display_output_handle_t*)firstOutput.Handle,
                    FbSrc = new ctl_rect_t { Left = 0, Top = 0, Right = targetWidth, Bottom = targetHeight },
                    FbPos = new ctl_rect_t { Left = 0, Top = 0, Right = targetWidth, Bottom = targetHeight },
                    DisplayOrientation = ctl_display_orientation_t.CTL_DISPLAY_ORIENTATION_0,
                    TargetMode = new ctl_child_display_target_mode_t { Width = targetWidth, Height = targetHeight, RefreshRate = targetRefreshRate }
                };

                childInfos[1] = new ctl_combined_display_child_info_t
                {
                    hDisplayOutput = (_ctl_display_output_handle_t*)secondOutput.Handle,
                    FbSrc = new ctl_rect_t { Left = targetWidth, Top = 0, Right = targetWidth * 2, Bottom = targetHeight },
                    FbPos = new ctl_rect_t { Left = 0, Top = 0, Right = targetWidth, Bottom = targetHeight },
                    DisplayOrientation = ctl_display_orientation_t.CTL_DISPLAY_ORIENTATION_0,
                    TargetMode = new ctl_child_display_target_mode_t { Width = targetWidth, Height = targetHeight, RefreshRate = targetRefreshRate }
                };

                for (var i = 0; i < childInfos.Length; i++)
                {
                    var orientation = childInfos[i].DisplayOrientation;
                    if (orientation != ctl_display_orientation_t.CTL_DISPLAY_ORIENTATION_0 &&
                        orientation != ctl_display_orientation_t.CTL_DISPLAY_ORIENTATION_180)
                    {
                        throw new SkipException("Only 0/180 degree rotation is supported.");
                    }
                }

                var combinedEnabled = false;
                var combinedOutput = IntPtr.Zero;

                var enableSkipped = false;
                try
                {
                    fixed (ctl_combined_display_child_info_t* pChildInfos = childInfos)
                    {
                        var supportArgs = new ctl_combined_display_args_t
                        {
                            Size = (uint)sizeof(ctl_combined_display_args_t),
                            Version = 1,
                            OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_IS_SUPPORTED_CONFIG,
                            NumOutputs = desiredOutputs,
                            CombinedDesktopWidth = (uint)combinedWidth,
                            CombinedDesktopHeight = (uint)combinedHeight,
                            pChildInfo = pChildInfos,
                            hCombinedDisplayOutput = null
                        };

                        var supportResult = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)_adapters[0], &supportArgs);
                        if (supportResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                            supportResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                        {
                            throw new SkipException($"Combined display unsupported: {supportResult}");
                        }

                        Assert.True(supportResult == ctl_result_t.CTL_RESULT_SUCCESS, $"Combined display support probe failed: {supportResult} (0x{(uint)supportResult:X}). {combinedDiag}");

                        if (supportArgs.IsSupported == 0)
                        {
                            enableSkipped = true;
                        }

                        if (!enableSkipped)
                        {
                            var enableArgs = supportArgs;
                            enableArgs.OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_ENABLE;

                            var enableResult = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)_adapters[0], &enableArgs);
                            if (enableResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                                enableResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                            {
                                throw new SkipException($"Combined display unsupported: {enableResult}");
                            }

                            Assert.True(enableResult == ctl_result_t.CTL_RESULT_SUCCESS, $"Combined display enable failed: {enableResult} (0x{(uint)enableResult:X}). {combinedDiag}");

                            combinedEnabled = true;
                            if (enableArgs.hCombinedDisplayOutput != null)
                            {
                                combinedOutput = (IntPtr)enableArgs.hCombinedDisplayOutput;
                            }
                        }
                    }

                    if (!enableSkipped)
                    {
                        var updatedDisplays = _api.EnumerateDisplays(_adapters[0]);
                        if (combinedOutput == IntPtr.Zero && updatedDisplays.Length > 0)
                        {
                            combinedOutput = FindCombinedDisplayOutput(updatedDisplays);
                        }

                        Assert.True(combinedOutput != IntPtr.Zero, "Combined display output handle not found after enable.");

                        var queryChildCount = probe.NumOutputs > 0 ? probe.NumOutputs : desiredOutputs;
                        var queryChildren = new ctl_combined_display_child_info_t[queryChildCount];
                        fixed (ctl_combined_display_child_info_t* pQueryChildren = queryChildren)
                        {
                            var queryArgs = new ctl_combined_display_args_t
                            {
                                Size = (uint)sizeof(ctl_combined_display_args_t),
                                Version = 1,
                                OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_QUERY_CONFIG,
                                NumOutputs = 0,
                                CombinedDesktopWidth = 0,
                                CombinedDesktopHeight = 0,
                                pChildInfo = pQueryChildren,
                                hCombinedDisplayOutput = (_ctl_display_output_handle_t*)combinedOutput
                            };

                            var queryResult = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)_adapters[0], &queryArgs);
                            if (queryResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                                queryResult == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                            {
                                throw new SkipException($"Combined display query unsupported: {queryResult}");
                            }

                            Assert.True(queryResult == ctl_result_t.CTL_RESULT_SUCCESS, $"Combined display query failed: {queryResult} (0x{(uint)queryResult:X})");
                            Assert.Equal(desiredOutputs, queryArgs.NumOutputs);
                            Assert.Equal((uint)combinedWidth, queryArgs.CombinedDesktopWidth);
                            Assert.Equal((uint)combinedHeight, queryArgs.CombinedDesktopHeight);
                        }

                        Console.WriteLine("Combined display enabled; waiting 10 seconds before disable.");
                        System.Threading.Thread.Sleep(10000);
                    }
                }
                finally
                {
                    if (combinedEnabled)
                    {
                        if (combinedOutput == IntPtr.Zero)
                        {
                            var currentDisplays = _api.EnumerateDisplays(_adapters[0]);
                            if (currentDisplays.Length > 0)
                            {
                                combinedOutput = FindCombinedDisplayOutput(currentDisplays);
                            }
                        }

                        Assert.True(combinedOutput != IntPtr.Zero, "Combined display output handle not found for disable.");

                        Console.WriteLine("Disabling combined display.");
                        var disableChildren = new ctl_combined_display_child_info_t[1];
                        fixed (ctl_combined_display_child_info_t* pDisableChildren = disableChildren)
                        {
                            var disableArgs = new ctl_combined_display_args_t
                            {
                                Size = (uint)sizeof(ctl_combined_display_args_t),
                                Version = 1,
                                OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_DISABLE,
                                NumOutputs = 1,
                                CombinedDesktopWidth = 0,
                                CombinedDesktopHeight = 0,
                                pChildInfo = pDisableChildren,
                                hCombinedDisplayOutput = (_ctl_display_output_handle_t*)combinedOutput
                            };

                            var disableResult = IGCL.ctlGetSetCombinedDisplay((_ctl_device_adapter_handle_t*)_adapters[0], &disableArgs);
                            Assert.True(disableResult == ctl_result_t.CTL_RESULT_SUCCESS, $"Combined display disable failed: {disableResult} (0x{(uint)disableResult:X})");
                        }
                    }
                }

                if (enableSkipped)
                {
                    throw new SkipException("Combined display configuration is not supported by the driver.");
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
