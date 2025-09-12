using Xunit;
using System;
using IGCLWrapper; // Use the new bindings project

namespace IGCLWrapper.Tests
{
    public class GpuServicesTests : IDisposable
    {
        private ctl_api_handle_t _apiHandle;
        private ctl_device_adapter_handle_t[] _adapters;

        public GpuServicesTests()
        {
            InitializeIGCL();
        }

        public void Dispose()
        {
            CleanupIGCL();
        }

        private void InitializeIGCL()
        {
            // Initialize IGCL API
            ctl_init_args_t initArgs = new ctl_init_args_t();
            initArgs.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(initArgs);
            initArgs.Version = 0; // Use default version
            initArgs.flags = ctl_init_flags_t.CTL_INIT_FLAG_USE_LEVEL_ZERO;
            initArgs.AppVersion = 0;
            initArgs.SupportedVersion = 0;

            ctl_result_t result = IGCL.ctlInit(ref initArgs, ref _apiHandle);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            Assert.NotEqual(IntPtr.Zero, _apiHandle);

            // Enumerate adapters
            uint adapterCount = 0;
            result = IGCL.ctlEnumerateDevices(_apiHandle, ref adapterCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            Assert.True(adapterCount > 0, "No adapters found");

            _adapters = new ctl_device_adapter_handle_t[adapterCount];
            result = IGCL.ctlEnumerateDevices(_apiHandle, ref adapterCount, _adapters);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }

        private void CleanupIGCL()
        {
            if (_apiHandle != IntPtr.Zero)
            {
                IGCL.ctlClose(_apiHandle);
                _apiHandle = IntPtr.Zero;
            }
        }

        [Fact]
        public void GetZeDevice_ShouldReturnValidHandle()
        {
            IntPtr zeDevice = IntPtr.Zero;
            IntPtr instance = IntPtr.Zero;

            ctl_result_t result = IGCL.ctlGetZeDevice(_adapters[0], ref zeDevice, ref instance);
            // Level Zero device may not be available on all systems
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_ZE_LOADER ||
                       result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
        }

        [Fact]
        public void GetEngineActivity_ShouldReturnValidStats()
        {
            uint engineCount = 0;
            ctl_result_t result = IGCL.ctlEnumEngineGroups(_adapters[0], ref engineCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (engineCount > 0)
            {
                ctl_engine_handle_t[] engines = new ctl_engine_handle_t[engineCount];
                result = IGCL.ctlEnumEngineGroups(_adapters[0], ref engineCount, engines);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting activity of first engine
                ctl_engine_stats_t stats = new ctl_engine_stats_t();
                stats.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(stats);
                stats.Version = 1;

                result = IGCL.ctlEngineGetActivity(engines[0], ref stats);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Verify stats are reasonable
                Assert.True(stats.activeTime >= 0);
                Assert.True(stats.timestamp >= 0);
            }
        }

        [Fact]
        public void GetFrequencyState_ShouldReturnValidState()
        {
            uint freqCount = 0;
            ctl_result_t result = IGCL.ctlEnumFrequencyDomains(_adapters[0], ref freqCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (freqCount > 0)
            {
                ctl_freq_handle_t[] freqDomains = new ctl_freq_handle_t[freqCount];
                result = IGCL.ctlEnumFrequencyDomains(_adapters[0], ref freqCount, freqDomains);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting state of first frequency domain
                ctl_freq_state_t state = new ctl_freq_state_t();
                state.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(state);
                state.Version = 1;

                result = IGCL.ctlFrequencyGetState(freqDomains[0], ref state);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Verify frequency values are reasonable
                Assert.True(state.request >= 0);
                Assert.True(state.actual >= 0);
                Assert.True(state.tdp >= 0);
            }
        }

        [Fact]
        public void GetFrequencyRange_ShouldReturnValidRange()
        {
            uint freqCount = 0;
            ctl_result_t result = IGCL.ctlEnumFrequencyDomains(_adapters[0], ref freqCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (freqCount > 0)
            {
                ctl_freq_handle_t[] freqDomains = new ctl_freq_handle_t[freqCount];
                result = IGCL.ctlEnumFrequencyDomains(_adapters[0], ref freqCount, freqDomains);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting range of first frequency domain
                ctl_freq_range_t range = new ctl_freq_range_t();
                range.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(range);
                range.Version = 1;

                result = IGCL.ctlFrequencyGetRange(freqDomains[0], ref range);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Verify range values are reasonable
                Assert.True(range.min >= 0);
                Assert.True(range.max >= range.min);
            }
        }

        [Fact]
        public void GetFrequencyThrottleTime_ShouldReturnValidTime()
        {
            uint freqCount = 0;
            ctl_result_t result = IGCL.ctlEnumFrequencyDomains(_adapters[0], ref freqCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (freqCount > 0)
            {
                ctl_freq_handle_t[] freqDomains = new ctl_freq_handle_t[freqCount];
                result = IGCL.ctlEnumFrequencyDomains(_adapters[0], ref freqCount, freqDomains);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting throttle time of first frequency domain
                ctl_freq_throttle_time_t throttleTime = new ctl_freq_throttle_time_t();
                throttleTime.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(throttleTime);
                throttleTime.Version = 1;

                result = IGCL.ctlFrequencyGetThrottleTime(freqDomains[0], ref throttleTime);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Verify throttle time values are reasonable
                Assert.True(throttleTime.throttleTime >= 0);
                Assert.True(throttleTime.timestamp >= 0);
            }
        }

        [Fact]
        public void GetMemoryState_ShouldReturnValidState()
        {
            uint memCount = 0;
            ctl_result_t result = IGCL.ctlEnumMemoryModules(_adapters[0], ref memCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (memCount > 0)
            {
                ctl_mem_handle_t[] memModules = new ctl_mem_handle_t[memCount];
                result = IGCL.ctlEnumMemoryModules(_adapters[0], ref memCount, memModules);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting state of first memory module
                ctl_mem_state_t state = new ctl_mem_state_t();
                state.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(state);
                state.Version = 1;

                result = IGCL.ctlMemoryGetState(memModules[0], ref state);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Verify memory values are reasonable
                Assert.True(state.free >= 0);
                Assert.True(state.size > 0);
                Assert.True(state.free <= state.size);
            }
        }

        [Fact]
        public void GetMemoryBandwidth_ShouldReturnValidBandwidth()
        {
            uint memCount = 0;
            ctl_result_t result = IGCL.ctlEnumMemoryModules(_adapters[0], ref memCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (memCount > 0)
            {
                ctl_mem_handle_t[] memModules = new ctl_mem_handle_t[memCount];
                result = IGCL.ctlEnumMemoryModules(_adapters[0], ref memCount, memModules);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting bandwidth of first memory module
                ctl_mem_bandwidth_t bandwidth = new ctl_mem_bandwidth_t();
                bandwidth.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(bandwidth);
                bandwidth.Version = 1;

                result = IGCL.ctlMemoryGetBandwidth(memModules[0], ref bandwidth);
                // Memory bandwidth may require special permissions
                Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                           result == ctl_result_t.CTL_RESULT_ERROR_INSUFFICIENT_PERMISSIONS ||
                           result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);

                if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    Assert.True(bandwidth.maxBandwidth >= 0);
                    Assert.True(bandwidth.timestamp >= 0);
                }
            }
        }

        [Fact]
        public void GetPowerEnergyCounter_ShouldReturnValidCounter()
        {
            uint powerCount = 0;
            ctl_result_t result = IGCL.ctlEnumPowerDomains(_adapters[0], ref powerCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (powerCount > 0)
            {
                ctl_pwr_handle_t[] powerDomains = new ctl_pwr_handle_t[powerCount];
                result = IGCL.ctlEnumPowerDomains(_adapters[0], ref powerCount, powerDomains);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting energy counter of first power domain
                ctl_power_energy_counter_t energyCounter = new ctl_power_energy_counter_t();
                energyCounter.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(energyCounter);
                energyCounter.Version = 1;

                result = IGCL.ctlPowerGetEnergyCounter(powerDomains[0], ref energyCounter);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Verify energy counter values are reasonable
                Assert.True(energyCounter.energy >= 0);
                Assert.True(energyCounter.timestamp >= 0);
            }
        }

        [Fact]
        public void GetPowerLimits_ShouldReturnValidLimits()
        {
            uint powerCount = 0;
            ctl_result_t result = IGCL.ctlEnumPowerDomains(_adapters[0], ref powerCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (powerCount > 0)
            {
                ctl_pwr_handle_t[] powerDomains = new ctl_pwr_handle_t[powerCount];
                result = IGCL.ctlEnumPowerDomains(_adapters[0], ref powerCount, powerDomains);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting power limits of first power domain
                ctl_power_limits_t limits = new ctl_power_limits_t();
                limits.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(limits);
                limits.Version = 1;

                result = IGCL.ctlPowerGetLimits(powerDomains[0], ref limits);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Verify power limit values are reasonable
                Assert.True(limits.sustainedPowerLimit.power >= 0);
                Assert.True(limits.burstPowerLimit.power >= 0);
            }
        }

        [Fact]
        public void GetTemperatureState_ShouldReturnValidTemperature()
        {
            uint tempCount = 0;
            ctl_result_t result = IGCL.ctlEnumTemperatureSensors(_adapters[0], ref tempCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (tempCount > 0)
            {
                ctl_temp_handle_t[] tempSensors = new ctl_temp_handle_t[tempCount];
                result = IGCL.ctlEnumTemperatureSensors(_adapters[0], ref tempCount, tempSensors);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting temperature of first sensor
                double temperature = 0.0;
                result = IGCL.ctlTemperatureGetState(tempSensors[0], ref temperature);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Verify temperature is in reasonable range (-50°C to 150°C)
                Assert.True(temperature >= -50.0 && temperature <= 150.0);
            }
        }

        [Fact]
        public void GetPowerTelemetry_ShouldReturnValidTelemetry()
        {
            ctl_power_telemetry_t telemetry = new ctl_power_telemetry_t();
            telemetry.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(telemetry);
            telemetry.Version = 1;

            ctl_result_t result = IGCL.ctlPowerTelemetryGet(_adapters[0], ref telemetry);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            // Verify telemetry timestamp is reasonable
            Assert.True(telemetry.timeStamp >= 0);
        }

        [Fact]
        public void GetOverclockPowerTelemetry_ShouldReturnValidTelemetry()
        {
            ctl_power_telemetry_t telemetry = new ctl_power_telemetry_t();
            telemetry.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(telemetry);
            telemetry.Version = 1;

            ctl_result_t result = IGCL.ctlPowerTelemetryGet(_adapters[0], ref telemetry);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            // Verify GPU power values are reasonable
            Assert.True(telemetry.gpuEnergyCounter >= 0);
            Assert.True(telemetry.gpuCurrentTemperature >= -50.0 && telemetry.gpuCurrentTemperature <= 150.0);
        }

        [Fact]
        public void GetFanState_ShouldWork()
        {
            uint fanCount = 0;
            ctl_result_t result = IGCL.ctlEnumFans(_adapters[0], ref fanCount, null);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS && fanCount > 0)
            {
                ctl_fan_handle_t[] fans = new ctl_fan_handle_t[fanCount];
                result = IGCL.ctlEnumFans(_adapters[0], ref fanCount, fans);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting state of first fan
                int speed = 0;
                result = IGCL.ctlFanGetState(fans[0], ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM, ref speed);
                // Fan state may not be available on all systems
                Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                           result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
            }
        }

        [Fact]
        public void GetLedState_ShouldWork()
        {
            uint ledCount = 0;
            ctl_result_t result = IGCL.ctlEnumLeds(_adapters[0], ref ledCount, null);
            if (result == ctl_result_t.CTL_RESULT_SUCCESS && ledCount > 0)
            {
                ctl_led_handle_t[] leds = new ctl_led_handle_t[ledCount];
                result = IGCL.ctlEnumLeds(_adapters[0], ref ledCount, leds);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting state of first LED
                ctl_led_state_t state = new ctl_led_state_t();
                state.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(state);
                state.Version = 1;

                result = IGCL.ctlLedGetState(leds[0], ref state);
                // LED state may not be available on all systems
                Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                           result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
            }
        }

        [Fact]
        public void GetEccState_ShouldReturnValidState()
        {
            ctl_ecc_state_desc_t state = new ctl_ecc_state_desc_t();
            state.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(state);
            state.Version = 1;

            ctl_result_t result = IGCL.ctlEccGetState(_adapters[0], ref state);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            // Verify ECC state is valid
            Assert.True(state.currentEccState == ctl_ecc_state_t.CTL_ECC_STATE_ECC_ENABLED_STATE ||
                       state.currentEccState == ctl_ecc_state_t.CTL_ECC_STATE_ECC_DISABLED_STATE);
        }
    }
}
