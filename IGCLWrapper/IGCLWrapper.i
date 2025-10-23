// IGCLWrapper (c) Terry MacDonald 2025
//-------------------------------------------------------------------------------------------------
// Purpose: SWIG interface for Intel Graphics Control Library (IGCL) C API -> C# bindings
// Notes : IGCL is a C API (igcl_api.h). We optionally use Intel's cApiWrapper.cpp for dynamic loading.
//         Keep handle types opaque in C#, surface helper pointer typedefs for array/out params.

// ----- SWIG module & language options -----
%module(directors="1") IGCL

// ----- Nullable reference type support for C# 8.0+ -----
// This typemap makes SWIG generate nullable return types (SWIGTYPE?) for pointer wrapper methods
// This fixes CS8600 errors where null is assigned to non-nullable reference types
%typemap(csout, excode=SWIGEXCODE) SWIGTYPE* {
    global::System.IntPtr cPtr = $imcall;$excode
    $csclassname? ret = (cPtr == global::System.IntPtr.Zero) ? null : new $csclassname(cPtr, $owner);
    return ret;
  }

%typemap(csvarout, excode=SWIGEXCODE2) SWIGTYPE* %{
    get {
      global::System.IntPtr cPtr = $imcall;$excode
      $csclassname? ret = (cPtr == global::System.IntPtr.Zero) ? null : new $csclassname(cPtr, $owner);
      return ret;
    } %}

// Make the return types nullable in method signatures
%typemap(cstype) SWIGTYPE* "$csclassname?"

%define FORCE_UINT_FLAGS(TypedefName, TagName)
/* Underlying type + “Flags” behavior in C# */
%typemap(csbase)       TypedefName "uint"
%typemap(csenumflags)  TypedefName "uint"
%typemap(csbase)       TagName     "uint"
%typemap(csenumflags)  TagName     "uint"
/* Optional: add [Flags] for nice C# semantics */
%csattributes          TypedefName "[System.Flags]"
%csattributes          TagName     "[System.Flags]"
%enddef

FORCE_UINT_FLAGS(ctl_init_flag_t, _ctl_init_flag_t)
FORCE_UINT_FLAGS(ctl_property_type_flag_t, _ctl_property_type_flag_t)
FORCE_UINT_FLAGS(ctl_firmware_config_flag_t, _ctl_firmware_config_flag_t)
FORCE_UINT_FLAGS(ctl_sharpness_filter_type_flag_t, _ctl_sharpness_filter_type_flag_t)
FORCE_UINT_FLAGS(ctl_pixtx_pipe_set_config_flag_t, _ctl_pixtx_pipe_set_config_flag_t)
FORCE_UINT_FLAGS(ctl_display_config_flag_t, _ctl_display_config_flag_t)
FORCE_UINT_FLAGS(ctl_protocol_converter_location_flag_t, _ctl_protocol_converter_location_flag_t)
FORCE_UINT_FLAGS(ctl_std_display_feature_flag_t, _ctl_std_display_feature_flag_t)
FORCE_UINT_FLAGS(ctl_supported_functions_flag_t, _ctl_supported_functions_flag_t)
FORCE_UINT_FLAGS(ctl_intel_display_feature_flag_t, _ctl_intel_display_feature_flag_t)
FORCE_UINT_FLAGS(ctl_display_setting_flag_t, _ctl_display_setting_flag_t)
FORCE_UINT_FLAGS(ctl_display_setting_picture_ar_flag_t, _ctl_display_setting_picture_ar_flag_t)
FORCE_UINT_FLAGS(ctl_3d_feature_misc_flag_t, _ctl_3d_feature_misc_flag_t)
FORCE_UINT_FLAGS(ctl_power_optimization_dpst_flag_t, _ctl_power_optimization_dpst_flag_t)
FORCE_UINT_FLAGS(ctl_power_optimization_flag_t, _ctl_power_optimization_flag_t)
FORCE_UINT_FLAGS(ctl_freq_throttle_reason_flag_t, _ctl_freq_throttle_reason_flag_t)
FORCE_UINT_FLAGS(ctl_power_optimization_lrr_flag_t, _ctl_power_optimization_lrr_flag_t)
FORCE_UINT_FLAGS(ctl_lace_trigger_flag_t, _ctl_lace_trigger_flag_t)
FORCE_UINT_FLAGS(ctl_3d_tier_profile_flag_t, _ctl_3d_tier_profile_flag_t)
FORCE_UINT_FLAGS(ctl_gaming_flip_mode_flag_t, _ctl_gaming_flip_mode_flag_t)
FORCE_UINT_FLAGS(ctl_3d_tier_type_flag_t, _ctl_3d_tier_type_flag_t)
FORCE_UINT_FLAGS(ctl_edid_management_out_flag_t, _ctl_edid_management_out_flag_t)
FORCE_UINT_FLAGS(ctl_encoder_config_flag_t, _ctl_encoder_config_flag_t)
FORCE_UINT_FLAGS(ctl_adapter_properties_flag_t, _ctl_adapter_properties_flag_t)
FORCE_UINT_FLAGS(ctl_retro_scaling_type_flag_t, _ctl_retro_scaling_type_flag_t)
FORCE_UINT_FLAGS(ctl_get_operation_flag_t, _ctl_get_operation_flag_t)
FORCE_UINT_FLAGS(ctl_i2c_flag_t, _ctl_i2c_flag_t)
FORCE_UINT_FLAGS(ctl_i2c_pinpair_flag_t, _ctl_i2c_pinpair_flag_t)
FORCE_UINT_FLAGS(ctl_scaling_type_flag_t, _ctl_scaling_type_flag_t)
FORCE_UINT_FLAGS(ctl_aux_flag_t, _ctl_aux_flag_t)
FORCE_UINT_FLAGS(ctl_video_processing_super_resolution_flag_t, _ctl_video_processing_super_resolution_flag_t)
FORCE_UINT_FLAGS(ctl_output_bpc_flag_t, _ctl_output_bpc_flag_t)


