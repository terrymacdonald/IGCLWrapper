using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnTemperatureGetProperties_t([NativeTypeName("ctl_temp_handle_t")] _ctl_temp_handle_t* param0, [NativeTypeName("ctl_temp_properties_t *")] _ctl_temp_properties_t* param1);
}
