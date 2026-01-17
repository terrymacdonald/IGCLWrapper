using System;
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
                var props = adapter.GetProperties();
                Assert.True(props.Size > 0);
                var displays = adapter.EnumerateDisplayOutputs();
                Assert.NotNull(displays);
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
    }
}