%{
// ----- C/C++ preamble visible to the generated wrapper -----
#include <Windows.h>

// IGCL headers (placed by prepare_igcl.ps1 / rebuild scripts)
#include "../drivers.gpu.control-library/include/igcl_api.h"

// Optional: include the dynamic wrapper helpers from Intel to auto-load the runtime.
// If you vendor these files into your native project, you can expose thin helpers here.
//#include "../drivers.gpu.control-library/Source/cApiWrapper.h"

// Windows wide-char convenience
typedef wchar_t WCHAR;

// Some toolchains define these; neutralize for wrapper side.
#ifndef CTL_APICALL
#define CTL_APICALL
#endif
#ifndef CTL_APIEXPORT
#define CTL_APIEXPORT
#endif

// Treat IGCL handles as opaque here (SWIG/C# will see them as IntPtr by default)
%}

// ----- Make the opaque handle types explicit in the interface -----
%pragma(csharp) moduleclassmodifiers="public partial class"
%pragma(csharp) imclassclassmodifiers="public partial class"

// Enable nullable reference types in the generated C# code via module code injection
%pragma(csharp) imclasscode=%{
#nullable enable
%}

// Add type aliases at the namespace level for easier consumption
%pragma(csharp) modulecode=%{
#nullable enable
%}

// Inject type aliases into the namespace (outside the IGCL class)
%typemap(csimports) SWIGTYPE %{
using System;
using System.Runtime.InteropServices;

#nullable enable
%}

// Add #nullable enable to all generated proxy classes (structs, enums, etc.)
%typemap(csclassmodifiers) SWIGTYPE "public partial class"
%typemap(cscode) SWIGTYPE %{
#nullable enable
%}

%typemap(cstype)  (ctl_device_adapter_handle_t) "System.IntPtr"
%typemap(imtype)  (ctl_device_adapter_handle_t) "IntPtr"
%typemap(cstype)  (ctl_display_output_handle_t) "System.IntPtr"
%typemap(imtype)  (ctl_display_output_handle_t) "IntPtr"

%include stdint.i
%include carrays.i
%include typemaps.i
%include windows.i
%include cpointer.i

// ----- Expose key IGCL primitive typedefs cleanly to C# (nice-to-have aliases) -----
%inline %{
typedef uint64_t igcl_uint64;
typedef uint32_t igcl_uint32;
typedef uint16_t igcl_uint16;
typedef uint8_t  igcl_uint8;
typedef int64_t  igcl_int64;
typedef int32_t  igcl_int32;
typedef int16_t  igcl_int16;
typedef int8_t   igcl_int8;
%}

