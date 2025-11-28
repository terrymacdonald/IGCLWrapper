using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnMemoryGetBandwidth_t([NativeTypeName("ctl_mem_handle_t")] _ctl_mem_handle_t* param0, [NativeTypeName("ctl_mem_bandwidth_t *")] _ctl_mem_bandwidth_t* param1);
}
