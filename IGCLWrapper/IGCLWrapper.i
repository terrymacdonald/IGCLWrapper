// IGCLWrapper (c) Terry MacDonald 2025
//-------------------------------------------------------------------------------------------------
// Purpose: SWIG interface for Intel Graphics Control Library (IGCL) C API -> C# bindings
// Notes : IGCL is a C API (igcl_api.h). We optionally use Intel's cApiWrapper.cpp for dynamic loading.
//         Keep handle types opaque in C#, surface helper pointer typedefs for array/out params.

// ----- SWIG module & language options -----
%module(directors="1") IGCL
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

// ---------- Optional: small C helpers for safer C# usage ----------
// Examples: tiny wrappers that avoid double-pointer gymnastics from C#.
// These are pure conveniences; you can remove if you prefer direct P/Invoke-like signatures.

%inline %{
// Initialize IGCL with default flags (Level Zero enabled)
static ctl_result_t IGCL_InitDefault(ctl_api_handle_t* phAPI) {
    if (!phAPI) return CTL_RESULT_ERROR_INVALID_NULL_POINTER;

    ctl_init_args_t args = {};
    args.Size  = sizeof(args);
    // Some SDKs don’t expose CTL_INIT_VERSION_* macros. Use a safe fallback.
    #ifdef CTL_INIT_VERSION_MAJOR
      args.Version = CTL_MAKE_VERSION(CTL_INIT_VERSION_MAJOR, CTL_INIT_VERSION_MINOR);
    #else
      args.Version = 0; // accepted by current runtimes
    #endif
    args.flags = CTL_INIT_FLAG_USE_LEVEL_ZERO;
    ZeroMemory(&args.ApplicationUID, sizeof(args.ApplicationUID));

    return ctlInit(&args, phAPI);
}

// Enumerate adapters: returns count and fills pre-allocated array
static ctl_result_t IGCL_EnumerateAdapters(ctl_api_handle_t hAPI,
                                           igcl_uint32* pCount,
                                           ctl_device_adapter_handle_t* pAdapters) {
    return ctlEnumerateDevices(hAPI, pCount, pAdapters);
}

// Enumerate displays for an adapter
static ctl_result_t IGCL_EnumerateDisplays(ctl_device_adapter_handle_t hAdapter,
                                           igcl_uint32* pCount,
                                           ctl_display_output_handle_t* pDisplays) {
    return ctlEnumerateDisplayOutputs(hAdapter, pCount, pDisplays);
}

// Get display properties with size set
static ctl_result_t IGCL_GetDisplayProperties(ctl_display_output_handle_t hDisplay,
                                              ctl_display_properties_t* pProps) {
    if (!pProps) return CTL_RESULT_ERROR_INVALID_NULL_POINTER;
    pProps->Size = sizeof(*pProps);
    pProps->Version = 1; // or CTL_CURRENT_VERSION
    return ctlGetDisplayProperties(hDisplay, pProps);
}

// Get adapter properties with size set
static ctl_result_t IGCL_GetAdapterProperties(ctl_device_adapter_handle_t hAdapter,
                                              ctl_device_adapter_properties_t* pProps) {
    if (!pProps) return CTL_RESULT_ERROR_INVALID_NULL_POINTER;
    pProps->Size = sizeof(*pProps);
    pProps->Version = 1;
    return ctlGetDeviceProperties(hAdapter, pProps);
}

// Helper for I2C access with buffer sizing
static ctl_result_t IGCL_I2CAccess(ctl_display_output_handle_t hDisplay,
                                   ctl_i2c_access_args_t* pArgs) {
    if (!pArgs) return CTL_RESULT_ERROR_INVALID_NULL_POINTER;
    pArgs->Size = sizeof(*pArgs);
    return ctlI2CAccess(hDisplay, pArgs);
}

// Helper for AUX access with buffer sizing
static ctl_result_t IGCL_AUXAccess(ctl_display_output_handle_t hDisplay,
                                   ctl_aux_access_args_t* pArgs) {
    if (!pArgs) return CTL_RESULT_ERROR_INVALID_NULL_POINTER;
    pArgs->Size = sizeof(*pArgs);
    return ctlAUXAccess(hDisplay, pArgs);
}

// Two-phase read of panel descriptor (EDID or panel data)
static ctl_result_t IGCL_GetPanelDescriptor(ctl_display_output_handle_t hDisplay,
                                            ctl_panel_descriptor_access_args_t* pArgs,
                                            uint8_t** pBuffer) {
    if (!pArgs || !pBuffer) return CTL_RESULT_ERROR_INVALID_NULL_POINTER;
    pArgs->Size = sizeof(*pArgs);
    // Phase 1: query size
    pArgs->DescriptorDataSize = 0;
    pArgs->pDescriptorData = nullptr;
    ctl_result_t result = ctlPanelDescriptorAccess(hDisplay, pArgs);
    if (result != CTL_RESULT_SUCCESS || pArgs->DescriptorDataSize == 0) return result;
    // Allocate buffer
    *pBuffer = (uint8_t*)malloc(pArgs->DescriptorDataSize);
    if (!(*pBuffer)) return CTL_RESULT_ERROR_OUT_OF_HOST_MEMORY;
    // Phase 2: actual read
    return ctlPanelDescriptorAccess(hDisplay, pArgs);
}


static ctl_result_t IGCL_GetSetDCE(ctl_display_output_handle_t hDisplay,
                                   ctl_dce_args_t* pArgs) {
    if (!pArgs) return CTL_RESULT_ERROR_INVALID_NULL_POINTER;
    pArgs->Size = sizeof(*pArgs);
    pArgs->Version = 1; // depending on spec
    return ctlGetSetDynamicContrastEnhancement(hDisplay, pArgs);
}

static ctl_result_t IGCL_EnumDisplays(ctl_device_adapter_handle_t hAdapter,
                                      igcl_uint32* pCount,
                                      ctl_display_output_handle_t** ppDisplays) {
    if (!pCount || !ppDisplays) return CTL_RESULT_ERROR_INVALID_NULL_POINTER;
    *ppDisplays = nullptr;
    ctl_result_t res = ctlEnumerateDisplayOutputs(hAdapter, pCount, nullptr);
    if (res != CTL_RESULT_SUCCESS || !*pCount) return res;
    *ppDisplays = (ctl_display_output_handle_t*)malloc(sizeof(ctl_display_output_handle_t) * *pCount);
    if (!*ppDisplays) return CTL_RESULT_ERROR_OUT_OF_HOST_MEMORY;
    return ctlEnumerateDisplayOutputs(hAdapter, pCount, *ppDisplays);
}

// Cleanup API handle
static ctl_result_t IGCL_Close(ctl_api_handle_t hAPI) {
    return ctlClose(hAPI);
}

// Free malloc-allocated buffer
static void IGCL_FreeBuffer(void* ptr) {
    if (ptr) free(ptr);
}

// Block until a display property changes
static ctl_result_t IGCL_WaitForDisplayChange(ctl_device_adapter_handle_t hAdapter,
                                              ctl_wait_property_change_args_t* pArgs) {
    if (!pArgs) return CTL_RESULT_ERROR_INVALID_NULL_POINTER;
    pArgs->Size = sizeof(*pArgs);
    return ctlWaitForPropertyChange(hAdapter, pArgs);
}

%}

// ---------- Directors / callbacks ----------
// IGCL is a C API; event/callbacks (if used) are function-pointer based,
// so SWIG directors are not typically required. If you add callback trampolines,
// declare them here and supply C glue that forwards to managed delegates.

// ---------- Notes ----------
// 1) Keep using Intel's cApiWrapper for runtime DLL loading in the native project.
//    From C#, you call into our wrapper DLL; it, in turn, loads the correct IGCL runtime.
// 2) If you need array marshaling helpers (e.g., gamma ramps, 3DLUT tables),
//    add more %pointer_functions or %apply typemaps for the concrete structs coming from igcl_api.h.
// 3) If you enable AUX/I2C access in C#, ensure admin/privilege requirements on the host.
// 4) Always set Size fields in IGCL structs before calling getters.