// ----- Expose IGCL Version Macros and Constants to C# -----
// These helper functions allow C# code to work with IGCL version numbers
// Note: The constants CTL_IMPL_MAJOR_VERSION, CTL_IMPL_MINOR_VERSION, and 
// CTL_IMPL_VERSION are already defined in igcl_api.h and will be automatically
// exposed to C# by SWIG

// Inline helper functions for version manipulation
%inline %{
    // Create a version number from major and minor components
    static inline uint32_t CTL_MakeVersion(uint32_t major, uint32_t minor) {
   return (major << 16) | (minor & 0x0000ffff);
    }
    
  // Extract major version from a version number
    static inline uint32_t CTL_GetMajorVersion(uint32_t version) {
return version >> 16;
    }
    
    // Extract minor version from a version number
static inline uint32_t CTL_GetMinorVersion(uint32_t version) {
        return version & 0x0000ffff;
    }
    
    // Get the current implementation version (wrapper around the macro)
    static inline uint32_t CTL_GetImplVersion() {
        return CTL_IMPL_VERSION;
    }
%}

// IGCL types (for handles)
//typedef void* ctl_api_handle_t;
//typedef void* ctl_device_adapter_handle_t;
//typedef void* ctl_display_output_handle_t;
//typedef void* ctl_i2c_pin_pair_handle_t;
typedef void* voidP_Ptr; // for pointer void*

// If the header typedefs these as pointers or uint64, this keeps the C# surface stable.
%apply void *VOID_INT_PTR { ctl_device_adapter_handle_t, ctl_display_output_handle_t };


// ----- Pointer helpers (common out parameters) -----
%pointer_functions(igcl_uint32, igcl_uint32P);
%pointer_functions(igcl_uint64, igcl_uint64P);
%pointer_functions(igcl_int32,  igcl_int32P);
%pointer_functions(igcl_int64,  igcl_int64P);
%pointer_functions(WCHAR,       wcharP);
%pointer_functions(ctl_api_handle_t, apiHandleP);
%pointer_functions(ctl_device_adapter_handle_t, deviceAdapterHandleP);
%pointer_functions(ctl_display_output_handle_t, displayOutputHandleP);
%pointer_functions(ctl_i2c_pin_pair_handle_t, i2cPinPairHandleP);
%pointer_functions(ctl_engine_handle_t, engineHandleP);
%pointer_functions(ctl_mem_handle_t, memHandleP);
%pointer_functions(ctl_property_t, propertyP);
%pointer_functions(ctl_display_properties_t, displayPropertiesP);
%pointer_functions(ctl_device_adapter_properties_t, adapterPropertiesP);
%pointer_functions(ctl_3d_feature_caps_t, featureCapsP);
%pointer_functions(ctl_engine_stats_t, engineStatsP);
%pointer_functions(ctl_mem_state_t, memStateP);
%pointer_functions(ctl_power_telemetry_t, powerTelemetryP);
%pointer_functions(ctl_i2c_access_args_t, i2cAccessArgsP);
%pointer_functions(ctl_aux_access_args_t, auxAccessArgsP);
%pointer_functions(ctl_panel_descriptor_access_args_t, panelDescriptorArgsP);
%pointer_functions(ctl_dce_args_t, dceArgsP);
%pointer_functions(ctl_wait_property_change_args_t, waitPropertyChangeArgsP);

// ----- Bring in the IGCL public API surface -----
// Note: Order matters a bit for enums/structs dependencies; include the main header last
// after typemap scaffolding so SWIG sees the macros/typedefs above.

%include "../drivers.gpu.control-library/include/igcl_api.h"

// ----- Automatic Structure Initialization for IGCL API Structures -----
// This macro adds a constructor to each IGCL structure that automatically initializes
// the Size and Version fields, preventing common initialization errors in C#.
//
// IMPORTANT: This must come AFTER %include of igcl_api.h so structures are defined!
//
// Usage in C#:
//   var props = new ctl_display_properties_t(); // Size and Version are auto-initialized!
//   IGCL.ctlGetDisplayProperties(hDisplay, props);
//
%define AUTO_INIT_IGCL_STRUCT(StructName, DefaultVersion)
%extend _##StructName {
    _##StructName() {
        _##StructName *s = (_##StructName *)calloc(1, sizeof(_##StructName));
        if (s) {
            s->Size = sizeof(_##StructName);
            s->Version = DefaultVersion;
        }
  return s;
    }
}
%enddef

