using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnEnumerateMuxDevices_t([NativeTypeName("ctl_api_handle_t")] _ctl_api_handle_t* param0, [NativeTypeName("uint32_t *")] uint* param1, [NativeTypeName("ctl_mux_output_handle_t *")] _ctl_mux_output_handle_t** param2);
}
