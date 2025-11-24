using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnSetRuntimePath_t([NativeTypeName("ctl_runtime_path_args_t *")] _ctl_runtime_path_args_t* param0);
}
