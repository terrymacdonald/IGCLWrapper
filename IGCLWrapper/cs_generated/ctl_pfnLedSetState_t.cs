using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnLedSetState_t([NativeTypeName("ctl_led_handle_t")] _ctl_led_handle_t* param0, void* param1, [NativeTypeName("uint32_t")] uint param2);
}
