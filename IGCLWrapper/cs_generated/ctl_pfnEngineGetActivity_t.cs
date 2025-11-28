using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnEngineGetActivity_t([NativeTypeName("ctl_engine_handle_t")] _ctl_engine_handle_t* param0, [NativeTypeName("ctl_engine_stats_t *")] _ctl_engine_stats_t* param1);
}
