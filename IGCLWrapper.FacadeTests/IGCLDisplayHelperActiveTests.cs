using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading;
using Xunit;
using Xunit.Sdk;
using SkipException = Xunit.SkipException;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("ActiveDisplay")]
    public class IGCLDisplayHelperActiveTests
    {
        private const int SettlingDelayMs = 2000;
        private const uint MaxBrightnessMilliPercent = 100000;
        private const uint BrightnessDelta = 2000;

        [Trait("Category", "Active")]
        [SkippableFact]
        public void SetCurrentSharpness_ShouldApplyAndRevert_WhenSupported()
        {
            RunActiveDisplayTest(nameof(SetCurrentSharpness_ShouldApplyAndRevert_WhenSupported), display =>
            {
                var capsInfo = FacadeTestUtils.InvokeOrSkip(() => display.GetSharpnessCaps(), "Sharpness caps unsupported");
                if (capsInfo.caps.SupportedFilterFlags == 0 || capsInfo.filters.Length == 0)
                    return false;

                var current = FacadeTestUtils.InvokeOrSkip(() => display.GetCurrentSharpness(), "Sharpness unsupported");
                if (!TryBuildSharpnessUpdate(capsInfo, current, out var updated))
                    return false;

                ApplyAndRevert(() => display.SetCurrentSharpness(updated), () => display.SetCurrentSharpness(current));
                return true;
            });
        }

        [Trait("Category", "Active")]
        [SkippableFact]
        public void SetPowerOptimizationSetting_ShouldApplyAndRevert_WhenSupported()
        {
            RunActiveDisplayTest(nameof(SetPowerOptimizationSetting_ShouldApplyAndRevert_WhenSupported), display =>
            {
                var caps = FacadeTestUtils.InvokeOrSkip(() => display.GetPowerOptimizationCaps(), "Power optimization caps unsupported");
                if (caps.SupportedFeatures == 0)
                    return false;

                if ((caps.SupportedFeatures & (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_PSR) != 0)
                    return TryApplyPowerOptimizationPsr(display);

                if ((caps.SupportedFeatures & (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LRR) != 0)
                    return TryApplyPowerOptimizationLrr(display);

                if ((caps.SupportedFeatures & (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_DPST) != 0)
                    return TryApplyPowerOptimizationDpst(display);

                return false;
            });
        }

        [Trait("Category", "Active")]
        [SkippableFact]
        public void SetBrightnessSetting_ShouldApplyAndRevert_WhenSupported()
        {
            RunActiveDisplayTest(nameof(SetBrightnessSetting_ShouldApplyAndRevert_WhenSupported), display =>
            {
                var encoder = FacadeTestUtils.InvokeOrSkip(() => display.GetAdapterDisplayEncoderProperties(), "Encoder properties unsupported");
                var isCompanion = (encoder.EncoderConfigFlags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COMPANION_DISPLAY) != 0;
                if (!isCompanion)
                    return false;

                var current = display.GetBrightnessSetting();
                var newTarget = PickDifferentBrightness(current.CurrentBrightness);
                if (newTarget == current.CurrentBrightness)
                    return false;

                var setArgs = IGCLDisplayHelper.CreateSetBrightness();
                setArgs.TargetBrightness = newTarget;
                setArgs.SmoothTransitionTimeInMs = 200;

                ApplyAndRevert(
                    () => display.SetBrightnessSetting(setArgs),
                    () =>
                    {
                        var revert = IGCLDisplayHelper.CreateSetBrightness();
                        revert.TargetBrightness = current.CurrentBrightness;
                        revert.SmoothTransitionTimeInMs = 200;
                        display.SetBrightnessSetting(revert);
                    });

                return true;
            });
        }

        [Trait("Category", "Active")]
        [SkippableFact]
        public void SetRetroScalingSettings_ShouldApplyAndRevert_WhenSupported()
        {
            RunActiveDisplayTest(nameof(SetRetroScalingSettings_ShouldApplyAndRevert_WhenSupported), display =>
            {
                var caps = FacadeTestUtils.InvokeOrSkip(() => display.GetSupportedRetroScalingCapability(), "Retro scaling caps unsupported");
                if (caps.SupportedRetroScaling == 0)
                    return false;

                var current = FacadeTestUtils.InvokeOrSkip(() => display.GetRetroScalingSettings(), "Retro scaling settings unsupported");
                if (!TryBuildRetroScalingUpdate(caps, current, out var updated))
                    return false;

                ApplyAndRevert(() => display.SetRetroScalingSettings(updated), () => display.SetRetroScalingSettings(current));
                return true;
            });
        }

        [Trait("Category", "Active")]
        [SkippableFact]
        public void SetCurrentScaling_ShouldApplyAndRevert_WhenSupported()
        {
            RunActiveDisplayTest(nameof(SetCurrentScaling_ShouldApplyAndRevert_WhenSupported), display =>
            {
                var caps = FacadeTestUtils.InvokeOrSkip(() => display.GetSupportedScalingCapability(), "Scaling caps unsupported");
                if (caps.SupportedScaling == 0)
                    return false;

                var current = FacadeTestUtils.InvokeOrSkip(() => display.GetCurrentScaling(), "Scaling settings unsupported");
                if (!TryBuildScalingUpdate(caps, current, out var updated))
                    return false;

                ApplyAndRevert(() => display.SetCurrentScaling(updated), () => display.SetCurrentScaling(current));
                return true;
            });
        }

        [Trait("Category", "Active")]
        [SkippableFact]
        public void SetLaceConfig_ShouldApplyAndRevert_WhenSupported()
        {
            RunActiveDisplayTest(nameof(SetLaceConfig_ShouldApplyAndRevert_WhenSupported), display =>
            {
                var caps = FacadeTestUtils.InvokeOrSkip(() => display.GetPowerOptimizationCaps(), "Power optimization caps unsupported");
                if ((caps.SupportedFeatures & (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LACE) == 0)
                    return false;

                var current = FacadeTestUtils.InvokeOrSkip(() => display.GetLACEConfig(), "LACE config unsupported");

                var updated = current;
                updated.OpTypeSet = ctl_set_operation_t.CTL_SET_OPERATION_CUSTOM;
                updated.Enabled = true;
                updated.Trigger = (uint)ctl_lace_trigger_flag_t.CTL_LACE_TRIGGER_FLAG_FIXED_AGGRESSIVENESS;

                var newAggressiveness = PickDifferentAggressiveness(current.LaceConfig.FixedAggressivenessLevelPercent);
                if (newAggressiveness == current.LaceConfig.FixedAggressivenessLevelPercent)
                    return false;

                updated.LaceConfig.FixedAggressivenessLevelPercent = newAggressiveness;

                ApplyAndRevert(
                    () => display.SetLACEConfig(updated),
                    () =>
                    {
                        var revert = current;
                        revert.OpTypeSet = ctl_set_operation_t.CTL_SET_OPERATION_CUSTOM;
                        display.SetLACEConfig(revert);
                    });

                return true;
            });
        }

        [Trait("Category", "Active")]
        [SkippableFact]
        public void SetIntelArcSyncProfile_ShouldApplyAndRevert_WhenSupported()
        {
            RunActiveDisplayTest(nameof(SetIntelArcSyncProfile_ShouldApplyAndRevert_WhenSupported), display =>
            {
                var monitorCaps = FacadeTestUtils.InvokeOrSkip(() => display.GetIntelArcSyncInfoForMonitor(), "Arc Sync info unsupported");
                if (!monitorCaps.IsIntelArcSyncSupported)
                    return false;

                var current = FacadeTestUtils.InvokeOrSkip(() => display.GetIntelArcSyncProfile(), "Arc Sync profile unsupported");
                var updated = current;
                updated.IntelArcSyncProfile = current.IntelArcSyncProfile == ctl_intel_arc_sync_profile_t.CTL_INTEL_ARC_SYNC_PROFILE_OFF
                    ? ctl_intel_arc_sync_profile_t.CTL_INTEL_ARC_SYNC_PROFILE_RECOMMENDED
                    : ctl_intel_arc_sync_profile_t.CTL_INTEL_ARC_SYNC_PROFILE_OFF;

                if (updated.IntelArcSyncProfile == current.IntelArcSyncProfile)
                    return false;

                ApplyAndRevert(() => display.SetIntelArcSyncProfile(updated), () => display.SetIntelArcSyncProfile(current));
                return true;
            });
        }

        [Trait("Category", "Active")]
        [SkippableFact]
        public void SetDynamicContrastEnhancement_ShouldApplyAndRevert_WhenSupported()
        {
            RunActiveDisplayTest(nameof(SetDynamicContrastEnhancement_ShouldApplyAndRevert_WhenSupported), display =>
            {
                var encoder = FacadeTestUtils.InvokeOrSkip(() => display.GetAdapterDisplayEncoderProperties(), "Encoder properties unsupported");
                var isInternal = (encoder.EncoderConfigFlags & (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_INTERNAL_DISPLAY) != 0;
                if (!isInternal)
                    return false;

                var currentResult = FacadeTestUtils.InvokeOrSkip(() => display.GetDynamicContrastEnhancement(), "DCE unsupported");
                var current = currentResult.args;
                if (!current.IsSupported)
                    return false;

                var histogram = currentResult.histogram;

                if (!TryBuildDceUpdate(current, out var updated))
                    return false;

                ApplyAndRevert(
                    () => display.SetDynamicContrastEnhancement(updated, histogram),
                    () => display.SetDynamicContrastEnhancement(current, histogram));
                return true;
            });
        }

        [Trait("Category", "Active")]
        [SkippableFact]
        public void SetWireFormat_ShouldApplyAndRevert_WhenSupported()
        {
            RunActiveDisplayTest(nameof(SetWireFormat_ShouldApplyAndRevert_WhenSupported), display =>
            {
                var current = FacadeTestUtils.InvokeOrSkip(() => display.GetWireFormat(), "Wire format unsupported");
                if (!TryBuildWireFormatUpdate(current, out var updated))
                    return false;

                ApplyAndRevert(() => display.SetWireFormat(updated), () => display.SetWireFormat(current));
                return true;
            });
        }

        [Trait("Category", "Active")]
        [SkippableFact]
        public void SetDisplaySettings_ShouldApplyAndRevert_WhenSupported()
        {
            RunActiveDisplayTest(nameof(SetDisplaySettings_ShouldApplyAndRevert_WhenSupported), display =>
            {
                var current = FacadeTestUtils.InvokeOrSkip(() => display.GetDisplaySettings(), "Display settings unsupported");
                if (!TryBuildDisplaySettingsUpdate(current, out var updated, out var validFlags))
                    return false;

                updated.ValidFlags = validFlags;
                var revert = current;
                revert.ValidFlags = validFlags;

                ApplyAndRevert(() => display.SetDisplaySettings(updated), () => display.SetDisplaySettings(revert));
                return true;
            });
        }

        [Trait("Category", "Active")]
        [SkippableFact]
        public void PixelTransformationSetConfig_ShouldApplyAndRevert_WhenSupported()
        {
            RunActiveDisplayTest(nameof(PixelTransformationSetConfig_ShouldApplyAndRevert_WhenSupported), display =>
            {
                var capabilityQuery = IGCLDisplayHelper.CreatePixtxPipeGetConfig();
                capabilityQuery.QueryType = ctl_pixtx_config_query_type_t.CTL_PIXTX_CONFIG_QUERY_TYPE_CAPABILITY;

                var capability = FacadeTestUtils.InvokeOrSkip(() => display.PixelTransformationGetConfig(capabilityQuery), "Pixel transformation unsupported");
                if (!TryPickPixTxMatrixBlock(capability.blocks, out var block))
                    return false;

                PrepareMatrixBlock(ref block);
                var currentQuery = IGCLDisplayHelper.CreatePixtxPipeGetConfig();
                currentQuery.QueryType = ctl_pixtx_config_query_type_t.CTL_PIXTX_CONFIG_QUERY_TYPE_CURRENT;
                var current = FacadeTestUtils.InvokeOrSkip(() => display.PixelTransformationGetConfig(currentQuery, new[] { block }), "Pixel transformation unsupported");
                if (!TryBuildPixTxMatrixUpdate(current.blocks, out var original, out var updated))
                    return false;

                ApplyAndRevert(() => SetPixTxConfig(display, updated), () => SetPixTxConfig(display, original));
                return true;
            });
        }

        [Trait("Category", "Active")]
        [SkippableFact]
        public void SoftwarePsr_ShouldApplyAndRevert_WhenSupported()
        {
            RunActiveDisplayTest(nameof(SoftwarePsr_ShouldApplyAndRevert_WhenSupported), display =>
            {
                var current = FacadeTestUtils.InvokeOrSkip(() => display.SoftwarePSR(new SwPsrSettingsDto { Set = false }), "Software PSR unsupported");
                if (!current.Supported)
                    return false;

                var updated = current;
                updated.Set = true;
                updated.Enable = !current.Enable;

                var revert = current;
                revert.Set = true;

                ApplyAndRevert(() => display.SoftwarePSR(updated), () => display.SoftwarePSR(revert));
                return true;
            });
        }

        private static void RunActiveDisplayTest(string testName, Func<IGCLDisplayHelper, bool> action)
        {
            if (!IGCLApiHelper.IsIGCLDllAvailable(out var dllError))
                throw new SkipException($"IGCL DLL unavailable: {dllError}");

            if (!IGCLHardwareDetection.HasIntelGPU(out var hwError))
                throw new SkipException($"Intel GPU not detected: {hwError}");

            using var api = IGCLApiHelper.Initialize();
            var adapters = api.EnumerateAdapters();
            if (adapters.Count == 0)
                throw new SkipException("No adapters returned from IGCL.");

            var failures = new List<string>();
            var activeDisplays = 0;
            var executedDisplays = 0;

            for (var adapterIndex = 0; adapterIndex < adapters.Count; adapterIndex++)
            {
                var adapter = adapters[adapterIndex];
                var adapterLabel = SafeAdapterLabel(adapter, adapterIndex);
                IReadOnlyList<IGCLDisplayHelper> displays;
                try
                {
                    displays = adapter.EnumerateDisplayOutputs();
                }
                catch (Exception ex)
                {
                    failures.Add($"{adapterLabel}: EnumerateDisplayOutputs failed: {ex.Message}");
                    continue;
                }

                foreach (var display in displays)
                {
                    ctl_display_properties_t props;
                    try
                    {
                        props = display.GetPropertiesNative();
                    }
                    catch (Exception ex)
                    {
                        failures.Add($"{adapterLabel}/{display.Name}: GetPropertiesNative failed: {ex.Message}");
                        continue;
                    }

                    var isActive = (props.DisplayConfigFlags & (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ACTIVE) != 0;
                    var isAttached = (props.DisplayConfigFlags & (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ATTACHED) != 0;
                    if (!isActive || !isAttached)
                        continue;

                    activeDisplays++;

                    try
                    {
                        if (action(display))
                            executedDisplays++;
                    }
                    catch (SkipException)
                    {
                        // Unsupported on this display.
                    }
                    catch (IGCLException ex) when (IsUnsupportedResult(ex.Result))
                    {
                        // Unsupported on this display.
                    }
                    catch (EntryPointNotFoundException)
                    {
                        // Unsupported on this system.
                    }
                    catch (Exception ex)
                    {
                        if (ex is IGCLException igclEx)
                        {
                            failures.Add($"{adapterLabel}/{display.Name}: {igclEx.Result} {igclEx.Message}");
                        }
                        else
                        {
                            failures.Add($"{adapterLabel}/{display.Name}: {ex.GetType().Name} {ex.Message}");
                        }
                    }
                }
            }

            if (activeDisplays == 0)
                throw new SkipException("No active displays connected.");

            if (executedDisplays == 0)
                throw new SkipException($"{testName} unsupported on active displays.");

            if (failures.Count > 0)
                throw new XunitException($"{testName} failures:{Environment.NewLine}{string.Join(Environment.NewLine, failures)}");
        }

        private static string SafeAdapterLabel(IGCLAdapterHelper adapter, int index)
        {
            try
            {
                var name = adapter.Name;
                if (!string.IsNullOrWhiteSpace(name))
                    return name;
            }
            catch
            {
                // Ignore adapter name failures.
            }
            return $"Adapter-{index}";
        }

        private static void ApplyAndRevert(Action apply, Action revert)
        {
            var applied = false;
            try
            {
                apply();
                applied = true;
                Thread.Sleep(500);
                WaitForSettle();
            }
            finally
            {
                if (applied)
                {
                    revert();
                    WaitForSettle();
                }
            }
        }

        private static void WaitForSettle()
        {
            Thread.Sleep(SettlingDelayMs);
        }

        private static bool IsUnsupportedResult(ctl_result_t result)
        {
            return result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                   result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION;
        }

        private static bool TryBuildSharpnessUpdate((ctl_sharpness_caps_t caps, ctl_sharpness_filter_properties_t[] filters) capsInfo, SharpnessSettingsDto current, out SharpnessSettingsDto updated)
        {
            updated = current;
            if (capsInfo.caps.SupportedFilterFlags == 0 || capsInfo.filters.Length == 0)
                return false;

            var filterType = PickSharpnessFilter(capsInfo, current.FilterType, out var range);
            if (filterType == 0)
                return false;

            updated.FilterType = filterType;

            if (TryPickDifferentFloat(current.Intensity, range, out var newIntensity))
                updated.Intensity = newIntensity;

            if (updated.FilterType == current.FilterType && updated.Intensity.Equals(current.Intensity))
                updated.Enable = !current.Enable;

            return updated.FilterType != current.FilterType ||
                   updated.Enable != current.Enable ||
                   !updated.Intensity.Equals(current.Intensity);
        }

        private static uint PickSharpnessFilter((ctl_sharpness_caps_t caps, ctl_sharpness_filter_properties_t[] filters) capsInfo, uint currentFilter, out ctl_property_range_info_t range)
        {
            range = default;
            foreach (var filter in capsInfo.filters)
            {
                if ((capsInfo.caps.SupportedFilterFlags & filter.FilterType) == 0)
                    continue;
                if (filter.FilterType == currentFilter)
                    continue;

                range = filter.FilterDetails;
                return filter.FilterType;
            }

            foreach (var filter in capsInfo.filters)
            {
                if ((capsInfo.caps.SupportedFilterFlags & filter.FilterType) == 0)
                    continue;
                if (filter.FilterType != currentFilter)
                    continue;

                range = filter.FilterDetails;
                return filter.FilterType;
            }

            return 0;
        }

        private static bool TryPickDifferentFloat(float current, ctl_property_range_info_t range, out float candidate)
        {
            var min = range.min_possible_value;
            var max = range.max_possible_value;

            if (max < min)
            {
                var temp = min;
                min = max;
                max = temp;
            }

            if (Math.Abs(max - min) < float.Epsilon)
            {
                candidate = min;
                return !candidate.Equals(current);
            }

            var step = range.step_size;
            if (step <= 0 || float.IsNaN(step))
                step = (max - min) / 10f;
            if (step <= 0 || float.IsNaN(step))
                step = 1f;

            candidate = current + step;
            if (candidate > max || candidate < min || float.IsNaN(candidate))
                candidate = current - step;
            if (candidate > max || candidate < min || float.IsNaN(candidate))
                candidate = min;

            return !candidate.Equals(current);
        }

        private static bool TryApplyPowerOptimizationPsr(IGCLDisplayHelper display)
        {
            var request = new PowerOptimizationSettingsDto
            {
                PowerOptimizationFeature = (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_PSR,
                PowerSource = ctl_power_source_t.CTL_POWER_SOURCE_DC,
                PowerOptimizationPlan = ctl_power_optimization_plan_t.CTL_POWER_OPTIMIZATION_PLAN_BALANCED
            };

            if (!TryGetPowerOptimizationSetting(display, request, out var current))
                return false;

            var updated = current;
            updated.PowerOptimizationFeature = request.PowerOptimizationFeature;
            updated.PowerSource = request.PowerSource;
            updated.PowerOptimizationPlan = request.PowerOptimizationPlan;
            updated.Enable = !current.Enable;

            ApplyAndRevert(() => display.SetPowerOptimizationSetting(updated), () => display.SetPowerOptimizationSetting(current));
            return true;
        }

        private static bool TryApplyPowerOptimizationLrr(IGCLDisplayHelper display)
        {
            var request = new PowerOptimizationSettingsDto
            {
                PowerOptimizationFeature = (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LRR,
                PowerSource = ctl_power_source_t.CTL_POWER_SOURCE_DC,
                PowerOptimizationPlan = ctl_power_optimization_plan_t.CTL_POWER_OPTIMIZATION_PLAN_BALANCED
            };

            if (!TryGetPowerOptimizationSetting(display, request, out var current))
                return false;

            var lrr = current.FeatureSpecificData.LrrInfo;
            if (lrr.SupportedLrrTypes == 0 || lrr.RequirePsrDisable)
                return false;

            var updated = current;
            updated.PowerOptimizationFeature = request.PowerOptimizationFeature;
            updated.PowerSource = request.PowerSource;
            updated.PowerOptimizationPlan = request.PowerOptimizationPlan;

            var newType = PickDifferentFlag(lrr.SupportedLrrTypes, lrr.CurrentLrrTypes, new[]
            {
                (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR10,
                (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR20,
                (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR25,
                (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_ALRR,
                (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_UBLRR,
                (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_UBZRR
            });

            if (newType != 0 && newType != lrr.CurrentLrrTypes)
            {
                updated.FeatureSpecificData.LrrInfo.CurrentLrrTypes = newType;
                updated.Enable = true;
            }
            else
            {
                updated.Enable = !current.Enable;
            }

            if (updated.Enable == current.Enable &&
                updated.FeatureSpecificData.LrrInfo.CurrentLrrTypes == lrr.CurrentLrrTypes)
                return false;

            ApplyAndRevert(() => display.SetPowerOptimizationSetting(updated), () => display.SetPowerOptimizationSetting(current));
            return true;
        }

        private static bool TryApplyPowerOptimizationDpst(IGCLDisplayHelper display)
        {
            var request = new PowerOptimizationSettingsDto
            {
                PowerOptimizationFeature = (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_DPST,
                PowerSource = ctl_power_source_t.CTL_POWER_SOURCE_DC,
                PowerOptimizationPlan = ctl_power_optimization_plan_t.CTL_POWER_OPTIMIZATION_PLAN_BALANCED
            };

            if (!TryGetPowerOptimizationSetting(display, request, out var current))
                return false;

            var dpst = current.FeatureSpecificData.DpstInfo;
            if (dpst.SupportedFeatures == 0)
                return false;

            var enabledFeature = (dpst.SupportedFeatures & (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_BKLT) != 0
                ? (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_BKLT
                : PickFirstSetBit(dpst.SupportedFeatures);

            if (enabledFeature == 0)
                return false;

            var updated = current;
            updated.PowerOptimizationFeature = request.PowerOptimizationFeature;
            updated.PowerSource = request.PowerSource;
            updated.PowerOptimizationPlan = request.PowerOptimizationPlan;
            updated.FeatureSpecificData.DpstInfo.EnabledFeatures = enabledFeature;

            if (dpst.MinLevel != dpst.MaxLevel)
            {
                var newLevel = dpst.Level == dpst.MaxLevel ? dpst.MinLevel : dpst.MaxLevel;
                updated.FeatureSpecificData.DpstInfo.Level = newLevel;
            }
            else
            {
                updated.Enable = !current.Enable;
            }

            if (updated.Enable == current.Enable &&
                updated.FeatureSpecificData.DpstInfo.Level == dpst.Level &&
                updated.FeatureSpecificData.DpstInfo.EnabledFeatures == dpst.EnabledFeatures)
                return false;

            ApplyAndRevert(() => display.SetPowerOptimizationSetting(updated), () => display.SetPowerOptimizationSetting(current));
            return true;
        }

        private static bool TryGetPowerOptimizationSetting(IGCLDisplayHelper display, PowerOptimizationSettingsDto request, out PowerOptimizationSettingsDto current)
        {
            try
            {
                current = display.GetPowerOptimizationSetting(request);
                return true;
            }
            catch (IGCLException ex) when (ex.Result == ctl_result_t.CTL_RESULT_ERROR_INVALID_POWERFEATURE_OPTIMIZATION_FLAG ||
                                           ex.Result == ctl_result_t.CTL_RESULT_ERROR_INVALID_POWERSOURCE_TYPE_FOR_DPST)
            {
                current = default;
                return false;
            }
        }

        private static bool TryBuildRetroScalingUpdate(ctl_retro_scaling_caps_t caps, RetroScalingSettingsDto current, out RetroScalingSettingsDto updated)
        {
            updated = current;
            if (caps.SupportedRetroScaling == 0)
                return false;

            var newType = PickDifferentFlag(caps.SupportedRetroScaling, current.RetroScalingType, new[]
            {
                (uint)ctl_retro_scaling_type_flag_t.CTL_RETRO_SCALING_TYPE_FLAG_INTEGER,
                (uint)ctl_retro_scaling_type_flag_t.CTL_RETRO_SCALING_TYPE_FLAG_NEAREST_NEIGHBOUR
            });

            if (newType == 0)
                return false;

            updated.RetroScalingType = newType;
            updated.Enable = true;

            return updated.RetroScalingType != current.RetroScalingType || updated.Enable != current.Enable;
        }

        private static bool TryBuildScalingUpdate(ctl_scaling_caps_t caps, ScalingSettingsDto current, out ScalingSettingsDto updated)
        {
            updated = current;
            if (caps.SupportedScaling == 0)
                return false;

            var newType = PickDifferentScalingType(caps.SupportedScaling, current.ScalingType);
            if (newType == 0)
                return false;

            updated.ScalingType = newType;
            if (!updated.Enable)
                updated.Enable = true;

            return updated.ScalingType != current.ScalingType || updated.Enable != current.Enable;
        }

        private static uint PickDifferentScalingType(uint supported, uint current)
        {
            var candidates = new[]
            {
                (uint)ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_IDENTITY,
                (uint)ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_CENTERED,
                (uint)ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_STRETCHED,
                (uint)ctl_scaling_type_flag_t.CTL_SCALING_TYPE_FLAG_ASPECT_RATIO_CENTERED_MAX
            };

            foreach (var candidate in candidates)
            {
                if ((supported & candidate) == 0)
                    continue;
                if (candidate == current)
                    continue;
                return candidate;
            }

            return 0;
        }

        private static bool TryBuildDceUpdate(DceArgsDto current, out DceArgsDto updated)
        {
            updated = current;
            updated.Enable = !current.Enable;

            var brightness = current.TargetBrightnessPercent;
            var newBrightness = brightness >= 95 ? 90u : brightness + 5u;
            updated.TargetBrightnessPercent = newBrightness;

            return updated.Enable != current.Enable ||
                   updated.TargetBrightnessPercent != current.TargetBrightnessPercent;
        }

        private static bool TryBuildWireFormatUpdate(WireFormatConfigDto current, out WireFormatConfigDto updated)
        {
            updated = current;
            if (current.SupportedWireFormat == null || current.SupportedWireFormat.Count == 0)
                return false;

            foreach (var candidate in current.SupportedWireFormat)
            {
                if (candidate.ColorModel == 0 && candidate.ColorDepth == 0)
                    continue;
                if (candidate.ColorModel == current.WireFormat.ColorModel &&
                    candidate.ColorDepth == current.WireFormat.ColorDepth)
                    continue;

                updated.WireFormat = candidate;
                return true;
            }

            return false;
        }

        private static bool TryBuildDisplaySettingsUpdate(DisplaySettingsDto current, out DisplaySettingsDto updated, out uint validFlags)
        {
            updated = current;
            validFlags = 0;

            if (IsSettingControllable(current, ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY))
            {
                updated.LowLatency = current.LowLatency == ctl_display_setting_low_latency_t.CTL_DISPLAY_SETTING_LOW_LATENCY_ENABLED
                    ? ctl_display_setting_low_latency_t.CTL_DISPLAY_SETTING_LOW_LATENCY_DISABLED
                    : ctl_display_setting_low_latency_t.CTL_DISPLAY_SETTING_LOW_LATENCY_ENABLED;
                validFlags = (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY;
                return true;
            }

            if (IsSettingControllable(current, ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_SOURCE_TM))
            {
                updated.SourceTm = current.SourceTm == ctl_display_setting_sourcetm_t.CTL_DISPLAY_SETTING_SOURCETM_ENABLED
                    ? ctl_display_setting_sourcetm_t.CTL_DISPLAY_SETTING_SOURCETM_DISABLED
                    : ctl_display_setting_sourcetm_t.CTL_DISPLAY_SETTING_SOURCETM_ENABLED;
                validFlags = (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_SOURCE_TM;
                return true;
            }

            if (IsSettingControllable(current, ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_CONTENT_TYPE))
            {
                updated.ContentType = current.ContentType == ctl_display_setting_content_type_t.CTL_DISPLAY_SETTING_CONTENT_TYPE_GAMING
                    ? ctl_display_setting_content_type_t.CTL_DISPLAY_SETTING_CONTENT_TYPE_DESKTOP
                    : ctl_display_setting_content_type_t.CTL_DISPLAY_SETTING_CONTENT_TYPE_GAMING;
                validFlags = (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_CONTENT_TYPE;
                return true;
            }

            if (IsSettingControllable(current, ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_QUANTIZATION_RANGE))
            {
                updated.QuantizationRange = current.QuantizationRange == ctl_display_setting_quantization_range_t.CTL_DISPLAY_SETTING_QUANTIZATION_RANGE_FULL_RANGE
                    ? ctl_display_setting_quantization_range_t.CTL_DISPLAY_SETTING_QUANTIZATION_RANGE_LIMITED_RANGE
                    : ctl_display_setting_quantization_range_t.CTL_DISPLAY_SETTING_QUANTIZATION_RANGE_FULL_RANGE;
                validFlags = (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_QUANTIZATION_RANGE;
                return true;
            }

            if (IsSettingControllable(current, ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_PICTURE_AR))
            {
                var candidate = PickDifferentPictureAr(current.SupportedPictureAr, current.PictureAr);
                if (candidate == current.PictureAr)
                    return false;

                updated.PictureAr = candidate;
                validFlags = (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_PICTURE_AR;
                return true;
            }

            if (IsSettingControllable(current, ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO))
            {
                updated.AudioSettings = current.AudioSettings == ctl_display_setting_audio_t.CTL_DISPLAY_SETTING_AUDIO_DISABLED
                    ? ctl_display_setting_audio_t.CTL_DISPLAY_SETTING_AUDIO_DEFAULT
                    : ctl_display_setting_audio_t.CTL_DISPLAY_SETTING_AUDIO_DISABLED;
                validFlags = (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO;
                return true;
            }

            return false;
        }

        private static bool IsSettingControllable(DisplaySettingsDto current, ctl_display_setting_flag_t flag)
        {
            var mask = (uint)flag;
            return (current.SupportedFlags & mask) != 0 && (current.ControllableFlags & mask) != 0;
        }

        private static ctl_display_setting_picture_ar_flag_t PickDifferentPictureAr(uint supported, ctl_display_setting_picture_ar_flag_t current)
        {
            var candidates = new[]
            {
                ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_16_9,
                ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_4_3,
                ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_64_27,
                ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_256_135,
                ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_DEFAULT,
                ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_DISABLED
            };

            foreach (var candidate in candidates)
            {
                var mask = (uint)candidate;
                if ((supported & mask) == 0)
                    continue;
                if (candidate == current)
                    continue;
                return candidate;
            }

            return current;
        }

        private static bool TryBuildPixTxMatrixUpdate(ctl_pixtx_block_config_t[] blocks, out ctl_pixtx_block_config_t original, out ctl_pixtx_block_config_t updated)
        {
            original = default;
            updated = default;

            for (var i = 0; i < blocks.Length; i++)
            {
                var block = blocks[i];
                if (block.BlockType != ctl_pixtx_block_type_t.CTL_PIXTX_BLOCK_TYPE_3X3_MATRIX &&
                    block.BlockType != ctl_pixtx_block_type_t.CTL_PIXTX_BLOCK_TYPE_3X3_MATRIX_AND_OFFSETS)
                {
                    continue;
                }

                original = block;
                PrepareMatrixBlock(ref original);

                updated = original;
                var matrixValues = GetMatrixSpan(ref updated);
                if (matrixValues.Length == 0)
                    return false;

                var originalValue = matrixValues[0];
                var newValue = originalValue + 0.01;
                if (newValue.Equals(originalValue))
                    newValue = originalValue - 0.01;
                if (newValue.Equals(originalValue))
                    return false;

                matrixValues[0] = newValue;
                return true;
            }

            return false;
        }

        private static bool TryPickPixTxMatrixBlock(ctl_pixtx_block_config_t[] blocks, out ctl_pixtx_block_config_t matrixBlock)
        {
            matrixBlock = default;

            for (var i = 0; i < blocks.Length; i++)
            {
                var block = blocks[i];
                if (block.BlockType != ctl_pixtx_block_type_t.CTL_PIXTX_BLOCK_TYPE_3X3_MATRIX &&
                    block.BlockType != ctl_pixtx_block_type_t.CTL_PIXTX_BLOCK_TYPE_3X3_MATRIX_AND_OFFSETS)
                {
                    continue;
                }

                matrixBlock = block;
                return true;
            }

            return false;
        }

        private static Span<double> GetMatrixSpan(ref ctl_pixtx_block_config_t block)
        {
            return MemoryMarshal.CreateSpan(ref block.Config.MatrixConfig.Matrix.e0_0, 9);
        }

        private static void PrepareMatrixBlock(ref ctl_pixtx_block_config_t block)
        {
            block.Size = (uint)Unsafe.SizeOf<ctl_pixtx_block_config_t>();
            block.Version = 0;
            block.Config.MatrixConfig.Size = (uint)Unsafe.SizeOf<ctl_pixtx_matrix_config_t>();
            block.Config.MatrixConfig.Version = 0;
        }

        private static unsafe void SetPixTxConfig(IGCLDisplayHelper display, ctl_pixtx_block_config_t block)
        {
            PrepareMatrixBlock(ref block);

            var args = IGCLDisplayHelper.CreatePixtxPipeSetConfig();
            args.OpertaionType = ctl_pixtx_config_opertaion_type_t.CTL_PIXTX_CONFIG_OPERTAION_TYPE_SET_CUSTOM;
            args.NumBlocks = 1;
            args.pBlockConfigs = &block;

            display.PixelTransformationSetConfig(args);
        }

        private static uint PickFirstSetBit(uint value)
        {
            for (var i = 0; i < 32; i++)
            {
                var bit = 1u << i;
                if ((value & bit) != 0)
                    return bit;
            }
            return 0;
        }

        private static uint PickDifferentFlag(uint supported, uint current, uint[] candidates)
        {
            foreach (var candidate in candidates)
            {
                if ((supported & candidate) == 0)
                    continue;
                if (candidate == current)
                    continue;
                return candidate;
            }

            return 0;
        }

        private static uint PickDifferentBrightness(uint current)
        {
            if (current + BrightnessDelta <= MaxBrightnessMilliPercent)
                return current + BrightnessDelta;
            if (current >= BrightnessDelta)
                return current - BrightnessDelta;
            return current;
        }

        private static byte PickDifferentAggressiveness(byte current)
        {
            const byte delta = 10;
            if (current + delta <= 100)
                return (byte)(current + delta);
            if (current >= delta)
                return (byte)(current - delta);
            return current;
        }
    }
}
