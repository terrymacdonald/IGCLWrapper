using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnSetIntelArcSyncProfile_t([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* param0, [NativeTypeName("ctl_intel_arc_sync_profile_params_t *")] _ctl_intel_arc_sync_profile_params_t* param1);
}
