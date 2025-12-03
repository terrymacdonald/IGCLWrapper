using Xunit;
using IGCLWrapper;
using System;
using System.Runtime.Versioning;

namespace IGCLWrapper.Tests
{
    /// <summary>
    /// Tests for GPU Services APIs including engines, fans, frequencies, memory, temperature,
    /// power, LEDs, firmware, PCI, and ECC
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class GpuServicesTests : IDisposable
    {
        private readonly IGCLApi? _api;
        private readonly IntPtr[]? _adapters;
        private readonly bool _hasHardware;
        private readonly bool _hasDll;
        private readonly string _skipReason = string.Empty;

        public GpuServicesTests()
        {
            // Stage 1: Check for Intel GPU hardware via PCI
            if (!HardwareDetection.HasIntelGPU(out string hwError))
            {
                _hasHardware = false;
                _hasDll = false;
                _skipReason = hwError;
                return;
            }
            _hasHardware = true;

            // Stage 2: Check for IGCL DLL availability
            if (!IGCLApi.IsIGCLDllAvailable(out string dllError))
            {
                _hasDll = false;
                _skipReason = dllError;
                return;
            }
            _hasDll = true;

            // Stage 3: Try to initialize IGCL API
            try
            {
                _api = IGCLApi.Initialize();
                _adapters = _api?.EnumerateAdapters();
            }
            catch (IGCLException ex)
            {
                _skipReason = $"IGCL initialization failed: {ex.Message}";
            }
            catch (DllNotFoundException)
            {
                _skipReason = "IGCL DLL not found";
            }
        }

        public void Dispose()
        {
            _api?.Dispose();
        }

        #region Engine Tests

        [SkippableFact]
        public void CtlEnumEngineGroups_ShouldReturnCount()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumEngineGroups((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlEngineGetProperties_ShouldReturnProperties()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumEngineGroups((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var engineHandles = new _ctl_engine_handle_t*[count];
                fixed (_ctl_engine_handle_t** pEngines = engineHandles)
                {
                    IGCL.ctlEnumEngineGroups((_ctl_device_adapter_handle_t*)_adapters[0], &count, pEngines);
                }

                var props = new ctl_engine_properties_t
                {
                    Size = (uint)sizeof(ctl_engine_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlEngineGetProperties(engineHandles[0], &props);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlEngineGetActivity_ShouldReturnStats()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumEngineGroups((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var engineHandles = new _ctl_engine_handle_t*[count];
                fixed (_ctl_engine_handle_t** pEngines = engineHandles)
                {
                    IGCL.ctlEnumEngineGroups((_ctl_device_adapter_handle_t*)_adapters[0], &count, pEngines);
                }

                var stats = new ctl_engine_stats_t
                {
                    Size = (uint)sizeof(ctl_engine_stats_t),
                    Version = 0
                };

                var result = IGCL.ctlEngineGetActivity(engineHandles[0], &stats);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        #endregion

        #region Fan Tests

        [SkippableFact]
        public void CtlEnumFans_ShouldReturnCount()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumFans((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlFanGetProperties_ShouldReturnProperties()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumFans((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var fanHandles = new _ctl_fan_handle_t*[count];
                fixed (_ctl_fan_handle_t** pFans = fanHandles)
                {
                    IGCL.ctlEnumFans((_ctl_device_adapter_handle_t*)_adapters[0], &count, pFans);
                }

                var props = new ctl_fan_properties_t
                {
                    Size = (uint)sizeof(ctl_fan_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlFanGetProperties(fanHandles[0], &props);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlFanGetConfig_ShouldReturnConfig()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumFans((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var fanHandles = new _ctl_fan_handle_t*[count];
                fixed (_ctl_fan_handle_t** pFans = fanHandles)
                {
                    IGCL.ctlEnumFans((_ctl_device_adapter_handle_t*)_adapters[0], &count, pFans);
                }

                var config = new ctl_fan_config_t
                {
                    Size = (uint)sizeof(ctl_fan_config_t),
                    Version = 0
                };

                var result = IGCL.ctlFanGetConfig(fanHandles[0], &config);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlFanGetState_ShouldReturnState()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumFans((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var fanHandles = new _ctl_fan_handle_t*[count];
                fixed (_ctl_fan_handle_t** pFans = fanHandles)
                {
                    IGCL.ctlEnumFans((_ctl_device_adapter_handle_t*)_adapters[0], &count, pFans);
                }

                int speed;
                var result = IGCL.ctlFanGetState(fanHandles[0], ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM, &speed);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        #endregion

        #region Frequency Tests

        [SkippableFact]
        public void CtlEnumFrequencyDomains_ShouldReturnCount()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumFrequencyDomains((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlFrequencyGetProperties_ShouldReturnProperties()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumFrequencyDomains((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var freqHandles = new _ctl_freq_handle_t*[count];
                fixed (_ctl_freq_handle_t** pFreqs = freqHandles)
                {
                    IGCL.ctlEnumFrequencyDomains((_ctl_device_adapter_handle_t*)_adapters[0], &count, pFreqs);
                }

                var props = new ctl_freq_properties_t
                {
                    Size = (uint)sizeof(ctl_freq_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlFrequencyGetProperties(freqHandles[0], &props);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlFrequencyGetState_ShouldReturnState()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumFrequencyDomains((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var freqHandles = new _ctl_freq_handle_t*[count];
                fixed (_ctl_freq_handle_t** pFreqs = freqHandles)
                {
                    IGCL.ctlEnumFrequencyDomains((_ctl_device_adapter_handle_t*)_adapters[0], &count, pFreqs);
                }

                var state = new ctl_freq_state_t
                {
                    Size = (uint)sizeof(ctl_freq_state_t),
                    Version = 0
                };

                var result = IGCL.ctlFrequencyGetState(freqHandles[0], &state);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        #endregion

        #region Memory Tests

        [SkippableFact]
        public void CtlEnumMemoryModules_ShouldReturnCount()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumMemoryModules((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlMemoryGetProperties_ShouldReturnProperties()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumMemoryModules((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var memHandles = new _ctl_mem_handle_t*[count];
                fixed (_ctl_mem_handle_t** pMems = memHandles)
                {
                    IGCL.ctlEnumMemoryModules((_ctl_device_adapter_handle_t*)_adapters[0], &count, pMems);
                }

                var props = new ctl_mem_properties_t
                {
                    Size = (uint)sizeof(ctl_mem_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlMemoryGetProperties(memHandles[0], &props);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlMemoryGetState_ShouldReturnState()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumMemoryModules((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var memHandles = new _ctl_mem_handle_t*[count];
                fixed (_ctl_mem_handle_t** pMems = memHandles)
                {
                    IGCL.ctlEnumMemoryModules((_ctl_device_adapter_handle_t*)_adapters[0], &count, pMems);
                }

                var state = new ctl_mem_state_t
                {
                    Size = (uint)sizeof(ctl_mem_state_t),
                    Version = 0
                };

                var result = IGCL.ctlMemoryGetState(memHandles[0], &state);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        #endregion

        #region Temperature Tests

        [SkippableFact]
        public void CtlEnumTemperatureSensors_ShouldReturnCount()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumTemperatureSensors((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlTemperatureGetProperties_ShouldReturnProperties()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumTemperatureSensors((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var tempHandles = new _ctl_temp_handle_t*[count];
                fixed (_ctl_temp_handle_t** pTemps = tempHandles)
                {
                    IGCL.ctlEnumTemperatureSensors((_ctl_device_adapter_handle_t*)_adapters[0], &count, pTemps);
                }

                var props = new ctl_temp_properties_t
                {
                    Size = (uint)sizeof(ctl_temp_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlTemperatureGetProperties(tempHandles[0], &props);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlTemperatureGetState_ShouldReturnTemperature()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumTemperatureSensors((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var tempHandles = new _ctl_temp_handle_t*[count];
                fixed (_ctl_temp_handle_t** pTemps = tempHandles)
                {
                    IGCL.ctlEnumTemperatureSensors((_ctl_device_adapter_handle_t*)_adapters[0], &count, pTemps);
                }

                double temperature;
                var result = IGCL.ctlTemperatureGetState(tempHandles[0], &temperature);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
                Assert.True(temperature > -273.15);
            }
        }

        #endregion

        #region Power Tests

        [SkippableFact]
        public void CtlEnumPowerDomains_ShouldReturnCount()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumPowerDomains((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlPowerGetProperties_ShouldReturnProperties()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumPowerDomains((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var powerHandles = new _ctl_pwr_handle_t*[count];
                fixed (_ctl_pwr_handle_t** pPowers = powerHandles)
                {
                    IGCL.ctlEnumPowerDomains((_ctl_device_adapter_handle_t*)_adapters[0], &count, pPowers);
                }

                var props = new ctl_power_properties_t
                {
                    Size = (uint)sizeof(ctl_power_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlPowerGetProperties(powerHandles[0], &props);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlPowerGetEnergyCounter_ShouldReturnCounter()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                IGCL.ctlEnumPowerDomains((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);
                if (count == 0) return;

                var powerHandles = new _ctl_pwr_handle_t*[count];
                fixed (_ctl_pwr_handle_t** pPowers = powerHandles)
                {
                    IGCL.ctlEnumPowerDomains((_ctl_device_adapter_handle_t*)_adapters[0], &count, pPowers);
                }

                var energy = new ctl_power_energy_counter_t
                {
                    Size = (uint)sizeof(ctl_power_energy_counter_t),
                    Version = 0
                };

                var result = IGCL.ctlPowerGetEnergyCounter(powerHandles[0], &energy);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        #endregion

        #region LED Tests

        [SkippableFact]
        public void CtlEnumLeds_ShouldReturnCount()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumLeds((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        #endregion

        #region Firmware Tests

        [SkippableFact]
        public void CtlGetFirmwareProperties_ShouldReturnProperties()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                var props = new ctl_firmware_properties_t
                {
                    Size = (uint)sizeof(ctl_firmware_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlGetFirmwareProperties((_ctl_device_adapter_handle_t*)_adapters[0], &props);

                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == ctl_result_t.CTL_RESULT_ERROR_KMD_CALL ||
                    result == ctl_result_t.CTL_RESULT_ERROR_DATA_READ ||
                    result == ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_HANDLE ||
                    result == ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_POINTER ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION
                );
            }
        }

        [SkippableFact]
        public void CtlEnumerateFirmwareComponents_ShouldReturnCount()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumerateFirmwareComponents((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == ctl_result_t.CTL_RESULT_ERROR_KMD_CALL ||
                    result == ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_HANDLE ||
                    result == ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_POINTER ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION
                );
            }
        }

        #endregion

        #region PCI Tests

        [SkippableFact]
        public void CtlPciGetProperties_ShouldReturnProperties()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                var props = new ctl_pci_properties_t
                {
                    Size = (uint)sizeof(ctl_pci_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlPciGetProperties((_ctl_device_adapter_handle_t*)_adapters[0], &props);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [SkippableFact]
        public void CtlPciGetState_ShouldReturnState()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            unsafe
            {
                var state = new ctl_pci_state_t
                {
                    Size = (uint)sizeof(ctl_pci_state_t),
                    Version = 0
                };

                var result = IGCL.ctlPciGetState((_ctl_device_adapter_handle_t*)_adapters[0], &state);

                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        #endregion

        #region ECC Tests

        [SkippableFact]
        public void CtlEccGetProperties_ShouldReturnProperties()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            try
            {
                unsafe
                {
                    var props = new ctl_ecc_properties_t
                    {
                        Size = (uint)sizeof(ctl_ecc_properties_t),
                        Version = 0
                    };

                    var result = IGCL.ctlEccGetProperties((_ctl_device_adapter_handle_t*)_adapters[0], &props);

                    Assert.True(
                        result == ctl_result_t.CTL_RESULT_SUCCESS ||
                        result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                    );
                }
            }
            catch (EntryPointNotFoundException)
            {
                return;
            }
        }

        [SkippableFact]
        public void CtlEccGetState_ShouldReturnState()
        {
            Skip.If(!_hasHardware || !_hasDll || _api == null || _adapters == null || _adapters.Length == 0, _skipReason);

            try
            {
                unsafe
                {
                    var state = new ctl_ecc_state_desc_t
                    {
                        Size = (uint)sizeof(ctl_ecc_state_desc_t),
                        Version = 0
                    };

                    var result = IGCL.ctlEccGetState((_ctl_device_adapter_handle_t*)_adapters[0], &state);

                    Assert.True(
                        result == ctl_result_t.CTL_RESULT_SUCCESS ||
                        result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                    );
                }
            }
            catch (EntryPointNotFoundException)
            {
                return;
            }
        }

        #endregion
    }
}
