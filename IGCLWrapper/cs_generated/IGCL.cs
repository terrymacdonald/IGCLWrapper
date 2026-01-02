using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    public static unsafe partial class IGCL
    {
        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlInit"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlInit(ctl_init_args_t* pInitDesc, [NativeTypeName("ctl_api_handle_t *")] _ctl_api_handle_t** phAPIHandle);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlClose"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlClose([NativeTypeName("ctl_api_handle_t")] _ctl_api_handle_t* hAPIHandle);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlSetRuntimePath"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlSetRuntimePath(ctl_runtime_path_args_t* pArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlWaitForPropertyChange"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlWaitForPropertyChange([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceAdapter, ctl_wait_property_change_args_t* pArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlReservedCall"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlReservedCall([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceAdapter, ctl_reserved_args_t* pArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSupported3DCapabilities"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSupported3DCapabilities([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, ctl_3d_feature_caps_t* pFeatureCaps);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSet3DFeature"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSet3DFeature([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, ctl_3d_feature_getset_t* pFeature);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlCheckDriverVersion"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlCheckDriverVersion([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceAdapter, [NativeTypeName("ctl_version_info_t")] uint version_info);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEnumerateDevices"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEnumerateDevices([NativeTypeName("ctl_api_handle_t")] _ctl_api_handle_t* hAPIHandle, [NativeTypeName("uint32_t *")] uint* pCount, [NativeTypeName("ctl_device_adapter_handle_t *")] _ctl_device_adapter_handle_t** phDevices);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEnumerateDisplayOutputs"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEnumerateDisplayOutputs([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceAdapter, [NativeTypeName("uint32_t *")] uint* pCount, [NativeTypeName("ctl_display_output_handle_t *")] _ctl_display_output_handle_t** phDisplayOutputs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEnumerateI2CPinPairs"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEnumerateI2CPinPairs([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceAdapter, [NativeTypeName("uint32_t *")] uint* pCount, [NativeTypeName("ctl_i2c_pin_pair_handle_t *")] _ctl_i2c_pin_pair_handle_t** phI2cPinPairs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetDeviceProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetDeviceProperties([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, ctl_device_adapter_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetDisplayProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetDisplayProperties([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_display_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetAdaperDisplayEncoderProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetAdaperDisplayEncoderProperties([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_adapter_display_encoder_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetZeDevice"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetZeDevice([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, void* pZeDevice, void** hInstance);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSharpnessCaps"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSharpnessCaps([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_sharpness_caps_t* pSharpnessCaps);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetCurrentSharpness"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetCurrentSharpness([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_sharpness_settings_t* pSharpnessSettings);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlSetCurrentSharpness"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlSetCurrentSharpness([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_sharpness_settings_t* pSharpnessSettings);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlI2CAccess"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlI2CAccess([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_i2c_access_args_t* pI2cAccessArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlI2CAccessOnPinPair"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlI2CAccessOnPinPair([NativeTypeName("ctl_i2c_pin_pair_handle_t")] _ctl_i2c_pin_pair_handle_t* hI2cPinPair, ctl_i2c_access_pinpair_args_t* pI2cAccessArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlAUXAccess"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlAUXAccess([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_aux_access_args_t* pAuxAccessArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetPowerOptimizationCaps"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetPowerOptimizationCaps([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_power_optimization_caps_t* pPowerOptimizationCaps);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetPowerOptimizationSetting"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetPowerOptimizationSetting([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_power_optimization_settings_t* pPowerOptimizationSettings);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlSetPowerOptimizationSetting"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlSetPowerOptimizationSetting([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_power_optimization_settings_t* pPowerOptimizationSettings);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlSetBrightnessSetting"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlSetBrightnessSetting([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_set_brightness_t* pSetBrightnessSetting);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetBrightnessSetting"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetBrightnessSetting([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_get_brightness_t* pGetBrightnessSetting);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlPixelTransformationGetConfig"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlPixelTransformationGetConfig([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_pixtx_pipe_get_config_t* pPixTxGetConfigArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlPixelTransformationSetConfig"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlPixelTransformationSetConfig([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_pixtx_pipe_set_config_t* pPixTxSetConfigArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlPanelDescriptorAccess"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlPanelDescriptorAccess([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_panel_descriptor_access_args_t* pPanelDescriptorAccessArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSupportedRetroScalingCapability"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSupportedRetroScalingCapability([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, ctl_retro_scaling_caps_t* pRetroScalingCaps);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSetRetroScaling"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSetRetroScaling([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, ctl_retro_scaling_settings_t* pGetSetRetroScalingType);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSupportedScalingCapability"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSupportedScalingCapability([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_scaling_caps_t* pScalingCaps);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetCurrentScaling"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetCurrentScaling([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_scaling_settings_t* pGetCurrentScalingType);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlSetCurrentScaling"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlSetCurrentScaling([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_scaling_settings_t* pSetScalingType);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetLACEConfig"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetLACEConfig([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_lace_config_t* pLaceConfig);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlSetLACEConfig"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlSetLACEConfig([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_lace_config_t* pLaceConfig);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlSoftwarePSR"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlSoftwarePSR([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_sw_psr_settings_t* pSoftwarePsrSetting);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetIntelArcSyncInfoForMonitor"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetIntelArcSyncInfoForMonitor([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_intel_arc_sync_monitor_params_t* pIntelArcSyncMonitorParams);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEnumerateMuxDevices"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEnumerateMuxDevices([NativeTypeName("ctl_api_handle_t")] _ctl_api_handle_t* hAPIHandle, [NativeTypeName("uint32_t *")] uint* pCount, [NativeTypeName("ctl_mux_output_handle_t *")] _ctl_mux_output_handle_t** phMuxDevices);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetMuxProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetMuxProperties([NativeTypeName("ctl_mux_output_handle_t")] _ctl_mux_output_handle_t* hMuxDevice, ctl_mux_properties_t* pMuxProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlSwitchMux"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlSwitchMux([NativeTypeName("ctl_mux_output_handle_t")] _ctl_mux_output_handle_t* hMuxDevice, [NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hInactiveDisplayOutput);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetIntelArcSyncProfile"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetIntelArcSyncProfile([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_intel_arc_sync_profile_params_t* pIntelArcSyncProfileParams);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlSetIntelArcSyncProfile"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlSetIntelArcSyncProfile([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_intel_arc_sync_profile_params_t* pIntelArcSyncProfileParams);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEdidManagement"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEdidManagement([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_edid_management_args_t* pEdidManagementArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSetCustomMode"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSetCustomMode([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_get_set_custom_mode_args_t* pCustomModeArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSetCombinedDisplay"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSetCombinedDisplay([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceAdapter, ctl_combined_display_args_t* pCombinedDisplayArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSetDisplayGenlock"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSetDisplayGenlock([NativeTypeName("ctl_device_adapter_handle_t *")] _ctl_device_adapter_handle_t** hDeviceAdapter, ctl_genlock_args_t* pGenlockArgs, [NativeTypeName("uint32_t")] uint AdapterCount, [NativeTypeName("ctl_device_adapter_handle_t *")] _ctl_device_adapter_handle_t** hFailureDeviceAdapter);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetVblankTimestamp"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetVblankTimestamp([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_vblank_ts_args_t* pVblankTSArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlLinkDisplayAdapters"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlLinkDisplayAdapters([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hPrimaryAdapter, ctl_lda_args_t* pLdaArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlUnlinkDisplayAdapters"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlUnlinkDisplayAdapters([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hPrimaryAdapter);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetLinkedDisplayAdapters"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetLinkedDisplayAdapters([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hPrimaryAdapter, ctl_lda_args_t* pLdaArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSetDynamicContrastEnhancement"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSetDynamicContrastEnhancement([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_dce_args_t* pDceArgs);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSetWireFormat"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSetWireFormat([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_get_set_wire_format_config_t* pGetSetWireFormatSetting);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSetDisplaySettings"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSetDisplaySettings([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* hDisplayOutput, ctl_display_settings_t* pDisplaySettings);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEccGetProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEccGetProperties([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, ctl_ecc_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEccGetState"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEccGetState([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, ctl_ecc_state_desc_t* pState);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEccSetState"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEccSetState([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, ctl_ecc_state_desc_t* pState);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEnumEngineGroups"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEnumEngineGroups([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, [NativeTypeName("uint32_t *")] uint* pCount, [NativeTypeName("ctl_engine_handle_t *")] _ctl_engine_handle_t** phEngine);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEngineGetProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEngineGetProperties([NativeTypeName("ctl_engine_handle_t")] _ctl_engine_handle_t* hEngine, ctl_engine_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEngineGetActivity"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEngineGetActivity([NativeTypeName("ctl_engine_handle_t")] _ctl_engine_handle_t* hEngine, ctl_engine_stats_t* pStats);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEnumFans"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEnumFans([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, [NativeTypeName("uint32_t *")] uint* pCount, [NativeTypeName("ctl_fan_handle_t *")] _ctl_fan_handle_t** phFan);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlFanGetProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlFanGetProperties([NativeTypeName("ctl_fan_handle_t")] _ctl_fan_handle_t* hFan, ctl_fan_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlFanGetConfig"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlFanGetConfig([NativeTypeName("ctl_fan_handle_t")] _ctl_fan_handle_t* hFan, ctl_fan_config_t* pConfig);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlFanSetDefaultMode"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlFanSetDefaultMode([NativeTypeName("ctl_fan_handle_t")] _ctl_fan_handle_t* hFan);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlFanSetFixedSpeedMode"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlFanSetFixedSpeedMode([NativeTypeName("ctl_fan_handle_t")] _ctl_fan_handle_t* hFan, [NativeTypeName("const ctl_fan_speed_t *")] ctl_fan_speed_t* speed);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlFanSetSpeedTableMode"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlFanSetSpeedTableMode([NativeTypeName("ctl_fan_handle_t")] _ctl_fan_handle_t* hFan, [NativeTypeName("const ctl_fan_speed_table_t *")] ctl_fan_speed_table_t* speedTable);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlFanGetState"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlFanGetState([NativeTypeName("ctl_fan_handle_t")] _ctl_fan_handle_t* hFan, ctl_fan_speed_units_t units, [NativeTypeName("int32_t *")] int* pSpeed);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetFirmwareProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetFirmwareProperties([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceAdapter, ctl_firmware_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEnumerateFirmwareComponents"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEnumerateFirmwareComponents([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceAdapter, [NativeTypeName("uint32_t *")] uint* pCount, [NativeTypeName("ctl_firmware_component_handle_t *")] _ctl_firmware_component_handle_t** phFirmware);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetFirmwareComponentProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetFirmwareComponentProperties([NativeTypeName("ctl_firmware_component_handle_t")] _ctl_firmware_component_handle_t* hFirmware, ctl_firmware_component_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlAllowPCIeLinkSpeedUpdate"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlAllowPCIeLinkSpeedUpdate([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceAdapter, [NativeTypeName("bool")] byte AllowPCIeLinkSpeedUpdate);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEnumFrequencyDomains"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEnumFrequencyDomains([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, [NativeTypeName("uint32_t *")] uint* pCount, [NativeTypeName("ctl_freq_handle_t *")] _ctl_freq_handle_t** phFrequency);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlFrequencyGetProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlFrequencyGetProperties([NativeTypeName("ctl_freq_handle_t")] _ctl_freq_handle_t* hFrequency, ctl_freq_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlFrequencyGetAvailableClocks"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlFrequencyGetAvailableClocks([NativeTypeName("ctl_freq_handle_t")] _ctl_freq_handle_t* hFrequency, [NativeTypeName("uint32_t *")] uint* pCount, double* phFrequency);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlFrequencyGetRange"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlFrequencyGetRange([NativeTypeName("ctl_freq_handle_t")] _ctl_freq_handle_t* hFrequency, ctl_freq_range_t* pLimits);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlFrequencySetRange"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlFrequencySetRange([NativeTypeName("ctl_freq_handle_t")] _ctl_freq_handle_t* hFrequency, [NativeTypeName("const ctl_freq_range_t *")] ctl_freq_range_t* pLimits);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlFrequencyGetState"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlFrequencyGetState([NativeTypeName("ctl_freq_handle_t")] _ctl_freq_handle_t* hFrequency, ctl_freq_state_t* pState);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlFrequencyGetThrottleTime"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlFrequencyGetThrottleTime([NativeTypeName("ctl_freq_handle_t")] _ctl_freq_handle_t* hFrequency, ctl_freq_throttle_time_t* pThrottleTime);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEnumLeds"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEnumLeds([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, [NativeTypeName("uint32_t *")] uint* pCount, [NativeTypeName("ctl_led_handle_t *")] _ctl_led_handle_t** phLed);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlLedGetProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlLedGetProperties([NativeTypeName("ctl_led_handle_t")] _ctl_led_handle_t* hLed, ctl_led_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlLedGetState"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlLedGetState([NativeTypeName("ctl_led_handle_t")] _ctl_led_handle_t* hLed, ctl_led_state_t* pState);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlLedSetState"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlLedSetState([NativeTypeName("ctl_led_handle_t")] _ctl_led_handle_t* hLed, void* pBuffer, [NativeTypeName("uint32_t")] uint bufferSize);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSupportedVideoProcessingCapabilities"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSupportedVideoProcessingCapabilities([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, ctl_video_processing_feature_caps_t* pFeatureCaps);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlGetSetVideoProcessingFeature"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlGetSetVideoProcessingFeature([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, ctl_video_processing_feature_getset_t* pFeature);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEnumMemoryModules"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEnumMemoryModules([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, [NativeTypeName("uint32_t *")] uint* pCount, [NativeTypeName("ctl_mem_handle_t *")] _ctl_mem_handle_t** phMemory);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlMemoryGetProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlMemoryGetProperties([NativeTypeName("ctl_mem_handle_t")] _ctl_mem_handle_t* hMemory, ctl_mem_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlMemoryGetState"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlMemoryGetState([NativeTypeName("ctl_mem_handle_t")] _ctl_mem_handle_t* hMemory, ctl_mem_state_t* pState);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlMemoryGetBandwidth"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlMemoryGetBandwidth([NativeTypeName("ctl_mem_handle_t")] _ctl_mem_handle_t* hMemory, ctl_mem_bandwidth_t* pBandwidth);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockGetProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockGetProperties([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, ctl_oc_properties_t* pOcProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockWaiverSet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockWaiverSet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockGpuFrequencyOffsetGet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockGpuFrequencyOffsetGet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double* pOcFrequencyOffset);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockGpuFrequencyOffsetSet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockGpuFrequencyOffsetSet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double ocFrequencyOffset);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockGpuVoltageOffsetGet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockGpuVoltageOffsetGet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double* pOcVoltageOffset);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockGpuVoltageOffsetSet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockGpuVoltageOffsetSet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double ocVoltageOffset);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockGpuLockGet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockGpuLockGet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, ctl_oc_vf_pair_t* pVfPair);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockGpuLockSet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockGpuLockSet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, ctl_oc_vf_pair_t vFPair);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockVramFrequencyOffsetGet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockVramFrequencyOffsetGet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double* pOcFrequencyOffset);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockVramFrequencyOffsetSet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockVramFrequencyOffsetSet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double ocFrequencyOffset);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockVramVoltageOffsetGet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockVramVoltageOffsetGet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double* pVoltage);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockVramVoltageOffsetSet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockVramVoltageOffsetSet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double voltage);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockPowerLimitGet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockPowerLimitGet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double* pSustainedPowerLimit);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockPowerLimitSet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockPowerLimitSet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double sustainedPowerLimit);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockTemperatureLimitGet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockTemperatureLimitGet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double* pTemperatureLimit);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockTemperatureLimitSet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockTemperatureLimitSet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double temperatureLimit);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlPowerTelemetryGet"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlPowerTelemetryGet([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, ctl_power_telemetry_t* pTelemetryInfo);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockResetToDefault"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockResetToDefault([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockGpuFrequencyOffsetGetV2"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockGpuFrequencyOffsetGetV2([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double* pOcFrequencyOffset);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockGpuFrequencyOffsetSetV2"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockGpuFrequencyOffsetSetV2([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double ocFrequencyOffset);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockGpuMaxVoltageOffsetGetV2"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockGpuMaxVoltageOffsetGetV2([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double* pOcMaxVoltageOffset);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockGpuMaxVoltageOffsetSetV2"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockGpuMaxVoltageOffsetSetV2([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double ocMaxVoltageOffset);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockVramMemSpeedLimitGetV2"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockVramMemSpeedLimitGetV2([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double* pOcVramMemSpeedLimit);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockVramMemSpeedLimitSetV2"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockVramMemSpeedLimitSetV2([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double ocVramMemSpeedLimit);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockPowerLimitGetV2"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockPowerLimitGetV2([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double* pSustainedPowerLimit);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockPowerLimitSetV2"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockPowerLimitSetV2([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double sustainedPowerLimit);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockTemperatureLimitGetV2"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockTemperatureLimitGetV2([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double* pTemperatureLimit);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockTemperatureLimitSetV2"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockTemperatureLimitSetV2([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceHandle, double temperatureLimit);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockReadVFCurve"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockReadVFCurve([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceAdapter, ctl_vf_curve_type_t VFCurveType, ctl_vf_curve_details_t VFCurveDetail, [NativeTypeName("uint32_t *")] uint* pNumPoints, ctl_voltage_frequency_point_t* pVFCurveTable);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlOverclockWriteCustomVFCurve"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlOverclockWriteCustomVFCurve([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDeviceAdapter, [NativeTypeName("uint32_t")] uint NumPoints, ctl_voltage_frequency_point_t* pCustomVFCurveTable);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlPciGetProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlPciGetProperties([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, ctl_pci_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlPciGetState"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlPciGetState([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, ctl_pci_state_t* pState);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEnumPowerDomains"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEnumPowerDomains([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, [NativeTypeName("uint32_t *")] uint* pCount, [NativeTypeName("ctl_pwr_handle_t *")] _ctl_pwr_handle_t** phPower);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlPowerGetProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlPowerGetProperties([NativeTypeName("ctl_pwr_handle_t")] _ctl_pwr_handle_t* hPower, ctl_power_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlPowerGetEnergyCounter"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlPowerGetEnergyCounter([NativeTypeName("ctl_pwr_handle_t")] _ctl_pwr_handle_t* hPower, ctl_power_energy_counter_t* pEnergy);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlPowerGetLimits"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlPowerGetLimits([NativeTypeName("ctl_pwr_handle_t")] _ctl_pwr_handle_t* hPower, ctl_power_limits_t* pPowerLimits);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlPowerSetLimits"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlPowerSetLimits([NativeTypeName("ctl_pwr_handle_t")] _ctl_pwr_handle_t* hPower, [NativeTypeName("const ctl_power_limits_t *")] ctl_power_limits_t* pPowerLimits);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlEnumTemperatureSensors"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlEnumTemperatureSensors([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* hDAhandle, [NativeTypeName("uint32_t *")] uint* pCount, [NativeTypeName("ctl_temp_handle_t *")] _ctl_temp_handle_t** phTemperature);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlTemperatureGetProperties"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlTemperatureGetProperties([NativeTypeName("ctl_temp_handle_t")] _ctl_temp_handle_t* hTemperature, ctl_temp_properties_t* pProperties);

        /// <include file='IGCL.xml' path='doc/member[@name="IGCL.ctlTemperatureGetState"]/*' />
        [DllImport("ControlLib", CallingConvention = CallingConvention.Cdecl, ExactSpelling = true)]
        public static extern ctl_result_t ctlTemperatureGetState([NativeTypeName("ctl_temp_handle_t")] _ctl_temp_handle_t* hTemperature, double* pTemperature);

        [NativeTypeName("#define CTL_IMPL_MAJOR_VERSION 1")]
        public const int CTL_IMPL_MAJOR_VERSION = 1;

        [NativeTypeName("#define CTL_IMPL_MINOR_VERSION 1")]
        public const int CTL_IMPL_MINOR_VERSION = 1;

        [NativeTypeName("#define CTL_IMPL_VERSION CTL_MAKE_VERSION( CTL_IMPL_MAJOR_VERSION, CTL_IMPL_MINOR_VERSION )")]
        public const int CTL_IMPL_VERSION = ((1 << 16) | (1 & 0x0000ffff));

        [NativeTypeName("#define CTL_MAX_DEVICE_NAME_LEN 100")]
        public const int CTL_MAX_DEVICE_NAME_LEN = 100;

        [NativeTypeName("#define CTL_MAX_RESERVED_SIZE 108")]
        public const int CTL_MAX_RESERVED_SIZE = 108;

        [NativeTypeName("#define CTL_I2C_MAX_DATA_SIZE 0x0080")]
        public const int CTL_I2C_MAX_DATA_SIZE = 0x0080;

        [NativeTypeName("#define CTL_AUX_MAX_DATA_SIZE 132")]
        public const int CTL_AUX_MAX_DATA_SIZE = 132;

        [NativeTypeName("#define CTL_MAX_NUM_SAMPLES_PER_CHANNEL_1D_LUT 8192")]
        public const int CTL_MAX_NUM_SAMPLES_PER_CHANNEL_1D_LUT = 8192;

        [NativeTypeName("#define CTL_MAX_DISPLAYS_FOR_MGPU_COLLAGE 16")]
        public const int CTL_MAX_DISPLAYS_FOR_MGPU_COLLAGE = 16;

        [NativeTypeName("#define CTL_MAX_WIREFORMAT_COLOR_MODELS_SUPPORTED 4")]
        public const int CTL_MAX_WIREFORMAT_COLOR_MODELS_SUPPORTED = 4;

        [NativeTypeName("#define CTL_FAN_TEMP_SPEED_PAIR_COUNT 32")]
        public const int CTL_FAN_TEMP_SPEED_PAIR_COUNT = 32;

        [NativeTypeName("#define CTL_FIRMWARE_PROPERTY_STR_SIZE 64")]
        public const int CTL_FIRMWARE_PROPERTY_STR_SIZE = 64;

        [NativeTypeName("#define CTL_MAX_FIRMWARE_PROPERTIES_RESERVED_SIZE 16")]
        public const int CTL_MAX_FIRMWARE_PROPERTIES_RESERVED_SIZE = 16;

        [NativeTypeName("#define CTL_MAX_FIRMWARE_COMPONENT_PROPERTIES_RESERVED_SIZE 20")]
        public const int CTL_MAX_FIRMWARE_COMPONENT_PROPERTIES_RESERVED_SIZE = 20;

        [NativeTypeName("#define CTL_PSU_COUNT 5")]
        public const int CTL_PSU_COUNT = 5;

        [NativeTypeName("#define CTL_FAN_COUNT 5")]
        public const int CTL_FAN_COUNT = 5;
    }
}
