using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnSwitchMux_t([NativeTypeName("ctl_mux_output_handle_t")] _ctl_mux_output_handle_t* param0, [NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* param1);
}
