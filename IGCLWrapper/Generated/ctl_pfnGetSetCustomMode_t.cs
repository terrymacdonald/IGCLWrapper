using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnGetSetCustomMode_t([NativeTypeName("ctl_display_output_handle_t")] _ctl_display_output_handle_t* param0, [NativeTypeName("ctl_get_set_custom_mode_args_t *")] _ctl_get_set_custom_mode_args_t* param1);
}
