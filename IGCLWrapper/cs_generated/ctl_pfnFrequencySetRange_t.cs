using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnFrequencySetRange_t([NativeTypeName("ctl_freq_handle_t")] _ctl_freq_handle_t* param0, [NativeTypeName("const ctl_freq_range_t *")] _ctl_freq_range_t* param1);
}
