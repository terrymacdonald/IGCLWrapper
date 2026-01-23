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
        /// Get display timing information.
        /// </summary>
        /// <returns>Display timing struct.</returns>
        public ctl_display_timing_t GetTiming()
        {
            var props = GetPropertiesNative();
            return props.Display_Timing_Info;
        }

        /// <summary>
        /// Check whether the display is active.
        /// </summary>
        /// <returns>True when active; otherwise, false.</returns>
        public bool IsActive()
        {
            var timing = GetTiming();
            return timing.HActive > 0 && timing.VActive > 0;
        }

        /// <summary>
        /// Get the current display resolution.
        /// </summary>
        /// <returns>Tuple containing width and height.</returns>
        public (uint width, uint height) GetResolution()
        {
            var timing = GetTiming();
            return (timing.HActive, timing.VActive);
        }

        /// <summary>
        /// Get the display refresh rate in Hz.
        /// </summary>
        /// <returns>Refresh rate in Hz.</returns>
        public double GetRefreshRateHz()
        {
            var timing = GetTiming();
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
        /// Get sharpness capabilities and filter properties.
        /// </summary>
        /// <returns>Tuple containing caps and filter properties array.</returns>
        public unsafe (ctl_sharpness_caps_t caps, ctl_sharpness_filter_properties_t[] filters) GetSharpnessCaps()
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
        /// Perform I2C access using the provided arguments.
        /// </summary>
        /// <param name="args">I2C access arguments.</param>
        public unsafe void I2CAccess(ref ctl_i2c_access_args_t args)
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
        /// Perform I2C access on a specific pin pair.
        /// </summary>
        /// <param name="pinPair">I2C pin pair handle.</param>
        /// <param name="args">I2C access arguments.</param>
        public unsafe void I2CAccessOnPinPair(IntPtr pinPair, ref ctl_i2c_access_pinpair_args_t args)
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
        /// Perform AUX channel access using the provided arguments.
        /// </summary>
        /// <param name="args">AUX access arguments.</param>
        public unsafe void AUXAccess(ref ctl_aux_access_args_t args)
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
        /// Get power optimization capability information.
        /// </summary>
        /// <returns>Power optimization capabilities struct.</returns>
        public unsafe ctl_power_optimization_caps_t GetPowerOptimizationCaps()
        {
            ThrowIfDisposed();
            var caps = CreatePowerOptimizationCaps();
            var result = IGCL.ctlGetPowerOptimizationCaps((_ctl_display_output_handle_t*)DisplayHandle, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get power optimization caps");
            return caps;
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
            SetPowerOptimizationSettingNative(settings.ToNative());
        }

        /// <summary>
        /// Set display brightness.
        /// </summary>
        /// <param name="brightness">Brightness settings struct.</param>
        public unsafe void SetBrightnessSetting(ctl_set_brightness_t brightness)
        {
            ThrowIfDisposed();
            var copy = brightness;
            var result = IGCL.ctlSetBrightnessSetting((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set brightness");
        }

        /// <summary>
        /// Get display brightness.
        /// </summary>
        /// <returns>Brightness settings struct.</returns>
        public unsafe ctl_get_brightness_t GetBrightnessSetting()
        {
            ThrowIfDisposed();
            var brightness = CreateGetBrightness();
            var result = IGCL.ctlGetBrightnessSetting((_ctl_display_output_handle_t*)DisplayHandle, &brightness);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to get brightness: {result}");
            return brightness;
        }

        /// <summary>
        /// Get pixel transformation configuration.
        /// </summary>
        /// <param name="args">Pipe get config arguments.</param>
        /// <returns>Tuple containing config and block array.</returns>
        public unsafe (ctl_pixtx_pipe_get_config_t config, ctl_pixtx_block_config_t[] blocks) PixelTransformationGetConfig(ctl_pixtx_pipe_get_config_t args)
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
        /// Set pixel transformation configuration.
        /// </summary>
        /// <param name="args">Pipe set config arguments.</param>
        public unsafe void PixelTransformationSetConfig(ctl_pixtx_pipe_set_config_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlPixelTransformationSetConfig((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set pixel transformation config");
        }

        /// <summary>
        /// Access the panel descriptor using the provided arguments.
        /// </summary>
        /// <param name="args">Panel descriptor access arguments.</param>
        /// <returns>Updated panel descriptor access arguments.</returns>
        public unsafe ctl_panel_descriptor_access_args_t PanelDescriptorAccess(ctl_panel_descriptor_access_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlPanelDescriptorAccess((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to access panel descriptor");
            return copy;
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

            sizeArgs = PanelDescriptorAccess(sizeArgs);

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
                readArgs = PanelDescriptorAccess(readArgs);
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

                extSizeArgs = PanelDescriptorAccess(extSizeArgs);

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
                    extReadArgs = PanelDescriptorAccess(extReadArgs);
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
        /// Get supported retro scaling capabilities.
        /// </summary>
        /// <returns>Retro scaling capability struct.</returns>
        public unsafe ctl_retro_scaling_caps_t GetSupportedRetroScalingCapability()
        {
            ThrowIfDisposed();
            var caps = CreateRetroScalingCaps();
            var result = IGCL.ctlGetSupportedRetroScalingCapability((_ctl_device_adapter_handle_t*)AdapterHandle, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get retro scaling capability");
            return caps;
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
        /// Get supported scaling capabilities.
        /// </summary>
        /// <returns>Scaling capability struct.</returns>
        public unsafe ctl_scaling_caps_t GetSupportedScalingCapability()
        {
            ThrowIfDisposed();
            var caps = CreateScalingCaps();
            var result = IGCL.ctlGetSupportedScalingCapability((_ctl_display_output_handle_t*)DisplayHandle, &caps);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get scaling capability");
            return caps;
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
        /// Get mux properties and its display outputs.
        /// </summary>
        /// <param name="muxHandle">Mux handle.</param>
        /// <returns>Tuple containing mux properties and display output handles.</returns>
        public unsafe (ctl_mux_properties_t properties, IntPtr[] displayOutputs) GetMuxProperties(IntPtr muxHandle)
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
        /// Get Intel Arc Sync profile parameters.
        /// </summary>
        /// <returns>Arc Sync profile params struct.</returns>
        public unsafe ctl_intel_arc_sync_profile_params_t GetIntelArcSyncProfile()
        {
            ThrowIfDisposed();
            var parameters = CreateArcSyncProfileParams();
            var result = IGCL.ctlGetIntelArcSyncProfile((_ctl_display_output_handle_t*)DisplayHandle, &parameters);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get Intel Arc Sync profile");
            return parameters;
        }

        /// <summary>
        /// Set Intel Arc Sync profile parameters.
        /// </summary>
        /// <param name="parameters">Arc Sync profile params struct.</param>
        public unsafe void SetIntelArcSyncProfile(ctl_intel_arc_sync_profile_params_t parameters)
        {
            ThrowIfDisposed();
            var copy = parameters;
            var result = IGCL.ctlSetIntelArcSyncProfile((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set Intel Arc Sync profile");
        }

        /// <summary>
        /// Perform EDID management using the provided arguments.
        /// </summary>
        /// <param name="args">EDID management arguments.</param>
        /// <returns>Updated EDID management arguments.</returns>
        public unsafe ctl_edid_management_args_t EdidManagement(ctl_edid_management_args_t args)
        {
            ThrowIfDisposed();
            var copy = args;
            var result = IGCL.ctlEdidManagement((_ctl_display_output_handle_t*)DisplayHandle, &copy);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to perform EDID management (op={args.OpType}, edidType={args.EdidType}, result={result})");
            return copy;
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

            args = EdidManagement(args);
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
                    args = EdidManagement(args);
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
        /// Get custom display modes.
        /// </summary>
        /// <returns>Tuple containing updated args and modes.</returns>
        public unsafe (ctl_get_set_custom_mode_args_t args, ctl_custom_src_mode_t[] modes) GetCustomModes()
        {
            var args = CreateCustomModeArgs();
            args.CustomModeOpType = ctl_custom_mode_operation_types_t.CTL_CUSTOM_MODE_OPERATION_TYPES_GET_CUSTOM_SOURCE_MODES;
            return GetCustomModesNative(args);
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
        /// Get vblank timestamp information.
        /// </summary>
        /// <returns>Vblank timestamp args struct.</returns>
        public unsafe ctl_vblank_ts_args_t GetVblankTimestamp()
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
        /// Call the native get/set DCE API using the provided struct.
        /// </summary>
        /// <param name="args">DCE args struct.</param>
        /// <param name="histogram">Histogram buffer or null.</param>
        /// <returns>Tuple containing updated args and histogram.</returns>
        public unsafe (ctl_dce_args_t args, uint[] histogram) GetSetDynamicContrastEnhancementNative(ctl_dce_args_t args, uint[]? histogram = null)
        {
            ThrowIfDisposed();
            var request = args;

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
            var request = settings;
            request.Set = true;
            GetSetDisplaySettingsNative(request.ToNative());
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
        public ctl_os_display_encoder_identifier_t OsDisplayEncoderHandle;
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
        public ctl_revision_datatype_t SupportedSpec;
        /// <summary>
        /// Supported output BPC flags.
        /// </summary>
        public uint SupportedOutputBpcFlags;
        /// <summary>
        /// Protocol converter type flags.
        /// </summary>
        public uint ProtocolConverterType;
        /// <summary>
        /// Display configuration flags.
        /// </summary>
        public uint DisplayConfigFlags;
        /// <summary>
        /// Feature enabled flags.
        /// </summary>
        public uint FeatureEnabledFlags;
        /// <summary>
        /// Feature supported flags.
        /// </summary>
        public uint FeatureSupportedFlags;
        /// <summary>
        /// Advanced feature enabled flags.
        /// </summary>
        public uint AdvancedFeatureEnabledFlags;
        /// <summary>
        /// Advanced feature supported flags.
        /// </summary>
        public uint AdvancedFeatureSupportedFlags;
        /// <summary>
        /// Display timing info.
        /// </summary>
        public ctl_display_timing_t DisplayTimingInfo;
        /// <summary>
        /// Reserved native fields.
        /// </summary>
        public uint[]? ReservedFields;

        /// <summary>
        /// Compare display properties while ignoring pointer-backed and reserved fields.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(DisplayPropertiesDto other)
        {
            // OsDisplayEncoderHandle contains pointer data; ReservedFields are native-only.
            return Size == other.Size &&
                   Version == other.Version &&
                   Type == other.Type &&
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
            hash.Add(Size);
            hash.Add(Version);
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
                OsDisplayEncoderHandle = native.Os_display_encoder_handle,
                Type = native.Type,
                AttachedDisplayMuxType = native.AttachedDisplayMuxType,
                ProtocolConverterOutput = native.ProtocolConverterOutput,
                SupportedSpec = native.SupportedSpec,
                SupportedOutputBpcFlags = native.SupportedOutputBPCFlags,
                ProtocolConverterType = native.ProtocolConverterType,
                DisplayConfigFlags = native.DisplayConfigFlags,
                FeatureEnabledFlags = native.FeatureEnabledFlags,
                FeatureSupportedFlags = native.FeatureSupportedFlags,
                AdvancedFeatureEnabledFlags = native.AdvancedFeatureEnabledFlags,
                AdvancedFeatureSupportedFlags = native.AdvancedFeatureSupportedFlags,
                DisplayTimingInfo = native.Display_Timing_Info,
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
                Os_display_encoder_handle = OsDisplayEncoderHandle,
                Type = Type,
                AttachedDisplayMuxType = AttachedDisplayMuxType,
                ProtocolConverterOutput = ProtocolConverterOutput,
                SupportedSpec = SupportedSpec,
                SupportedOutputBPCFlags = SupportedOutputBpcFlags,
                ProtocolConverterType = ProtocolConverterType,
                DisplayConfigFlags = DisplayConfigFlags,
                FeatureEnabledFlags = FeatureEnabledFlags,
                FeatureSupportedFlags = FeatureSupportedFlags,
                AdvancedFeatureEnabledFlags = AdvancedFeatureEnabledFlags,
                AdvancedFeatureSupportedFlags = AdvancedFeatureSupportedFlags,
                Display_Timing_Info = DisplayTimingInfo
            };

            WriteReservedFields(ReservedFields, ref native.ReservedFields);
            return native;
        }

        private static unsafe uint[] ReadReservedFields(ctl_display_properties_t._ReservedFields_e__FixedBuffer buffer)
        {
            var values = new uint[ReservedFieldCount];
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedFieldCount; i++)
                values[i] = pValues[i];
            return values;
        }

        private static unsafe void WriteReservedFields(uint[]? values, ref ctl_display_properties_t._ReservedFields_e__FixedBuffer buffer)
        {
            var pValues = (uint*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < ReservedFieldCount; i++)
                pValues[i] = 0;

            if (values == null || values.Length == 0)
                return;

            var count = Math.Min(values.Length, ReservedFieldCount);
            for (var i = 0; i < count; i++)
                pValues[i] = values[i];
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
        public ctl_wire_format_t[]? SupportedWireFormat;
        /// <summary>
        /// Selected wire format.
        /// </summary>
        public ctl_wire_format_t WireFormat;

        /// <summary>
        /// Compare wire format settings.
        /// </summary>
        /// <param name="other">Other settings instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(WireFormatConfigDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Operation == other.Operation &&
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
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Operation);
            hash.Add(WireFormat);
            if (SupportedWireFormat != null)
            {
                hash.Add(SupportedWireFormat.Length);
                for (var i = 0; i < SupportedWireFormat.Length; i++)
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
                WireFormat = native.WireFormat
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
                WireFormat = WireFormat
            };

            WriteSupportedWireFormat(SupportedWireFormat, ref native.SupportedWireFormat);
            return native;
        }

        private static unsafe ctl_wire_format_t[] ReadSupportedWireFormat(ctl_get_set_wire_format_config_t._SupportedWireFormat_e__FixedBuffer buffer)
        {
            var values = new ctl_wire_format_t[SupportedWireFormatCount];
            var pValues = (ctl_wire_format_t*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < SupportedWireFormatCount; i++)
                values[i] = pValues[i];
            return values;
        }

        private static unsafe void WriteSupportedWireFormat(ctl_wire_format_t[]? values, ref ctl_get_set_wire_format_config_t._SupportedWireFormat_e__FixedBuffer buffer)
        {
            var pValues = (ctl_wire_format_t*)Unsafe.AsPointer(ref buffer.e0);
            for (var i = 0; i < SupportedWireFormatCount; i++)
                pValues[i] = default;

            if (values == null || values.Length == 0)
                return;

            var count = Math.Min(values.Length, SupportedWireFormatCount);
            for (var i = 0; i < count; i++)
                pValues[i] = values[i];
        }

        private static bool AreSupportedWireFormatsEqual(ctl_wire_format_t[]? left, ctl_wire_format_t[]? right)
        {
            if (ReferenceEquals(left, right))
                return true;
            if (left == null || right == null)
                return false;
            if (left.Length != right.Length)
                return false;
            for (var i = 0; i < left.Length; i++)
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
        public ctl_os_display_encoder_identifier_t OsDisplayEncoderHandle;
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
        public ctl_revision_datatype_t SupportedSpec;
        /// <summary>
        /// Supported output bits-per-component flags.
        /// </summary>
        public uint SupportedOutputBpcFlags;
        /// <summary>
        /// Encoder configuration flags.
        /// </summary>
        public uint EncoderConfigFlags;
        /// <summary>
        /// Feature supported flags.
        /// </summary>
        public uint FeatureSupportedFlags;
        /// <summary>
        /// Advanced feature supported flags.
        /// </summary>
        public uint AdvancedFeatureSupportedFlags;
        /// <summary>
        /// Reserved native fields.
        /// </summary>
        public ctl_adapter_display_encoder_properties_t._ReservedFields_e__FixedBuffer ReservedFields;

        /// <summary>
        /// Compare adapter display encoder properties while ignoring reserved native fields.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(AdapterDisplayEncoderPropertiesDto other)
        {
            // OsDisplayEncoderHandle contains pointer data; ReservedFields are native-only.
            return Size == other.Size &&
                   Version == other.Version &&
                   Type == other.Type &&
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
            hash.Add(Size);
            hash.Add(Version);
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
                OsDisplayEncoderHandle = native.Os_display_encoder_handle,
                Type = native.Type,
                IsOnBoardProtocolConverterOutputPresent = IGCLDisplayDtoBool.ToBool(native.IsOnBoardProtocolConverterOutputPresent),
                SupportedSpec = native.SupportedSpec,
                SupportedOutputBpcFlags = native.SupportedOutputBPCFlags,
                EncoderConfigFlags = native.EncoderConfigFlags,
                FeatureSupportedFlags = native.FeatureSupportedFlags,
                AdvancedFeatureSupportedFlags = native.AdvancedFeatureSupportedFlags,
                ReservedFields = native.ReservedFields
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>Adapter display encoder properties struct.</returns>
        public ctl_adapter_display_encoder_properties_t ToNative()
        {
            return new ctl_adapter_display_encoder_properties_t
            {
                Size = Size,
                Version = Version,
                Os_display_encoder_handle = OsDisplayEncoderHandle,
                Type = Type,
                IsOnBoardProtocolConverterOutputPresent = IGCLDisplayDtoBool.ToByte(IsOnBoardProtocolConverterOutputPresent),
                SupportedSpec = SupportedSpec,
                SupportedOutputBPCFlags = SupportedOutputBpcFlags,
                EncoderConfigFlags = EncoderConfigFlags,
                FeatureSupportedFlags = FeatureSupportedFlags,
                AdvancedFeatureSupportedFlags = AdvancedFeatureSupportedFlags,
                ReservedFields = ReservedFields
            };
        }
    }

    /// <summary>
    /// DTO for dynamic contrast enhancement arguments.
    /// </summary>
    public unsafe struct DceArgsDto : IEquatable<DceArgsDto>
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
        /// Pointer to histogram buffer.
        /// </summary>
        public IntPtr Histogram;

        /// <summary>
        /// Compare DCE args while ignoring pointer fields.
        /// </summary>
        /// <param name="other">Other args instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(DceArgsDto other)
        {
            // Histogram is a pointer to a buffer and is intentionally excluded.
            return Size == other.Size &&
                   Version == other.Version &&
                   Set == other.Set &&
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
            hash.Add(Size);
            hash.Add(Version);
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
        public static DceArgsDto FromNative(ctl_dce_args_t native)
        {
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
                Histogram = (IntPtr)native.pHistogram
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

            return new ctl_dce_args_t
            {
                Size = size,
                Version = Version,
                Set = IGCLDisplayDtoBool.ToByte(Set),
                TargetBrightnessPercent = TargetBrightnessPercent,
                PhaseinSpeedMultiplier = PhaseinSpeedMultiplier,
                NumBins = NumBins,
                Enable = IGCLDisplayDtoBool.ToByte(Enable),
                IsSupported = IGCLDisplayDtoBool.ToByte(IsSupported),
                pHistogram = (uint*)Histogram
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
        /// <summary>
        /// Controllable flags.
        /// </summary>
        public uint ControllableFlags;
        /// <summary>
        /// Valid flags.
        /// </summary>
        public uint ValidFlags;
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
        public ctl_display_settings_t._Reserved_e__FixedBuffer Reserved;

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
                Reserved = native.Reserved
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

            return new ctl_display_settings_t
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
                AudioSettings = AudioSettings,
                Reserved = Reserved
            };
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
        public ctl_lace_aggr_config_t LaceConfig;

        /// <summary>
        /// Compare LACE configuration.
        /// </summary>
        /// <param name="other">Other config instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(LaceConfigDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   Enabled == other.Enabled &&
                   OpTypeGet == other.OpTypeGet &&
                   OpTypeSet == other.OpTypeSet &&
                   Trigger == other.Trigger &&
                   AreLaceAggrConfigsEqual(LaceConfig, other.LaceConfig);
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
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(Enabled);
            hash.Add(OpTypeGet);
            hash.Add(OpTypeSet);
            hash.Add(Trigger);
            hash.Add(LaceConfig.FixedAggressivenessLevelPercent);
            hash.Add(LaceConfig.AggrLevelMap.MaxNumEntries);
            hash.Add(LaceConfig.AggrLevelMap.NumEntries);
            return hash.ToHashCode();
        }

        private static bool AreLaceAggrConfigsEqual(ctl_lace_aggr_config_t left, ctl_lace_aggr_config_t right)
        {
            return left.FixedAggressivenessLevelPercent == right.FixedAggressivenessLevelPercent &&
                   left.AggrLevelMap.MaxNumEntries == right.AggrLevelMap.MaxNumEntries &&
                   left.AggrLevelMap.NumEntries == right.AggrLevelMap.NumEntries;
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
                LaceConfig = native.LaceConfig
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
                LaceConfig = LaceConfig
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
        /// <summary>
        /// Enable flag.
        /// </summary>
        public bool Enable;
        /// <summary>
        /// Feature-specific data.
        /// </summary>
        public ctl_power_optimization_feature_specific_info_t FeatureSpecificData;
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
            return Size == other.Size &&
                   Version == other.Version &&
                   PowerOptimizationPlan == other.PowerOptimizationPlan &&
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
            hash.Add(Size);
            hash.Add(Version);
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
                FeatureSpecificData = native.FeatureSpecificData,
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
                FeatureSpecificData = FeatureSpecificData,
                PowerSource = PowerSource
            };
        }
    }
}

