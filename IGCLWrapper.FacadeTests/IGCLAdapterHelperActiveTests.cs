using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("ActiveCombined")]
    public class IGCLAdapterHelperActiveTests
    {
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

                var existing = adapter.GetCombinedDisplay();
                if (!existing.HasValue)
                    throw new SkipException("Combined display query unsupported.");

                if (existing.Value.NumOutputs > 0)
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

                    DisplayPropertiesDto props;
                    AdapterDisplayEncoderPropertiesDto encoderProps;
                    try
                    {
                        props = display.GetProperties();
                        var encoderPropsNullable = display.GetAdapterDisplayEncoderProperties();
                        if (!encoderPropsNullable.HasValue)
                            continue;
                        encoderProps = encoderPropsNullable.Value;
                    }
                    catch (IGCLException)
                    {
                        continue;
                    }

                    var isDisplayActive = props.IsDisplayActive;
                    var isDisplayAttached = props.IsDisplayAttached;
                    var encoderFlags = encoderProps.EncoderConfigFlags;
                    var isCombinedAvailable = encoderFlags == 0 || (encoderFlags & combinedAllowedEncoderTypes) != 0;

                    if (!isDisplayActive || !isDisplayAttached || !isCombinedAvailable)
                        continue;

                    var width = (int)props.DisplayTimingInfo.HActive;
                    var height = (int)props.DisplayTimingInfo.VActive;
                    if (width <= 0 || height <= 0)
                        continue;

                    var refreshRate = props.DisplayTimingInfo.RefreshRate;
                    var encoderId = encoderProps.OsDisplayEncoderHandle.WindowsDisplayEncoderId;
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

                var childInfos = new List<CombinedDisplayChildInfoDto>
                {
                    new CombinedDisplayChildInfoDto
                    {
                        DisplayOutputWindowsDisplayEncoderId = firstOutput.EncoderId,
                        FbSrc = new RectDto { Left = 0, Top = 0, Right = targetWidth, Bottom = targetHeight },
                        FbPos = new RectDto { Left = 0, Top = 0, Right = targetWidth, Bottom = targetHeight },
                        DisplayOrientation = ctl_display_orientation_t.CTL_DISPLAY_ORIENTATION_0,
                        TargetMode = new ChildDisplayTargetModeDto { Width = targetWidth, Height = targetHeight, RefreshRate = targetRefreshRate }
                    },
                    new CombinedDisplayChildInfoDto
                    {
                        DisplayOutputWindowsDisplayEncoderId = secondOutput.EncoderId,
                        FbSrc = new RectDto { Left = targetWidth, Top = 0, Right = targetWidth * 2, Bottom = targetHeight },
                        FbPos = new RectDto { Left = 0, Top = 0, Right = targetWidth, Bottom = targetHeight },
                        DisplayOrientation = ctl_display_orientation_t.CTL_DISPLAY_ORIENTATION_0,
                        TargetMode = new ChildDisplayTargetModeDto { Width = targetWidth, Height = targetHeight, RefreshRate = targetRefreshRate }
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
                try
                {
                    FacadeTestUtils.InvokeOrSkip(() => adapter.SetCombinedDisplay(enableArgs), "Combined display enable unsupported");
                    combinedEnabled = true;

                    var updated = adapter.GetCombinedDisplay();
                    Assert.True(updated.HasValue, "Combined display returned null after enable.");
                    Assert.Equal(desiredOutputs, updated.Value.NumOutputs);
                    Assert.Equal((uint)combinedWidth, updated.Value.CombinedDesktopWidth);
                    Assert.Equal((uint)combinedHeight, updated.Value.CombinedDesktopHeight);
                    Assert.NotNull(updated.Value.ChildInfos);

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
                            OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_DISABLE
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
