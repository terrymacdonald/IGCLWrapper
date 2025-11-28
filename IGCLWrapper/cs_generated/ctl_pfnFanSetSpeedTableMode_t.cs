using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnFanSetSpeedTableMode_t([NativeTypeName("ctl_fan_handle_t")] _ctl_fan_handle_t* param0, [NativeTypeName("const ctl_fan_speed_table_t *")] _ctl_fan_speed_table_t* param1);
}
