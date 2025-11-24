using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnI2CAccessOnPinPair_t([NativeTypeName("ctl_i2c_pin_pair_handle_t")] _ctl_i2c_pin_pair_handle_t* param0, [NativeTypeName("ctl_i2c_access_pinpair_args_t *")] _ctl_i2c_access_pinpair_args_t* param1);
}
