using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    public class IGCLAdapterHelperTests
    {
        [SkippableFact]
        public void GetProperties_And_Displays()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var props = adapter.GetPropertiesNative();
                Assert.True(props.Size > 0);
                var displays = adapter.EnumerateDisplayOutputs();
                Assert.NotNull(displays);
            }
        }

        [SkippableFact]
        public void GetDevicePropertiesDto_ShouldBeSafeToConsume()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var props = adapter.GetDeviceProperties();
                Assert.True(props.Size > 0);
                Assert.NotNull(props.Name);
                Assert.NotNull(props.Reserved);
                Assert.Equal(108, props.Reserved!.Length);
                Assert.True(props.Equals(props));
                _ = props.GetHashCode();
            }
        }

        [SkippableFact]
        public void GetPropertiesDto_ShouldBeSafeToConsume()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var props = adapter.GetProperties();
                Assert.True(props.Size > 0);
                Assert.NotNull(props.Name);
                Assert.NotNull(props.Reserved);
                Assert.Equal(108, props.Reserved!.Length);
                Assert.True(props.Equals(props));
                _ = props.GetHashCode();
            }
        }

        [SkippableFact]
        public void WaitForPropertyChange_ReturnsOrSkips()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var args = new ctl_wait_property_change_args_t { Size = 0, Version = 0, PropertyType = (uint)ctl_property_type_flag_t.CTL_PROPERTY_TYPE_FLAG_DISPLAY, TimeOutMilliSec = 100 };
                try
                {
                    adapter.WaitForPropertyChange(args);
                }
                catch (EntryPointNotFoundException ex)
                {
                    throw new SkipException($"WaitForPropertyChange unsupported: {ex.Message}");
                }
                catch (IGCLException ex) when (ex.Result == ctl_result_t.CTL_RESULT_ERROR_WAIT_TIMEOUT)
                {
                    // Expected if no property changes occur within the timeout.
                }
                catch (IGCLException ex) when (ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                                              ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                {
                    throw new SkipException($"WaitForPropertyChange unsupported: {ex.Result}");
                }
            }
        }

        [SkippableFact]
        public void GetCombinedDisplay_ShouldReturnChildInfos_WhenConfigured()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                CombinedDisplayArgsDto combined;
                try
                {
                    combined = adapter.GetCombinedDisplay();
                }
                catch (IGCLException ex) when (ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                                               ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION ||
                                               ex.Result == ctl_result_t.CTL_RESULT_ERROR_INVALID_OPERATION_TYPE ||
                                               ex.Result == ctl_result_t.CTL_RESULT_ERROR_INVALID_ARGUMENT)
                {
                    throw new SkipException($"Combined display query unsupported: {ex.Result}");
                }

                if (combined.NumOutputs == 0 || combined.ChildInfos == null || combined.ChildInfos.Length == 0)
                {
                    throw new SkipException("Combined display not configured.");
                }

                var displayLookup = new System.Collections.Generic.Dictionary<string, IGCLDisplayHelper>(StringComparer.OrdinalIgnoreCase);
                foreach (var display in adapter.EnumerateDisplayOutputs())
                {
                    displayLookup[display.Name] = display;
                }

                Console.WriteLine("Combined display detected.");
                Console.WriteLine($" - NumOutputs={combined.NumOutputs} Width={combined.CombinedDesktopWidth} Height={combined.CombinedDesktopHeight}");

                Assert.True(combined.ChildInfos.Length >= combined.NumOutputs);
                Assert.True(combined.CombinedDisplayOutput != IntPtr.Zero);
                for (var i = 0; i < combined.NumOutputs; i++)
                {
                    var child = combined.ChildInfos[i];
                    Assert.True(child.DisplayOutput != IntPtr.Zero);

                    var displayName = $"Display-{child.DisplayOutput.ToInt64():X}";
                    if (displayLookup.TryGetValue(displayName, out var displayHelper))
                    {
                        try
                        {
                            var props = displayHelper.GetProperties();
                            Console.WriteLine($" - Child {i}: name={displayHelper.Name} handle=0x{child.DisplayOutput.ToInt64():X} type={props.Type} flags=0x{props.DisplayConfigFlags:X}");
                        }
                        catch (IGCLException ex)
                        {
                            Console.WriteLine($" - Child {i}: name={displayHelper.Name} handle=0x{child.DisplayOutput.ToInt64():X} (properties unavailable: {ex.Result})");
                        }
                    }
                    else
                    {
                        Console.WriteLine($" - Child {i}: handle=0x{child.DisplayOutput.ToInt64():X} (no matching display helper)");
                    }
                }
            }
        }

        [Trait("Category", "Active")]
        [SkippableFact]
        public void SetCombinedDisplay_EnableDisable_ShouldCreateAndRevert_WhenActiveTestsEnabled()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                const byte desiredOutputs = 2;
                const int targetWidth = 2560;
                const int targetHeight = 1440;
                const float targetRefreshRate = 60.0f;
                var displays = adapter.EnumerateDisplayOutputs();
                if (displays == null || displays.Count == 0)
                    throw new SkipException("No display outputs available for combined display.");

                CombinedDisplayArgsDto existing;
                try
                {
                    existing = adapter.GetCombinedDisplay();
                }
                catch (IGCLException ex) when (ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                                               ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION ||
                                               ex.Result == ctl_result_t.CTL_RESULT_ERROR_INVALID_OPERATION_TYPE ||
                                               ex.Result == ctl_result_t.CTL_RESULT_ERROR_INVALID_ARGUMENT)
                {
                    throw new SkipException($"Combined display query unsupported: {ex.Result}");
                }

                if (existing.NumOutputs > 0 && existing.CombinedDisplayOutput != IntPtr.Zero)
                    throw new SkipException("Combined display already active; refusing to modify current layout.");

                uint combinedAllowedEncoderTypes =
                    (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TYPEC_CAPABLE |
                    (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TBT_CAPABLE |
                    (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_DITHERING_SUPPORTED |
                    (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_INTERNAL_DISPLAY;

                var activeOutputs = new List<(IntPtr Handle, int Width, int Height, float RefreshRate, uint DisplayConfigFlags, uint EncoderFlags, uint EncoderId)>();
                var matchingOutputs = new List<(IntPtr Handle, int Width, int Height, float RefreshRate, uint DisplayConfigFlags, uint EncoderFlags, uint EncoderId)>();

                foreach (var display in displays)
                {
                    if (!TryParseDisplayHandle(display, out var handle))
                        continue;

                    ctl_display_properties_t props;
                    AdapterDisplayEncoderPropertiesDto encoderProps;
                    try
                    {
                        props = display.GetProperties();
                        encoderProps = display.GetAdapterDisplayEncoderProperties();
                    }
                    catch (IGCLException)
                    {
                        continue;
                    }

                    var isDisplayActive = ((uint)props.DisplayConfigFlags & (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ACTIVE) != 0;
                    var isDisplayAttached = ((uint)props.DisplayConfigFlags & (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ATTACHED) != 0;
                    var encoderFlags = encoderProps.EncoderConfigFlags;
                    var isCombinedAvailable = encoderFlags == 0 || (encoderFlags & combinedAllowedEncoderTypes) != 0;

                    if (!isDisplayActive || !isDisplayAttached || !isCombinedAvailable)
                        continue;

                    var width = (int)props.Display_Timing_Info.HActive;
                    var height = (int)props.Display_Timing_Info.VActive;
                    if (width <= 0 || height <= 0)
                        continue;

                    var refreshRate = props.Display_Timing_Info.RefreshRate;
                    var encoderId = encoderProps.OsDisplayEncoderHandle.WindowsDisplayEncoderID;
                    var entry = (handle, width, height, refreshRate, props.DisplayConfigFlags, encoderFlags, encoderId);
                    activeOutputs.Add(entry);
                    if (width == targetWidth && height == targetHeight)
                    {
                        matchingOutputs.Add(entry);
                    }
                }

                if (activeOutputs.Count < desiredOutputs)
                    throw new SkipException($"Combined display requires {desiredOutputs} active outputs but only {activeOutputs.Count} are available.");

                if (matchingOutputs.Count > 4)
                    matchingOutputs.RemoveRange(4, matchingOutputs.Count - 4);

                if (matchingOutputs.Count < desiredOutputs)
                {
                    throw new SkipException($"Combined display requires {desiredOutputs} active outputs at {targetWidth}x{targetHeight} but only {matchingOutputs.Count} are available.");
                }

                var displayOrder = new[] { 1, 0 };
                if (displayOrder.Length != desiredOutputs)
                    throw new SkipException($"Display order length {displayOrder.Length} does not match NumOutputs {desiredOutputs}.");

                var firstOutput = matchingOutputs[displayOrder[0]];
                var secondOutput = matchingOutputs[displayOrder[1]];
                const int combinedWidth = targetWidth * 2;
                const int combinedHeight = targetHeight;

                static string DescribeOutput(string label, (IntPtr Handle, int Width, int Height, float RefreshRate, uint DisplayConfigFlags, uint EncoderFlags, uint EncoderId) output)
                {
                    return $"{label}: handle=0x{output.Handle.ToString("X")} size={output.Width}x{output.Height} refresh={output.RefreshRate:F3} displayFlags=0x{output.DisplayConfigFlags:X} encoderFlags=0x{output.EncoderFlags:X} encoderId={output.EncoderId}";
                }

                Console.WriteLine($"Combined layout (IGCL order): combined={combinedWidth}x{combinedHeight} target={targetWidth}x{targetHeight}@{targetRefreshRate:F1}");
                Console.WriteLine($"  {DescribeOutput("first", firstOutput)}");
                Console.WriteLine($"  {DescribeOutput("second", secondOutput)}");
                Console.WriteLine($"DisplayOrder: {string.Join(",", displayOrder)}");
                Console.WriteLine("Placement: left=first, right=second");

                var childInfos = new[]
                {
                    new CombinedDisplayChildInfoDto
                    {
                        DisplayOutput = firstOutput.Handle,
                        FbSrc = new ctl_rect_t { Left = 0, Top = 0, Right = targetWidth, Bottom = targetHeight },
                        FbPos = new ctl_rect_t { Left = 0, Top = 0, Right = targetWidth, Bottom = targetHeight },
                        DisplayOrientation = ctl_display_orientation_t.CTL_DISPLAY_ORIENTATION_0,
                        TargetMode = new ctl_child_display_target_mode_t { Width = targetWidth, Height = targetHeight, RefreshRate = targetRefreshRate }
                    },
                    new CombinedDisplayChildInfoDto
                    {
                        DisplayOutput = secondOutput.Handle,
                        FbSrc = new ctl_rect_t { Left = targetWidth, Top = 0, Right = targetWidth * 2, Bottom = targetHeight },
                        FbPos = new ctl_rect_t { Left = 0, Top = 0, Right = targetWidth, Bottom = targetHeight },
                        DisplayOrientation = ctl_display_orientation_t.CTL_DISPLAY_ORIENTATION_0,
                        TargetMode = new ctl_child_display_target_mode_t { Width = targetWidth, Height = targetHeight, RefreshRate = targetRefreshRate }
                    }
                };

                var enableArgs = new CombinedDisplayArgsDto
                {
                    OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_ENABLE,
                    NumOutputs = desiredOutputs,
                    CombinedDesktopWidth = combinedWidth,
                    CombinedDesktopHeight = combinedHeight,
                    ChildInfos = childInfos
                };

                var combinedEnabled = false;
                IntPtr combinedOutput = IntPtr.Zero;
                try
                {
                    FacadeTestUtils.InvokeOrSkip(() => adapter.SetCombinedDisplay(enableArgs), "Combined display enable unsupported");
                    combinedEnabled = true;

                    var updated = adapter.GetCombinedDisplay();
                    Assert.Equal(desiredOutputs, updated.NumOutputs);
                    Assert.Equal((uint)combinedWidth, updated.CombinedDesktopWidth);
                    Assert.Equal((uint)combinedHeight, updated.CombinedDesktopHeight);
                    Assert.True(updated.CombinedDisplayOutput != IntPtr.Zero);
                    combinedOutput = updated.CombinedDisplayOutput;

                    Console.WriteLine("Combined display enabled; waiting 10 seconds before disable.");
                    System.Threading.Thread.Sleep(10000);
                }
                finally
                {
                    if (combinedEnabled)
                    {
                        Console.WriteLine("Disabling combined display.");
                        var disableArgs = new CombinedDisplayArgsDto
                        {
                            OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_DISABLE,
                            CombinedDisplayOutput = combinedOutput
                        };

                        FacadeTestUtils.InvokeOrSkip(() => adapter.SetCombinedDisplay(disableArgs), "Combined display disable unsupported");
                    }
                }
            }
        }

        private static bool TryParseDisplayHandle(IGCLDisplayHelper display, out IntPtr handle)
        {
            handle = IntPtr.Zero;
            const string prefix = "Display-";
            var name = display.Name;
            if (!name.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                return false;

            var hex = name.Substring(prefix.Length);
            if (!long.TryParse(hex, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var value))
                return false;

            handle = new IntPtr(value);
            return handle != IntPtr.Zero;
        }
    }
}
