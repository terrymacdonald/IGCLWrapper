using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnInit_t([NativeTypeName("ctl_init_args_t *")] _ctl_init_args_t* param0, [NativeTypeName("ctl_api_handle_t *")] _ctl_api_handle_t** param1);
}
