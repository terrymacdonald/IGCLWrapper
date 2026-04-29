using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    /// <summary>
    /// Display helper facade for IGCL display handles.
    /// </summary>
    public sealed class IGCLDisplayHelper : IDisposable
    {
        private readonly object _lock = new();
        private ctl_display_properties_t? _properties;
        private bool _disposed;
        internal IGCLApiHelper Api { get; }
        internal IntPtr AdapterHandle { get; }
        internal IntPtr DisplayHandle { get; }

        internal IGCLDisplayHelper(IGCLApiHelper api, IntPtr adapterHandle, IntPtr displayHandle)
        {
            Api = api ?? throw new ArgumentNullException(nameof(api));
            AdapterHandle = adapterHandle;
            DisplayHandle = displayHandle;
        }

        private static unsafe ctl_display_properties_t CreateDisplayProperties() => new ctl_display_properties_t { Size = (uint)sizeof(ctl_display_properties_t), Version = 0 };
        private static unsafe ctl_mux_properties_t CreateMuxProperties() => new ctl_mux_properties_t { Size = (uint)sizeof(ctl_mux_properties_t), Version = 0 };
        private static unsafe ctl_retro_scaling_caps_t CreateRetroScalingCaps() => new ctl_retro_scaling_caps_t { Size = (uint)sizeof(ctl_retro_scaling_caps_t), Version = 0 };
        private static unsafe ctl_scaling_caps_t CreateScalingCaps() => new ctl_scaling_caps_t { Size = (uint)sizeof(ctl_scaling_caps_t), Version = 0 };
        /// <summary>
        /// Create a scaling settings struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized scaling settings struct.</returns>
        public static unsafe ctl_scaling_settings_t CreateScalingSettings() => new ctl_scaling_settings_t { Size = (uint)sizeof(ctl_scaling_settings_t), Version = 0 };
        /// <summary>
        /// Create a sharpness settings struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized sharpness settings struct.</returns>
        public static unsafe ctl_sharpness_settings_t CreateSharpnessSettings() => new ctl_sharpness_settings_t { Size = (uint)sizeof(ctl_sharpness_settings_t), Version = 0 };
        private static unsafe ctl_sharpness_caps_t CreateSharpnessCaps() => new ctl_sharpness_caps_t { Size = (uint)sizeof(ctl_sharpness_caps_t), Version = 0 };
        private static unsafe ctl_power_optimization_caps_t CreatePowerOptimizationCaps() => new ctl_power_optimization_caps_t { Size = (uint)sizeof(ctl_power_optimization_caps_t), Version = 0 };
        /// <summary>
        /// Create a power optimization settings struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized power optimization settings struct.</returns>
        public static unsafe ctl_power_optimization_settings_t CreatePowerOptimizationSettings() => new ctl_power_optimization_settings_t { Size = (uint)sizeof(ctl_power_optimization_settings_t), Version = 0 };
        private static unsafe ctl_get_brightness_t CreateGetBrightness() => new ctl_get_brightness_t { Size = (uint)sizeof(ctl_get_brightness_t), Version = 0 };
        /// <summary>
        /// Create a LACE config struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized LACE config struct.</returns>
        public static unsafe ctl_lace_config_t CreateLaceConfig() => new ctl_lace_config_t { Size = (uint)sizeof(ctl_lace_config_t), Version = 0 };
        private static unsafe ctl_intel_arc_sync_monitor_params_t CreateArcSyncMonitorParams() => new ctl_intel_arc_sync_monitor_params_t { Size = (uint)sizeof(ctl_intel_arc_sync_monitor_params_t), Version = 0 };
        /// <summary>
        /// Create an Arc Sync profile params struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized Arc Sync profile params struct.</returns>
        public static unsafe ctl_intel_arc_sync_profile_params_t CreateArcSyncProfileParams() => new ctl_intel_arc_sync_profile_params_t { Size = (uint)sizeof(ctl_intel_arc_sync_profile_params_t), Version = 0 };
        /// <summary>
        /// Create a set brightness struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized set brightness struct.</returns>
        public static unsafe ctl_set_brightness_t CreateSetBrightness() => new ctl_set_brightness_t { Size = (uint)sizeof(ctl_set_brightness_t), Version = 0 };
        /// <summary>
        /// Create a retro scaling settings struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized retro scaling settings struct.</returns>
        public static unsafe ctl_retro_scaling_settings_t CreateRetroScalingSettings() => new ctl_retro_scaling_settings_t { Size = (uint)sizeof(ctl_retro_scaling_settings_t), Version = 0 };
        /// <summary>
        /// Create a software PSR settings struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized software PSR settings struct.</returns>
        public static unsafe ctl_sw_psr_settings_t CreateSoftwarePsrSettings() => new ctl_sw_psr_settings_t { Size = (uint)sizeof(ctl_sw_psr_settings_t), Version = 0 };
        /// <summary>
        /// Create a wire format config struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized wire format config struct.</returns>
        public static unsafe ctl_get_set_wire_format_config_t CreateWireFormatConfig() => new ctl_get_set_wire_format_config_t { Size = (uint)sizeof(ctl_get_set_wire_format_config_t), Version = 0 };
        /// <summary>
        /// Create a display settings struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized display settings struct.</returns>
        public static unsafe ctl_display_settings_t CreateDisplaySettings() => new ctl_display_settings_t { Size = (uint)sizeof(ctl_display_settings_t), Version = 0 };
        /// <summary>
        /// Create an EDID management args struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized EDID management args struct.</returns>
        public static unsafe ctl_edid_management_args_t CreateEdidManagementArgs() => new ctl_edid_management_args_t { Size = (uint)sizeof(ctl_edid_management_args_t), Version = 0 };
        /// <summary>
        /// Create a panel descriptor access args struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized panel descriptor access args struct.</returns>
        public static unsafe ctl_panel_descriptor_access_args_t CreatePanelDescriptorArgs() => new ctl_panel_descriptor_access_args_t { Size = (uint)sizeof(ctl_panel_descriptor_access_args_t), Version = 0 };
        /// <summary>
        /// Create a pixel transformation pipe set config struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized pipe set config struct.</returns>
        public static unsafe ctl_pixtx_pipe_set_config_t CreatePixtxPipeSetConfig() => new ctl_pixtx_pipe_set_config_t { Size = (uint)sizeof(ctl_pixtx_pipe_set_config_t), Version = 0 };
        /// <summary>
        /// Create a pixel transformation pipe get config struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized pipe get config struct.</returns>
        public static unsafe ctl_pixtx_pipe_get_config_t CreatePixtxPipeGetConfig() => new ctl_pixtx_pipe_get_config_t { Size = (uint)sizeof(ctl_pixtx_pipe_get_config_t), Version = 0 };
        /// <summary>
        /// Create a vblank timestamp args struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized vblank timestamp args struct.</returns>
        public static unsafe ctl_vblank_ts_args_t CreateVblankTimestampArgs() => new ctl_vblank_ts_args_t { Size = (uint)sizeof(ctl_vblank_ts_args_t), Version = 0 };
        /// <summary>
        /// Create a custom mode args struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized custom mode args struct.</returns>
        public static unsafe ctl_get_set_custom_mode_args_t CreateCustomModeArgs() => new ctl_get_set_custom_mode_args_t { Size = (uint)sizeof(ctl_get_set_custom_mode_args_t), Version = 0 };
        /// <summary>
        /// Create a dynamic contrast enhancement args struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized DCE args struct.</returns>
        public static unsafe ctl_dce_args_t CreateDceArgs() => new ctl_dce_args_t { Size = (uint)sizeof(ctl_dce_args_t), Version = 0 };

        /// <summary>
        /// Compare display properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreDisplayPropertiesEqual(ctl_display_properties_t left, ctl_display_properties_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.Type == right.Type &&
                   left.AttachedDisplayMuxType == right.AttachedDisplayMuxType &&
                   left.ProtocolConverterOutput == right.ProtocolConverterOutput &&
                   AreRevisionEqual(left.SupportedSpec, right.SupportedSpec) &&
                   left.SupportedOutputBPCFlags == right.SupportedOutputBPCFlags &&
                   left.ProtocolConverterType == right.ProtocolConverterType &&
                   left.DisplayConfigFlags == right.DisplayConfigFlags &&
                   left.FeatureEnabledFlags == right.FeatureEnabledFlags &&
                   left.FeatureSupportedFlags == right.FeatureSupportedFlags &&
                   left.AdvancedFeatureEnabledFlags == right.AdvancedFeatureEnabledFlags &&
                   left.AdvancedFeatureSupportedFlags == right.AdvancedFeatureSupportedFlags &&
                   AreDisplayTimingEqual(left.Display_Timing_Info, right.Display_Timing_Info);
        }

        /// <summary>
        /// Compare display timing while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left timing struct.</param>
        /// <param name="right">Right timing struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreDisplayTimingEqual(ctl_display_timing_t left, ctl_display_timing_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.PixelClock == right.PixelClock &&
                   left.HActive == right.HActive &&
                   left.VActive == right.VActive &&
                   left.HTotal == right.HTotal &&
                   left.VTotal == right.VTotal &&
                   left.HBlank == right.HBlank &&
                   left.VBlank == right.VBlank &&
                   left.HSync == right.HSync &&
                   left.VSync == right.VSync &&
                   left.RefreshRate.Equals(right.RefreshRate) &&
                   left.SignalStandard == right.SignalStandard &&
                   left.VicId == right.VicId;
        }

        /// <summary>
        /// Compare adapter display encoder properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreAdapterDisplayEncoderPropertiesEqual(ctl_adapter_display_encoder_properties_t left, ctl_adapter_display_encoder_properties_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.Type == right.Type &&
                   left.IsOnBoardProtocolConverterOutputPresent == right.IsOnBoardProtocolConverterOutputPresent &&
                   AreRevisionEqual(left.SupportedSpec, right.SupportedSpec) &&
                   left.SupportedOutputBPCFlags == right.SupportedOutputBPCFlags &&
                   left.EncoderConfigFlags == right.EncoderConfigFlags &&
                   left.FeatureSupportedFlags == right.FeatureSupportedFlags &&
                   left.AdvancedFeatureSupportedFlags == right.AdvancedFeatureSupportedFlags;
        }

        /// <summary>
        /// Compare sharpness capabilities while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left caps struct.</param>
        /// <param name="right">Right caps struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreSharpnessCapsEqual(ctl_sharpness_caps_t left, ctl_sharpness_caps_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.SupportedFilterFlags == right.SupportedFilterFlags &&
                   left.NumFilterTypes == right.NumFilterTypes;
        }

        /// <summary>
        /// Compare sharpness filter properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left filter properties struct.</param>
        /// <param name="right">Right filter properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreSharpnessFilterPropertiesEqual(ctl_sharpness_filter_properties_t left, ctl_sharpness_filter_properties_t right)
        {
            return left.FilterType == right.FilterType &&
                   ArePropertyRangeInfoEqual(left.FilterDetails, right.FilterDetails);
        }

        /// <summary>
        /// Compare sharpness settings while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left settings struct.</param>
        /// <param name="right">Right settings struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreSharpnessSettingsEqual(ctl_sharpness_settings_t left, ctl_sharpness_settings_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.Enable == right.Enable &&
                   left.FilterType == right.FilterType &&
                   left.Intensity.Equals(right.Intensity);
        }

        /// <summary>
        /// Compare I2C access args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreI2CAccessArgsEqual(ctl_i2c_access_args_t left, ctl_i2c_access_args_t right)
        {
            if (left.Size != right.Size ||
                left.Version != right.Version ||
                left.DataSize != right.DataSize ||
                left.Address != right.Address ||
                left.OpType != right.OpType ||
                left.Offset != right.Offset ||
                left.Flags != right.Flags ||
                left.RAD != right.RAD)
            {
                return false;
            }

            var count = left.DataSize > 128u ? 128 : (int)left.DataSize;
            var leftSpan = MemoryMarshal.CreateReadOnlySpan(ref left.Data.e0, 128);
            var rightSpan = MemoryMarshal.CreateReadOnlySpan(ref right.Data.e0, 128);
            return leftSpan.Slice(0, count).SequenceEqual(rightSpan.Slice(0, count));
        }

        /// <summary>
        /// Compare I2C pin-pair access args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreI2CAccessPinPairArgsEqual(ctl_i2c_access_pinpair_args_t left, ctl_i2c_access_pinpair_args_t right)
        {
            if (left.Size != right.Size ||
                left.Version != right.Version ||
                left.DataSize != right.DataSize ||
                left.Address != right.Address ||
                left.OpType != right.OpType ||
                left.Offset != right.Offset ||
                left.Flags != right.Flags)
            {
                return false;
            }

            var count = left.DataSize > 128u ? 128 : (int)left.DataSize;
            var leftSpan = MemoryMarshal.CreateReadOnlySpan(ref left.Data.e0, 128);
            var rightSpan = MemoryMarshal.CreateReadOnlySpan(ref right.Data.e0, 128);
            return leftSpan.Slice(0, count).SequenceEqual(rightSpan.Slice(0, count));
        }

        /// <summary>
        /// Compare AUX access args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreAuxAccessArgsEqual(ctl_aux_access_args_t left, ctl_aux_access_args_t right)
        {
            if (left.Size != right.Size ||
                left.Version != right.Version ||
                left.OpType != right.OpType ||
                left.Flags != right.Flags ||
                left.Address != right.Address ||
                left.RAD != right.RAD ||
                left.PortID != right.PortID ||
                left.DataSize != right.DataSize)
            {
                return false;
            }

            var count = left.DataSize > 132u ? 132 : (int)left.DataSize;
            var leftSpan = MemoryMarshal.CreateReadOnlySpan(ref left.Data.e0, 132);
            var rightSpan = MemoryMarshal.CreateReadOnlySpan(ref right.Data.e0, 132);
            return leftSpan.Slice(0, count).SequenceEqual(rightSpan.Slice(0, count));
        }

        /// <summary>
        /// Compare power optimization capabilities while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left caps struct.</param>
        /// <param name="right">Right caps struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool ArePowerOptimizationCapsEqual(ctl_power_optimization_caps_t left, ctl_power_optimization_caps_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.SupportedFeatures == right.SupportedFeatures;
        }

        /// <summary>
        /// Compare power optimization settings while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left settings struct.</param>
        /// <param name="right">Right settings struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool ArePowerOptimizationSettingsEqual(ctl_power_optimization_settings_t left, ctl_power_optimization_settings_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.PowerOptimizationPlan == right.PowerOptimizationPlan &&
                   left.PowerOptimizationFeature == right.PowerOptimizationFeature &&
                   left.Enable == right.Enable &&
                   left.PowerSource == right.PowerSource &&
                   ArePowerOptimizationFeatureSpecificInfoEqual(left.FeatureSpecificData, right.FeatureSpecificData);
        }

        /// <summary>
        /// Compare set brightness args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreSetBrightnessEqual(ctl_set_brightness_t left, ctl_set_brightness_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.TargetBrightness == right.TargetBrightness &&
                   left.SmoothTransitionTimeInMs == right.SmoothTransitionTimeInMs;
        }

        /// <summary>
        /// Compare get brightness args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreGetBrightnessEqual(ctl_get_brightness_t left, ctl_get_brightness_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.TargetBrightness == right.TargetBrightness &&
                   left.CurrentBrightness == right.CurrentBrightness;
        }

        /// <summary>
        /// Compare pixel transformation pipe get configs while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left config struct.</param>
        /// <param name="right">Right config struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool ArePixtxPipeGetConfigEqual(ctl_pixtx_pipe_get_config_t left, ctl_pixtx_pipe_get_config_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.QueryType == right.QueryType &&
                   ArePixtxPixelFormatEqual(left.InputPixelFormat, right.InputPixelFormat) &&
                   ArePixtxPixelFormatEqual(left.OutputPixelFormat, right.OutputPixelFormat) &&
                   left.NumBlocks == right.NumBlocks;
        }

        /// <summary>
        /// Compare pixel transformation pipe set configs while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left config struct.</param>
        /// <param name="right">Right config struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool ArePixtxPipeSetConfigEqual(ctl_pixtx_pipe_set_config_t left, ctl_pixtx_pipe_set_config_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.OpertaionType == right.OpertaionType &&
                   left.Flags == right.Flags &&
                   left.NumBlocks == right.NumBlocks;
        }

        /// <summary>
        /// Compare pixel transformation block configs while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left config struct.</param>
        /// <param name="right">Right config struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool ArePixtxBlockConfigEqual(ctl_pixtx_block_config_t left, ctl_pixtx_block_config_t right)
        {
            if (left.Size != right.Size ||
                left.Version != right.Version ||
                left.BlockId != right.BlockId ||
                left.BlockType != right.BlockType)
            {
                return false;
            }

            return ArePixtxConfigEqual(left.BlockType, left.Config, right.Config);
        }

        /// <summary>
        /// Compare panel descriptor access args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool ArePanelDescriptorAccessArgsEqual(ctl_panel_descriptor_access_args_t left, ctl_panel_descriptor_access_args_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.OpType == right.OpType &&
                   left.BlockNumber == right.BlockNumber &&
                   left.DescriptorDataSize == right.DescriptorDataSize;
        }

        /// <summary>
        /// Compare retro scaling capabilities while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left caps struct.</param>
        /// <param name="right">Right caps struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreRetroScalingCapsEqual(ctl_retro_scaling_caps_t left, ctl_retro_scaling_caps_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.SupportedRetroScaling == right.SupportedRetroScaling;
        }

        /// <summary>
        /// Compare retro scaling settings while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left settings struct.</param>
        /// <param name="right">Right settings struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreRetroScalingSettingsEqual(ctl_retro_scaling_settings_t left, ctl_retro_scaling_settings_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.Get == right.Get &&
                   left.Enable == right.Enable &&
                   left.RetroScalingType == right.RetroScalingType;
        }

        /// <summary>
        /// Compare scaling capabilities while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left caps struct.</param>
        /// <param name="right">Right caps struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreScalingCapsEqual(ctl_scaling_caps_t left, ctl_scaling_caps_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.SupportedScaling == right.SupportedScaling;
        }

        /// <summary>
        /// Compare scaling settings while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left settings struct.</param>
        /// <param name="right">Right settings struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreScalingSettingsEqual(ctl_scaling_settings_t left, ctl_scaling_settings_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.Enable == right.Enable &&
                   left.ScalingType == right.ScalingType &&
                   left.CustomScalingX == right.CustomScalingX &&
                   left.CustomScalingY == right.CustomScalingY &&
                   left.HardwareModeSet == right.HardwareModeSet &&
                   left.PreferredScalingType == right.PreferredScalingType;
        }

        /// <summary>
        /// Compare LACE config while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left config struct.</param>
        /// <param name="right">Right config struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreLaceConfigEqual(ctl_lace_config_t left, ctl_lace_config_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.Enabled == right.Enabled &&
                   left.OpTypeGet == right.OpTypeGet &&
                   left.OpTypeSet == right.OpTypeSet &&
                   left.Trigger == right.Trigger &&
                   AreLaceAggrConfigEqual(left.LaceConfig, right.LaceConfig);
        }

        /// <summary>
        /// Compare software PSR settings while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left settings struct.</param>
        /// <param name="right">Right settings struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreSoftwarePsrSettingsEqual(ctl_sw_psr_settings_t left, ctl_sw_psr_settings_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.Set == right.Set &&
                   left.Supported == right.Supported &&
                   left.Enable == right.Enable;
        }

        /// <summary>
        /// Compare Intel Arc Sync monitor params while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left params struct.</param>
        /// <param name="right">Right params struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreIntelArcSyncMonitorParamsEqual(ctl_intel_arc_sync_monitor_params_t left, ctl_intel_arc_sync_monitor_params_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.IsIntelArcSyncSupported == right.IsIntelArcSyncSupported &&
                   left.MinimumRefreshRateInHz.Equals(right.MinimumRefreshRateInHz) &&
                   left.MaximumRefreshRateInHz.Equals(right.MaximumRefreshRateInHz) &&
                   left.MaxFrameTimeIncreaseInUs == right.MaxFrameTimeIncreaseInUs &&
                   left.MaxFrameTimeDecreaseInUs == right.MaxFrameTimeDecreaseInUs;
        }

        /// <summary>
        /// Compare mux properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreMuxPropertiesEqual(ctl_mux_properties_t left, ctl_mux_properties_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.MuxId == right.MuxId &&
                   left.Count == right.Count &&
                   left.IndexOfDisplayOutputOwningMux == right.IndexOfDisplayOutputOwningMux;
        }

        /// <summary>
        /// Compare Intel Arc Sync profile params while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left params struct.</param>
        /// <param name="right">Right params struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreIntelArcSyncProfileParamsEqual(ctl_intel_arc_sync_profile_params_t left, ctl_intel_arc_sync_profile_params_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.IntelArcSyncProfile == right.IntelArcSyncProfile &&
                   left.MaxRefreshRateInHz.Equals(right.MaxRefreshRateInHz) &&
                   left.MinRefreshRateInHz.Equals(right.MinRefreshRateInHz) &&
                   left.MaxFrameTimeIncreaseInUs == right.MaxFrameTimeIncreaseInUs &&
                   left.MaxFrameTimeDecreaseInUs == right.MaxFrameTimeDecreaseInUs;
        }

        /// <summary>
        /// Compare EDID management args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreEdidManagementArgsEqual(ctl_edid_management_args_t left, ctl_edid_management_args_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.OpType == right.OpType &&
                   left.EdidType == right.EdidType &&
                   left.EdidSize == right.EdidSize &&
                   left.OutFlags == right.OutFlags;
        }

        /// <summary>
        /// Compare custom mode args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreCustomModeArgsEqual(ctl_get_set_custom_mode_args_t left, ctl_get_set_custom_mode_args_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.CustomModeOpType == right.CustomModeOpType &&
                   left.NumOfModes == right.NumOfModes;
        }

        /// <summary>
        /// Compare custom source modes while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left mode struct.</param>
        /// <param name="right">Right mode struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreCustomSrcModeEqual(ctl_custom_src_mode_t left, ctl_custom_src_mode_t right)
        {
            return left.SourceX == right.SourceX &&
                   left.SourceY == right.SourceY;
        }

        /// <summary>
        /// Compare vblank timestamp args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreVblankTimestampArgsEqual(ctl_vblank_ts_args_t left, ctl_vblank_ts_args_t right)
        {
            if (left.Size != right.Size ||
                left.Version != right.Version ||
                left.NumOfTargets != right.NumOfTargets)
            {
                return false;
            }

            // var count = Math.Min((int)left.NumOfTargets, 16);
            // var leftSpan = MemoryMarshal.CreateReadOnlySpan(ref left.VblankTS.e0, 16);
            // var rightSpan = MemoryMarshal.CreateReadOnlySpan(ref right.VblankTS.e0, 16);
            // return leftSpan.Slice(0, count).SequenceEqual(rightSpan.Slice(0, count));

            return true;
        }

        /// <summary>
        /// Compare DCE args while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left args struct.</param>
        /// <param name="right">Right args struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreDceArgsEqual(ctl_dce_args_t left, ctl_dce_args_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.Set == right.Set &&
                   left.TargetBrightnessPercent == right.TargetBrightnessPercent &&
                   left.PhaseinSpeedMultiplier.Equals(right.PhaseinSpeedMultiplier) &&
                   left.NumBins == right.NumBins &&
                   left.Enable == right.Enable &&
                   left.IsSupported == right.IsSupported;
        }

        /// <summary>
        /// Compare wire format config while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left config struct.</param>
        /// <param name="right">Right config struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreWireFormatConfigEqual(ctl_get_set_wire_format_config_t left, ctl_get_set_wire_format_config_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.Operation == right.Operation &&
                   AreSupportedWireFormatsEqual(left, right) &&
                   AreWireFormatEqual(left.WireFormat, right.WireFormat);
        }

        /// <summary>
        /// Compare display settings while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left settings struct.</param>
        /// <param name="right">Right settings struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreDisplaySettingsEqual(ctl_display_settings_t left, ctl_display_settings_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.Set == right.Set &&
                   left.SupportedFlags == right.SupportedFlags &&
                   left.ControllableFlags == right.ControllableFlags &&
                   left.ValidFlags == right.ValidFlags &&
                   left.LowLatency == right.LowLatency &&
                   left.SourceTM == right.SourceTM &&
                   left.ContentType == right.ContentType &&
                   left.QuantizationRange == right.QuantizationRange &&
                   left.SupportedPictureAR == right.SupportedPictureAR &&
                   left.PictureAR == right.PictureAR &&
                   left.AudioSettings == right.AudioSettings;
        }

        private static bool AreRevisionEqual(ctl_revision_datatype_t left, ctl_revision_datatype_t right)
        {
            return left.major_version == right.major_version &&
                   left.minor_version == right.minor_version &&
                   left.revision_version == right.revision_version;
        }

        private static bool ArePropertyRangeInfoEqual(ctl_property_range_info_t left, ctl_property_range_info_t right)
        {
            return left.min_possible_value.Equals(right.min_possible_value) &&
                   left.max_possible_value.Equals(right.max_possible_value) &&
                   left.step_size.Equals(right.step_size) &&
                   left.default_value.Equals(right.default_value);
        }

        private static bool ArePowerOptimizationFeatureSpecificInfoEqual(ctl_power_optimization_feature_specific_info_t left, ctl_power_optimization_feature_specific_info_t right)
        {
            return ArePowerOptimizationDpstEqual(left.DPSTInfo, right.DPSTInfo) &&
                   ArePowerOptimizationPsrEqual(left.PSRInfo, right.PSRInfo) &&
                   ArePowerOptimizationLrrEqual(left.LRRInfo, right.LRRInfo);
        }

        private static bool ArePowerOptimizationDpstEqual(ctl_power_optimization_dpst_t left, ctl_power_optimization_dpst_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.MinLevel == right.MinLevel &&
                   left.MaxLevel == right.MaxLevel &&
                   left.Level == right.Level &&
                   left.SupportedFeatures == right.SupportedFeatures &&
                   left.EnabledFeatures == right.EnabledFeatures;
        }

        private static bool ArePowerOptimizationPsrEqual(ctl_power_optimization_psr_t left, ctl_power_optimization_psr_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.PSRVersion == right.PSRVersion &&
                   left.FullFetchUpdate == right.FullFetchUpdate;
        }

        private static bool ArePowerOptimizationLrrEqual(ctl_power_optimization_lrr_t left, ctl_power_optimization_lrr_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.SupportedLRRTypes == right.SupportedLRRTypes &&
                   left.CurrentLRRTypes == right.CurrentLRRTypes &&
                   left.bRequirePSRDisable == right.bRequirePSRDisable &&
                   left.LowRR == right.LowRR;
        }

        private static bool AreWireFormatEqual(ctl_wire_format_t left, ctl_wire_format_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.ColorModel == right.ColorModel &&
                   left.ColorDepth == right.ColorDepth;
        }

        private static bool AreSupportedWireFormatsEqual(ctl_get_set_wire_format_config_t left, ctl_get_set_wire_format_config_t right)
        {
            var leftSpan = MemoryMarshal.CreateReadOnlySpan(ref left.SupportedWireFormat.e0, 4);
            var rightSpan = MemoryMarshal.CreateReadOnlySpan(ref right.SupportedWireFormat.e0, 4);
            for (var i = 0; i < 4; i++)
            {
                if (!AreWireFormatEqual(leftSpan[i], rightSpan[i]))
                    return false;
            }

            return true;
        }

        private static bool ArePixtxPixelFormatEqual(ctl_pixtx_pixel_format_t left, ctl_pixtx_pixel_format_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.BitsPerColor == right.BitsPerColor &&
                   left.IsFloat == right.IsFloat &&
                   left.EncodingType == right.EncodingType &&
                   left.ColorSpace == right.ColorSpace &&
                   left.ColorModel == right.ColorModel &&
                   ArePixtxColorPrimariesEqual(left.ColorPrimaries, right.ColorPrimaries) &&
                   left.MaxBrightness.Equals(right.MaxBrightness) &&
                   left.MinBrightness.Equals(right.MinBrightness);
        }

        private static bool ArePixtxColorPrimariesEqual(ctl_pixtx_color_primaries_t left, ctl_pixtx_color_primaries_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.xR.Equals(right.xR) &&
                   left.yR.Equals(right.yR) &&
                   left.xG.Equals(right.xG) &&
                   left.yG.Equals(right.yG) &&
                   left.xB.Equals(right.xB) &&
                   left.yB.Equals(right.yB) &&
                   left.xW.Equals(right.xW) &&
                   left.yW.Equals(right.yW);
        }

        private static bool ArePixtxConfigEqual(ctl_pixtx_block_type_t blockType, ctl_pixtx_config_t left, ctl_pixtx_config_t right)
        {
            return blockType switch
            {
                ctl_pixtx_block_type_t.CTL_PIXTX_BLOCK_TYPE_1D_LUT =>
                    ArePixtx1dLutConfigEqual(left.OneDLutConfig, right.OneDLutConfig),
                ctl_pixtx_block_type_t.CTL_PIXTX_BLOCK_TYPE_3D_LUT =>
                    ArePixtx3dLutConfigEqual(left.ThreeDLutConfig, right.ThreeDLutConfig),
                ctl_pixtx_block_type_t.CTL_PIXTX_BLOCK_TYPE_3X3_MATRIX =>
                    ArePixtxMatrixConfigEqual(left.MatrixConfig, right.MatrixConfig),
                ctl_pixtx_block_type_t.CTL_PIXTX_BLOCK_TYPE_3X3_MATRIX_AND_OFFSETS =>
                    ArePixtxMatrixConfigEqual(left.MatrixConfig, right.MatrixConfig),
                _ => ArePixtx1dLutConfigEqual(left.OneDLutConfig, right.OneDLutConfig) &&
                     ArePixtx3dLutConfigEqual(left.ThreeDLutConfig, right.ThreeDLutConfig) &&
                     ArePixtxMatrixConfigEqual(left.MatrixConfig, right.MatrixConfig)
            };
        }

        private static bool ArePixtx1dLutConfigEqual(ctl_pixtx_1dlut_config_t left, ctl_pixtx_1dlut_config_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.SamplingType == right.SamplingType &&
                   left.NumSamplesPerChannel == right.NumSamplesPerChannel &&
                   left.NumChannels == right.NumChannels;
        }

        private static bool ArePixtx3dLutConfigEqual(ctl_pixtx_3dlut_config_t left, ctl_pixtx_3dlut_config_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.NumSamplesPerChannel == right.NumSamplesPerChannel;
        }

        private static bool ArePixtxMatrixConfigEqual(ctl_pixtx_matrix_config_t left, ctl_pixtx_matrix_config_t right)
        {
            if (left.Size != right.Size || left.Version != right.Version)
                return false;

            var leftPre = MemoryMarshal.CreateReadOnlySpan(ref left.PreOffsets.e0, 3);
            var rightPre = MemoryMarshal.CreateReadOnlySpan(ref right.PreOffsets.e0, 3);
            if (!leftPre.SequenceEqual(rightPre))
                return false;

            var leftPost = MemoryMarshal.CreateReadOnlySpan(ref left.PostOffsets.e0, 3);
            var rightPost = MemoryMarshal.CreateReadOnlySpan(ref right.PostOffsets.e0, 3);
            if (!leftPost.SequenceEqual(rightPost))
                return false;

            var leftMatrix = MemoryMarshal.CreateReadOnlySpan(ref left.Matrix.e0_0, 9);
            var rightMatrix = MemoryMarshal.CreateReadOnlySpan(ref right.Matrix.e0_0, 9);
            return leftMatrix.SequenceEqual(rightMatrix);
        }

        private static bool AreLaceAggrConfigEqual(ctl_lace_aggr_config_t left, ctl_lace_aggr_config_t right)
        {
            return left.FixedAggressivenessLevelPercent == right.FixedAggressivenessLevelPercent &&
                   AreLaceLuxAggrMapEqual(left.AggrLevelMap, right.AggrLevelMap);
        }

        private static bool AreLaceLuxAggrMapEqual(ctl_lace_lux_aggr_map_t left, ctl_lace_lux_aggr_map_t right)
        {
            return left.MaxNumEntries == right.MaxNumEntries &&
                   left.NumEntries == right.NumEntries;
        }


        /// <summary>
        /// Get display properties for this display handle.
        /// </summary>
        /// <returns>Display properties struct.</returns>
        public unsafe ctl_display_properties_t GetPropertiesNative()
        {
            ThrowIfDisposed();
            lock (_lock)
            {
                if (_properties.HasValue)
                {
                    return _properties.Value;
                }

                var props = CreateDisplayProperties();
                var result = IGCL.ctlGetDisplayProperties((_ctl_display_output_handle_t*)DisplayHandle, &props);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    throw new IGCLException(result, "Failed to get display properties");
                }

                _properties = props;
                return props;
            }
        }

        /// <summary>
        /// Get display properties for this display handle as a DTO.
        /// </summary>
        /// <returns>Display properties DTO.</returns>
        public unsafe DisplayPropertiesDto GetProperties()
        {
            ThrowIfDisposed();
            var props = CreateDisplayProperties();
            var result = IGCL.ctlGetDisplayProperties((_ctl_display_output_handle_t*)DisplayHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
            {
                throw new IGCLException(result, "Failed to get display properties");
            }

            return DisplayPropertiesDto.FromNative(props);
        }

        /// <summary>
        /// Get display timing information using the native struct.
        /// </summary>
        /// <returns>Display timing struct.</returns>
        public ctl_display_timing_t GetTimingNative()
        {
            var props = GetPropertiesNative();
            return props.Display_Timing_Info;
        }

        /// <summary>
        /// Get display timing information as a DTO.
        /// </summary>
        /// <returns>Display timing DTO.</returns>
        public DisplayTimingDto GetTiming()
        {
            return DisplayTimingDto.FromNative(GetTimingNative());
        }

        /// <summary>
        /// Check whether the display is active.
        /// </summary>
        /// <returns>True when active; otherwise, false.</returns>
        public bool IsActive()
        {
            var timing = GetTimingNative();
            return timing.HActive > 0 && timing.VActive > 0;
        }

        /// <summary>
        /// Get the current display resolution.
        /// </summary>
        /// <returns>Tuple containing width and height.</returns>
        public (uint width, uint height) GetResolution()
        {
            var timing = GetTimingNative();
            return (timing.HActive, timing.VActive);
        }

        /// <summary>
        /// Get the display refresh rate in Hz.
        /// </summary>
        /// <returns>Refresh rate in Hz.</returns>
        public double GetRefreshRateHz()
        {
            var timing = GetTimingNative();
            return timing.RefreshRate / 1000.0;
        }

        /// <summary>
        /// Friendly name for the display handle.
        /// </summary>
        public string Name => $"Display-{DisplayHandle.ToInt64():X}";

        /// <summary>
        /// Check the driver version against the provided version info.
        /// </summary>
        /// <param name="versionInfo">Version info value.</param>
        /// <returns>IGCL result code.</returns>
        public unsafe ctl_result_t CheckDriverVersion(uint versionInfo)
        {
            ThrowIfDisposed();
            return IGCL.ctlCheckDriverVersion((_ctl_device_adapter_handle_t*)AdapterHandle, versionInfo);
        }

        /// <summary>
        /// Get adapter display encoder properties using the native struct.
        /// </summary>
        /// <returns>Adapter display encoder properties struct.</returns>
        public unsafe ctl_adapter_display_encoder_properties_t GetAdapterDisplayEncoderPropertiesNative()
        {
            ThrowIfDisposed();
            var props = new ctl_adapter_display_encoder_properties_t { Size = (uint)sizeof(ctl_adapter_display_encoder_properties_t), Version = 0 };
            var result = IGCL.ctlGetAdaperDisplayEncoderProperties((_ctl_display_output_handle_t*)DisplayHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get adapter display encoder properties");
            return props;
        }

        /// <summary>
        /// Get adapter display encoder properties as a DTO.
        /// </summary>
        /// <returns>Adapter display encoder properties DTO.</returns>
        public AdapterDisplayEncoderPropertiesDto GetAdapterDisplayEncoderProperties()
        {
            var native = GetAdapterDisplayEncoderPropertiesNative();
            return AdapterDisplayEncoderPropertiesDto.FromNative(native);
        }

        /// <summary>
        /// Get sharpness capabilities and filter properties using native structs.
        /// </summary>
        /// <returns>Tuple containing caps and filter properties array.</returns>
        public unsafe (ctl_sharpness_caps_t caps, ctl_sharpness_filter_properties_t[] filters) GetSharpnessCapsNative()
        {
            ThrowIfDisposed();
            var caps = CreateSharpnessCaps();

            // First pass: get count
            var result = IGCL.ctlGetSharpnessCaps((_ctl_display_output_handle_t*)DisplayHandle, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && caps.NumFilterTypes == 0)
                throw new IGCLException(result, "Failed to get sharpness caps");

            if (caps.NumFilterTypes == 0)
                return (caps, Array.Empty<ctl_sharpness_filter_properties_t>());

            var filters = new ctl_sharpness_filter_properties_t[caps.NumFilterTypes];
            fixed (ctl_sharpness_filter_properties_t* pFilters = filters)
            {
                caps.pFilterProperty = pFilters;
                result = IGCL.ctlGetSharpnessCaps((_ctl_display_output_handle_t*)DisplayHandle, &caps);
                caps.pFilterProperty = null;
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get sharpness caps");
            }

            return (caps, filters);
        }

        /// <summary>
        /// Get sharpness capabilities and filter properties as a DTO.
        /// </summary>
        /// <returns>Sharpness capabilities DTO.</returns>
        public SharpnessCapsDto GetSharpnessCaps()
        {
            var native = GetSharpnessCapsNative();
            return SharpnessCapsDto.FromNative(native.caps, native.filters);
        }

        /// <summary>
        /// Get current sharpness settings using the native struct.
        /// </summary>
        /// <returns>Sharpness settings struct.</returns>
        public unsafe ctl_sharpness_settings_t GetCurrentSharpnessNative()
        {
            ThrowIfDisposed();
            var settings = CreateSharpnessSettings();
            var result = IGCL.ctlGetCurrentSharpness((_ctl_display_output_handle_t*)DisplayHandle, &settings);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get current sharpness");
            return settings;
        }

        /// <summary>
        /// Get current sharpness settings as a DTO.
        /// </summary>
        /// <returns>Sharpness settings DTO.</returns>
        public SharpnessSettingsDto GetCurrentSharpness()
        {
            var native = GetCurrentSharpnessNative();
            return SharpnessSettingsDto.FromNative(native);
        }

        /// <summary>
        /// Set sharpness settings using the native struct.
        /// </summary>
        /// <param name="settings">Sharpness settings struct.</param>
        public unsafe void SetCurrentSharpnessNative(ctl_sharpness_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlSetCurrentSharpness((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set sharpness");
        }

        /// <summary>
        /// Set sharpness settings using a DTO.
        /// </summary>
        /// <param name="settings">Sharpness settings DTO.</param>
        public void SetCurrentSharpness(SharpnessSettingsDto settings)
        {
            SetCurrentSharpnessNative(settings.ToNative());
        }

        /// <summary>
        /// Perform I2C access using native arguments.
        /// </summary>
        /// <param name="args">I2C access arguments.</param>
        public unsafe void I2CAccessNative(ref ctl_i2c_access_args_t args)
        {
            ThrowIfDisposed();
            fixed (ctl_i2c_access_args_t* pArgs = &args)
            {
                var result = IGCL.ctlI2CAccess((_ctl_display_output_handle_t*)DisplayHandle, pArgs);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "I2C access failed");
            }
        }

        /// <summary>
        /// Perform I2C access using a DTO.
        /// </summary>
        /// <param name="args">I2C access arguments DTO.</param>
        /// <returns>Updated I2C access arguments DTO.</returns>
        public I2CAccessArgsDto I2CAccess(I2CAccessArgsDto args)
        {
            var native = args.ToNative();
            I2CAccessNative(ref native);
            return I2CAccessArgsDto.FromNative(native);
        }

        /// <summary>
        /// Perform I2C access on a specific pin pair using native arguments.
        /// </summary>
        /// <param name="pinPair">I2C pin pair handle.</param>
        /// <param name="args">I2C access arguments.</param>
        public unsafe void I2CAccessOnPinPairNative(IntPtr pinPair, ref ctl_i2c_access_pinpair_args_t args)
        {
            ThrowIfDisposed();
            fixed (ctl_i2c_access_pinpair_args_t* pArgs = &args)
            {
                var result = IGCL.ctlI2CAccessOnPinPair((_ctl_i2c_pin_pair_handle_t*)pinPair, pArgs);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "I2C access on pin pair failed");
            }
        }

        /// <summary>
        /// Perform I2C access on a specific pin pair using a DTO.
        /// </summary>
        /// <param name="pinPair">I2C pin pair handle.</param>
        /// <param name="args">I2C access arguments DTO.</param>
        /// <returns>Updated I2C access pin pair arguments DTO.</returns>
        public I2CAccessPinPairArgsDto I2CAccessOnPinPair(IntPtr pinPair, I2CAccessPinPairArgsDto args)
        {
            var native = args.ToNative();
            I2CAccessOnPinPairNative(pinPair, ref native);
            return I2CAccessPinPairArgsDto.FromNative(native);
        }

        /// <summary>
        /// Perform AUX channel access using native arguments.
        /// </summary>
        /// <param name="args">AUX access arguments.</param>
        public unsafe void AUXAccessNative(ref ctl_aux_access_args_t args)
        {
            ThrowIfDisposed();
            fixed (ctl_aux_access_args_t* pArgs = &args)
            {
                var result = IGCL.ctlAUXAccess((_ctl_display_output_handle_t*)DisplayHandle, pArgs);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "AUX access failed");
            }
        }

        /// <summary>
        /// Perform AUX channel access using a DTO.
        /// </summary>
        /// <param name="args">AUX access arguments DTO.</param>
        /// <returns>Updated AUX access arguments DTO.</returns>
        public AuxAccessArgsDto AUXAccess(AuxAccessArgsDto args)
        {
            var native = args.ToNative();
            AUXAccessNative(ref native);
            return AuxAccessArgsDto.FromNative(native);
        }

        /// <summary>
        /// Get power optimization capability information using the native struct.
        /// </summary>
        /// <returns>Power optimization capabilities struct.</returns>
        public unsafe ctl_power_optimization_caps_t GetPowerOptimizationCapsNative()
        {
            ThrowIfDisposed();
            var caps = CreatePowerOptimizationCaps();
            var result = IGCL.ctlGetPowerOptimizationCaps((_ctl_display_output_handle_t*)DisplayHandle, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power optimization caps");
            return caps;
        }

        /// <summary>
        /// Get power optimization capabilities as a DTO.
        /// </summary>
        /// <returns>Power optimization capabilities DTO.</returns>
        public PowerOptimizationCapsDto GetPowerOptimizationCaps()
        {
            return PowerOptimizationCapsDto.FromNative(GetPowerOptimizationCapsNative());
        }

        /// <summary>
        /// Get power optimization settings using the native struct.
        /// </summary>
        /// <param name="settings">Settings request struct.</param>
        /// <returns>Updated settings struct.</returns>
        public unsafe ctl_power_optimization_settings_t GetPowerOptimizationSettingNative(ctl_power_optimization_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            if (copy.Size == 0)
                copy.Size = (uint)sizeof(ctl_power_optimization_settings_t);
            if (copy.Version == 0)
                copy.Version = 0;
            var result = IGCL.ctlGetPowerOptimizationSetting((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power optimization settings");
            return copy;
        }

        /// <summary>
        /// Get power optimization settings using a DTO.
        /// </summary>
        /// <param name="settings">Settings request DTO.</param>
        /// <returns>Updated settings DTO.</returns>
        public PowerOptimizationSettingsDto GetPowerOptimizationSetting(PowerOptimizationSettingsDto settings)
        {
            var native = GetPowerOptimizationSettingNative(settings.ToNative());
            return PowerOptimizationSettingsDto.FromNative(native);
        }

        /// <summary>
        /// Set power optimization settings using the native struct.
        /// </summary>
        /// <param name="settings">Settings struct.</param>
        public unsafe void SetPowerOptimizationSettingNative(ctl_power_optimization_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlSetPowerOptimizationSetting((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set power optimization settings");
        }

        /// <summary>
        /// Set power optimization settings using a DTO.
        /// </summary>
        /// <param name="settings">Settings DTO.</param>
        public void SetPowerOptimizationSetting(PowerOptimizationSettingsDto settings)
        {
            ValidateSetPowerOptimizationSettingsRequest(settings);
            SetPowerOptimizationSettingNative(settings.ToNative());
        }

        /// <summary>
        /// Set display brightness using the native struct.
        /// </summary>
        /// <param name="brightness">Brightness settings struct.</param>
        public unsafe void SetBrightnessSettingNative(ctl_set_brightness_t brightness)
        {
            ThrowIfDisposed();
            var copy = brightness;
            var result = IGCL.ctlSetBrightnessSetting((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set brightness");
        }

        /// <summary>
        /// Set display brightness using a DTO.
        /// </summary>
        /// <param name="brightness">Brightness settings DTO.</param>
        public void SetBrightnessSetting(BrightnessSetDto brightness)
        {
            SetBrightnessSettingNative(brightness.ToNative());
        }

        /// <summary>
        /// Get display brightness using the native struct.
        /// </summary>
        /// <returns>Brightness settings struct.</returns>
        public unsafe ctl_get_brightness_t GetBrightnessSettingNative()
        {
            ThrowIfDisposed();
            var brightness = CreateGetBrightness();
            var result = IGCL.ctlGetBrightnessSetting((_ctl_display_output_handle_t*)DisplayHandle, &brightness);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to get brightness: {result}");
            return brightness;
        }

        /// <summary>
        /// Get display brightness as a DTO.
        /// </summary>
        /// <returns>Brightness settings DTO.</returns>
        public BrightnessGetDto GetBrightnessSetting()
        {
            return BrightnessGetDto.FromNative(GetBrightnessSettingNative());
        }

        /// <summary>
        /// Get pixel transformation configuration using native structs.
        /// </summary>
        /// <param name="args">Pipe get config arguments.</param>
        /// <returns>Tuple containing config and block array.</returns>
        public unsafe (ctl_pixtx_pipe_get_config_t config, ctl_pixtx_block_config_t[] blocks) PixelTransformationGetConfigNative(ctl_pixtx_pipe_get_config_t args)
        {
            ThrowIfDisposed();
            var config = args;

            // First pass: get NumBlocks
            var result = IGCL.ctlPixelTransformationGetConfig((_ctl_display_output_handle_t*)DisplayHandle, &config);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && config.NumBlocks == 0)
                throw new IGCLException(result, "Failed to get pixel transformation config");

            if (config.NumBlocks == 0)
                return (config, Array.Empty<ctl_pixtx_block_config_t>());

            var blocks = new ctl_pixtx_block_config_t[config.NumBlocks];
            for (int i = 0; i < blocks.Length; i++)
            {
                blocks[i].Size = (uint)sizeof(ctl_pixtx_block_config_t);
                blocks[i].Version = 0;
            }

            fixed (ctl_pixtx_block_config_t* pBlocks = blocks)
            {
                config.pBlockConfigs = pBlocks;
                result = IGCL.ctlPixelTransformationGetConfig((_ctl_display_output_handle_t*)DisplayHandle, &config);
                config.pBlockConfigs = null;
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get pixel transformation config");
            }

            return (config, blocks);
        }

        /// <summary>
        /// Get pixel transformation configuration using the provided blocks (native structs).
        /// </summary>
        /// <param name="args">Pipe get config arguments.</param>
        /// <param name="blocks">Block configs to query.</param>
        /// <returns>Tuple containing config and block array.</returns>
        public unsafe (ctl_pixtx_pipe_get_config_t config, ctl_pixtx_block_config_t[] blocks) PixelTransformationGetConfigNative(ctl_pixtx_pipe_get_config_t args, ctl_pixtx_block_config_t[] blocks)
        {
            ThrowIfDisposed();

            if (blocks == null || blocks.Length == 0)
                return PixelTransformationGetConfigNative(args);

            var config = args;
            for (var i = 0; i < blocks.Length; i++)
            {
                blocks[i].Size = (uint)sizeof(ctl_pixtx_block_config_t);
                blocks[i].Version = 0;
            }

            fixed (ctl_pixtx_block_config_t* pBlocks = blocks)
            {
                config.NumBlocks = (uint)blocks.Length;
                config.pBlockConfigs = pBlocks;
                var result = IGCL.ctlPixelTransformationGetConfig((_ctl_display_output_handle_t*)DisplayHandle, &config);
                config.pBlockConfigs = null;
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get pixel transformation config");
            }

            return (config, blocks);
        }

        /// <summary>
        /// Set pixel transformation configuration using the native struct.
        /// </summary>
        /// <param name="args">Pipe set config arguments.</param>
        public unsafe void PixelTransformationSetConfigNative(ctl_pixtx_pipe_set_config_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlPixelTransformationSetConfig((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set pixel transformation config");
        }

        /// <summary>
        /// Get pixel transformation configuration as DTOs (metadata only; LUT sample values require native methods).
        /// </summary>
        /// <param name="args">Pipe get config DTO.</param>
        /// <returns>Pixel transformation get result DTO.</returns>
        public PixelTransformationGetResultDto PixelTransformationGetConfig(PixtxPipeGetConfigDto args)
        {
            var native = PixelTransformationGetConfigNative(args.ToNative());
            return PixelTransformationGetResultDto.FromNative(native.config, native.blocks);
        }

        /// <summary>
        /// Set pixel transformation configuration using a DTO (metadata only; LUT sample values require native methods).
        /// </summary>
        /// <param name="args">Pipe set config DTO.</param>
        public void PixelTransformationSetConfig(PixtxPipeSetConfigDto args)
        {
            PixelTransformationSetConfigNative(args.ToNative());
        }

        /// <summary>
        /// Access the panel descriptor using native arguments.
        /// </summary>
        /// <param name="args">Panel descriptor access arguments.</param>
        /// <returns>Updated panel descriptor access arguments.</returns>
        public unsafe ctl_panel_descriptor_access_args_t PanelDescriptorAccessNative(ctl_panel_descriptor_access_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlPanelDescriptorAccess((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to access panel descriptor");
            return copy;
        }

        /// <summary>
        /// Access the panel descriptor using a DTO.
        /// </summary>
        /// <param name="args">Panel descriptor access arguments DTO.</param>
        /// <returns>Updated panel descriptor access arguments DTO.</returns>
        public unsafe PanelDescriptorAccessArgsDto PanelDescriptorAccess(PanelDescriptorAccessArgsDto args)
        {
            var native = args.ToNativeMetadata();
            byte[]? dataBuffer = null;

            if (native.DescriptorDataSize > 0)
                dataBuffer = new byte[native.DescriptorDataSize];
            else if (args.DescriptorData != null && args.DescriptorData.Count > 0)
            {
                dataBuffer = args.DescriptorData.ToArray();
                native.DescriptorDataSize = (uint)dataBuffer.Length;
            }

            if (dataBuffer != null)
            {
                fixed (byte* pData = dataBuffer)
                {
                    native.pDescriptorData = pData;
                    try
                    {
                        var resultWithData = PanelDescriptorAccessNative(native);
                        var readLength = (int)Math.Min(resultWithData.DescriptorDataSize, (uint)dataBuffer.Length);
                        var copy = new byte[readLength];
                        if (readLength > 0)
                            Buffer.BlockCopy(dataBuffer, 0, copy, 0, readLength);
                        return PanelDescriptorAccessArgsDto.FromNative(resultWithData, copy);
                    }
                    finally
                    {
                        native.pDescriptorData = null;
                    }
                }
            }

            var result = PanelDescriptorAccessNative(native);
            return PanelDescriptorAccessArgsDto.FromNative(result);
        }

        /// <summary>
        /// Read EDID via panel descriptor access as a single concatenated byte array.
        /// </summary>
        /// <returns>Concatenated panel descriptor bytes.</returns>
        public unsafe byte[] GetPanelEdidData()
        {
            ThrowIfDisposed();

            var sizeArgs = CreatePanelDescriptorArgs();
            sizeArgs.OpType = ctl_operation_type_t.CTL_OPERATION_TYPE_READ;
            sizeArgs.BlockNumber = 0;
            sizeArgs.DescriptorDataSize = 0;
            sizeArgs.pDescriptorData = null;

            sizeArgs = PanelDescriptorAccessNative(sizeArgs);

            if (sizeArgs.DescriptorDataSize == 0)
                return Array.Empty<byte>();

            var baseBlock = new byte[sizeArgs.DescriptorDataSize];
            fixed (byte* pBase = baseBlock)
            {
                var readArgs = CreatePanelDescriptorArgs();
                readArgs.OpType = ctl_operation_type_t.CTL_OPERATION_TYPE_READ;
                readArgs.BlockNumber = 0;
                readArgs.DescriptorDataSize = sizeArgs.DescriptorDataSize;
                readArgs.pDescriptorData = pBase;
                readArgs = PanelDescriptorAccessNative(readArgs);
            }

            byte extensionCount = 0;
            if (baseBlock.Length > 126)
                extensionCount = baseBlock[126];

            if (extensionCount == 0)
                return baseBlock;

            var blocks = new List<byte[]>(extensionCount + 1) { baseBlock };
            for (var i = 0; i < extensionCount; i++)
            {
                var extSizeArgs = CreatePanelDescriptorArgs();
                extSizeArgs.OpType = ctl_operation_type_t.CTL_OPERATION_TYPE_READ;
                extSizeArgs.BlockNumber = (uint)(i + 1);
                extSizeArgs.DescriptorDataSize = 0;
                extSizeArgs.pDescriptorData = null;

                extSizeArgs = PanelDescriptorAccessNative(extSizeArgs);

                if (extSizeArgs.DescriptorDataSize == 0)
                    continue;

                var extBlock = new byte[extSizeArgs.DescriptorDataSize];
                fixed (byte* pExt = extBlock)
                {
                    var extReadArgs = CreatePanelDescriptorArgs();
                    extReadArgs.OpType = ctl_operation_type_t.CTL_OPERATION_TYPE_READ;
                    extReadArgs.BlockNumber = (uint)(i + 1);
                    extReadArgs.DescriptorDataSize = extSizeArgs.DescriptorDataSize;
                    extReadArgs.pDescriptorData = pExt;
                    extReadArgs = PanelDescriptorAccessNative(extReadArgs);
                }

                blocks.Add(extBlock);
            }

            var totalLength = 0;
            foreach (var block in blocks)
                totalLength += block.Length;

            var result = new byte[totalLength];
            var offset = 0;
            foreach (var block in blocks)
            {
                Buffer.BlockCopy(block, 0, result, offset, block.Length);
                offset += block.Length;
            }

            return result;
        }

        /// <summary>
        /// Get supported retro scaling capabilities using the native struct.
        /// </summary>
        /// <returns>Retro scaling capability struct.</returns>
        public unsafe ctl_retro_scaling_caps_t GetSupportedRetroScalingCapabilityNative()
        {
            ThrowIfDisposed();
            var caps = CreateRetroScalingCaps();
            var result = IGCL.ctlGetSupportedRetroScalingCapability((_ctl_device_adapter_handle_t*)AdapterHandle, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get retro scaling capability");
            return caps;
        }

        /// <summary>
        /// Get supported retro scaling capabilities as a DTO.
        /// </summary>
        /// <returns>Retro scaling capabilities DTO.</returns>
        public RetroScalingCapsDto GetSupportedRetroScalingCapability()
        {
            return RetroScalingCapsDto.FromNative(GetSupportedRetroScalingCapabilityNative());
        }

        /// <summary>
        /// Call the native get/set retro scaling API using the provided struct.
        /// </summary>
        /// <param name="settings">Retro scaling settings struct.</param>
        /// <returns>Updated retro scaling settings struct.</returns>
        public unsafe ctl_retro_scaling_settings_t GetSetRetroScalingNative(ctl_retro_scaling_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            if (copy.Size == 0)
                copy.Size = (uint)sizeof(ctl_retro_scaling_settings_t);
            if (copy.Version == 0)
                copy.Version = 0;
            var result = IGCL.ctlGetSetRetroScaling((_ctl_device_adapter_handle_t*)AdapterHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set retro scaling");
            return copy;
        }

        /// <summary>
        /// Get retro scaling settings as a DTO.
        /// </summary>
        /// <returns>Retro scaling settings DTO.</returns>
        public RetroScalingSettingsDto GetRetroScalingSettings()
        {
            var request = new RetroScalingSettingsDto { Get = true };
            var native = GetSetRetroScalingNative(request.ToNative());
            return RetroScalingSettingsDto.FromNative(native);
        }

        /// <summary>
        /// Get supported retro scaling capabilities (DTO-first convenience alias).
        /// </summary>
        [System.ComponentModel.EditorBrowsable(System.ComponentModel.EditorBrowsableState.Never)]
        [Obsolete("Use GetSupportedRetroScalingCapability() instead.")]
        public RetroScalingCapsDto GetSupportedRetroScalingCapabilityDto() => GetSupportedRetroScalingCapability();

        /// <summary>
        /// Set retro scaling settings using a DTO.
        /// </summary>
        /// <param name="settings">Retro scaling settings DTO.</param>
        public void SetRetroScalingSettings(RetroScalingSettingsDto settings)
        {
            var request = settings;
            request.Get = false;
            GetSetRetroScalingNative(request.ToNative());
        }

        /// <summary>
        /// Get supported scaling capabilities using the native struct.
        /// </summary>
        /// <returns>Scaling capability struct.</returns>
        public unsafe ctl_scaling_caps_t GetSupportedScalingCapabilityNative()
        {
            ThrowIfDisposed();
            var caps = CreateScalingCaps();
            var result = IGCL.ctlGetSupportedScalingCapability((_ctl_display_output_handle_t*)DisplayHandle, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get scaling capability");
            return caps;
        }

        /// <summary>
        /// Get supported scaling capabilities as a DTO.
        /// </summary>
        /// <returns>Scaling capabilities DTO.</returns>
        public ScalingCapsDto GetSupportedScalingCapability()
        {
            return ScalingCapsDto.FromNative(GetSupportedScalingCapabilityNative());
        }

        /// <summary>
        /// Get current scaling settings using the native struct.
        /// </summary>
        /// <returns>Scaling settings struct.</returns>
        public unsafe ctl_scaling_settings_t GetCurrentScalingNative()
        {
            ThrowIfDisposed();
            var settings = CreateScalingSettings();
            var result = IGCL.ctlGetCurrentScaling((_ctl_display_output_handle_t*)DisplayHandle, &settings);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get current scaling");
            return settings;
        }

        /// <summary>
        /// Get current scaling settings as a DTO.
        /// </summary>
        /// <returns>Scaling settings DTO.</returns>
        public ScalingSettingsDto GetCurrentScaling()
        {
            var native = GetCurrentScalingNative();
            return ScalingSettingsDto.FromNative(native);
        }

        /// <summary>
        /// Set scaling settings using the native struct.
        /// </summary>
        /// <param name="settings">Scaling settings struct.</param>
        public unsafe void SetCurrentScalingNative(ctl_scaling_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlSetCurrentScaling((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set scaling");
        }

        /// <summary>
        /// Set scaling settings using a DTO.
        /// </summary>
        /// <param name="settings">Scaling settings DTO.</param>
        public void SetCurrentScaling(ScalingSettingsDto settings)
        {
            SetCurrentScalingNative(settings.ToNative());
        }

        /// <summary>
        /// Get LACE configuration using the native struct.
        /// </summary>
        /// <returns>LACE config struct.</returns>
        public unsafe ctl_lace_config_t GetLACEConfigNative()
        {
            ThrowIfDisposed();
            var config = CreateLaceConfig();
            config.OpTypeGet = (uint)ctl_get_operation_flag_t.CTL_GET_OPERATION_FLAG_CURRENT;
            var result = IGCL.ctlGetLACEConfig((_ctl_display_output_handle_t*)DisplayHandle, &config);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get LACE config");
            return config;
        }

        /// <summary>
        /// Get LACE configuration as a DTO.
        /// </summary>
        /// <returns>LACE config DTO.</returns>
        public LaceConfigDto GetLACEConfig()
        {
            var native = GetLACEConfigNative();
            return LaceConfigDto.FromNative(native);
        }

        /// <summary>
        /// Set LACE configuration using the native struct.
        /// </summary>
        /// <param name="config">LACE config struct.</param>
        public unsafe void SetLACEConfigNative(ctl_lace_config_t config)
        {
            ThrowIfDisposed();
            var copy = config;
            var result = IGCL.ctlSetLACEConfig((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set LACE config");
        }

        /// <summary>
        /// Set LACE configuration using a DTO.
        /// </summary>
        /// <param name="config">LACE config DTO.</param>
        public void SetLACEConfig(LaceConfigDto config)
        {
            SetLACEConfigNative(config.ToNative());
        }

        /// <summary>
        /// Call the software PSR API using the native struct.
        /// </summary>
        /// <param name="settings">Software PSR settings struct.</param>
        /// <returns>Updated software PSR settings struct.</returns>
        public unsafe ctl_sw_psr_settings_t SoftwarePSRNative(ctl_sw_psr_settings_t settings)
        {
            ThrowIfDisposed();
            var copy = settings;
            var result = IGCL.ctlSoftwarePSR((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set software PSR");
            return copy;
        }

        /// <summary>
        /// Call the software PSR API using a DTO.
        /// </summary>
        /// <param name="settings">Software PSR settings DTO.</param>
        /// <returns>Updated software PSR settings DTO.</returns>
        public SwPsrSettingsDto SoftwarePSR(SwPsrSettingsDto settings)
        {
            var native = SoftwarePSRNative(settings.ToNative());
            return SwPsrSettingsDto.FromNative(native);
        }

        /// <summary>
        /// Get Intel Arc Sync info for a monitor using the native struct.
        /// </summary>
        /// <returns>Monitor params struct.</returns>
        public unsafe ctl_intel_arc_sync_monitor_params_t GetIntelArcSyncInfoForMonitorNative()
        {
            ThrowIfDisposed();
            var parameters = CreateArcSyncMonitorParams();
            var result = IGCL.ctlGetIntelArcSyncInfoForMonitor((_ctl_display_output_handle_t*)DisplayHandle, &parameters);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get Intel Arc Sync info");
            return parameters;
        }

        /// <summary>
        /// Get Intel Arc Sync info for a monitor as a DTO.
        /// </summary>
        /// <returns>Monitor params DTO.</returns>
        public IntelArcSyncMonitorParamsDto GetIntelArcSyncInfoForMonitor()
        {
            var native = GetIntelArcSyncInfoForMonitorNative();
            return IntelArcSyncMonitorParamsDto.FromNative(native);
        }

        /// <summary>
        /// Enumerate mux device handles.
        /// </summary>
        /// <returns>Array of mux handles.</returns>
        public unsafe IntPtr[] EnumerateMuxDevices()
        {
            ThrowIfDisposed();
            uint count = 0;
            var result = IGCL.ctlEnumerateMuxDevices((_ctl_api_handle_t*)Api.ApiHandle, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get mux device count");
            if (count == 0)
                return Array.Empty<IntPtr>();

            var muxes = new IntPtr[count];
            fixed (IntPtr* pMuxes = muxes)
            {
                result = IGCL.ctlEnumerateMuxDevices((_ctl_api_handle_t*)Api.ApiHandle, &count, (_ctl_mux_output_handle_t**)pMuxes);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate mux devices");
            }
            return muxes;
        }

        /// <summary>
        /// Get mux properties and its display outputs using native structs.
        /// </summary>
        /// <param name="muxHandle">Mux handle.</param>
        /// <returns>Tuple containing mux properties and display output handles.</returns>
        public unsafe (ctl_mux_properties_t properties, IntPtr[] displayOutputs) GetMuxPropertiesNative(IntPtr muxHandle)
        {
            ThrowIfDisposed();
            var props = CreateMuxProperties();
            var result = IGCL.ctlGetMuxProperties((_ctl_mux_output_handle_t*)muxHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && props.Count == 0)
                throw new IGCLException(result, "Failed to get mux properties");

            var outputs = Array.Empty<IntPtr>();
            if (props.Count > 0)
            {
                outputs = new IntPtr[props.Count];
                fixed (IntPtr* pOutputs = outputs)
                {
                    props.phDisplayOutputs = (_ctl_display_output_handle_t**)pOutputs;
                    result = IGCL.ctlGetMuxProperties((_ctl_mux_output_handle_t*)muxHandle, &props);
                    props.phDisplayOutputs = null;
                    if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                        throw new IGCLException(result, "Failed to get mux properties");
                }
            }

            return (props, outputs);
        }

        /// <summary>
        /// Get mux properties and display outputs as a DTO.
        /// </summary>
        /// <param name="muxHandle">Mux handle.</param>
        /// <returns>Mux properties DTO.</returns>
        public MuxPropertiesDto GetMuxProperties(IntPtr muxHandle)
        {
            var native = GetMuxPropertiesNative(muxHandle);
            return MuxPropertiesDto.FromNative(native.properties, native.displayOutputs);
        }

        /// <summary>
        /// Switch mux to the specified inactive display output.
        /// </summary>
        /// <param name="muxHandle">Mux handle.</param>
        /// <param name="inactiveDisplayOutput">Inactive display output handle.</param>
        public unsafe void SwitchMux(IntPtr muxHandle, IntPtr inactiveDisplayOutput)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlSwitchMux((_ctl_mux_output_handle_t*)muxHandle, (_ctl_display_output_handle_t*)inactiveDisplayOutput);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to switch mux output");
        }

        /// <summary>
        /// Get Intel Arc Sync profile parameters using the native struct.
        /// </summary>
        /// <returns>Arc Sync profile params struct.</returns>
        public unsafe ctl_intel_arc_sync_profile_params_t GetIntelArcSyncProfileNative()
        {
            ThrowIfDisposed();
            var parameters = CreateArcSyncProfileParams();
            var result = IGCL.ctlGetIntelArcSyncProfile((_ctl_display_output_handle_t*)DisplayHandle, &parameters);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get Intel Arc Sync profile");
            return parameters;
        }

        /// <summary>
        /// Get Intel Arc Sync profile parameters as a DTO.
        /// </summary>
        /// <returns>Arc Sync profile parameters DTO.</returns>
        public IntelArcSyncProfileParamsDto GetIntelArcSyncProfile()
        {
            return IntelArcSyncProfileParamsDto.FromNative(GetIntelArcSyncProfileNative());
        }

        /// <summary>
        /// Set Intel Arc Sync profile parameters using the native struct.
        /// </summary>
        /// <param name="parameters">Arc Sync profile params struct.</param>
        public unsafe void SetIntelArcSyncProfileNative(ctl_intel_arc_sync_profile_params_t parameters)
        {
            ThrowIfDisposed();
            var copy = parameters;
            var result = IGCL.ctlSetIntelArcSyncProfile((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set Intel Arc Sync profile");
        }

        /// <summary>
        /// Set Intel Arc Sync profile parameters using a DTO.
        /// </summary>
        /// <param name="parameters">Arc Sync profile parameters DTO.</param>
        public void SetIntelArcSyncProfile(IntelArcSyncProfileParamsDto parameters)
        {
            SetIntelArcSyncProfileNative(parameters.ToNative());
        }

        /// <summary>
        /// Perform EDID management using native arguments.
        /// </summary>
        /// <param name="args">EDID management arguments.</param>
        /// <returns>Updated EDID management arguments.</returns>
        public unsafe ctl_edid_management_args_t EdidManagementNative(ctl_edid_management_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlEdidManagement((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to perform EDID management (op={args.OpType}, edidType={args.EdidType}, result={result})");
            return copy;
        }

        /// <summary>
        /// Perform EDID management using a DTO.
        /// </summary>
        /// <param name="args">EDID management arguments DTO.</param>
        /// <returns>Updated EDID management arguments DTO.</returns>
        public unsafe EdidManagementArgsDto EdidManagement(EdidManagementArgsDto args)
        {
            var native = args.ToNativeMetadata();
            byte[]? edidBuffer = null;

            if (native.EdidSize > 0)
                edidBuffer = new byte[native.EdidSize];
            else if (args.EdidData != null && args.EdidData.Count > 0)
            {
                edidBuffer = args.EdidData.ToArray();
                native.EdidSize = (uint)edidBuffer.Length;
            }

            if (edidBuffer != null)
            {
                fixed (byte* pEdid = edidBuffer)
                {
                    native.pEdidBuf = pEdid;
                    try
                    {
                        var resultWithData = EdidManagementNative(native);
                        var readLength = (int)Math.Min(resultWithData.EdidSize, (uint)edidBuffer.Length);
                        var copy = new byte[readLength];
                        if (readLength > 0)
                            Buffer.BlockCopy(edidBuffer, 0, copy, 0, readLength);
                        return EdidManagementArgsDto.FromNative(resultWithData, copy);
                    }
                    finally
                    {
                        native.pEdidBuf = null;
                    }
                }
            }

            var result = EdidManagementNative(native);
            return EdidManagementArgsDto.FromNative(result);
        }

        /// <summary>
        /// Read EDID bytes via the EDID management API.
        /// </summary>
        /// <param name="edidType">EDID type to read.</param>
        /// <returns>EDID bytes.</returns>
        public byte[] GetEdidManagement(ctl_edid_type_t edidType = ctl_edid_type_t.CTL_EDID_TYPE_CURRENT)
        {
            var result = GetEdidManagementWithFlags(edidType);
            return result.edid;
        }

        /// <summary>
        /// Read EDID bytes via the EDID management API and return output flags.
        /// </summary>
        /// <param name="edidType">EDID type to read.</param>
        /// <returns>Tuple containing EDID bytes and output flags.</returns>
        public unsafe (byte[] edid, uint outFlags) GetEdidManagementWithFlags(ctl_edid_type_t edidType = ctl_edid_type_t.CTL_EDID_TYPE_CURRENT)
        {
            ThrowIfDisposed();

            var args = CreateEdidManagementArgs();
            args.OpType = ctl_edid_management_optype_t.CTL_EDID_MANAGEMENT_OPTYPE_READ_EDID;
            args.EdidType = edidType;
            args.EdidSize = 0;
            args.pEdidBuf = null;

            args = EdidManagementNative(args);
            var outFlags = args.OutFlags;
            if (args.EdidSize == 0)
                return (Array.Empty<byte>(), outFlags);

            var buffer = new byte[args.EdidSize];
            for (var attempt = 0; attempt < 2; attempt++)
            {
                fixed (byte* pBuffer = buffer)
                {
                    args.EdidSize = (uint)buffer.Length;
                    args.pEdidBuf = pBuffer;
                    args = EdidManagementNative(args);
                }

                outFlags = args.OutFlags;
                if (args.EdidSize <= buffer.Length)
                {
                    if (args.EdidSize == buffer.Length)
                        return (buffer, outFlags);

                    if (args.EdidSize == 0)
                        return (Array.Empty<byte>(), outFlags);

                    var trimmed = new byte[args.EdidSize];
                    Array.Copy(buffer, trimmed, trimmed.Length);
                    return (trimmed, outFlags);
                }

                buffer = new byte[args.EdidSize];
            }

            return (buffer, outFlags);
        }

        /// <summary>
        /// Get custom display modes using the provided native arguments.
        /// </summary>
        /// <param name="args">Custom mode args.</param>
        /// <returns>Tuple containing updated args and modes.</returns>
        public unsafe (ctl_get_set_custom_mode_args_t args, ctl_custom_src_mode_t[] modes) GetCustomModesNative(ctl_get_set_custom_mode_args_t args)
        {
            ThrowIfDisposed();
            var request = args;
            if (request.Size == 0)
                request.Size = (uint)sizeof(ctl_get_set_custom_mode_args_t);
            if (request.Version == 0)
                request.Version = 0;

            // Get path: two-pass to retrieve modes
            var result = IGCL.ctlGetSetCustomMode((_ctl_display_output_handle_t*)DisplayHandle, &request);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && request.NumOfModes == 0)
                throw new IGCLException(result, "Failed to get custom modes");

            if (request.NumOfModes == 0)
                return (request, Array.Empty<ctl_custom_src_mode_t>());

            var modesOut = new ctl_custom_src_mode_t[request.NumOfModes];
            fixed (ctl_custom_src_mode_t* pModes = modesOut)
            {
                request.pCustomSrcModeList = pModes;
                result = IGCL.ctlGetSetCustomMode((_ctl_display_output_handle_t*)DisplayHandle, &request);
                request.pCustomSrcModeList = null;
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get custom modes");
            }

            return (request, modesOut);
        }

        /// <summary>
        /// Get custom display modes (native struct result).
        /// </summary>
        /// <returns>Tuple containing updated args and modes.</returns>
        public unsafe (ctl_get_set_custom_mode_args_t args, ctl_custom_src_mode_t[] modes) GetCustomModesNative()
        {
            var args = CreateCustomModeArgs();
            args.CustomModeOpType = ctl_custom_mode_operation_types_t.CTL_CUSTOM_MODE_OPERATION_TYPES_GET_CUSTOM_SOURCE_MODES;
            return GetCustomModesNative(args);
        }

        /// <summary>
        /// Get custom display modes as DTOs.
        /// </summary>
        /// <returns>Custom mode result DTO.</returns>
        public CustomModesResultDto GetCustomModes()
        {
            var native = GetCustomModesNative();
            return CustomModesResultDto.FromNative(native.args, native.modes);
        }

        /// <summary>
        /// Set custom display modes using the provided arguments and mode list.
        /// </summary>
        /// <param name="args">Custom mode args.</param>
        /// <param name="modes">Custom modes array.</param>
        public unsafe void SetCustomModes(ctl_get_set_custom_mode_args_t args, ctl_custom_src_mode_t[] modes)
        {
            ThrowIfDisposed();
            if (modes == null || modes.Length == 0)
                throw new ArgumentException("At least one mode is required", nameof(modes));

            var request = args;
            if (request.Size == 0)
                request.Size = (uint)sizeof(ctl_get_set_custom_mode_args_t);
            if (request.Version == 0)
                request.Version = 0;

            request.NumOfModes = (uint)modes.Length;
            fixed (ctl_custom_src_mode_t* pModes = modes)
            {
                request.pCustomSrcModeList = pModes;
                var setResult = IGCL.ctlGetSetCustomMode((_ctl_display_output_handle_t*)DisplayHandle, &request);
                request.pCustomSrcModeList = null;
                if (setResult != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(setResult, "Failed to set custom mode");
            }
        }

        /// <summary>
        /// Set custom display modes using DTOs.
        /// </summary>
        /// <param name="args">Custom mode args DTO.</param>
        /// <param name="modes">Custom mode DTOs.</param>
        public void SetCustomModes(CustomModeArgsDto args, IReadOnlyList<CustomSourceModeDto> modes)
        {
            if (modes == null || modes.Count == 0)
                throw new ArgumentException("At least one mode is required", nameof(modes));

            var nativeModes = new ctl_custom_src_mode_t[modes.Count];
            for (var i = 0; i < modes.Count; i++)
                nativeModes[i] = modes[i].ToNative();

            SetCustomModes(args.ToNative(), nativeModes);
        }

        /// <summary>
        /// Get vblank timestamp information using the native struct.
        /// </summary>
        /// <returns>Vblank timestamp args struct.</returns>
        public unsafe ctl_vblank_ts_args_t GetVblankTimestampNative()
        {
            ThrowIfDisposed();
            var args = CreateVblankTimestampArgs();
            args.NumOfTargets = 16; // max entries in the fixed buffer

            var result = IGCL.ctlGetVblankTimestamp((_ctl_display_output_handle_t*)DisplayHandle, &args);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get vblank timestamp");
            return args;
        }

        /// <summary>
        /// Get vblank timestamp information as a DTO.
        /// </summary>
        /// <returns>Vblank timestamp DTO.</returns>
        public VblankTimestampArgsDto GetVblankTimestamp()
        {
            return VblankTimestampArgsDto.FromNative(GetVblankTimestampNative());
        }

        /// <summary>
        /// Call the native get/set DCE API using the provided struct.
        /// </summary>
        /// <param name="args">DCE args struct.</param>
        /// <param name="histogram">Histogram buffer or null.</param>
        /// <returns>Tuple containing updated args and histogram.</returns>
        public unsafe (ctl_dce_args_t args, uint[] histogram) GetSetDynamicContrastEnhancementNative(ctl_dce_args_t args, uint[]? histogram = null)
        {
            ThrowIfDisposed();
            var request = args;

            if (request.Set != 0)
            {
                // Set path: caller provided histogram to write
                if (histogram != null && histogram.Length > 0)
                {
                    request.NumBins = (uint)histogram.Length;
                    fixed (uint* pHist = histogram)
                    {
                        request.pHistogram = pHist;
                        var setResult = IGCL.ctlGetSetDynamicContrastEnhancement((_ctl_display_output_handle_t*)DisplayHandle, &request);
                        request.pHistogram = null;
                        if (setResult != ctl_result_t.CTL_RESULT_SUCCESS)
                            throw new IGCLException(setResult, "Failed to set dynamic contrast enhancement");
                    }
                    return (request, histogram);
                }

                request.NumBins = 0;
                request.pHistogram = null;
                var setResultNoHistogram = IGCL.ctlGetSetDynamicContrastEnhancement((_ctl_display_output_handle_t*)DisplayHandle, &request);
                if (setResultNoHistogram != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(setResultNoHistogram, "Failed to set dynamic contrast enhancement");
                return (request, Array.Empty<uint>());
            }

            // Get path: first call to discover NumBins
            var result = IGCL.ctlGetSetDynamicContrastEnhancement((_ctl_display_output_handle_t*)DisplayHandle, &request);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && request.NumBins == 0)
                throw new IGCLException(result, "Failed to get dynamic contrast enhancement");

            if (request.NumBins == 0)
                return (request, Array.Empty<uint>());

            var bins = new uint[request.NumBins];
            fixed (uint* pBins = bins)
            {
                request.pHistogram = pBins;
                result = IGCL.ctlGetSetDynamicContrastEnhancement((_ctl_display_output_handle_t*)DisplayHandle, &request);
                request.pHistogram = null;
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to get dynamic contrast enhancement");
            }

            return (request, bins);
        }

        /// <summary>
        /// Get dynamic contrast enhancement settings as a DTO.
        /// </summary>
        /// <returns>Tuple containing args DTO and histogram.</returns>
        public (DceArgsDto args, uint[] histogram) GetDynamicContrastEnhancement()
        {
            var request = new DceArgsDto { Set = false };
            var result = GetSetDynamicContrastEnhancementNative(request.ToNative(), null);
            return (DceArgsDto.FromNative(result.args), result.histogram);
        }

        /// <summary>
        /// Set dynamic contrast enhancement settings using a DTO and histogram.
        /// </summary>
        /// <param name="args">DCE args DTO.</param>
        /// <param name="histogram">Histogram buffer.</param>
        public void SetDynamicContrastEnhancement(DceArgsDto args, uint[] histogram)
        {
            var request = args;
            request.Set = true;
            GetSetDynamicContrastEnhancementNative(request.ToNative(), histogram);
        }

        /// <summary>
        /// Call the native get/set wire format API using the provided struct.
        /// </summary>
        /// <param name="args">Wire format args struct.</param>
        /// <returns>Updated wire format args struct.</returns>
        public unsafe ctl_get_set_wire_format_config_t GetSetWireFormatNative(ctl_get_set_wire_format_config_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlGetSetWireFormat((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set wire format");
            return copy;
        }

        /// <summary>
        /// Get wire format settings.
        /// </summary>
        /// <returns>Wire format args struct.</returns>
        public unsafe ctl_get_set_wire_format_config_t GetWireFormatNative()
        {
            var request = new ctl_get_set_wire_format_config_t
            {
                Size = (uint)sizeof(ctl_get_set_wire_format_config_t),
                Version = 0,
                Operation = ctl_wire_format_operation_type_t.CTL_WIRE_FORMAT_OPERATION_TYPE_GET
            };
            return GetSetWireFormatNative(request);
        }

        /// <summary>
        /// Get wire format settings as a DTO.
        /// </summary>
        /// <returns>Wire format settings DTO.</returns>
        public unsafe WireFormatConfigDto GetWireFormat()
        {
            ThrowIfDisposed();
            var request = new ctl_get_set_wire_format_config_t
            {
                Size = (uint)sizeof(ctl_get_set_wire_format_config_t),
                Version = 0,
                Operation = ctl_wire_format_operation_type_t.CTL_WIRE_FORMAT_OPERATION_TYPE_GET
            };

            var result = IGCL.ctlGetSetWireFormat((_ctl_display_output_handle_t*)DisplayHandle, &request);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get wire format");

            return WireFormatConfigDto.FromNative(request);
        }

        /// <summary>
        /// Set wire format settings using a native struct.
        /// </summary>
        /// <param name="args">Wire format args struct.</param>
        public unsafe void SetWireFormatNative(ctl_get_set_wire_format_config_t args)
        {
            var request = args;
            if (request.Size == 0)
                request.Size = (uint)sizeof(ctl_get_set_wire_format_config_t);
            if (request.Version == 0)
                request.Version = 0;
            request.Operation = ctl_wire_format_operation_type_t.CTL_WIRE_FORMAT_OPERATION_TYPE_SET;
            GetSetWireFormatNative(request);
        }

        /// <summary>
        /// Set wire format settings using a DTO.
        /// </summary>
        /// <param name="args">Wire format settings DTO.</param>
        public unsafe void SetWireFormat(WireFormatConfigDto args)
        {
            ThrowIfDisposed();
            var request = args.ToNative();
            if (request.Size == 0)
                request.Size = (uint)sizeof(ctl_get_set_wire_format_config_t);
            if (request.Version == 0)
                request.Version = 0;
            request.Operation = ctl_wire_format_operation_type_t.CTL_WIRE_FORMAT_OPERATION_TYPE_SET;

            var result = IGCL.ctlGetSetWireFormat((_ctl_display_output_handle_t*)DisplayHandle, &request);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set wire format");
        }

        /// <summary>
        /// Call the native get/set display settings API using the provided struct.
        /// </summary>
        /// <param name="args">Display settings struct.</param>
        /// <returns>Updated display settings struct.</returns>
        public unsafe ctl_display_settings_t GetSetDisplaySettingsNative(ctl_display_settings_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlGetSetDisplaySettings((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get/set display settings");
            return copy;
        }

        /// <summary>
        /// Get display settings as a DTO.
        /// </summary>
        /// <returns>Display settings DTO.</returns>
        public DisplaySettingsDto GetDisplaySettings()
        {
            var request = new DisplaySettingsDto { Set = false };
            var native = GetSetDisplaySettingsNative(request.ToNative());
            return DisplaySettingsDto.FromNative(native);
        }

        /// <summary>
        /// Set display settings using a DTO.
        /// </summary>
        /// <param name="settings">Display settings DTO.</param>
        public void SetDisplaySettings(DisplaySettingsDto settings)
        {
            ValidateSetDisplaySettingsRequest(settings);
            var request = settings;
            request.Set = true;
            GetSetDisplaySettingsNative(request.ToNative());
        }

        /// <summary>
        /// Create a DTO request for display-settings get operations.
        /// </summary>
        /// <returns>Initialized get request DTO.</returns>
        public static DisplaySettingsDto CreateDisplaySettingsGetRequest()
        {
            return new DisplaySettingsDto { Set = false };
        }

        /// <summary>
        /// Create a DTO request for display-settings set operations.
        /// </summary>
        /// <param name="validFlags">Flags indicating which settings are being changed.</param>
        /// <returns>Initialized set request DTO.</returns>
        public static DisplaySettingsDto CreateDisplaySettingsSetRequest(uint validFlags)
        {
            return new DisplaySettingsDto
            {
                Set = true,
                ValidFlags = validFlags
            };
        }

        /// <summary>
        /// Create a DTO request for power-optimization get operations.
        /// </summary>
        /// <param name="featureFlags">Feature flags to query.</param>
        /// <returns>Initialized get request DTO.</returns>
        public static PowerOptimizationSettingsDto CreatePowerOptimizationSettingsGetRequest(uint featureFlags)
        {
            return new PowerOptimizationSettingsDto
            {
                PowerOptimizationFeature = featureFlags
            };
        }

        /// <summary>
        /// Create a DTO request for power-optimization set operations.
        /// </summary>
        /// <param name="featureFlags">Feature flags to set.</param>
        /// <returns>Initialized set request DTO.</returns>
        public static PowerOptimizationSettingsDto CreatePowerOptimizationSettingsSetRequest(uint featureFlags)
        {
            return new PowerOptimizationSettingsDto
            {
                PowerOptimizationFeature = featureFlags,
                Enable = true
            };
        }

        /// <summary>
        /// Validate a display-settings set request.
        /// </summary>
        /// <param name="settings">Settings DTO.</param>
        /// <exception cref="ArgumentException">Thrown when no valid setting flags are provided.</exception>
        public static void ValidateSetDisplaySettingsRequest(DisplaySettingsDto settings)
        {
            if (settings.ValidFlags == 0)
            {
                throw new ArgumentException(
                    "Display settings set request must specify at least one ValidFlags bit.",
                    nameof(settings));
            }
        }

        /// <summary>
        /// Validate a power-optimization set request.
        /// </summary>
        /// <param name="settings">Settings DTO.</param>
        /// <exception cref="ArgumentException">Thrown when no optimization feature flags are provided.</exception>
        public static void ValidateSetPowerOptimizationSettingsRequest(PowerOptimizationSettingsDto settings)
        {
            if (settings.PowerOptimizationFeature == 0)
            {
                throw new ArgumentException(
                    "Power optimization set request must specify at least one PowerOptimizationFeature flag.",
                    nameof(settings));
            }
        }


        internal void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLDisplayHelper));
        }

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCLDisplayDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    /// <summary>
    /// DTO for generic native void data.
    /// </summary>
    public struct GenericVoidDatatypeDto : IEquatable<GenericVoidDatatypeDto>
    {
        /// <summary>
        /// Native data bytes.
        /// </summary>
        public List<byte>? Data;
        /// <summary>
        /// Size of native data in bytes.
        /// </summary>
        public uint DataSize;

        public bool Equals(GenericVoidDatatypeDto other)
        {
            return DataSize == other.DataSize &&
                   AreByteListsEqual(Data, other.Data);
        }

        public override bool Equals(object? obj) => obj is GenericVoidDatatypeDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(DataSize);
            if (Data != null)
            {
                hash.Add(Data.Count);
                for (var i = 0; i < Data.Count; i++)
                    hash.Add(Data[i]);
            }
            return hash.ToHashCode();
        }

        public static unsafe GenericVoidDatatypeDto FromNative(ctl_generic_void_datatype_t native)
        {
            var values = default(List<byte>);
            if (native.pData != null && native.size > 0)
            {
                values = new List<byte>((int)native.size);
                var pData = (byte*)native.pData;
                for (var i = 0; i < native.size; i++)
                    values.Add(pData[i]);
            }

            return new GenericVoidDatatypeDto
            {
                Data = values,
                DataSize = native.size
            };
        }

        public unsafe ctl_generic_void_datatype_t ToNative()
        {
            return new ctl_generic_void_datatype_t
            {
                pData = null,
                size = Data == null ? DataSize : (uint)Data.Count
            };
        }

        private static bool AreByteListsEqual(List<byte>? left, List<byte>? right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left == null || right == null)
                return false;
            if (left.Count != right.Count)
                return false;
            for (var i = 0; i < left.Count; i++)
            {
                if (left[i] != right[i])
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// DTO for OS display encoder identifier data.
    /// </summary>
    public struct OsDisplayEncoderIdentifierDto : IEquatable<OsDisplayEncoderIdentifierDto>
    {
        /// <summary>
        /// Windows display encoder identifier.
        /// </summary>
        public uint WindowsDisplayEncoderId;
        /// <summary>
        /// Generic encoder identifier data.
        /// </summary>
        public GenericVoidDatatypeDto DisplayEncoderId;

        public bool Equals(OsDisplayEncoderIdentifierDto other)
        {
            return WindowsDisplayEncoderId == other.WindowsDisplayEncoderId &&
                   DisplayEncoderId.Equals(other.DisplayEncoderId);
        }

        public override bool Equals(object? obj) => obj is OsDisplayEncoderIdentifierDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(WindowsDisplayEncoderId);
            hash.Add(DisplayEncoderId);
            return hash.ToHashCode();
        }

        public static OsDisplayEncoderIdentifierDto FromNative(ctl_os_display_encoder_identifier_t native)
        {
            return new OsDisplayEncoderIdentifierDto
            {
                WindowsDisplayEncoderId = native.WindowsDisplayEncoderID,
                DisplayEncoderId = GenericVoidDatatypeDto.FromNative(native.DisplayEncoderID)
            };
        }

        public ctl_os_display_encoder_identifier_t ToNative()
        {
            var native = new ctl_os_display_encoder_identifier_t
            {
                WindowsDisplayEncoderID = WindowsDisplayEncoderId
            };

            if (DisplayEncoderId.Data != null && DisplayEncoderId.Data.Count > 0)
                native.DisplayEncoderID = DisplayEncoderId.ToNative();

            return native;
        }
    }

    /// <summary>
    /// DTO for revision data.
    /// </summary>
    public struct RevisionDatatypeDto : IEquatable<RevisionDatatypeDto>
    {
        /// <summary>
        /// Major version value.
        /// </summary>
        public byte MajorVersion;
        /// <summary>
        /// Minor version value.
        /// </summary>
        public byte MinorVersion;
        /// <summary>
        /// Revision version value.
        /// </summary>
        public byte RevisionVersion;

        public bool Equals(RevisionDatatypeDto other)
        {
            return MajorVersion == other.MajorVersion &&
                   MinorVersion == other.MinorVersion &&
                   RevisionVersion == other.RevisionVersion;
        }

        public override bool Equals(object? obj) => obj is RevisionDatatypeDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(MajorVersion);
            hash.Add(MinorVersion);
            hash.Add(RevisionVersion);
            return hash.ToHashCode();
        }

        public static RevisionDatatypeDto FromNative(ctl_revision_datatype_t native)
        {
            return new RevisionDatatypeDto
            {
                MajorVersion = native.major_version,
                MinorVersion = native.minor_version,
                RevisionVersion = native.revision_version
            };
        }

        public ctl_revision_datatype_t ToNative()
        {
            return new ctl_revision_datatype_t
            {
                major_version = MajorVersion,
                minor_version = MinorVersion,
                revision_version = RevisionVersion
            };
        }
    }

    /// <summary>
    /// DTO for display timing information.
    /// </summary>
    public struct DisplayTimingDto : IEquatable<DisplayTimingDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Pixel clock in Hz.
        /// </summary>
        public ulong PixelClock;
        /// <summary>
        /// Horizontal active pixels.
        /// </summary>
        public uint HActive;
        /// <summary>
        /// Vertical active pixels.
        /// </summary>
        public uint VActive;
        /// <summary>
        /// Horizontal total pixels.
        /// </summary>
        public uint HTotal;
        /// <summary>
        /// Vertical total pixels.
        /// </summary>
        public uint VTotal;
        /// <summary>
        /// Horizontal blanking pixels.
        /// </summary>
        public uint HBlank;
        /// <summary>
        /// Vertical blanking lines.
        /// </summary>
        public uint VBlank;
        /// <summary>
        /// Horizontal sync width.
        /// </summary>
        public uint HSync;
        /// <summary>
        /// Vertical sync width.
        /// </summary>
        public uint VSync;
        /// <summary>
        /// Refresh rate in Hz.
        /// </summary>
        public float RefreshRate;
        /// <summary>
        /// Signal standard type.
        /// </summary>
        public ctl_signal_standard_type_t SignalStandard;
        /// <summary>
        /// VIC identifier.
        /// </summary>
        public byte VicId;

        public bool Equals(DisplayTimingDto other)
        {
            return PixelClock == other.PixelClock &&
                   HActive == other.HActive &&
                   VActive == other.VActive &&
                   HTotal == other.HTotal &&
                   VTotal == other.VTotal &&
                   HBlank == other.HBlank &&
                   VBlank == other.VBlank &&
                   HSync == other.HSync &&
                   VSync == other.VSync &&
                   RefreshRate.Equals(other.RefreshRate) &&
                   SignalStandard == other.SignalStandard &&
                   VicId == other.VicId;
        }

        public override bool Equals(object? obj) => obj is DisplayTimingDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(PixelClock);
            hash.Add(HActive);
            hash.Add(VActive);
            hash.Add(HTotal);
            hash.Add(VTotal);
            hash.Add(HBlank);
            hash.Add(VBlank);
            hash.Add(HSync);
            hash.Add(VSync);
            hash.Add(RefreshRate);
            hash.Add(SignalStandard);
            hash.Add(VicId);
            return hash.ToHashCode();
        }

        public static DisplayTimingDto FromNative(ctl_display_timing_t native)
        {
            return new DisplayTimingDto
            {
                Size = native.Size,
                Version = native.Version,
                PixelClock = native.PixelClock,
                HActive = native.HActive,
                VActive = native.VActive,
                HTotal = native.HTotal,
                VTotal = native.VTotal,
                HBlank = native.HBlank,
                VBlank = native.VBlank,
                HSync = native.HSync,
                VSync = native.VSync,
                RefreshRate = native.RefreshRate,
                SignalStandard = native.SignalStandard,
                VicId = native.VicId
            };
        }

        public unsafe ctl_display_timing_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_display_timing_t);

            return new ctl_display_timing_t
            {
                Size = size,
                Version = Version,
                PixelClock = PixelClock,
                HActive = HActive,
                VActive = VActive,
                HTotal = HTotal,
                VTotal = VTotal,
                HBlank = HBlank,
                VBlank = VBlank,
                HSync = HSync,
                VSync = VSync,
                RefreshRate = RefreshRate,
                SignalStandard = SignalStandard,
                VicId = VicId
            };
        }
    }

    /// <summary>
    /// DTO for display properties.
    /// </summary>
    public struct DisplayPropertiesDto : IEquatable<DisplayPropertiesDto>
    {
        private const int ReservedFieldCount = 16;
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// OS display encoder handle.
        /// </summary>
        public OsDisplayEncoderIdentifierDto OsDisplayEncoderHandle;
        /// <summary>
        /// Display output type.
        /// </summary>
        public ctl_display_output_types_t Type;
        /// <summary>
        /// Attached display mux type.
        /// </summary>
        public ctl_attached_display_mux_type_t AttachedDisplayMuxType;
        /// <summary>
        /// Protocol converter output type.
        /// </summary>
        public ctl_display_output_types_t ProtocolConverterOutput;
        /// <summary>
        /// Supported specification version.
        /// </summary>
        public RevisionDatatypeDto SupportedSpec;
        /// <summary>
        /// Supported output BPC flags.
        /// </summary>
        public uint SupportedOutputBpcFlags;
        public bool Supports6Bpc
        {
            readonly get => HasFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_6BPC);
            set => SupportedOutputBpcFlags = SetFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_6BPC, value);
        }
        public bool Supports8Bpc
        {
            readonly get => HasFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_8BPC);
            set => SupportedOutputBpcFlags = SetFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_8BPC, value);
        }
        public bool Supports10Bpc
        {
            readonly get => HasFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_10BPC);
            set => SupportedOutputBpcFlags = SetFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_10BPC, value);
        }
        public bool Supports12Bpc
        {
            readonly get => HasFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_12BPC);
            set => SupportedOutputBpcFlags = SetFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_12BPC, value);
        }
        /// <summary>
        /// Protocol converter type flags.
        /// </summary>
        public uint ProtocolConverterType;
        public bool HasOnboardProtocolConverter
        {
            readonly get => HasFlag(ProtocolConverterType, (uint)ctl_protocol_converter_location_flag_t.CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_ONBOARD);
            set => ProtocolConverterType = SetFlag(ProtocolConverterType, (uint)ctl_protocol_converter_location_flag_t.CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_ONBOARD, value);
        }
        public bool HasExternalProtocolConverter
        {
            readonly get => HasFlag(ProtocolConverterType, (uint)ctl_protocol_converter_location_flag_t.CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_EXTERNAL);
            set => ProtocolConverterType = SetFlag(ProtocolConverterType, (uint)ctl_protocol_converter_location_flag_t.CTL_PROTOCOL_CONVERTER_LOCATION_FLAG_EXTERNAL, value);
        }
        /// <summary>
        /// Display configuration flags.
        /// </summary>
        public uint DisplayConfigFlags;
        public bool IsDisplayActive
        {
            readonly get => HasFlag(DisplayConfigFlags, (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ACTIVE);
            set => DisplayConfigFlags = SetFlag(DisplayConfigFlags, (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ACTIVE, value);
        }
        public bool IsDisplayAttached
        {
            readonly get => HasFlag(DisplayConfigFlags, (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ATTACHED);
            set => DisplayConfigFlags = SetFlag(DisplayConfigFlags, (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DISPLAY_ATTACHED, value);
        }
        public bool IsDongleConnectedToEncoder
        {
            readonly get => HasFlag(DisplayConfigFlags, (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_IS_DONGLE_CONNECTED_TO_ENCODER);
            set => DisplayConfigFlags = SetFlag(DisplayConfigFlags, (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_IS_DONGLE_CONNECTED_TO_ENCODER, value);
        }
        public bool IsDitheringEnabled
        {
            readonly get => HasFlag(DisplayConfigFlags, (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DITHERING_ENABLED);
            set => DisplayConfigFlags = SetFlag(DisplayConfigFlags, (uint)ctl_display_config_flag_t.CTL_DISPLAY_CONFIG_FLAG_DITHERING_ENABLED, value);
        }
        /// <summary>
        /// Feature enabled flags.
        /// </summary>
        public uint FeatureEnabledFlags;
        public bool IsHdcpEnabled
        {
            readonly get => HasFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDCP);
            set => FeatureEnabledFlags = SetFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDCP, value);
        }
        public bool IsHdAudioEnabled
        {
            readonly get => HasFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HD_AUDIO);
            set => FeatureEnabledFlags = SetFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HD_AUDIO, value);
        }
        public bool IsPsrEnabled
        {
            readonly get => HasFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_PSR);
            set => FeatureEnabledFlags = SetFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_PSR, value);
        }
        public bool IsAdaptiveSyncVrrEnabled
        {
            readonly get => HasFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_ADAPTIVESYNC_VRR);
            set => FeatureEnabledFlags = SetFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_ADAPTIVESYNC_VRR, value);
        }
        public bool IsVesaCompressionEnabled
        {
            readonly get => HasFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_COMPRESSION);
            set => FeatureEnabledFlags = SetFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_COMPRESSION, value);
        }
        public bool IsHdrEnabled
        {
            readonly get => HasFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR);
            set => FeatureEnabledFlags = SetFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR, value);
        }
        public bool IsHdmiQmsEnabled
        {
            readonly get => HasFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDMI_QMS);
            set => FeatureEnabledFlags = SetFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDMI_QMS, value);
        }
        public bool IsHdr10PlusCertifiedEnabled
        {
            readonly get => HasFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR10_PLUS_CERTIFIED);
            set => FeatureEnabledFlags = SetFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR10_PLUS_CERTIFIED, value);
        }
        public bool IsVesaHdrCertifiedEnabled
        {
            readonly get => HasFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_HDR_CERTIFIED);
            set => FeatureEnabledFlags = SetFlag(FeatureEnabledFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_HDR_CERTIFIED, value);
        }
        /// <summary>
        /// Feature supported flags.
        /// </summary>
        public uint FeatureSupportedFlags;
        public bool SupportsHdcp
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDCP);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDCP, value);
        }
        public bool SupportsHdAudio
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HD_AUDIO);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HD_AUDIO, value);
        }
        public bool SupportsPsr
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_PSR);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_PSR, value);
        }
        public bool SupportsAdaptiveSyncVrr
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_ADAPTIVESYNC_VRR);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_ADAPTIVESYNC_VRR, value);
        }
        public bool SupportsVesaCompression
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_COMPRESSION);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_COMPRESSION, value);
        }
        public bool SupportsHdr
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR, value);
        }
        public bool SupportsHdmiQms
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDMI_QMS);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDMI_QMS, value);
        }
        public bool SupportsHdr10PlusCertified
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR10_PLUS_CERTIFIED);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR10_PLUS_CERTIFIED, value);
        }
        public bool SupportsVesaHdrCertified
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_HDR_CERTIFIED);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_HDR_CERTIFIED, value);
        }
        /// <summary>
        /// Advanced feature enabled flags.
        /// </summary>
        public uint AdvancedFeatureEnabledFlags;
        public bool IsDpstEnabled
        {
            readonly get => HasFlag(AdvancedFeatureEnabledFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DPST);
            set => AdvancedFeatureEnabledFlags = SetFlag(AdvancedFeatureEnabledFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DPST, value);
        }
        public bool IsLaceEnabled
        {
            readonly get => HasFlag(AdvancedFeatureEnabledFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_LACE);
            set => AdvancedFeatureEnabledFlags = SetFlag(AdvancedFeatureEnabledFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_LACE, value);
        }
        public bool IsDrrsEnabled
        {
            readonly get => HasFlag(AdvancedFeatureEnabledFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DRRS);
            set => AdvancedFeatureEnabledFlags = SetFlag(AdvancedFeatureEnabledFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DRRS, value);
        }
        public bool IsArcAdaptiveSyncCertifiedEnabled
        {
            readonly get => HasFlag(AdvancedFeatureEnabledFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_ARC_ADAPTIVE_SYNC_CERTIFIED);
            set => AdvancedFeatureEnabledFlags = SetFlag(AdvancedFeatureEnabledFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_ARC_ADAPTIVE_SYNC_CERTIFIED, value);
        }
        /// <summary>
        /// Advanced feature supported flags.
        /// </summary>
        public uint AdvancedFeatureSupportedFlags;
        public bool SupportsDpst
        {
            readonly get => HasFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DPST);
            set => AdvancedFeatureSupportedFlags = SetFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DPST, value);
        }
        public bool SupportsLace
        {
            readonly get => HasFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_LACE);
            set => AdvancedFeatureSupportedFlags = SetFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_LACE, value);
        }
        public bool SupportsDrrs
        {
            readonly get => HasFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DRRS);
            set => AdvancedFeatureSupportedFlags = SetFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DRRS, value);
        }
        public bool SupportsArcAdaptiveSyncCertified
        {
            readonly get => HasFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_ARC_ADAPTIVE_SYNC_CERTIFIED);
            set => AdvancedFeatureSupportedFlags = SetFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_ARC_ADAPTIVE_SYNC_CERTIFIED, value);
        }
        /// <summary>
        /// Display timing info.
        /// </summary>
        public DisplayTimingDto DisplayTimingInfo;
        /// <summary>
        /// Reserved native fields.
        /// </summary>
        public List<uint>? ReservedFields;

        /// <summary>
        /// Compare display properties while ignoring pointer-backed and reserved fields.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(DisplayPropertiesDto other)
        {
            // OsDisplayEncoderHandle contains pointer data; ReservedFields are native-only.
                 return Type == other.Type &&
                   AttachedDisplayMuxType == other.AttachedDisplayMuxType &&
                   ProtocolConverterOutput == other.ProtocolConverterOutput &&
                     SupportedSpec.Equals(other.SupportedSpec) &&
                   SupportedOutputBpcFlags == other.SupportedOutputBpcFlags &&
                   ProtocolConverterType == other.ProtocolConverterType &&
                   DisplayConfigFlags == other.DisplayConfigFlags &&
                   FeatureEnabledFlags == other.FeatureEnabledFlags &&
                   FeatureSupportedFlags == other.FeatureSupportedFlags &&
                   AdvancedFeatureEnabledFlags == other.AdvancedFeatureEnabledFlags &&
                   AdvancedFeatureSupportedFlags == other.AdvancedFeatureSupportedFlags &&
                   DisplayTimingInfo.Equals(other.DisplayTimingInfo);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is DisplayPropertiesDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Type);
            hash.Add(AttachedDisplayMuxType);
            hash.Add(ProtocolConverterOutput);
            hash.Add(SupportedSpec);
            hash.Add(SupportedOutputBpcFlags);
            hash.Add(ProtocolConverterType);
            hash.Add(DisplayConfigFlags);
            hash.Add(FeatureEnabledFlags);
            hash.Add(FeatureSupportedFlags);
            hash.Add(AdvancedFeatureEnabledFlags);
            hash.Add(AdvancedFeatureSupportedFlags);
            hash.Add(DisplayTimingInfo);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Display properties DTO.</returns>
        public static DisplayPropertiesDto FromNative(ctl_display_properties_t native)
        {
            return new DisplayPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                OsDisplayEncoderHandle = OsDisplayEncoderIdentifierDto.FromNative(native.Os_display_encoder_handle),
                Type = native.Type,
                AttachedDisplayMuxType = native.AttachedDisplayMuxType,
                ProtocolConverterOutput = native.ProtocolConverterOutput,
                SupportedSpec = RevisionDatatypeDto.FromNative(native.SupportedSpec),
                SupportedOutputBpcFlags = native.SupportedOutputBPCFlags,
                ProtocolConverterType = native.ProtocolConverterType,
                DisplayConfigFlags = native.DisplayConfigFlags,
                FeatureEnabledFlags = native.FeatureEnabledFlags,
                FeatureSupportedFlags = native.FeatureSupportedFlags,
                AdvancedFeatureEnabledFlags = native.AdvancedFeatureEnabledFlags,
                AdvancedFeatureSupportedFlags = native.AdvancedFeatureSupportedFlags,
                DisplayTimingInfo = DisplayTimingDto.FromNative(native.Display_Timing_Info),
                ReservedFields = ReadReservedFields(native.ReservedFields)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Display properties struct.</returns>
        public unsafe ctl_display_properties_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_display_properties_t);

            var native = new ctl_display_properties_t
            {
                Size = size,
                Version = Version,
                Os_display_encoder_handle = OsDisplayEncoderHandle.ToNative(),
                Type = Type,
                AttachedDisplayMuxType = AttachedDisplayMuxType,
                ProtocolConverterOutput = ProtocolConverterOutput,
                SupportedSpec = SupportedSpec.ToNative(),
                SupportedOutputBPCFlags = SupportedOutputBpcFlags,
                ProtocolConverterType = ProtocolConverterType,
                DisplayConfigFlags = DisplayConfigFlags,
                FeatureEnabledFlags = FeatureEnabledFlags,
                FeatureSupportedFlags = FeatureSupportedFlags,
                AdvancedFeatureEnabledFlags = AdvancedFeatureEnabledFlags,
                AdvancedFeatureSupportedFlags = AdvancedFeatureSupportedFlags,
                Display_Timing_Info = DisplayTimingInfo.ToNative()
            };

            WriteReservedFields(ReservedFields, ref native.ReservedFields);
            return native;
        }

        private static unsafe List<uint> ReadReservedFields(ctl_display_properties_t._ReservedFields_e__FixedBuffer buffer)
        {
            var values = new List<uint>(ReservedFieldCount);
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedFieldCount; i++)
                values.Add(pValues[i]);
            return values;
        }

        private static unsafe void WriteReservedFields(List<uint>? values, ref ctl_display_properties_t._ReservedFields_e__FixedBuffer buffer)
        {
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedFieldCount; i++)
                pValues[i] = 0;

            if (values == null || values.Count == 0)
                return;

            var count = Math.Min(values.Count, ReservedFieldCount);
            for (var i = 0; i < count; i++)
                pValues[i] = values[i];
        }

        private static bool HasFlag(uint value, uint flag) => (value & flag) != 0;

        private static uint SetFlag(uint value, uint flag, bool enabled)
        {
            return enabled ? (value | flag) : (value & ~flag);
        }
    }

    /// <summary>
    /// DTO for wire format data.
    /// </summary>
    public struct WireFormatDto : IEquatable<WireFormatDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Wire color model.
        /// </summary>
        public ctl_wire_format_color_model_t ColorModel;
        /// <summary>
        /// Wire color depth flags.
        /// </summary>
        public uint ColorDepth;

        public bool Equals(WireFormatDto other)
        {
            return ColorModel == other.ColorModel &&
                   ColorDepth == other.ColorDepth;
        }

        public override bool Equals(object? obj) => obj is WireFormatDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(ColorModel);
            hash.Add(ColorDepth);
            return hash.ToHashCode();
        }

        public static WireFormatDto FromNative(ctl_wire_format_t native)
        {
            return new WireFormatDto
            {
                Size = native.Size,
                Version = native.Version,
                ColorModel = native.ColorModel,
                ColorDepth = native.ColorDepth
            };
        }

        public unsafe ctl_wire_format_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_wire_format_t);

            return new ctl_wire_format_t
            {
                Size = size,
                Version = Version,
                ColorModel = ColorModel,
                ColorDepth = ColorDepth
            };
        }
    }

    /// <summary>
    /// DTO for wire format settings.
    /// </summary>
    public struct WireFormatConfigDto : IEquatable<WireFormatConfigDto>
    {
        private const int SupportedWireFormatCount = 4;
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Wire format operation type.
        /// </summary>
        public ctl_wire_format_operation_type_t Operation;
        /// <summary>
        /// Supported wire format values.
        /// </summary>
        public List<WireFormatDto>? SupportedWireFormat;
        /// <summary>
        /// Selected wire format.
        /// </summary>
        public WireFormatDto WireFormat;

        /// <summary>
        /// Compare wire format settings.
        /// </summary>
        /// <param name="other">Other settings instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(WireFormatConfigDto other)
        {
            return Operation == other.Operation &&
                   WireFormat.Equals(other.WireFormat) &&
                   AreSupportedWireFormatsEqual(SupportedWireFormat, other.SupportedWireFormat);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is WireFormatConfigDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Operation);
            hash.Add(WireFormat);
            if (SupportedWireFormat != null)
            {
                hash.Add(SupportedWireFormat.Count);
                for (var i = 0; i < SupportedWireFormat.Count; i++)
                    hash.Add(SupportedWireFormat[i]);
            }
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Wire format settings DTO.</returns>
        public static WireFormatConfigDto FromNative(ctl_get_set_wire_format_config_t native)
        {
            return new WireFormatConfigDto
            {
                Size = native.Size,
                Version = native.Version,
                Operation = native.Operation,
                SupportedWireFormat = ReadSupportedWireFormat(native.SupportedWireFormat),
                WireFormat = WireFormatDto.FromNative(native.WireFormat)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Wire format settings struct.</returns>
        public unsafe ctl_get_set_wire_format_config_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_get_set_wire_format_config_t);

            var native = new ctl_get_set_wire_format_config_t
            {
                Size = size,
                Version = Version,
                Operation = Operation,
                WireFormat = WireFormat.ToNative()
            };

            WriteSupportedWireFormat(SupportedWireFormat, ref native.SupportedWireFormat);
            return native;
        }

        private static unsafe List<WireFormatDto> ReadSupportedWireFormat(ctl_get_set_wire_format_config_t._SupportedWireFormat_e__FixedBuffer buffer)
        {
            var values = new List<WireFormatDto>(SupportedWireFormatCount);
            var pValues = (ctl_wire_format_t*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < SupportedWireFormatCount; i++)
                values.Add(WireFormatDto.FromNative(pValues[i]));
            return values;
        }

        private static unsafe void WriteSupportedWireFormat(List<WireFormatDto>? values, ref ctl_get_set_wire_format_config_t._SupportedWireFormat_e__FixedBuffer buffer)
        {
            var pValues = (ctl_wire_format_t*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < SupportedWireFormatCount; i++)
                pValues[i] = default;

            if (values == null || values.Count == 0)
                return;

            var count = Math.Min(values.Count, SupportedWireFormatCount);
            for (var i = 0; i < count; i++)
                pValues[i] = values[i].ToNative();
        }

        private static bool AreSupportedWireFormatsEqual(List<WireFormatDto>? left, List<WireFormatDto>? right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left == null || right == null)
                return false;
            if (left.Count != right.Count)
                return false;
            for (var i = 0; i < left.Count; i++)
            {
                if (!left[i].Equals(right[i]))
                    return false;
            }
            return true;
        }
    }

    /// <summary>
    /// DTO for adapter display encoder properties.
    /// </summary>
    public struct AdapterDisplayEncoderPropertiesDto : IEquatable<AdapterDisplayEncoderPropertiesDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// OS display encoder handle.
        /// </summary>
        public OsDisplayEncoderIdentifierDto OsDisplayEncoderHandle;
        /// <summary>
        /// Display output type.
        /// </summary>
        public ctl_display_output_types_t Type;
        /// <summary>
        /// Indicates whether an onboard protocol converter output is present.
        /// </summary>
        public bool IsOnBoardProtocolConverterOutputPresent;
        /// <summary>
        /// Supported specification revision.
        /// </summary>
        public RevisionDatatypeDto SupportedSpec;
        /// <summary>
        /// Supported output bits-per-component flags.
        /// </summary>
        public uint SupportedOutputBpcFlags;
        public bool Supports6Bpc
        {
            readonly get => HasFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_6BPC);
            set => SupportedOutputBpcFlags = SetFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_6BPC, value);
        }
        public bool Supports8Bpc
        {
            readonly get => HasFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_8BPC);
            set => SupportedOutputBpcFlags = SetFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_8BPC, value);
        }
        public bool Supports10Bpc
        {
            readonly get => HasFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_10BPC);
            set => SupportedOutputBpcFlags = SetFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_10BPC, value);
        }
        public bool Supports12Bpc
        {
            readonly get => HasFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_12BPC);
            set => SupportedOutputBpcFlags = SetFlag(SupportedOutputBpcFlags, (uint)ctl_output_bpc_flag_t.CTL_OUTPUT_BPC_FLAG_12BPC, value);
        }
        /// <summary>
        /// Encoder configuration flags.
        /// </summary>
        public uint EncoderConfigFlags;
        public bool IsInternalDisplay
        {
            readonly get => HasFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_INTERNAL_DISPLAY);
            set => EncoderConfigFlags = SetFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_INTERNAL_DISPLAY, value);
        }
        public bool IsVesaTiledDisplay
        {
            readonly get => HasFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_VESA_TILED_DISPLAY);
            set => EncoderConfigFlags = SetFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_VESA_TILED_DISPLAY, value);
        }
        public bool IsTypeCCapable
        {
            readonly get => HasFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TYPEC_CAPABLE);
            set => EncoderConfigFlags = SetFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TYPEC_CAPABLE, value);
        }
        public bool IsThunderboltCapable
        {
            readonly get => HasFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TBT_CAPABLE);
            set => EncoderConfigFlags = SetFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_TBT_CAPABLE, value);
        }
        public bool SupportsDithering
        {
            readonly get => HasFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_DITHERING_SUPPORTED);
            set => EncoderConfigFlags = SetFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_DITHERING_SUPPORTED, value);
        }
        public bool IsVirtualDisplay
        {
            readonly get => HasFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_VIRTUAL_DISPLAY);
            set => EncoderConfigFlags = SetFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_VIRTUAL_DISPLAY, value);
        }
        public bool IsHiddenDisplay
        {
            readonly get => HasFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_HIDDEN_DISPLAY);
            set => EncoderConfigFlags = SetFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_HIDDEN_DISPLAY, value);
        }
        public bool IsCollageDisplay
        {
            readonly get => HasFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COLLAGE_DISPLAY);
            set => EncoderConfigFlags = SetFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COLLAGE_DISPLAY, value);
        }
        public bool IsSplitDisplay
        {
            readonly get => HasFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_SPLIT_DISPLAY);
            set => EncoderConfigFlags = SetFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_SPLIT_DISPLAY, value);
        }
        public bool IsCompanionDisplay
        {
            readonly get => HasFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COMPANION_DISPLAY);
            set => EncoderConfigFlags = SetFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_COMPANION_DISPLAY, value);
        }
        public bool IsMultiGpuCollageDisplay
        {
            readonly get => HasFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_MGPU_COLLAGE_DISPLAY);
            set => EncoderConfigFlags = SetFlag(EncoderConfigFlags, (uint)ctl_encoder_config_flag_t.CTL_ENCODER_CONFIG_FLAG_MGPU_COLLAGE_DISPLAY, value);
        }
        /// <summary>
        /// Feature supported flags.
        /// </summary>
        public uint FeatureSupportedFlags;
        public bool SupportsHdcp
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDCP);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDCP, value);
        }
        public bool SupportsHdAudio
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HD_AUDIO);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HD_AUDIO, value);
        }
        public bool SupportsPsr
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_PSR);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_PSR, value);
        }
        public bool SupportsAdaptiveSyncVrr
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_ADAPTIVESYNC_VRR);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_ADAPTIVESYNC_VRR, value);
        }
        public bool SupportsVesaCompression
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_COMPRESSION);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_COMPRESSION, value);
        }
        public bool SupportsHdr
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR, value);
        }
        public bool SupportsHdmiQms
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDMI_QMS);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDMI_QMS, value);
        }
        public bool SupportsHdr10PlusCertified
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR10_PLUS_CERTIFIED);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_HDR10_PLUS_CERTIFIED, value);
        }
        public bool SupportsVesaHdrCertified
        {
            readonly get => HasFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_HDR_CERTIFIED);
            set => FeatureSupportedFlags = SetFlag(FeatureSupportedFlags, (uint)ctl_std_display_feature_flag_t.CTL_STD_DISPLAY_FEATURE_FLAG_VESA_HDR_CERTIFIED, value);
        }
        /// <summary>
        /// Advanced feature supported flags.
        /// </summary>
        public uint AdvancedFeatureSupportedFlags;
        public bool SupportsDpst
        {
            readonly get => HasFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DPST);
            set => AdvancedFeatureSupportedFlags = SetFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DPST, value);
        }
        public bool SupportsLace
        {
            readonly get => HasFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_LACE);
            set => AdvancedFeatureSupportedFlags = SetFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_LACE, value);
        }
        public bool SupportsDrrs
        {
            readonly get => HasFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DRRS);
            set => AdvancedFeatureSupportedFlags = SetFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_DRRS, value);
        }
        public bool SupportsArcAdaptiveSyncCertified
        {
            readonly get => HasFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_ARC_ADAPTIVE_SYNC_CERTIFIED);
            set => AdvancedFeatureSupportedFlags = SetFlag(AdvancedFeatureSupportedFlags, (uint)ctl_intel_display_feature_flag_t.CTL_INTEL_DISPLAY_FEATURE_FLAG_ARC_ADAPTIVE_SYNC_CERTIFIED, value);
        }
        /// <summary>
        /// Reserved native fields.
        /// </summary>
        public List<uint>? ReservedFields;

        /// <summary>
        /// Compare adapter display encoder properties while ignoring reserved native fields.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(AdapterDisplayEncoderPropertiesDto other)
        {
            // OsDisplayEncoderHandle contains pointer data; ReservedFields are native-only.
                 return Type == other.Type &&
                   IsOnBoardProtocolConverterOutputPresent == other.IsOnBoardProtocolConverterOutputPresent &&
                   SupportedSpec.Equals(other.SupportedSpec) &&
                   SupportedOutputBpcFlags == other.SupportedOutputBpcFlags &&
                   EncoderConfigFlags == other.EncoderConfigFlags &&
                   FeatureSupportedFlags == other.FeatureSupportedFlags &&
                   AdvancedFeatureSupportedFlags == other.AdvancedFeatureSupportedFlags;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is AdapterDisplayEncoderPropertiesDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Type);
            hash.Add(IsOnBoardProtocolConverterOutputPresent);
            hash.Add(SupportedSpec);
            hash.Add(SupportedOutputBpcFlags);
            hash.Add(EncoderConfigFlags);
            hash.Add(FeatureSupportedFlags);
            hash.Add(AdvancedFeatureSupportedFlags);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Adapter display encoder properties DTO.</returns>
        public static AdapterDisplayEncoderPropertiesDto FromNative(ctl_adapter_display_encoder_properties_t native)
        {
            return new AdapterDisplayEncoderPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                OsDisplayEncoderHandle = OsDisplayEncoderIdentifierDto.FromNative(native.Os_display_encoder_handle),
                Type = native.Type,
                IsOnBoardProtocolConverterOutputPresent = IGCLDisplayDtoBool.ToBool(native.IsOnBoardProtocolConverterOutputPresent),
                SupportedSpec = RevisionDatatypeDto.FromNative(native.SupportedSpec),
                SupportedOutputBpcFlags = native.SupportedOutputBPCFlags,
                EncoderConfigFlags = native.EncoderConfigFlags,
                FeatureSupportedFlags = native.FeatureSupportedFlags,
                AdvancedFeatureSupportedFlags = native.AdvancedFeatureSupportedFlags,
                ReservedFields = ReadReservedFields(native.ReservedFields)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Adapter display encoder properties struct.</returns>
        public unsafe ctl_adapter_display_encoder_properties_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_adapter_display_encoder_properties_t);

            var native = new ctl_adapter_display_encoder_properties_t
            {
                Size = size,
                Version = Version,
                Os_display_encoder_handle = OsDisplayEncoderHandle.ToNative(),
                Type = Type,
                IsOnBoardProtocolConverterOutputPresent = IGCLDisplayDtoBool.ToByte(IsOnBoardProtocolConverterOutputPresent),
                SupportedSpec = SupportedSpec.ToNative(),
                SupportedOutputBPCFlags = SupportedOutputBpcFlags,
                EncoderConfigFlags = EncoderConfigFlags,
                FeatureSupportedFlags = FeatureSupportedFlags,
                AdvancedFeatureSupportedFlags = AdvancedFeatureSupportedFlags
            };

            WriteReservedFields(ReservedFields, ref native.ReservedFields);
            return native;
        }

        private static unsafe List<uint> ReadReservedFields(ctl_adapter_display_encoder_properties_t._ReservedFields_e__FixedBuffer buffer)
        {
            const int reservedFieldCount = 16;
            var values = new List<uint>(reservedFieldCount);
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < reservedFieldCount; i++)
                values.Add(pValues[i]);
            return values;
        }

        private static unsafe void WriteReservedFields(List<uint>? values, ref ctl_adapter_display_encoder_properties_t._ReservedFields_e__FixedBuffer buffer)
        {
            const int reservedFieldCount = 16;
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < reservedFieldCount; i++)
                pValues[i] = 0;

            if (values == null || values.Count == 0)
                return;

            var count = Math.Min(values.Count, reservedFieldCount);
            for (var i = 0; i < count; i++)
                pValues[i] = values[i];
        }

        private static bool HasFlag(uint value, uint flag) => (value & flag) != 0;

        private static uint SetFlag(uint value, uint flag, bool enabled)
        {
            return enabled ? (value | flag) : (value & ~flag);
        }
    }

    /// <summary>
    /// DTO for dynamic contrast enhancement arguments.
    /// </summary>
    public struct DceArgsDto : IEquatable<DceArgsDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// True to set values, false to get.
        /// </summary>
        public bool Set;
        /// <summary>
        /// Target brightness percentage.
        /// </summary>
        public uint TargetBrightnessPercent;
        /// <summary>
        /// Phase-in speed multiplier.
        /// </summary>
        public double PhaseinSpeedMultiplier;
        /// <summary>
        /// Number of histogram bins.
        /// </summary>
        public uint NumBins;
        /// <summary>
        /// Enable flag.
        /// </summary>
        public bool Enable;
        /// <summary>
        /// Indicates whether the feature is supported.
        /// </summary>
        public bool IsSupported;
        /// <summary>
        /// Histogram bins.
        /// </summary>
        public List<uint>? Histogram;

        /// <summary>
        /// Compare DCE args while ignoring pointer fields.
        /// </summary>
        /// <param name="other">Other args instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(DceArgsDto other)
        {
                 return Set == other.Set &&
                   TargetBrightnessPercent == other.TargetBrightnessPercent &&
                   PhaseinSpeedMultiplier.Equals(other.PhaseinSpeedMultiplier) &&
                   NumBins == other.NumBins &&
                   Enable == other.Enable &&
                   IsSupported == other.IsSupported;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is DceArgsDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Set);
            hash.Add(TargetBrightnessPercent);
            hash.Add(PhaseinSpeedMultiplier);
            hash.Add(NumBins);
            hash.Add(Enable);
            hash.Add(IsSupported);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>DCE args DTO.</returns>
        public static unsafe DceArgsDto FromNative(ctl_dce_args_t native)
        {
            List<uint>? histogram = null;
            if (native.pHistogram != null && native.NumBins > 0)
            {
                histogram = new List<uint>((int)native.NumBins);
                for (var i = 0; i < native.NumBins; i++)
                    histogram.Add(native.pHistogram[i]);
            }

            return new DceArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                Set = IGCLDisplayDtoBool.ToBool(native.Set),
                TargetBrightnessPercent = native.TargetBrightnessPercent,
                PhaseinSpeedMultiplier = native.PhaseinSpeedMultiplier,
                NumBins = native.NumBins,
                Enable = IGCLDisplayDtoBool.ToBool(native.Enable),
                IsSupported = IGCLDisplayDtoBool.ToBool(native.IsSupported),
                Histogram = histogram
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>DCE args struct.</returns>
        public unsafe ctl_dce_args_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_dce_args_t);

            var numBins = NumBins;
            if (Histogram != null)
                numBins = (uint)Histogram.Count;

            return new ctl_dce_args_t
            {
                Size = size,
                Version = Version,
                Set = IGCLDisplayDtoBool.ToByte(Set),
                TargetBrightnessPercent = TargetBrightnessPercent,
                PhaseinSpeedMultiplier = PhaseinSpeedMultiplier,
                NumBins = numBins,
                Enable = IGCLDisplayDtoBool.ToByte(Enable),
                IsSupported = IGCLDisplayDtoBool.ToByte(IsSupported),
                // Pointer population is handled by call sites that pin managed arrays.
                pHistogram = null
            };
        }
    }

    /// <summary>
    /// DTO for display settings.
    /// </summary>
    public struct DisplaySettingsDto : IEquatable<DisplaySettingsDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// True to set values, false to get.
        /// </summary>
        public bool Set;
        /// <summary>
        /// Supported flags.
        /// </summary>
        public uint SupportedFlags;
        public bool IsLowLatencySupported
        {
            readonly get => HasFlag(SupportedFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY);
            set => SupportedFlags = SetFlag(SupportedFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY, value);
        }
        public bool IsSourceTmSupported
        {
            readonly get => HasFlag(SupportedFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_SOURCE_TM);
            set => SupportedFlags = SetFlag(SupportedFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_SOURCE_TM, value);
        }
        public bool IsContentTypeSupported
        {
            readonly get => HasFlag(SupportedFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_CONTENT_TYPE);
            set => SupportedFlags = SetFlag(SupportedFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_CONTENT_TYPE, value);
        }
        public bool IsQuantizationRangeSupported
        {
            readonly get => HasFlag(SupportedFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_QUANTIZATION_RANGE);
            set => SupportedFlags = SetFlag(SupportedFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_QUANTIZATION_RANGE, value);
        }
        public bool IsPictureArSupported
        {
            readonly get => HasFlag(SupportedFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_PICTURE_AR);
            set => SupportedFlags = SetFlag(SupportedFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_PICTURE_AR, value);
        }
        public bool IsAudioSettingsSupported
        {
            readonly get => HasFlag(SupportedFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO);
            set => SupportedFlags = SetFlag(SupportedFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO, value);
        }
        /// <summary>
        /// Controllable flags.
        /// </summary>
        public uint ControllableFlags;
        public bool IsLowLatencyControllable
        {
            readonly get => HasFlag(ControllableFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY);
            set => ControllableFlags = SetFlag(ControllableFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY, value);
        }
        public bool IsSourceTmControllable
        {
            readonly get => HasFlag(ControllableFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_SOURCE_TM);
            set => ControllableFlags = SetFlag(ControllableFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_SOURCE_TM, value);
        }
        public bool IsContentTypeControllable
        {
            readonly get => HasFlag(ControllableFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_CONTENT_TYPE);
            set => ControllableFlags = SetFlag(ControllableFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_CONTENT_TYPE, value);
        }
        public bool IsQuantizationRangeControllable
        {
            readonly get => HasFlag(ControllableFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_QUANTIZATION_RANGE);
            set => ControllableFlags = SetFlag(ControllableFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_QUANTIZATION_RANGE, value);
        }
        public bool IsPictureArControllable
        {
            readonly get => HasFlag(ControllableFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_PICTURE_AR);
            set => ControllableFlags = SetFlag(ControllableFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_PICTURE_AR, value);
        }
        public bool IsAudioSettingsControllable
        {
            readonly get => HasFlag(ControllableFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO);
            set => ControllableFlags = SetFlag(ControllableFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO, value);
        }
        /// <summary>
        /// Valid flags.
        /// </summary>
        public uint ValidFlags;
        public bool IsLowLatencyValid
        {
            readonly get => HasFlag(ValidFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY);
            set => ValidFlags = SetFlag(ValidFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_LOW_LATENCY, value);
        }
        public bool IsSourceTmValid
        {
            readonly get => HasFlag(ValidFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_SOURCE_TM);
            set => ValidFlags = SetFlag(ValidFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_SOURCE_TM, value);
        }
        public bool IsContentTypeValid
        {
            readonly get => HasFlag(ValidFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_CONTENT_TYPE);
            set => ValidFlags = SetFlag(ValidFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_CONTENT_TYPE, value);
        }
        public bool IsQuantizationRangeValid
        {
            readonly get => HasFlag(ValidFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_QUANTIZATION_RANGE);
            set => ValidFlags = SetFlag(ValidFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_QUANTIZATION_RANGE, value);
        }
        public bool IsPictureArValid
        {
            readonly get => HasFlag(ValidFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_PICTURE_AR);
            set => ValidFlags = SetFlag(ValidFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_PICTURE_AR, value);
        }
        public bool IsAudioSettingsValid
        {
            readonly get => HasFlag(ValidFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO);
            set => ValidFlags = SetFlag(ValidFlags, (uint)ctl_display_setting_flag_t.CTL_DISPLAY_SETTING_FLAG_AUDIO, value);
        }
        /// <summary>
        /// Low latency settings.
        /// </summary>
        public ctl_display_setting_low_latency_t LowLatency;
        /// <summary>
        /// Source tone mapping settings.
        /// </summary>
        public ctl_display_setting_sourcetm_t SourceTm;
        /// <summary>
        /// Content type settings.
        /// </summary>
        public ctl_display_setting_content_type_t ContentType;
        /// <summary>
        /// Quantization range settings.
        /// </summary>
        public ctl_display_setting_quantization_range_t QuantizationRange;
        /// <summary>
        /// Supported picture aspect ratio flags.
        /// </summary>
        public uint SupportedPictureAr;
        public bool SupportsPictureArDefault
        {
            readonly get => HasFlag(SupportedPictureAr, (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_DEFAULT);
            set => SupportedPictureAr = SetFlag(SupportedPictureAr, (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_DEFAULT, value);
        }
        public bool SupportsPictureArDisabled
        {
            readonly get => HasFlag(SupportedPictureAr, (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_DISABLED);
            set => SupportedPictureAr = SetFlag(SupportedPictureAr, (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_DISABLED, value);
        }
        public bool SupportsPictureAr4By3
        {
            readonly get => HasFlag(SupportedPictureAr, (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_4_3);
            set => SupportedPictureAr = SetFlag(SupportedPictureAr, (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_4_3, value);
        }
        public bool SupportsPictureAr16By9
        {
            readonly get => HasFlag(SupportedPictureAr, (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_16_9);
            set => SupportedPictureAr = SetFlag(SupportedPictureAr, (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_16_9, value);
        }
        public bool SupportsPictureAr64By27
        {
            readonly get => HasFlag(SupportedPictureAr, (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_64_27);
            set => SupportedPictureAr = SetFlag(SupportedPictureAr, (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_64_27, value);
        }
        public bool SupportsPictureAr256By135
        {
            readonly get => HasFlag(SupportedPictureAr, (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_256_135);
            set => SupportedPictureAr = SetFlag(SupportedPictureAr, (uint)ctl_display_setting_picture_ar_flag_t.CTL_DISPLAY_SETTING_PICTURE_AR_FLAG_AR_256_135, value);
        }
        /// <summary>
        /// Picture aspect ratio settings.
        /// </summary>
        public ctl_display_setting_picture_ar_flag_t PictureAr;
        /// <summary>
        /// Audio settings.
        /// </summary>
        public ctl_display_setting_audio_t AudioSettings;
        /// <summary>
        /// Reserved native fields.
        /// </summary>
        public List<uint>? Reserved;

        /// <summary>
        /// Compare display settings while ignoring reserved native fields.
        /// </summary>
        /// <param name="other">Other settings instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(DisplaySettingsDto other)
        {
            // Reserved is an inline array in the native struct and is intentionally excluded.
            return Size == other.Size &&
                   Version == other.Version &&
                   Set == other.Set &&
                   SupportedFlags == other.SupportedFlags &&
                   ControllableFlags == other.ControllableFlags &&
                   ValidFlags == other.ValidFlags &&
                   LowLatency == other.LowLatency &&
                   SourceTm == other.SourceTm &&
                   ContentType == other.ContentType &&
                   QuantizationRange == other.QuantizationRange &&
                   SupportedPictureAr == other.SupportedPictureAr &&
                   PictureAr == other.PictureAr &&
                   AudioSettings == other.AudioSettings;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is DisplaySettingsDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Set);
            hash.Add(SupportedFlags);
            hash.Add(ControllableFlags);
            hash.Add(ValidFlags);
            hash.Add(LowLatency);
            hash.Add(SourceTm);
            hash.Add(ContentType);
            hash.Add(QuantizationRange);
            hash.Add(SupportedPictureAr);
            hash.Add(PictureAr);
            hash.Add(AudioSettings);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Display settings DTO.</returns>
        public static DisplaySettingsDto FromNative(ctl_display_settings_t native)
        {
            return new DisplaySettingsDto
            {
                Size = native.Size,
                Version = native.Version,
                Set = IGCLDisplayDtoBool.ToBool(native.Set),
                SupportedFlags = native.SupportedFlags,
                ControllableFlags = native.ControllableFlags,
                ValidFlags = native.ValidFlags,
                LowLatency = native.LowLatency,
                SourceTm = native.SourceTM,
                ContentType = native.ContentType,
                QuantizationRange = native.QuantizationRange,
                SupportedPictureAr = native.SupportedPictureAR,
                PictureAr = native.PictureAR,
                AudioSettings = native.AudioSettings,
                Reserved = ReadReserved(native.Reserved)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Display settings struct.</returns>
        public unsafe ctl_display_settings_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_display_settings_t);

            var result = new ctl_display_settings_t
            {
                Size = size,
                Version = Version,
                Set = IGCLDisplayDtoBool.ToByte(Set),
                SupportedFlags = SupportedFlags,
                ControllableFlags = ControllableFlags,
                ValidFlags = ValidFlags,
                LowLatency = LowLatency,
                SourceTM = SourceTm,
                ContentType = ContentType,
                QuantizationRange = QuantizationRange,
                SupportedPictureAR = SupportedPictureAr,
                PictureAR = PictureAr,
                AudioSettings = AudioSettings
            };

            WriteReserved(Reserved, ref result.Reserved);
            return result;
        }

        private static unsafe List<uint> ReadReserved(ctl_display_settings_t._Reserved_e__FixedBuffer buffer)
        {
            const int reservedCount = 25;
            var values = new List<uint>(reservedCount);
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < reservedCount; i++)
                values.Add(pValues[i]);
            return values;
        }

        private static unsafe void WriteReserved(List<uint>? values, ref ctl_display_settings_t._Reserved_e__FixedBuffer buffer)
        {
            const int reservedCount = 25;
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < reservedCount; i++)
                pValues[i] = 0;

            if (values == null || values.Count == 0)
                return;

            var count = Math.Min(values.Count, reservedCount);
            for (var i = 0; i < count; i++)
                pValues[i] = values[i];
        }

        private static bool HasFlag(uint value, uint flag) => (value & flag) != 0;

        private static uint SetFlag(uint value, uint flag, bool enabled)
        {
            return enabled ? (value | flag) : (value & ~flag);
        }
    }

    /// <summary>
    /// DTO for Intel Arc Sync monitor parameters.
    /// </summary>
    public struct IntelArcSyncMonitorParamsDto : IEquatable<IntelArcSyncMonitorParamsDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Indicates whether Intel Arc Sync is supported.
        /// </summary>
        public bool IsIntelArcSyncSupported;
        /// <summary>
        /// Minimum refresh rate in Hz.
        /// </summary>
        public float MinimumRefreshRateInHz;
        /// <summary>
        /// Maximum refresh rate in Hz.
        /// </summary>
        public float MaximumRefreshRateInHz;
        /// <summary>
        /// Maximum frame time increase in microseconds.
        /// </summary>
        public uint MaxFrameTimeIncreaseInUs;
        /// <summary>
        /// Maximum frame time decrease in microseconds.
        /// </summary>
        public uint MaxFrameTimeDecreaseInUs;

        /// <summary>
        /// Compare Arc Sync monitor parameters.
        /// </summary>
        /// <param name="other">Other params instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(IntelArcSyncMonitorParamsDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   IsIntelArcSyncSupported == other.IsIntelArcSyncSupported &&
                   MinimumRefreshRateInHz.Equals(other.MinimumRefreshRateInHz) &&
                   MaximumRefreshRateInHz.Equals(other.MaximumRefreshRateInHz) &&
                   MaxFrameTimeIncreaseInUs == other.MaxFrameTimeIncreaseInUs &&
                   MaxFrameTimeDecreaseInUs == other.MaxFrameTimeDecreaseInUs;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is IntelArcSyncMonitorParamsDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(IsIntelArcSyncSupported);
            hash.Add(MinimumRefreshRateInHz);
            hash.Add(MaximumRefreshRateInHz);
            hash.Add(MaxFrameTimeIncreaseInUs);
            hash.Add(MaxFrameTimeDecreaseInUs);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Intel Arc Sync monitor params DTO.</returns>
        public static IntelArcSyncMonitorParamsDto FromNative(ctl_intel_arc_sync_monitor_params_t native)
        {
            return new IntelArcSyncMonitorParamsDto
            {
                Size = native.Size,
                Version = native.Version,
                IsIntelArcSyncSupported = IGCLDisplayDtoBool.ToBool(native.IsIntelArcSyncSupported),
                MinimumRefreshRateInHz = native.MinimumRefreshRateInHz,
                MaximumRefreshRateInHz = native.MaximumRefreshRateInHz,
                MaxFrameTimeIncreaseInUs = native.MaxFrameTimeIncreaseInUs,
                MaxFrameTimeDecreaseInUs = native.MaxFrameTimeDecreaseInUs
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Intel Arc Sync monitor params struct.</returns>
        public unsafe ctl_intel_arc_sync_monitor_params_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_intel_arc_sync_monitor_params_t);

            return new ctl_intel_arc_sync_monitor_params_t
            {
                Size = size,
                Version = Version,
                IsIntelArcSyncSupported = IGCLDisplayDtoBool.ToByte(IsIntelArcSyncSupported),
                MinimumRefreshRateInHz = MinimumRefreshRateInHz,
                MaximumRefreshRateInHz = MaximumRefreshRateInHz,
                MaxFrameTimeIncreaseInUs = MaxFrameTimeIncreaseInUs,
                MaxFrameTimeDecreaseInUs = MaxFrameTimeDecreaseInUs
            };
        }
    }

    /// <summary>
    /// DTO for a single LACE lux-to-aggressiveness mapping entry.
    /// </summary>
    public struct LaceLuxAggrMapEntryDto : IEquatable<LaceLuxAggrMapEntryDto>
    {
        /// <summary>
        /// Ambient lux value.
        /// </summary>
        public uint Lux;
        /// <summary>
        /// Aggressiveness value in percent.
        /// </summary>
        public byte AggressivenessPercent;

        public bool Equals(LaceLuxAggrMapEntryDto other)
        {
            return Lux == other.Lux &&
                   AggressivenessPercent == other.AggressivenessPercent;
        }

        public override bool Equals(object? obj) => obj is LaceLuxAggrMapEntryDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Lux);
            hash.Add(AggressivenessPercent);
            return hash.ToHashCode();
        }

        public static LaceLuxAggrMapEntryDto FromNative(ctl_lace_lux_aggr_map_entry_t native)
        {
            return new LaceLuxAggrMapEntryDto
            {
                Lux = native.Lux,
                AggressivenessPercent = native.AggressivenessPercent
            };
        }

        public ctl_lace_lux_aggr_map_entry_t ToNative()
        {
            return new ctl_lace_lux_aggr_map_entry_t
            {
                Lux = Lux,
                AggressivenessPercent = AggressivenessPercent
            };
        }
    }

    /// <summary>
    /// DTO for LACE lux aggressiveness map.
    /// </summary>
    public struct LaceLuxAggrMapDto : IEquatable<LaceLuxAggrMapDto>
    {
        /// <summary>
        /// Maximum supported entries.
        /// </summary>
        public uint MaxNumEntries;
        /// <summary>
        /// Number of active entries.
        /// </summary>
        public uint NumEntries;
        /// <summary>
        /// Managed lux/aggressiveness mapping table.
        /// </summary>
        public List<LaceLuxAggrMapEntryDto>? LuxToAggrMappingTable;

        public bool Equals(LaceLuxAggrMapDto other)
        {
            return MaxNumEntries == other.MaxNumEntries &&
                   NumEntries == other.NumEntries;
        }

        public override bool Equals(object? obj) => obj is LaceLuxAggrMapDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(MaxNumEntries);
            hash.Add(NumEntries);
            return hash.ToHashCode();
        }

        public static unsafe LaceLuxAggrMapDto FromNative(ctl_lace_lux_aggr_map_t native)
        {
            List<LaceLuxAggrMapEntryDto>? entries = null;
            if (native.pLuxToAggrMappingTable != null && native.NumEntries > 0)
            {
                entries = new List<LaceLuxAggrMapEntryDto>((int)native.NumEntries);
                for (var i = 0; i < native.NumEntries; i++)
                    entries.Add(LaceLuxAggrMapEntryDto.FromNative(native.pLuxToAggrMappingTable[i]));
            }

            return new LaceLuxAggrMapDto
            {
                MaxNumEntries = native.MaxNumEntries,
                NumEntries = native.NumEntries,
                LuxToAggrMappingTable = entries
            };
        }

        public ctl_lace_lux_aggr_map_t ToNative()
        {
            var numEntries = NumEntries;
            if (LuxToAggrMappingTable != null)
                numEntries = (uint)LuxToAggrMappingTable.Count;

            return new ctl_lace_lux_aggr_map_t
            {
                MaxNumEntries = MaxNumEntries,
                NumEntries = numEntries,
                // Pointer population is handled by native call paths when pinning buffers.
                pLuxToAggrMappingTable = null
            };
        }
    }

    /// <summary>
    /// DTO for LACE aggregation configuration.
    /// </summary>
    public struct LaceAggrConfigDto : IEquatable<LaceAggrConfigDto>
    {
        /// <summary>
        /// Fixed aggressiveness level percentage.
        /// </summary>
        public byte FixedAggressivenessLevelPercent;
        /// <summary>
        /// Lux-to-aggressiveness map configuration.
        /// </summary>
        public LaceLuxAggrMapDto AggrLevelMap;

        public bool Equals(LaceAggrConfigDto other)
        {
            return FixedAggressivenessLevelPercent == other.FixedAggressivenessLevelPercent &&
                   AggrLevelMap.Equals(other.AggrLevelMap);
        }

        public override bool Equals(object? obj) => obj is LaceAggrConfigDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(FixedAggressivenessLevelPercent);
            hash.Add(AggrLevelMap);
            return hash.ToHashCode();
        }

        public static LaceAggrConfigDto FromNative(ctl_lace_aggr_config_t native)
        {
            return new LaceAggrConfigDto
            {
                FixedAggressivenessLevelPercent = native.FixedAggressivenessLevelPercent,
                AggrLevelMap = LaceLuxAggrMapDto.FromNative(native.AggrLevelMap)
            };
        }

        public ctl_lace_aggr_config_t ToNative()
        {
            var native = new ctl_lace_aggr_config_t();
            if (AggrLevelMap.NumEntries > 0 || (AggrLevelMap.LuxToAggrMappingTable?.Count ?? 0) > 0)
                native.AggrLevelMap = AggrLevelMap.ToNative();
            else
                native.FixedAggressivenessLevelPercent = FixedAggressivenessLevelPercent;
            return native;
        }
    }

    /// <summary>
    /// DTO for LACE configuration.
    /// </summary>
    public struct LaceConfigDto : IEquatable<LaceConfigDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Enable flag.
        /// </summary>
        public bool Enabled;
        /// <summary>
        /// Get operation type.
        /// </summary>
        public uint OpTypeGet;
        /// <summary>
        /// Set operation type.
        /// </summary>
        public ctl_set_operation_t OpTypeSet;
        /// <summary>
        /// Trigger flags.
        /// </summary>
        public uint Trigger;
        /// <summary>
        /// Aggregation configuration.
        /// </summary>
        public LaceAggrConfigDto LaceConfig;

        /// <summary>
        /// Compare LACE configuration.
        /// </summary>
        /// <param name="other">Other config instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(LaceConfigDto other)
        {
             return Enabled == other.Enabled &&
                   OpTypeGet == other.OpTypeGet &&
                   OpTypeSet == other.OpTypeSet &&
                   Trigger == other.Trigger &&
                 LaceConfig.Equals(other.LaceConfig);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is LaceConfigDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Enabled);
            hash.Add(OpTypeGet);
            hash.Add(OpTypeSet);
            hash.Add(Trigger);
            hash.Add(LaceConfig);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>LACE config DTO.</returns>
        public static LaceConfigDto FromNative(ctl_lace_config_t native)
        {
            return new LaceConfigDto
            {
                Size = native.Size,
                Version = native.Version,
                Enabled = IGCLDisplayDtoBool.ToBool(native.Enabled),
                OpTypeGet = native.OpTypeGet,
                OpTypeSet = native.OpTypeSet,
                Trigger = native.Trigger,
                LaceConfig = LaceAggrConfigDto.FromNative(native.LaceConfig)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>LACE config struct.</returns>
        public unsafe ctl_lace_config_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_lace_config_t);

            return new ctl_lace_config_t
            {
                Size = size,
                Version = Version,
                Enabled = IGCLDisplayDtoBool.ToByte(Enabled),
                OpTypeGet = OpTypeGet,
                OpTypeSet = OpTypeSet,
                Trigger = Trigger,
                LaceConfig = LaceConfig.ToNative()
            };
        }
    }

    /// <summary>
    /// DTO for retro scaling settings.
    /// </summary>
    public struct RetroScalingSettingsDto : IEquatable<RetroScalingSettingsDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// True to get settings, false to set.
        /// </summary>
        public bool Get;
        /// <summary>
        /// Enable flag.
        /// </summary>
        public bool Enable;
        /// <summary>
        /// Retro scaling type.
        /// </summary>
        public uint RetroScalingType;

        /// <summary>
        /// Compare retro scaling settings.
        /// </summary>
        /// <param name="other">Other settings instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(RetroScalingSettingsDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Get == other.Get &&
                   Enable == other.Enable &&
                   RetroScalingType == other.RetroScalingType;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is RetroScalingSettingsDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Get);
            hash.Add(Enable);
            hash.Add(RetroScalingType);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Retro scaling settings DTO.</returns>
        public static RetroScalingSettingsDto FromNative(ctl_retro_scaling_settings_t native)
        {
            return new RetroScalingSettingsDto
            {
                Size = native.Size,
                Version = native.Version,
                Get = IGCLDisplayDtoBool.ToBool(native.Get),
                Enable = IGCLDisplayDtoBool.ToBool(native.Enable),
                RetroScalingType = native.RetroScalingType
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Retro scaling settings struct.</returns>
        public unsafe ctl_retro_scaling_settings_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_retro_scaling_settings_t);

            return new ctl_retro_scaling_settings_t
            {
                Size = size,
                Version = Version,
                Get = IGCLDisplayDtoBool.ToByte(Get),
                Enable = IGCLDisplayDtoBool.ToByte(Enable),
                RetroScalingType = RetroScalingType
            };
        }
    }

    /// <summary>
    /// DTO for scaling settings.
    /// </summary>
    public struct ScalingSettingsDto : IEquatable<ScalingSettingsDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Enable flag.
        /// </summary>
        public bool Enable;
        /// <summary>
        /// Scaling type.
        /// </summary>
        public uint ScalingType;
        /// <summary>
        /// Custom scaling X value.
        /// </summary>
        public uint CustomScalingX;
        /// <summary>
        /// Custom scaling Y value.
        /// </summary>
        public uint CustomScalingY;
        /// <summary>
        /// Hardware mode set flag.
        /// </summary>
        public bool HardwareModeSet;
        /// <summary>
        /// Preferred scaling type.
        /// </summary>
        public uint PreferredScalingType;

        /// <summary>
        /// Compare scaling settings.
        /// </summary>
        /// <param name="other">Other settings instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(ScalingSettingsDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Enable == other.Enable &&
                   ScalingType == other.ScalingType &&
                   CustomScalingX == other.CustomScalingX &&
                   CustomScalingY == other.CustomScalingY &&
                   HardwareModeSet == other.HardwareModeSet &&
                   PreferredScalingType == other.PreferredScalingType;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is ScalingSettingsDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Enable);
            hash.Add(ScalingType);
            hash.Add(CustomScalingX);
            hash.Add(CustomScalingY);
            hash.Add(HardwareModeSet);
            hash.Add(PreferredScalingType);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Scaling settings DTO.</returns>
        public static ScalingSettingsDto FromNative(ctl_scaling_settings_t native)
        {
            return new ScalingSettingsDto
            {
                Size = native.Size,
                Version = native.Version,
                Enable = IGCLDisplayDtoBool.ToBool(native.Enable),
                ScalingType = native.ScalingType,
                CustomScalingX = native.CustomScalingX,
                CustomScalingY = native.CustomScalingY,
                HardwareModeSet = IGCLDisplayDtoBool.ToBool(native.HardwareModeSet),
                PreferredScalingType = native.PreferredScalingType
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Scaling settings struct.</returns>
        public unsafe ctl_scaling_settings_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_scaling_settings_t);

            return new ctl_scaling_settings_t
            {
                Size = size,
                Version = Version,
                Enable = IGCLDisplayDtoBool.ToByte(Enable),
                ScalingType = ScalingType,
                CustomScalingX = CustomScalingX,
                CustomScalingY = CustomScalingY,
                HardwareModeSet = IGCLDisplayDtoBool.ToByte(HardwareModeSet),
                PreferredScalingType = PreferredScalingType
            };
        }
    }

    /// <summary>
    /// DTO for sharpness settings.
    /// </summary>
    public struct SharpnessSettingsDto : IEquatable<SharpnessSettingsDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Enable flag.
        /// </summary>
        public bool Enable;
        /// <summary>
        /// Filter type.
        /// </summary>
        public uint FilterType;
        /// <summary>
        /// Intensity value.
        /// </summary>
        public float Intensity;

        /// <summary>
        /// Compare sharpness settings.
        /// </summary>
        /// <param name="other">Other settings instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(SharpnessSettingsDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Enable == other.Enable &&
                   FilterType == other.FilterType &&
                   Intensity.Equals(other.Intensity);
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is SharpnessSettingsDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Enable);
            hash.Add(FilterType);
            hash.Add(Intensity);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Sharpness settings DTO.</returns>
        public static SharpnessSettingsDto FromNative(ctl_sharpness_settings_t native)
        {
            return new SharpnessSettingsDto
            {
                Size = native.Size,
                Version = native.Version,
                Enable = IGCLDisplayDtoBool.ToBool(native.Enable),
                FilterType = native.FilterType,
                Intensity = native.Intensity
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Sharpness settings struct.</returns>
        public unsafe ctl_sharpness_settings_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_sharpness_settings_t);

            return new ctl_sharpness_settings_t
            {
                Size = size,
                Version = Version,
                Enable = IGCLDisplayDtoBool.ToByte(Enable),
                FilterType = FilterType,
                Intensity = Intensity
            };
        }
    }

    /// <summary>
    /// DTO for software PSR settings.
    /// </summary>
    public struct SwPsrSettingsDto : IEquatable<SwPsrSettingsDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// True to set values, false to get.
        /// </summary>
        public bool Set;
        /// <summary>
        /// Supported flag.
        /// </summary>
        public bool Supported;
        /// <summary>
        /// Enable flag.
        /// </summary>
        public bool Enable;

        /// <summary>
        /// Compare software PSR settings.
        /// </summary>
        /// <param name="other">Other settings instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(SwPsrSettingsDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Set == other.Set &&
                   Supported == other.Supported &&
                   Enable == other.Enable;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is SwPsrSettingsDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Set);
            hash.Add(Supported);
            hash.Add(Enable);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Software PSR settings DTO.</returns>
        public static SwPsrSettingsDto FromNative(ctl_sw_psr_settings_t native)
        {
            return new SwPsrSettingsDto
            {
                Size = native.Size,
                Version = native.Version,
                Set = IGCLDisplayDtoBool.ToBool(native.Set),
                Supported = IGCLDisplayDtoBool.ToBool(native.Supported),
                Enable = IGCLDisplayDtoBool.ToBool(native.Enable)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Software PSR settings struct.</returns>
        public unsafe ctl_sw_psr_settings_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_sw_psr_settings_t);

            return new ctl_sw_psr_settings_t
            {
                Size = size,
                Version = Version,
                Set = IGCLDisplayDtoBool.ToByte(Set),
                Supported = IGCLDisplayDtoBool.ToByte(Supported),
                Enable = IGCLDisplayDtoBool.ToByte(Enable)
            };
        }
    }

    /// <summary>
    /// DTO for DPST power optimization data.
    /// </summary>
    public struct PowerOptimizationDpstDto : IEquatable<PowerOptimizationDpstDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Minimum DPST level.
        /// </summary>
        public byte MinLevel;
        /// <summary>
        /// Maximum DPST level.
        /// </summary>
        public byte MaxLevel;
        /// <summary>
        /// Current DPST level.
        /// </summary>
        public byte Level;
        /// <summary>
        /// Supported DPST feature flags.
        /// </summary>
        public uint SupportedFeatures;
        public bool SupportsBacklight
        {
            readonly get => HasFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_BKLT);
            set => SupportedFeatures = SetFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_BKLT, value);
        }
        public bool SupportsPanelCabc
        {
            readonly get => HasFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_PANEL_CABC);
            set => SupportedFeatures = SetFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_PANEL_CABC, value);
        }
        public bool SupportsOpst
        {
            readonly get => HasFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_OPST);
            set => SupportedFeatures = SetFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_OPST, value);
        }
        public bool SupportsElp
        {
            readonly get => HasFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_ELP);
            set => SupportedFeatures = SetFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_ELP, value);
        }
        public bool SupportsEpsm
        {
            readonly get => HasFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_EPSM);
            set => SupportedFeatures = SetFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_EPSM, value);
        }
        public bool SupportsApd
        {
            readonly get => HasFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_APD);
            set => SupportedFeatures = SetFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_APD, value);
        }
        public bool SupportsPixoptix
        {
            readonly get => HasFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_PIXOPTIX);
            set => SupportedFeatures = SetFlag(SupportedFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_PIXOPTIX, value);
        }
        /// <summary>
        /// Enabled DPST feature flags.
        /// </summary>
        public uint EnabledFeatures;
        public bool IsBacklightEnabled
        {
            readonly get => HasFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_BKLT);
            set => EnabledFeatures = SetFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_BKLT, value);
        }
        public bool IsPanelCabcEnabled
        {
            readonly get => HasFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_PANEL_CABC);
            set => EnabledFeatures = SetFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_PANEL_CABC, value);
        }
        public bool IsOpstEnabled
        {
            readonly get => HasFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_OPST);
            set => EnabledFeatures = SetFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_OPST, value);
        }
        public bool IsElpEnabled
        {
            readonly get => HasFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_ELP);
            set => EnabledFeatures = SetFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_ELP, value);
        }
        public bool IsEpsmEnabled
        {
            readonly get => HasFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_EPSM);
            set => EnabledFeatures = SetFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_EPSM, value);
        }
        public bool IsApdEnabled
        {
            readonly get => HasFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_APD);
            set => EnabledFeatures = SetFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_APD, value);
        }
        public bool IsPixoptixEnabled
        {
            readonly get => HasFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_PIXOPTIX);
            set => EnabledFeatures = SetFlag(EnabledFeatures, (uint)ctl_power_optimization_dpst_flag_t.CTL_POWER_OPTIMIZATION_DPST_FLAG_PIXOPTIX, value);
        }

        public bool Equals(PowerOptimizationDpstDto other)
        {
            return MinLevel == other.MinLevel &&
                   MaxLevel == other.MaxLevel &&
                   Level == other.Level &&
                   SupportedFeatures == other.SupportedFeatures &&
                   EnabledFeatures == other.EnabledFeatures;
        }

        public override bool Equals(object? obj) => obj is PowerOptimizationDpstDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(MinLevel);
            hash.Add(MaxLevel);
            hash.Add(Level);
            hash.Add(SupportedFeatures);
            hash.Add(EnabledFeatures);
            return hash.ToHashCode();
        }

        public static PowerOptimizationDpstDto FromNative(ctl_power_optimization_dpst_t native)
        {
            return new PowerOptimizationDpstDto
            {
                Size = native.Size,
                Version = native.Version,
                MinLevel = native.MinLevel,
                MaxLevel = native.MaxLevel,
                Level = native.Level,
                SupportedFeatures = native.SupportedFeatures,
                EnabledFeatures = native.EnabledFeatures
            };
        }

        public unsafe ctl_power_optimization_dpst_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_power_optimization_dpst_t);

            return new ctl_power_optimization_dpst_t
            {
                Size = size,
                Version = Version,
                MinLevel = MinLevel,
                MaxLevel = MaxLevel,
                Level = Level,
                SupportedFeatures = SupportedFeatures,
                EnabledFeatures = EnabledFeatures
            };
        }

        private static bool HasFlag(uint value, uint flag) => (value & flag) != 0;

        private static uint SetFlag(uint value, uint flag, bool enabled)
        {
            return enabled ? (value | flag) : (value & ~flag);
        }
    }

    /// <summary>
    /// DTO for PSR power optimization data.
    /// </summary>
    public struct PowerOptimizationPsrDto : IEquatable<PowerOptimizationPsrDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// PSR version.
        /// </summary>
        public byte PSRVersion;
        /// <summary>
        /// Full fetch update flag.
        /// </summary>
        public bool FullFetchUpdate;

        public bool Equals(PowerOptimizationPsrDto other)
        {
            return PSRVersion == other.PSRVersion &&
                   FullFetchUpdate == other.FullFetchUpdate;
        }

        public override bool Equals(object? obj) => obj is PowerOptimizationPsrDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(PSRVersion);
            hash.Add(FullFetchUpdate);
            return hash.ToHashCode();
        }

        public static PowerOptimizationPsrDto FromNative(ctl_power_optimization_psr_t native)
        {
            return new PowerOptimizationPsrDto
            {
                Size = native.Size,
                Version = native.Version,
                PSRVersion = native.PSRVersion,
                FullFetchUpdate = IGCLDisplayDtoBool.ToBool(native.FullFetchUpdate)
            };
        }

        public unsafe ctl_power_optimization_psr_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_power_optimization_psr_t);

            return new ctl_power_optimization_psr_t
            {
                Size = size,
                Version = Version,
                PSRVersion = PSRVersion,
                FullFetchUpdate = IGCLDisplayDtoBool.ToByte(FullFetchUpdate)
            };
        }
    }

    /// <summary>
    /// DTO for LRR power optimization data.
    /// </summary>
    public struct PowerOptimizationLrrDto : IEquatable<PowerOptimizationLrrDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Supported LRR flags.
        /// </summary>
        public uint SupportedLrrTypes;
        public bool SupportsLrr10
        {
            readonly get => HasFlag(SupportedLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR10);
            set => SupportedLrrTypes = SetFlag(SupportedLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR10, value);
        }
        public bool SupportsLrr20
        {
            readonly get => HasFlag(SupportedLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR20);
            set => SupportedLrrTypes = SetFlag(SupportedLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR20, value);
        }
        public bool SupportsLrr25
        {
            readonly get => HasFlag(SupportedLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR25);
            set => SupportedLrrTypes = SetFlag(SupportedLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR25, value);
        }
        public bool SupportsAlrr
        {
            readonly get => HasFlag(SupportedLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_ALRR);
            set => SupportedLrrTypes = SetFlag(SupportedLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_ALRR, value);
        }
        public bool SupportsUblrr
        {
            readonly get => HasFlag(SupportedLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_UBLRR);
            set => SupportedLrrTypes = SetFlag(SupportedLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_UBLRR, value);
        }
        public bool SupportsUbzrr
        {
            readonly get => HasFlag(SupportedLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_UBZRR);
            set => SupportedLrrTypes = SetFlag(SupportedLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_UBZRR, value);
        }
        /// <summary>
        /// Current LRR flags.
        /// </summary>
        public uint CurrentLrrTypes;
        public bool IsLrr10Current
        {
            readonly get => HasFlag(CurrentLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR10);
            set => CurrentLrrTypes = SetFlag(CurrentLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR10, value);
        }
        public bool IsLrr20Current
        {
            readonly get => HasFlag(CurrentLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR20);
            set => CurrentLrrTypes = SetFlag(CurrentLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR20, value);
        }
        public bool IsLrr25Current
        {
            readonly get => HasFlag(CurrentLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR25);
            set => CurrentLrrTypes = SetFlag(CurrentLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_LRR25, value);
        }
        public bool IsAlrrCurrent
        {
            readonly get => HasFlag(CurrentLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_ALRR);
            set => CurrentLrrTypes = SetFlag(CurrentLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_ALRR, value);
        }
        public bool IsUblrrCurrent
        {
            readonly get => HasFlag(CurrentLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_UBLRR);
            set => CurrentLrrTypes = SetFlag(CurrentLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_UBLRR, value);
        }
        public bool IsUbzrrCurrent
        {
            readonly get => HasFlag(CurrentLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_UBZRR);
            set => CurrentLrrTypes = SetFlag(CurrentLrrTypes, (uint)ctl_power_optimization_lrr_flag_t.CTL_POWER_OPTIMIZATION_LRR_FLAG_UBZRR, value);
        }
        /// <summary>
        /// Whether PSR must be disabled.
        /// </summary>
        public bool RequirePsrDisable;
        /// <summary>
        /// Low refresh rate.
        /// </summary>
        public ushort LowRr;

        public bool Equals(PowerOptimizationLrrDto other)
        {
            return SupportedLrrTypes == other.SupportedLrrTypes &&
                   CurrentLrrTypes == other.CurrentLrrTypes &&
                   RequirePsrDisable == other.RequirePsrDisable &&
                   LowRr == other.LowRr;
        }

        public override bool Equals(object? obj) => obj is PowerOptimizationLrrDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(SupportedLrrTypes);
            hash.Add(CurrentLrrTypes);
            hash.Add(RequirePsrDisable);
            hash.Add(LowRr);
            return hash.ToHashCode();
        }

        public static PowerOptimizationLrrDto FromNative(ctl_power_optimization_lrr_t native)
        {
            return new PowerOptimizationLrrDto
            {
                Size = native.Size,
                Version = native.Version,
                SupportedLrrTypes = native.SupportedLRRTypes,
                CurrentLrrTypes = native.CurrentLRRTypes,
                RequirePsrDisable = IGCLDisplayDtoBool.ToBool(native.bRequirePSRDisable),
                LowRr = native.LowRR
            };
        }

        public unsafe ctl_power_optimization_lrr_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_power_optimization_lrr_t);

            return new ctl_power_optimization_lrr_t
            {
                Size = size,
                Version = Version,
                SupportedLRRTypes = SupportedLrrTypes,
                CurrentLRRTypes = CurrentLrrTypes,
                bRequirePSRDisable = IGCLDisplayDtoBool.ToByte(RequirePsrDisable),
                LowRR = LowRr
            };
        }

        private static bool HasFlag(uint value, uint flag) => (value & flag) != 0;

        private static uint SetFlag(uint value, uint flag, bool enabled)
        {
            return enabled ? (value | flag) : (value & ~flag);
        }
    }

    /// <summary>
    /// DTO for power optimization feature-specific data.
    /// </summary>
    public struct PowerOptimizationFeatureSpecificInfoDto : IEquatable<PowerOptimizationFeatureSpecificInfoDto>
    {
        /// <summary>
        /// LRR configuration data.
        /// </summary>
        public PowerOptimizationLrrDto LrrInfo;
        /// <summary>
        /// PSR configuration data.
        /// </summary>
        public PowerOptimizationPsrDto PsrInfo;
        /// <summary>
        /// DPST configuration data.
        /// </summary>
        public PowerOptimizationDpstDto DpstInfo;

        public bool Equals(PowerOptimizationFeatureSpecificInfoDto other)
        {
            return LrrInfo.Equals(other.LrrInfo) &&
                   PsrInfo.Equals(other.PsrInfo) &&
                   DpstInfo.Equals(other.DpstInfo);
        }

        public override bool Equals(object? obj) => obj is PowerOptimizationFeatureSpecificInfoDto other && Equals(other);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(LrrInfo);
            hash.Add(PsrInfo);
            hash.Add(DpstInfo);
            return hash.ToHashCode();
        }

        public static PowerOptimizationFeatureSpecificInfoDto FromNative(ctl_power_optimization_feature_specific_info_t native)
        {
            return new PowerOptimizationFeatureSpecificInfoDto
            {
                LrrInfo = PowerOptimizationLrrDto.FromNative(native.LRRInfo),
                PsrInfo = PowerOptimizationPsrDto.FromNative(native.PSRInfo),
                DpstInfo = PowerOptimizationDpstDto.FromNative(native.DPSTInfo)
            };
        }

        public ctl_power_optimization_feature_specific_info_t ToNative()
        {
            var native = new ctl_power_optimization_feature_specific_info_t();

            // This native type is a union. Prefer a populated member in deterministic order.
            if (DpstInfo.Size != 0 || DpstInfo.SupportedFeatures != 0 || DpstInfo.EnabledFeatures != 0 || DpstInfo.Level != 0 || DpstInfo.MinLevel != 0 || DpstInfo.MaxLevel != 0)
                native.DPSTInfo = DpstInfo.ToNative();
            else if (PsrInfo.Size != 0 || PsrInfo.PSRVersion != 0 || PsrInfo.FullFetchUpdate)
                native.PSRInfo = PsrInfo.ToNative();
            else
                native.LRRInfo = LrrInfo.ToNative();

            return native;
        }
    }

    /// <summary>
    /// DTO for power optimization settings.
    /// </summary>
    public struct PowerOptimizationSettingsDto : IEquatable<PowerOptimizationSettingsDto>
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Power optimization plan.
        /// </summary>
        public ctl_power_optimization_plan_t PowerOptimizationPlan;
        /// <summary>
        /// Power optimization feature flags.
        /// </summary>
        public uint PowerOptimizationFeature;
        public bool UsesFbc
        {
            readonly get => HasFlag(PowerOptimizationFeature, (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_FBC);
            set => PowerOptimizationFeature = SetFlag(PowerOptimizationFeature, (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_FBC, value);
        }
        public bool UsesPsr
        {
            readonly get => HasFlag(PowerOptimizationFeature, (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_PSR);
            set => PowerOptimizationFeature = SetFlag(PowerOptimizationFeature, (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_PSR, value);
        }
        public bool UsesDpst
        {
            readonly get => HasFlag(PowerOptimizationFeature, (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_DPST);
            set => PowerOptimizationFeature = SetFlag(PowerOptimizationFeature, (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_DPST, value);
        }
        public bool UsesLrr
        {
            readonly get => HasFlag(PowerOptimizationFeature, (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LRR);
            set => PowerOptimizationFeature = SetFlag(PowerOptimizationFeature, (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LRR, value);
        }
        public bool UsesLace
        {
            readonly get => HasFlag(PowerOptimizationFeature, (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LACE);
            set => PowerOptimizationFeature = SetFlag(PowerOptimizationFeature, (uint)ctl_power_optimization_flag_t.CTL_POWER_OPTIMIZATION_FLAG_LACE, value);
        }
        /// <summary>
        /// Enable flag.
        /// </summary>
        public bool Enable;
        /// <summary>
        /// Feature-specific data.
        /// </summary>
        public PowerOptimizationFeatureSpecificInfoDto FeatureSpecificData;
        /// <summary>
        /// Power source.
        /// </summary>
        public ctl_power_source_t PowerSource;

        /// <summary>
        /// Compare power optimization settings.
        /// </summary>
        /// <param name="other">Other settings instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(PowerOptimizationSettingsDto other)
        {
            return PowerOptimizationPlan == other.PowerOptimizationPlan &&
                   PowerOptimizationFeature == other.PowerOptimizationFeature &&
                   Enable == other.Enable &&
                   FeatureSpecificData.Equals(other.FeatureSpecificData) &&
                   PowerSource == other.PowerSource;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is PowerOptimizationSettingsDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(PowerOptimizationPlan);
            hash.Add(PowerOptimizationFeature);
            hash.Add(Enable);
            hash.Add(FeatureSpecificData);
            hash.Add(PowerSource);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>Power optimization settings DTO.</returns>
        public static PowerOptimizationSettingsDto FromNative(ctl_power_optimization_settings_t native)
        {
            return new PowerOptimizationSettingsDto
            {
                Size = native.Size,
                Version = native.Version,
                PowerOptimizationPlan = native.PowerOptimizationPlan,
                PowerOptimizationFeature = native.PowerOptimizationFeature,
                Enable = IGCLDisplayDtoBool.ToBool(native.Enable),
                FeatureSpecificData = PowerOptimizationFeatureSpecificInfoDto.FromNative(native.FeatureSpecificData),
                PowerSource = native.PowerSource
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Power optimization settings struct.</returns>
        public unsafe ctl_power_optimization_settings_t ToNative()
        {
            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_power_optimization_settings_t);

            return new ctl_power_optimization_settings_t
            {
                Size = size,
                Version = Version,
                PowerOptimizationPlan = PowerOptimizationPlan,
                PowerOptimizationFeature = PowerOptimizationFeature,
                Enable = IGCLDisplayDtoBool.ToByte(Enable),
                FeatureSpecificData = FeatureSpecificData.ToNative(),
                PowerSource = PowerSource
            };
        }

        private static bool HasFlag(uint value, uint flag) => (value & flag) != 0;

        private static uint SetFlag(uint value, uint flag, bool enabled)
        {
            return enabled ? (value | flag) : (value & ~flag);
        }
    }

    /// <summary>
    /// DTO for brightness get arguments.
    /// </summary>
    public struct BrightnessGetDto
    {
        public uint Size;
        public byte Version;
        public uint TargetBrightness;
        public uint CurrentBrightness;
        public List<uint>? ReservedFields;

        public static unsafe BrightnessGetDto FromNative(ctl_get_brightness_t native)
        {
            return new BrightnessGetDto
            {
                Size = native.Size,
                Version = native.Version,
                TargetBrightness = native.TargetBrightness,
                CurrentBrightness = native.CurrentBrightness,
                ReservedFields = ReadReserved(native.ReservedFields)
            };
        }

        public unsafe ctl_get_brightness_t ToNative()
        {
            var size = Size == 0 ? (uint)sizeof(ctl_get_brightness_t) : Size;
            var native = new ctl_get_brightness_t
            {
                Size = size,
                Version = Version,
                TargetBrightness = TargetBrightness,
                CurrentBrightness = CurrentBrightness
            };

            WriteReserved(ReservedFields, ref native.ReservedFields);
            return native;
        }

        private static unsafe List<uint> ReadReserved(ctl_get_brightness_t._ReservedFields_e__FixedBuffer buffer)
        {
            const int count = 4;
            var values = new List<uint>(count);
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < count; i++)
                values.Add(pValues[i]);
            return values;
        }

        private static unsafe void WriteReserved(List<uint>? values, ref ctl_get_brightness_t._ReservedFields_e__FixedBuffer buffer)
        {
            const int count = 4;
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < count; i++)
                pValues[i] = 0;

            if (values == null)
                return;

            var writeCount = Math.Min(values.Count, count);
            for (var i = 0; i < writeCount; i++)
                pValues[i] = values[i];
        }
    }

    /// <summary>
    /// DTO for brightness set arguments.
    /// </summary>
    public struct BrightnessSetDto
    {
        public uint Size;
        public byte Version;
        public uint TargetBrightness;
        public uint SmoothTransitionTimeInMs;
        public List<uint>? ReservedFields;

        public static unsafe BrightnessSetDto FromNative(ctl_set_brightness_t native)
        {
            return new BrightnessSetDto
            {
                Size = native.Size,
                Version = native.Version,
                TargetBrightness = native.TargetBrightness,
                SmoothTransitionTimeInMs = native.SmoothTransitionTimeInMs,
                ReservedFields = ReadReserved(native.ReservedFields)
            };
        }

        public unsafe ctl_set_brightness_t ToNative()
        {
            var size = Size == 0 ? (uint)sizeof(ctl_set_brightness_t) : Size;
            var native = new ctl_set_brightness_t
            {
                Size = size,
                Version = Version,
                TargetBrightness = TargetBrightness,
                SmoothTransitionTimeInMs = SmoothTransitionTimeInMs
            };

            WriteReserved(ReservedFields, ref native.ReservedFields);
            return native;
        }

        private static unsafe List<uint> ReadReserved(ctl_set_brightness_t._ReservedFields_e__FixedBuffer buffer)
        {
            const int count = 4;
            var values = new List<uint>(count);
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < count; i++)
                values.Add(pValues[i]);
            return values;
        }

        private static unsafe void WriteReserved(List<uint>? values, ref ctl_set_brightness_t._ReservedFields_e__FixedBuffer buffer)
        {
            const int count = 4;
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < count; i++)
                pValues[i] = 0;

            if (values == null)
                return;

            var writeCount = Math.Min(values.Count, count);
            for (var i = 0; i < writeCount; i++)
                pValues[i] = values[i];
        }
    }

    /// <summary>
    /// DTO for scaling capabilities.
    /// </summary>
    public struct ScalingCapsDto
    {
        public uint Size;
        public byte Version;
        public uint SupportedScaling;

        public static ScalingCapsDto FromNative(ctl_scaling_caps_t native)
        {
            return new ScalingCapsDto
            {
                Size = native.Size,
                Version = native.Version,
                SupportedScaling = native.SupportedScaling
            };
        }

        public unsafe ctl_scaling_caps_t ToNative()
        {
            return new ctl_scaling_caps_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_scaling_caps_t) : Size,
                Version = Version,
                SupportedScaling = SupportedScaling
            };
        }
    }

    /// <summary>
    /// DTO for retro scaling capabilities.
    /// </summary>
    public struct RetroScalingCapsDto
    {
        public uint Size;
        public byte Version;
        public uint SupportedRetroScaling;

        public static RetroScalingCapsDto FromNative(ctl_retro_scaling_caps_t native)
        {
            return new RetroScalingCapsDto
            {
                Size = native.Size,
                Version = native.Version,
                SupportedRetroScaling = native.SupportedRetroScaling
            };
        }

        public unsafe ctl_retro_scaling_caps_t ToNative()
        {
            return new ctl_retro_scaling_caps_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_retro_scaling_caps_t) : Size,
                Version = Version,
                SupportedRetroScaling = SupportedRetroScaling
            };
        }
    }

    /// <summary>
    /// DTO for power optimization capabilities.
    /// </summary>
    public struct PowerOptimizationCapsDto
    {
        public uint Size;
        public byte Version;
        public uint SupportedFeatures;

        public static PowerOptimizationCapsDto FromNative(ctl_power_optimization_caps_t native)
        {
            return new PowerOptimizationCapsDto
            {
                Size = native.Size,
                Version = native.Version,
                SupportedFeatures = native.SupportedFeatures
            };
        }

        public unsafe ctl_power_optimization_caps_t ToNative()
        {
            return new ctl_power_optimization_caps_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_power_optimization_caps_t) : Size,
                Version = Version,
                SupportedFeatures = SupportedFeatures
            };
        }
    }

    /// <summary>
    /// DTO for property range information.
    /// </summary>
    public struct PropertyRangeInfoDto
    {
        public float MinPossibleValue;
        public float MaxPossibleValue;
        public float StepSize;
        public float DefaultValue;

        public static PropertyRangeInfoDto FromNative(ctl_property_range_info_t native)
        {
            return new PropertyRangeInfoDto
            {
                MinPossibleValue = native.min_possible_value,
                MaxPossibleValue = native.max_possible_value,
                StepSize = native.step_size,
                DefaultValue = native.default_value
            };
        }

        public ctl_property_range_info_t ToNative()
        {
            return new ctl_property_range_info_t
            {
                min_possible_value = MinPossibleValue,
                max_possible_value = MaxPossibleValue,
                step_size = StepSize,
                default_value = DefaultValue
            };
        }
    }

    /// <summary>
    /// DTO for sharpness filter properties.
    /// </summary>
    public struct SharpnessFilterPropertiesDto
    {
        public uint FilterType;
        public PropertyRangeInfoDto FilterDetails;

        public static SharpnessFilterPropertiesDto FromNative(ctl_sharpness_filter_properties_t native)
        {
            return new SharpnessFilterPropertiesDto
            {
                FilterType = native.FilterType,
                FilterDetails = PropertyRangeInfoDto.FromNative(native.FilterDetails)
            };
        }

        public ctl_sharpness_filter_properties_t ToNative()
        {
            return new ctl_sharpness_filter_properties_t
            {
                FilterType = FilterType,
                FilterDetails = FilterDetails.ToNative()
            };
        }
    }

    /// <summary>
    /// DTO for sharpness capabilities and filter properties.
    /// </summary>
    public struct SharpnessCapsDto
    {
        public uint Size;
        public byte Version;
        public uint SupportedFilterFlags;
        public byte NumFilterTypes;
        public List<SharpnessFilterPropertiesDto>? FilterProperties;

        public static SharpnessCapsDto FromNative(ctl_sharpness_caps_t caps, ctl_sharpness_filter_properties_t[] filters)
        {
            var list = new List<SharpnessFilterPropertiesDto>(filters.Length);
            for (var i = 0; i < filters.Length; i++)
                list.Add(SharpnessFilterPropertiesDto.FromNative(filters[i]));

            return new SharpnessCapsDto
            {
                Size = caps.Size,
                Version = caps.Version,
                SupportedFilterFlags = caps.SupportedFilterFlags,
                NumFilterTypes = caps.NumFilterTypes,
                FilterProperties = list
            };
        }

        public unsafe ctl_sharpness_caps_t ToNative()
        {
            return new ctl_sharpness_caps_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_sharpness_caps_t) : Size,
                Version = Version,
                SupportedFilterFlags = SupportedFilterFlags,
                NumFilterTypes = NumFilterTypes == 0 && FilterProperties != null ? (byte)FilterProperties.Count : NumFilterTypes,
                pFilterProperty = null
            };
        }
    }

    /// <summary>
    /// DTO for Intel Arc Sync profile params.
    /// </summary>
    public struct IntelArcSyncProfileParamsDto
    {
        public uint Size;
        public byte Version;
        public ctl_intel_arc_sync_profile_t IntelArcSyncProfile;
        public float MaxRefreshRateInHz;
        public float MinRefreshRateInHz;
        public uint MaxFrameTimeIncreaseInUs;
        public uint MaxFrameTimeDecreaseInUs;

        public static IntelArcSyncProfileParamsDto FromNative(ctl_intel_arc_sync_profile_params_t native)
        {
            return new IntelArcSyncProfileParamsDto
            {
                Size = native.Size,
                Version = native.Version,
                IntelArcSyncProfile = native.IntelArcSyncProfile,
                MaxRefreshRateInHz = native.MaxRefreshRateInHz,
                MinRefreshRateInHz = native.MinRefreshRateInHz,
                MaxFrameTimeIncreaseInUs = native.MaxFrameTimeIncreaseInUs,
                MaxFrameTimeDecreaseInUs = native.MaxFrameTimeDecreaseInUs
            };
        }

        public unsafe ctl_intel_arc_sync_profile_params_t ToNative()
        {
            return new ctl_intel_arc_sync_profile_params_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_intel_arc_sync_profile_params_t) : Size,
                Version = Version,
                IntelArcSyncProfile = IntelArcSyncProfile,
                MaxRefreshRateInHz = MaxRefreshRateInHz,
                MinRefreshRateInHz = MinRefreshRateInHz,
                MaxFrameTimeIncreaseInUs = MaxFrameTimeIncreaseInUs,
                MaxFrameTimeDecreaseInUs = MaxFrameTimeDecreaseInUs
            };
        }
    }

    /// <summary>
    /// DTO for custom source mode.
    /// </summary>
    public struct CustomSourceModeDto
    {
        public uint SourceX;
        public uint SourceY;

        public static CustomSourceModeDto FromNative(ctl_custom_src_mode_t native)
        {
            return new CustomSourceModeDto
            {
                SourceX = native.SourceX,
                SourceY = native.SourceY
            };
        }

        public ctl_custom_src_mode_t ToNative()
        {
            return new ctl_custom_src_mode_t
            {
                SourceX = SourceX,
                SourceY = SourceY
            };
        }
    }

    /// <summary>
    /// DTO for custom mode args.
    /// </summary>
    public struct CustomModeArgsDto
    {
        public uint Size;
        public byte Version;
        public ctl_custom_mode_operation_types_t CustomModeOpType;
        public uint NumOfModes;
        public List<CustomSourceModeDto>? Modes;

        public static CustomModeArgsDto FromNative(ctl_get_set_custom_mode_args_t native)
        {
            return new CustomModeArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                CustomModeOpType = native.CustomModeOpType,
                NumOfModes = native.NumOfModes
            };
        }

        public unsafe ctl_get_set_custom_mode_args_t ToNative()
        {
            return new ctl_get_set_custom_mode_args_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_get_set_custom_mode_args_t) : Size,
                Version = Version,
                CustomModeOpType = CustomModeOpType,
                NumOfModes = NumOfModes == 0 && Modes != null ? (uint)Modes.Count : NumOfModes,
                pCustomSrcModeList = null
            };
        }
    }

    /// <summary>
    /// DTO for custom mode get results.
    /// </summary>
    public struct CustomModesResultDto
    {
        public CustomModeArgsDto Args;
        public List<CustomSourceModeDto>? Modes;

        public static CustomModesResultDto FromNative(ctl_get_set_custom_mode_args_t args, ctl_custom_src_mode_t[] modes)
        {
            var modeList = new List<CustomSourceModeDto>(modes.Length);
            for (var i = 0; i < modes.Length; i++)
                modeList.Add(CustomSourceModeDto.FromNative(modes[i]));

            var dtoArgs = CustomModeArgsDto.FromNative(args);
            dtoArgs.Modes = modeList;

            return new CustomModesResultDto
            {
                Args = dtoArgs,
                Modes = modeList
            };
        }
    }

    /// <summary>
    /// DTO for mux properties and display outputs.
    /// </summary>
    public struct MuxPropertiesDto
    {
        public uint Size;
        public byte Version;
        public byte MuxId;
        public uint Count;
        public byte IndexOfDisplayOutputOwningMux;
        public List<nint>? DisplayOutputs;

        public static MuxPropertiesDto FromNative(ctl_mux_properties_t native, IntPtr[] outputs)
        {
            var list = new List<nint>(outputs.Length);
            for (var i = 0; i < outputs.Length; i++)
                list.Add(outputs[i]);

            return new MuxPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                MuxId = native.MuxId,
                Count = native.Count,
                IndexOfDisplayOutputOwningMux = native.IndexOfDisplayOutputOwningMux,
                DisplayOutputs = list
            };
        }

        public unsafe ctl_mux_properties_t ToNative()
        {
            return new ctl_mux_properties_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_mux_properties_t) : Size,
                Version = Version,
                MuxId = MuxId,
                Count = Count == 0 && DisplayOutputs != null ? (uint)DisplayOutputs.Count : Count,
                phDisplayOutputs = null,
                IndexOfDisplayOutputOwningMux = IndexOfDisplayOutputOwningMux
            };
        }
    }

    /// <summary>
    /// DTO for vblank timestamp args.
    /// </summary>
    public struct VblankTimestampArgsDto
    {
        public uint Size;
        public byte Version;
        public byte NumOfTargets;
        public List<ulong>? VblankTimestamps;

        public static unsafe VblankTimestampArgsDto FromNative(ctl_vblank_ts_args_t native)
        {
            const int maxTargets = 16;
            var values = new List<ulong>(maxTargets);
            var pValues = (ulong*)Unsafe.AsPointer(ref native.VblankTS.e0);
            for (var i = 0; i < maxTargets; i++)
                values.Add(pValues[i]);

            return new VblankTimestampArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                NumOfTargets = native.NumOfTargets,
                VblankTimestamps = values
            };
        }

        public unsafe ctl_vblank_ts_args_t ToNative()
        {
            var native = new ctl_vblank_ts_args_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_vblank_ts_args_t) : Size,
                Version = Version,
                NumOfTargets = NumOfTargets
            };

            const int maxTargets = 16;
            var pValues = (ulong*)Unsafe.AsPointer(ref native.VblankTS.e0);
            for (var i = 0; i < maxTargets; i++)
                pValues[i] = 0;

            if (VblankTimestamps != null)
            {
                var count = Math.Min(VblankTimestamps.Count, maxTargets);
                for (var i = 0; i < count; i++)
                    pValues[i] = VblankTimestamps[i];

                if (native.NumOfTargets == 0)
                    native.NumOfTargets = (byte)count;
            }

            return native;
        }
    }

    /// <summary>
    /// DTO for I2C access arguments.
    /// </summary>
    public struct I2CAccessArgsDto : IEquatable<I2CAccessArgsDto>
    {
        public uint Size;
        public byte Version;
        public uint Address;
        public uint DataSize;
        public ctl_operation_type_t OpType;
        public uint Offset;
        public uint Flags;
        public ulong RAD;
        public List<byte>? Data;

        public static unsafe I2CAccessArgsDto FromNative(ctl_i2c_access_args_t native)
        {
            const int maxBytes = 128;
            var data = new List<byte>((int)Math.Min(native.DataSize, (uint)maxBytes));
            var pData = (byte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref native.Data.e0);
            var readCount = (int)Math.Min(native.DataSize, (uint)maxBytes);
            for (var i = 0; i < readCount; i++)
                data.Add(pData[i]);

            return new I2CAccessArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                Address = native.Address,
                DataSize = native.DataSize,
                OpType = native.OpType,
                Offset = native.Offset,
                Flags = native.Flags,
                RAD = native.RAD,
                Data = data
            };
        }

        public unsafe ctl_i2c_access_args_t ToNative()
        {
            var native = new ctl_i2c_access_args_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_i2c_access_args_t) : Size,
                Version = Version,
                Address = Address,
                DataSize = DataSize,
                OpType = OpType,
                Offset = Offset,
                Flags = Flags,
                RAD = RAD
            };

            const int maxBytes = 128;
            var pData = (byte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref native.Data.e0);
            for (var i = 0; i < maxBytes; i++)
                pData[i] = 0;

            if (Data != null)
            {
                var writeCount = Math.Min(Data.Count, maxBytes);
                for (var i = 0; i < writeCount; i++)
                    pData[i] = Data[i];

                if (native.DataSize == 0)
                    native.DataSize = (uint)writeCount;
            }

            return native;
        }

        public bool Equals(I2CAccessArgsDto other)
        {
            if (Size != other.Size || Version != other.Version || Address != other.Address ||
                DataSize != other.DataSize || OpType != other.OpType || Offset != other.Offset ||
                Flags != other.Flags || RAD != other.RAD)
                return false;

            if (Data == null && other.Data == null) return true;
            if (Data == null || other.Data == null) return false;
            if (Data.Count != other.Data.Count) return false;
            for (var i = 0; i < Data.Count; i++)
                if (Data[i] != other.Data[i]) return false;
            return true;
        }

        public override bool Equals(object? obj) => obj is I2CAccessArgsDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Address, DataSize, (int)OpType, Offset, Flags, RAD);

        public static I2CAccessArgsDto CreateReadRequest(uint address, uint offset, uint dataSize)
            => new I2CAccessArgsDto { Address = address, Offset = offset, DataSize = dataSize, OpType = ctl_operation_type_t.CTL_OPERATION_TYPE_READ };

        public static I2CAccessArgsDto CreateWriteRequest(uint address, uint offset, List<byte> data)
                => new I2CAccessArgsDto { Address = address, Offset = offset, DataSize = (uint)data.Count, Data = data, OpType = ctl_operation_type_t.CTL_OPERATION_TYPE_WRITE };
    }

    /// <summary>
    /// DTO for I2C access pin-pair arguments.
    /// </summary>
    public struct I2CAccessPinPairArgsDto : IEquatable<I2CAccessPinPairArgsDto>
    {
        public uint Size;
        public byte Version;
        public uint Address;
        public uint DataSize;
        public ctl_operation_type_t OpType;
        public uint Offset;
        public uint Flags;
        public List<byte>? Data;

        public static unsafe I2CAccessPinPairArgsDto FromNative(ctl_i2c_access_pinpair_args_t native)
        {
            const int maxBytes = 128;
            var data = new List<byte>((int)Math.Min(native.DataSize, (uint)maxBytes));
            var pData = (byte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref native.Data.e0);
            var readCount = (int)Math.Min(native.DataSize, (uint)maxBytes);
            for (var i = 0; i < readCount; i++)
                data.Add(pData[i]);

            return new I2CAccessPinPairArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                Address = native.Address,
                DataSize = native.DataSize,
                OpType = native.OpType,
                Offset = native.Offset,
                Flags = native.Flags,
                Data = data
            };
        }

        public unsafe ctl_i2c_access_pinpair_args_t ToNative()
        {
            var native = new ctl_i2c_access_pinpair_args_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_i2c_access_pinpair_args_t) : Size,
                Version = Version,
                Address = Address,
                DataSize = DataSize,
                OpType = OpType,
                Offset = Offset,
                Flags = Flags
            };

            const int maxBytes = 128;
            var pData = (byte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref native.Data.e0);
            for (var i = 0; i < maxBytes; i++)
                pData[i] = 0;

            if (Data != null)
            {
                var writeCount = Math.Min(Data.Count, maxBytes);
                for (var i = 0; i < writeCount; i++)
                    pData[i] = Data[i];

                if (native.DataSize == 0)
                    native.DataSize = (uint)writeCount;
            }

            return native;
        }

        public bool Equals(I2CAccessPinPairArgsDto other)
        {
            if (Size != other.Size || Version != other.Version || Address != other.Address ||
                DataSize != other.DataSize || OpType != other.OpType || Offset != other.Offset ||
                Flags != other.Flags)
                return false;

            if (Data == null && other.Data == null) return true;
            if (Data == null || other.Data == null) return false;
            if (Data.Count != other.Data.Count) return false;
            for (var i = 0; i < Data.Count; i++)
                if (Data[i] != other.Data[i]) return false;
            return true;
        }

        public override bool Equals(object? obj) => obj is I2CAccessPinPairArgsDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(Address, DataSize, (int)OpType, Offset, Flags);

        public static I2CAccessPinPairArgsDto CreateReadRequest(uint address, uint offset, uint dataSize)
            => new I2CAccessPinPairArgsDto { Address = address, Offset = offset, DataSize = dataSize, OpType = ctl_operation_type_t.CTL_OPERATION_TYPE_READ };

        public static I2CAccessPinPairArgsDto CreateWriteRequest(uint address, uint offset, List<byte> data)
                => new I2CAccessPinPairArgsDto { Address = address, Offset = offset, DataSize = (uint)data.Count, Data = data, OpType = ctl_operation_type_t.CTL_OPERATION_TYPE_WRITE };
    }

    /// <summary>
    /// DTO for AUX channel access arguments.
    /// </summary>
    public struct AuxAccessArgsDto : IEquatable<AuxAccessArgsDto>
    {
        public uint Size;
        public byte Version;
        public ctl_operation_type_t OpType;
        public uint Flags;
        public uint Address;
        public ulong RAD;
        public uint PortID;
        public uint DataSize;
        public List<byte>? Data;

        public static unsafe AuxAccessArgsDto FromNative(ctl_aux_access_args_t native)
        {
            const int maxBytes = 132;
            var data = new List<byte>((int)Math.Min(native.DataSize, (uint)maxBytes));
            var pData = (byte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref native.Data.e0);
            var readCount = (int)Math.Min(native.DataSize, (uint)maxBytes);
            for (var i = 0; i < readCount; i++)
                data.Add(pData[i]);

            return new AuxAccessArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                OpType = native.OpType,
                Flags = native.Flags,
                Address = native.Address,
                RAD = native.RAD,
                PortID = native.PortID,
                DataSize = native.DataSize,
                Data = data
            };
        }

        public unsafe ctl_aux_access_args_t ToNative()
        {
            var native = new ctl_aux_access_args_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_aux_access_args_t) : Size,
                Version = Version,
                OpType = OpType,
                Flags = Flags,
                Address = Address,
                RAD = RAD,
                PortID = PortID,
                DataSize = DataSize
            };

            const int maxBytes = 132;
            var pData = (byte*)System.Runtime.CompilerServices.Unsafe.AsPointer(ref native.Data.e0);
            for (var i = 0; i < maxBytes; i++)
                pData[i] = 0;

            if (Data != null)
            {
                var writeCount = Math.Min(Data.Count, maxBytes);
                for (var i = 0; i < writeCount; i++)
                    pData[i] = Data[i];

                if (native.DataSize == 0)
                    native.DataSize = (uint)writeCount;
            }

            return native;
        }

        public bool Equals(AuxAccessArgsDto other)
        {
            if (Size != other.Size || Version != other.Version || OpType != other.OpType ||
                Flags != other.Flags || Address != other.Address || RAD != other.RAD ||
                PortID != other.PortID || DataSize != other.DataSize)
                return false;

            if (Data == null && other.Data == null) return true;
            if (Data == null || other.Data == null) return false;
            if (Data.Count != other.Data.Count) return false;
            for (var i = 0; i < Data.Count; i++)
                if (Data[i] != other.Data[i]) return false;
            return true;
        }

        public override bool Equals(object? obj) => obj is AuxAccessArgsDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine((int)OpType, Address, DataSize, Flags, RAD, PortID);

        public static AuxAccessArgsDto CreateReadRequest(uint address, uint dataSize)
            => new AuxAccessArgsDto { Address = address, DataSize = dataSize, OpType = ctl_operation_type_t.CTL_OPERATION_TYPE_READ };

        public static AuxAccessArgsDto CreateWriteRequest(uint address, List<byte> data)
                => new AuxAccessArgsDto { Address = address, DataSize = (uint)data.Count, Data = data, OpType = ctl_operation_type_t.CTL_OPERATION_TYPE_WRITE };
    }

    /// <summary>
    /// DTO for panel descriptor access arguments.
    /// </summary>
    public struct PanelDescriptorAccessArgsDto : IEquatable<PanelDescriptorAccessArgsDto>
    {
        public uint Size;
        public byte Version;
        public ctl_operation_type_t OpType;
        public uint BlockNumber;
        public uint DescriptorDataSize;
        public List<byte>? DescriptorData;

        public static PanelDescriptorAccessArgsDto FromNative(ctl_panel_descriptor_access_args_t native, byte[]? data = null)
        {
            return new PanelDescriptorAccessArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                OpType = native.OpType,
                BlockNumber = native.BlockNumber,
                DescriptorDataSize = native.DescriptorDataSize,
                DescriptorData = data != null ? new List<byte>(data) : null
            };
        }

        public unsafe ctl_panel_descriptor_access_args_t ToNativeMetadata()
        {
            return new ctl_panel_descriptor_access_args_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_panel_descriptor_access_args_t) : Size,
                Version = Version,
                OpType = OpType,
                BlockNumber = BlockNumber,
                DescriptorDataSize = DescriptorDataSize,
                pDescriptorData = null
            };
        }

        public bool Equals(PanelDescriptorAccessArgsDto other)
            => Size == other.Size && Version == other.Version && OpType == other.OpType &&
               BlockNumber == other.BlockNumber && DescriptorDataSize == other.DescriptorDataSize;

        public override bool Equals(object? obj) => obj is PanelDescriptorAccessArgsDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine((int)OpType, BlockNumber, DescriptorDataSize);

        public static PanelDescriptorAccessArgsDto CreateReadRequest(uint blockNumber)
            => new PanelDescriptorAccessArgsDto { OpType = ctl_operation_type_t.CTL_OPERATION_TYPE_READ, BlockNumber = blockNumber };
    }

    /// <summary>
    /// DTO for EDID management arguments.
    /// </summary>
    public struct EdidManagementArgsDto : IEquatable<EdidManagementArgsDto>
    {
        public uint Size;
        public byte Version;
        public ctl_edid_management_optype_t OpType;
        public ctl_edid_type_t EdidType;
        public uint EdidSize;
        public uint OutFlags;
        public List<byte>? EdidData;

        public static EdidManagementArgsDto FromNative(ctl_edid_management_args_t native, byte[]? edidData = null)
        {
            return new EdidManagementArgsDto
            {
                Size = native.Size,
                Version = native.Version,
                OpType = native.OpType,
                EdidType = native.EdidType,
                EdidSize = native.EdidSize,
                OutFlags = native.OutFlags,
                EdidData = edidData != null ? new List<byte>(edidData) : null
            };
        }

        public unsafe ctl_edid_management_args_t ToNativeMetadata()
        {
            return new ctl_edid_management_args_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_edid_management_args_t) : Size,
                Version = Version,
                OpType = OpType,
                EdidType = EdidType,
                EdidSize = EdidSize,
                OutFlags = OutFlags,
                pEdidBuf = null
            };
        }

        public bool Equals(EdidManagementArgsDto other)
            => Size == other.Size && Version == other.Version && OpType == other.OpType &&
               EdidType == other.EdidType && EdidSize == other.EdidSize && OutFlags == other.OutFlags;

        public override bool Equals(object? obj) => obj is EdidManagementArgsDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine((int)OpType, (int)EdidType, EdidSize, OutFlags);

        public static EdidManagementArgsDto CreateReadRequest(ctl_edid_type_t edidType = ctl_edid_type_t.CTL_EDID_TYPE_CURRENT)
            => new EdidManagementArgsDto { OpType = ctl_edid_management_optype_t.CTL_EDID_MANAGEMENT_OPTYPE_READ_EDID, EdidType = edidType };
    }

    /// <summary>
    /// DTO for pixel transformation color primaries.
    /// </summary>
    public struct PixtxColorPrimariesDto : IEquatable<PixtxColorPrimariesDto>
    {
        public uint Size;
        public byte Version;
        public double xR;
        public double yR;
        public double xG;
        public double yG;
        public double xB;
        public double yB;
        public double xW;
        public double yW;

        public static PixtxColorPrimariesDto FromNative(ctl_pixtx_color_primaries_t native)
            => new PixtxColorPrimariesDto { Size = native.Size, Version = native.Version, xR = native.xR, yR = native.yR, xG = native.xG, yG = native.yG, xB = native.xB, yB = native.yB, xW = native.xW, yW = native.yW };

        public ctl_pixtx_color_primaries_t ToNative()
            => new ctl_pixtx_color_primaries_t { Size = Size, Version = Version, xR = xR, yR = yR, xG = xG, yG = yG, xB = xB, yB = yB, xW = xW, yW = yW };

        public bool Equals(PixtxColorPrimariesDto other)
            => Size == other.Size && Version == other.Version && xR.Equals(other.xR) && yR.Equals(other.yR) &&
               xG.Equals(other.xG) && yG.Equals(other.yG) && xB.Equals(other.xB) && yB.Equals(other.yB) &&
               xW.Equals(other.xW) && yW.Equals(other.yW);

        public override bool Equals(object? obj) => obj is PixtxColorPrimariesDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(xR, yR, xG, yG, xB, yB, xW, yW);
    }

    /// <summary>
    /// DTO for pixel transformation pixel format.
    /// </summary>
    public struct PixtxPixelFormatDto : IEquatable<PixtxPixelFormatDto>
    {
        public uint Size;
        public byte Version;
        public uint BitsPerColor;
        public byte IsFloat;
        public bool IsFloatBool
        {
            readonly get => IGCLDisplayDtoBool.ToBool(IsFloat);
            set => IsFloat = IGCLDisplayDtoBool.ToByte(value);
        }
        public ctl_pixtx_gamma_encoding_type_t EncodingType;
        public ctl_pixtx_color_space_t ColorSpace;
        public ctl_pixtx_color_model_t ColorModel;
        public PixtxColorPrimariesDto ColorPrimaries;
        public double MaxBrightness;
        public double MinBrightness;

        public static PixtxPixelFormatDto FromNative(ctl_pixtx_pixel_format_t native)
            => new PixtxPixelFormatDto
            {
                Size = native.Size,
                Version = native.Version,
                BitsPerColor = native.BitsPerColor,
                IsFloat = native.IsFloat,
                EncodingType = native.EncodingType,
                ColorSpace = native.ColorSpace,
                ColorModel = native.ColorModel,
                ColorPrimaries = PixtxColorPrimariesDto.FromNative(native.ColorPrimaries),
                MaxBrightness = native.MaxBrightness,
                MinBrightness = native.MinBrightness
            };

        public ctl_pixtx_pixel_format_t ToNative()
            => new ctl_pixtx_pixel_format_t
            {
                Size = Size,
                Version = Version,
                BitsPerColor = BitsPerColor,
                IsFloat = IsFloat,
                EncodingType = EncodingType,
                ColorSpace = ColorSpace,
                ColorModel = ColorModel,
                ColorPrimaries = ColorPrimaries.ToNative(),
                MaxBrightness = MaxBrightness,
                MinBrightness = MinBrightness
            };

        public bool Equals(PixtxPixelFormatDto other)
            => Size == other.Size && Version == other.Version && BitsPerColor == other.BitsPerColor &&
               IsFloat == other.IsFloat && EncodingType == other.EncodingType && ColorSpace == other.ColorSpace &&
               ColorModel == other.ColorModel && ColorPrimaries.Equals(other.ColorPrimaries) &&
               MaxBrightness.Equals(other.MaxBrightness) && MinBrightness.Equals(other.MinBrightness);

        public override bool Equals(object? obj) => obj is PixtxPixelFormatDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(BitsPerColor, (int)EncodingType, (int)ColorSpace, (int)ColorModel);
    }

    /// <summary>
    /// DTO for pixel transformation pipe get config (metadata only; LUT sample values require native methods).
    /// </summary>
    public struct PixtxPipeGetConfigDto : IEquatable<PixtxPipeGetConfigDto>
    {
        public uint Size;
        public byte Version;
        public ctl_pixtx_config_query_type_t QueryType;
        public PixtxPixelFormatDto InputPixelFormat;
        public PixtxPixelFormatDto OutputPixelFormat;
        public uint NumBlocks;

        public static PixtxPipeGetConfigDto FromNative(ctl_pixtx_pipe_get_config_t native)
            => new PixtxPipeGetConfigDto
            {
                Size = native.Size,
                Version = native.Version,
                QueryType = native.QueryType,
                InputPixelFormat = PixtxPixelFormatDto.FromNative(native.InputPixelFormat),
                OutputPixelFormat = PixtxPixelFormatDto.FromNative(native.OutputPixelFormat),
                NumBlocks = native.NumBlocks
            };

        public unsafe ctl_pixtx_pipe_get_config_t ToNative()
            => new ctl_pixtx_pipe_get_config_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_pixtx_pipe_get_config_t) : Size,
                Version = Version,
                QueryType = QueryType,
                InputPixelFormat = InputPixelFormat.ToNative(),
                OutputPixelFormat = OutputPixelFormat.ToNative(),
                NumBlocks = NumBlocks,
                pBlockConfigs = null
            };

        public bool Equals(PixtxPipeGetConfigDto other)
            => Size == other.Size && Version == other.Version && QueryType == other.QueryType &&
               InputPixelFormat.Equals(other.InputPixelFormat) && OutputPixelFormat.Equals(other.OutputPixelFormat) &&
               NumBlocks == other.NumBlocks;

        public override bool Equals(object? obj) => obj is PixtxPipeGetConfigDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine((int)QueryType, NumBlocks);

        public static PixtxPipeGetConfigDto CreateCapabilityRequest()
            => new PixtxPipeGetConfigDto { QueryType = ctl_pixtx_config_query_type_t.CTL_PIXTX_CONFIG_QUERY_TYPE_CAPABILITY };

        public static PixtxPipeGetConfigDto CreateCurrentRequest()
            => new PixtxPipeGetConfigDto { QueryType = ctl_pixtx_config_query_type_t.CTL_PIXTX_CONFIG_QUERY_TYPE_CURRENT };
    }

    /// <summary>
    /// DTO for pixel transformation block config (metadata only; LUT sample values require native methods).
    /// </summary>
    public struct PixtxBlockConfigDto : IEquatable<PixtxBlockConfigDto>
    {
        public uint Size;
        public byte Version;
        public uint BlockId;
        public ctl_pixtx_block_type_t BlockType;

        public static PixtxBlockConfigDto FromNative(ctl_pixtx_block_config_t native)
            => new PixtxBlockConfigDto
            {
                Size = native.Size,
                Version = native.Version,
                BlockId = native.BlockId,
                BlockType = native.BlockType
            };

        public bool Equals(PixtxBlockConfigDto other)
            => Size == other.Size && Version == other.Version && BlockId == other.BlockId && BlockType == other.BlockType;

        public override bool Equals(object? obj) => obj is PixtxBlockConfigDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(BlockId, (int)BlockType);
    }

    /// <summary>
    /// DTO for pixel transformation pipe set config.
    /// </summary>
    public struct PixtxPipeSetConfigDto : IEquatable<PixtxPipeSetConfigDto>
    {
        public uint Size;
        public byte Version;
        public ctl_pixtx_config_opertaion_type_t OpertaionType;
        public uint Flags;
        public uint NumBlocks;

        public static PixtxPipeSetConfigDto FromNative(ctl_pixtx_pipe_set_config_t native)
            => new PixtxPipeSetConfigDto
            {
                Size = native.Size,
                Version = native.Version,
                OpertaionType = native.OpertaionType,
                Flags = native.Flags,
                NumBlocks = native.NumBlocks
            };

        public unsafe ctl_pixtx_pipe_set_config_t ToNative()
            => new ctl_pixtx_pipe_set_config_t
            {
                Size = Size == 0 ? (uint)sizeof(ctl_pixtx_pipe_set_config_t) : Size,
                Version = Version,
                OpertaionType = OpertaionType,
                Flags = Flags,
                NumBlocks = NumBlocks,
                pBlockConfigs = null
            };

        public bool Equals(PixtxPipeSetConfigDto other)
            => Size == other.Size && Version == other.Version && OpertaionType == other.OpertaionType &&
               Flags == other.Flags && NumBlocks == other.NumBlocks;

        public override bool Equals(object? obj) => obj is PixtxPipeSetConfigDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine((int)OpertaionType, Flags, NumBlocks);
    }

    /// <summary>
    /// DTO result for pixel transformation get config (metadata only; LUT sample values require native methods).
    /// </summary>
    public struct PixelTransformationGetResultDto : IEquatable<PixelTransformationGetResultDto>
    {
        public PixtxPipeGetConfigDto PipeConfig;
        public List<PixtxBlockConfigDto>? Blocks;

        public static PixelTransformationGetResultDto FromNative(ctl_pixtx_pipe_get_config_t config, ctl_pixtx_block_config_t[] blocks)
        {
            var blockDtos = new List<PixtxBlockConfigDto>(blocks.Length);
            foreach (var b in blocks)
                blockDtos.Add(PixtxBlockConfigDto.FromNative(b));

            return new PixelTransformationGetResultDto
            {
                PipeConfig = PixtxPipeGetConfigDto.FromNative(config),
                Blocks = blockDtos
            };
        }

        public bool Equals(PixelTransformationGetResultDto other)
        {
            if (!PipeConfig.Equals(other.PipeConfig)) return false;
            if (Blocks == null && other.Blocks == null) return true;
            if (Blocks == null || other.Blocks == null) return false;
            if (Blocks.Count != other.Blocks.Count) return false;
            for (var i = 0; i < Blocks.Count; i++)
                if (!Blocks[i].Equals(other.Blocks[i])) return false;
            return true;
        }

        public override bool Equals(object? obj) => obj is PixelTransformationGetResultDto other && Equals(other);
        public override int GetHashCode() => HashCode.Combine(PipeConfig, Blocks?.Count ?? 0);
    }
}

