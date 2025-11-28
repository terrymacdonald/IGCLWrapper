using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnOverclockReadVFCurve_t([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* param0, [NativeTypeName("ctl_vf_curve_type_t")] _ctl_vf_curve_type_t param1, [NativeTypeName("ctl_vf_curve_details_t")] _ctl_vf_curve_details_t param2, [NativeTypeName("uint32_t *")] uint* param3, [NativeTypeName("ctl_voltage_frequency_point_t *")] _ctl_voltage_frequency_point_t* param4);
}
