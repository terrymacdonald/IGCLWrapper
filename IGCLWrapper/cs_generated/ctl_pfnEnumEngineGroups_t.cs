using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnEnumEngineGroups_t([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* param0, [NativeTypeName("uint32_t *")] uint* param1, [NativeTypeName("ctl_engine_handle_t *")] _ctl_engine_handle_t** param2);
}
