using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnPowerTelemetryGet_t([NativeTypeName("ctl_device_adapter_handle_t")] _ctl_device_adapter_handle_t* param0, [NativeTypeName("ctl_power_telemetry_t *")] _ctl_power_telemetry_t* param1);
}