// Apply automatic initialization to all IGCL API structures with Size/Version fields
// Only include structures that actually exist in igcl_api.h
AUTO_INIT_IGCL_STRUCT(ctl_init_args_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_device_adapter_properties_t, 1)
AUTO_INIT_IGCL_STRUCT(ctl_display_properties_t, 1)
AUTO_INIT_IGCL_STRUCT(ctl_3d_feature_caps_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_3d_feature_getset_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_sharpness_caps_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_sharpness_settings_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_i2c_access_args_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_aux_access_args_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_power_optimization_caps_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_power_optimization_settings_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_set_brightness_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_get_brightness_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_pixtx_pipe_get_config_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_pixtx_pipe_set_config_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_pixtx_1dlut_config_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_pixtx_3dlut_config_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_panel_descriptor_access_args_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_retro_scaling_caps_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_retro_scaling_settings_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_scaling_caps_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_scaling_settings_t, 1)
AUTO_INIT_IGCL_STRUCT(ctl_intel_arc_sync_monitor_params_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_intel_arc_sync_profile_params_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_edid_management_args_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_get_set_custom_mode_args_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_combined_display_args_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_engine_properties_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_engine_stats_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_fan_properties_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_fan_config_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_fan_speed_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_fan_speed_table_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_video_processing_feature_caps_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_video_processing_feature_getset_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_mem_properties_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_mem_state_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_mem_bandwidth_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_power_properties_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_power_energy_counter_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_power_limits_t, 0)
AUTO_INIT_IGCL_STRUCT(ctl_power_telemetry_t, 0)

// ---------- Optional: small C helpers for safer C# usage ----------

// ----- C# Helper Functions (defined inline for SWIG wrapping) -----
// These helpers simplify common initialization and enumeration patterns from C#
%inline %{

// Initialize IGCL with default settings
// Returns the initialized API handle through pApiHandle
ctl_result_t IGCL_InitDefault(ctl_api_handle_t *pApiHandle)
{
    ctl_init_args_t initArgs;
    memset(&initArgs, 0, sizeof(initArgs));
    initArgs.Size = sizeof(ctl_init_args_t);
    initArgs.Version = 0;
    initArgs.AppVersion = CTL_MAKE_VERSION(CTL_IMPL_MAJOR_VERSION, CTL_IMPL_MINOR_VERSION);
    initArgs.flags = CTL_INIT_FLAG_USE_LEVEL_ZERO;
    initArgs.SupportedVersion = CTL_IMPL_VERSION;
    return ctlInit(&initArgs, pApiHandle);
}

// Close/cleanup IGCL API handle
ctl_result_t IGCL_Close(ctl_api_handle_t hApiHandle)
{
    return ctlClose(hApiHandle);
}

// Enumerate all GPU adapters
// First call with pAdapters=NULL to get count, second call to get array
ctl_result_t IGCL_EnumerateAdapters(ctl_api_handle_t hApiHandle, uint32_t *pCount, ctl_device_adapter_handle_t *pAdapters)
{
    return ctlEnumerateDevices(hApiHandle, pCount, pAdapters);
}

// Enumerate displays attached to an adapter
// First call with pDisplays=NULL to get count, second call to get array
ctl_result_t IGCL_EnumerateDisplays(ctl_device_adapter_handle_t hAdapter, uint32_t *pCount, ctl_display_output_handle_t *pDisplays)
{
    return ctlEnumerateDisplayOutputs(hAdapter, pCount, pDisplays);
}

// Get adapter properties (GPU info)
ctl_result_t IGCL_GetAdapterProperties(ctl_device_adapter_handle_t hAdapter, ctl_device_adapter_properties_t *pProps)
{
    if (!pProps) return CTL_RESULT_ERROR_INVALID_NULL_POINTER;
    memset(pProps, 0, sizeof(ctl_device_adapter_properties_t));
    pProps->Size = sizeof(ctl_device_adapter_properties_t);
    pProps->Version = 1;
    return ctlGetDeviceProperties(hAdapter, pProps);
}

// Get display properties (monitor info)
ctl_result_t IGCL_GetDisplayProperties(ctl_display_output_handle_t hDisplay, ctl_display_properties_t *pProps)
{
    if (!pProps) return CTL_RESULT_ERROR_INVALID_NULL_POINTER;
    memset(pProps, 0, sizeof(ctl_display_properties_t));
pProps->Size = sizeof(ctl_display_properties_t);
    pProps->Version = 1;
    return ctlGetDisplayProperties(hDisplay, pProps);
}

%}

