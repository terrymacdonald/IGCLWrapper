using System.Runtime.InteropServices;

namespace IGCLWrapper
{
    [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
    [return: NativeTypeName("ctl_result_t")]
    public unsafe delegate _ctl_result_t ctl_pfnPowerGetEnergyCounter_t([NativeTypeName("ctl_pwr_handle_t")] _ctl_pwr_handle_t* param0, [NativeTypeName("ctl_power_energy_counter_t *")] _ctl_power_energy_counter_t* param1);
}
