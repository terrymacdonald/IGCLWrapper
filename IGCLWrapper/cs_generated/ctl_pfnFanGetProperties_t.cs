using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnFanGetProperties_t([NativeTypeName("ctl_fan_handle_t")] _ctl_fan_handle_t* param0, [NativeTypeName("ctl_fan_properties_t *")] _ctl_fan_properties_t* param1);
}
