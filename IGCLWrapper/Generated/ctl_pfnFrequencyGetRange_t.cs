using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnFrequencyGetRange_t([NativeTypeName("ctl_freq_handle_t")] _ctl_freq_handle_t* param0, [NativeTypeName("ctl_freq_range_t *")] _ctl_freq_range_t* param1);
}
