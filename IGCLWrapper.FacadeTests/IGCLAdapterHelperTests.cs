using System;
using System.Collections.Generic;
using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
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
                Assert.Equal(108, props.Reserved!.Count);
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
                Assert.Equal(108, props.Reserved!.Count);
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

                if (combined.NumOutputs == 0 || combined.ChildInfos == null || combined.ChildInfos.Count == 0)
                {
                    throw new SkipException("Combined display not configured.");
                }

                Console.WriteLine("Combined display detected.");
                Console.WriteLine($" - NumOutputs={combined.NumOutputs} Width={combined.CombinedDesktopWidth} Height={combined.CombinedDesktopHeight}");

                Assert.True(combined.ChildInfos.Count >= combined.NumOutputs);
                for (var i = 0; i < combined.NumOutputs; i++)
                {
                    var child = combined.ChildInfos[i];

                    Assert.True(child.TargetMode.Width >= 0);
                    Assert.True(child.TargetMode.Height >= 0);
                    Console.WriteLine($" - Child {i}: encoderId={child.DisplayOutputWindowsDisplayEncoderId} orientation={child.DisplayOrientation}");
                }
            }
        }

        [Fact]
        public unsafe void CombinedDisplayArgsDto_ToNative_ShouldUseManagedChildInfos()
        {
            var dto = new CombinedDisplayArgsDto
            {
                OpType = ctl_combined_display_optype_t.CTL_COMBINED_DISPLAY_OPTYPE_ENABLE,
                IsSupported = true,
                CombinedDesktopWidth = 3840,
                CombinedDesktopHeight = 1080,
                ChildInfos = new List<CombinedDisplayChildInfoDto>
                {
                    new CombinedDisplayChildInfoDto
                    {
                        DisplayOutputWindowsDisplayEncoderId = 100,
                        FbSrc = new RectDto { Left = 0, Top = 0, Right = 1919, Bottom = 1079 },
                        FbPos = new RectDto { Left = 0, Top = 0, Right = 1919, Bottom = 1079 },
                        DisplayOrientation = ctl_display_orientation_t.CTL_DISPLAY_ORIENTATION_0,
                        TargetMode = new ChildDisplayTargetModeDto { Width = 1920, Height = 1080, RefreshRate = 60.0f }
                    },
                    new CombinedDisplayChildInfoDto
                    {
                        DisplayOutputWindowsDisplayEncoderId = 101,
                        FbSrc = new RectDto { Left = 1920, Top = 0, Right = 3839, Bottom = 1079 },
                        FbPos = new RectDto { Left = 1920, Top = 0, Right = 3839, Bottom = 1079 },
                        DisplayOrientation = ctl_display_orientation_t.CTL_DISPLAY_ORIENTATION_0,
                        TargetMode = new ChildDisplayTargetModeDto { Width = 1920, Height = 1080, RefreshRate = 60.0f }
                    }
                }
            };

            var native = dto.ToNative();

            Assert.Equal((byte)2, native.NumOutputs);
            Assert.True(native.pChildInfo == null);
            Assert.True(native.hCombinedDisplayOutput == null);
            Assert.Equal((byte)1, native.IsSupported);

            var childNative = dto.ChildInfos![0].ToNative();
            Assert.True(childNative.hDisplayOutput == null);
            Assert.Equal(dto.ChildInfos[0].FbSrc.Left, childNative.FbSrc.Left);
            Assert.Equal(dto.ChildInfos[0].TargetMode.Width, childNative.TargetMode.Width);
        }

        [Fact]
        public void DeviceAdapterPropertiesDto_FlagBooleans_ShouldTrackRawMasks()
        {
            var dto = new DeviceAdapterPropertiesDto
            {
                SupportedSubfunctionFlags = (uint)ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_DISPLAY |
                                            (uint)ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_MEDIA,
                GraphicsAdapterProperties = (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_INTEGRATED
            };

            Assert.True(dto.SupportsDisplay);
            Assert.False(dto.Supports3D);
            Assert.True(dto.SupportsMedia);
            Assert.True(dto.IsIntegratedGraphicsAdapter);
            Assert.False(dto.IsLdaPrimary);
            Assert.False(dto.IsLdaSecondary);

            dto.Supports3D = true;
            dto.IsLdaPrimary = true;
            dto.IsIntegratedGraphicsAdapter = false;

            Assert.True((dto.SupportedSubfunctionFlags & (uint)ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_3D) != 0);
            Assert.True((dto.GraphicsAdapterProperties & (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_LDA_PRIMARY) != 0);
            Assert.True((dto.GraphicsAdapterProperties & (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_INTEGRATED) == 0);
        }

        [Fact]
        public void DeviceAdapterPropertiesDto_AllFlagBooleans_ShouldRoundTripMasks()
        {
            var dto = new DeviceAdapterPropertiesDto();

            dto.SupportsDisplay = true;
            dto.Supports3D = true;
            dto.SupportsMedia = true;
            dto.IsIntegratedGraphicsAdapter = true;
            dto.IsLdaPrimary = true;
            dto.IsLdaSecondary = true;

            Assert.Equal(
                (uint)ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_DISPLAY |
                (uint)ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_3D |
                (uint)ctl_supported_functions_flag_t.CTL_SUPPORTED_FUNCTIONS_FLAG_MEDIA,
                dto.SupportedSubfunctionFlags);

            Assert.Equal(
                (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_INTEGRATED |
                (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_LDA_PRIMARY |
                (uint)ctl_adapter_properties_flag_t.CTL_ADAPTER_PROPERTIES_FLAG_LDA_SECONDARY,
                dto.GraphicsAdapterProperties);
        }

    }
}
