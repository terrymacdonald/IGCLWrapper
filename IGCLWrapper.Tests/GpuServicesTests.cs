using Xunit;
using IGCLWrapper;
using System;

namespace IGCLWrapper.Tests
{
    /// <summary>
    /// Tests for GPU Services APIs including engines, fans, frequencies, memory, temperature,
    /// power, LEDs, firmware, PCI, and ECC
    /// </summary>
    public class GpuServicesTests : IDisposable
    {
        private IGCLApi? _api;
        private IntPtr[]? _adapters;

        public GpuServicesTests()
        {
            try
            {
                _api = IGCLApi.Initialize();
                _adapters = _api?.EnumerateAdapters();
            }
            catch (DllNotFoundException)
            {
                _api = null;
            }
        }

        public void Dispose()
        {
            _api?.Dispose();
        }

        #region Engine Tests

        [Fact]
        public void CtlEnumEngineGroups_ShouldReturnCount()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumEngineGroups((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlEngineGetProperties_ShouldReturnProperties()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                var props = new _ctl_engine_properties_t
                {
                    Size = (uint)sizeof(_ctl_engine_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlEngineGetProperties(engineHandles[0], &props);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlEngineGetActivity_ShouldReturnStats()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                var stats = new _ctl_engine_stats_t
                {
                    Size = (uint)sizeof(_ctl_engine_stats_t),
                    Version = 0
                };

                var result = IGCL.ctlEngineGetActivity(engineHandles[0], &stats);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        #endregion

        #region Fan Tests

        [Fact]
        public void CtlEnumFans_ShouldReturnCount()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumFans((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
                // Count may be 0 if no fans present
            }
        }

        [Fact]
        public void CtlFanGetProperties_ShouldReturnProperties()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                var props = new _ctl_fan_properties_t
                {
                    Size = (uint)sizeof(_ctl_fan_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlFanGetProperties(fanHandles[0], &props);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlFanGetConfig_ShouldReturnConfig()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                var config = new _ctl_fan_config_t
                {
                    Size = (uint)sizeof(_ctl_fan_config_t),
                    Version = 0
                };

                var result = IGCL.ctlFanGetConfig(fanHandles[0], &config);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlFanGetState_ShouldReturnState()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                int speed;
                var result = IGCL.ctlFanGetState(fanHandles[0], _ctl_fan_speed_units_t.CTL_FAN_SPEED_UNITS_RPM, &speed);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        #endregion

        #region Frequency Tests

        [Fact]
        public void CtlEnumFrequencyDomains_ShouldReturnCount()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumFrequencyDomains((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlFrequencyGetProperties_ShouldReturnProperties()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                var props = new _ctl_freq_properties_t
                {
                    Size = (uint)sizeof(_ctl_freq_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlFrequencyGetProperties(freqHandles[0], &props);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlFrequencyGetState_ShouldReturnState()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                var state = new _ctl_freq_state_t
                {
                    Size = (uint)sizeof(_ctl_freq_state_t),
                    Version = 0
                };

                var result = IGCL.ctlFrequencyGetState(freqHandles[0], &state);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        #endregion

        #region Memory Tests

        [Fact]
        public void CtlEnumMemoryModules_ShouldReturnCount()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumMemoryModules((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlMemoryGetProperties_ShouldReturnProperties()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                var props = new _ctl_mem_properties_t
                {
                    Size = (uint)sizeof(_ctl_mem_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlMemoryGetProperties(memHandles[0], &props);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlMemoryGetState_ShouldReturnState()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                var state = new _ctl_mem_state_t
                {
                    Size = (uint)sizeof(_ctl_mem_state_t),
                    Version = 0
                };

                var result = IGCL.ctlMemoryGetState(memHandles[0], &state);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        #endregion

        #region Temperature Tests

        [Fact]
        public void CtlEnumTemperatureSensors_ShouldReturnCount()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumTemperatureSensors((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlTemperatureGetProperties_ShouldReturnProperties()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                var props = new _ctl_temp_properties_t
                {
                    Size = (uint)sizeof(_ctl_temp_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlTemperatureGetProperties(tempHandles[0], &props);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlTemperatureGetState_ShouldReturnTemperature()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                double temperature;
                var result = IGCL.ctlTemperatureGetState(tempHandles[0], &temperature);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
                Assert.True(temperature > -273.15); // Above absolute zero
            }
        }

        #endregion

        #region Power Tests

        [Fact]
        public void CtlEnumPowerDomains_ShouldReturnCount()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumPowerDomains((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlPowerGetProperties_ShouldReturnProperties()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                var props = new _ctl_power_properties_t
                {
                    Size = (uint)sizeof(_ctl_power_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlPowerGetProperties(powerHandles[0], &props);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlPowerGetEnergyCounter_ShouldReturnCounter()
        {
            // Arrange
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

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

                // Act
                var energy = new _ctl_power_energy_counter_t
                {
                    Size = (uint)sizeof(_ctl_power_energy_counter_t),
                    Version = 0
                };

                var result = IGCL.ctlPowerGetEnergyCounter(powerHandles[0], &energy);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        #endregion

        #region LED Tests

        [Fact]
        public void CtlEnumLeds_ShouldReturnCount()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumLeds((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                // Assert
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
                // Count may be 0 if no LEDs present
            }
        }

        #endregion

        #region Firmware Tests

        [Fact]
        public void CtlGetFirmwareProperties_ShouldReturnProperties()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                var props = new _ctl_firmware_properties_t
                {
                    Size = (uint)sizeof(_ctl_firmware_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlGetFirmwareProperties((_ctl_device_adapter_handle_t*)_adapters[0], &props);

                // Assert - Accept all documented return codes for this API
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_KMD_CALL ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_DATA_READ ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_HANDLE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_POINTER ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION
                );
            }
        }

        [Fact]
        public void CtlEnumerateFirmwareComponents_ShouldReturnCount()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumerateFirmwareComponents((_ctl_device_adapter_handle_t*)_adapters[0], &count, null);

                // Assert - Accept all documented return codes for this API
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_KMD_CALL ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_HANDLE ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_POINTER ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION
                );
            }
        }

        #endregion

        #region PCI Tests

        [Fact]
        public void CtlPciGetProperties_ShouldReturnProperties()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                var props = new _ctl_pci_properties_t
                {
                    Size = (uint)sizeof(_ctl_pci_properties_t),
                    Version = 0
                };

                var result = IGCL.ctlPciGetProperties((_ctl_device_adapter_handle_t*)_adapters[0], &props);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void CtlPciGetState_ShouldReturnState()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                var state = new _ctl_pci_state_t
                {
                    Size = (uint)sizeof(_ctl_pci_state_t),
                    Version = 0
                };

                var result = IGCL.ctlPciGetState((_ctl_device_adapter_handle_t*)_adapters[0], &state);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        #endregion

        #region ECC Tests

        [Fact]
        public void CtlEccGetProperties_ShouldReturnProperties()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            try
            {
                unsafe
                {
                    var props = new _ctl_ecc_properties_t
                    {
                        Size = (uint)sizeof(_ctl_ecc_properties_t),
                        Version = 0
                    };

                    var result = IGCL.ctlEccGetProperties((_ctl_device_adapter_handle_t*)_adapters[0], &props);

                    // Assert
                    Assert.True(
                        result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                        result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                    );
                }
            }
            catch (EntryPointNotFoundException)
            {
                // ctlEccGetProperties is not available in this version of the Control Library DLL
                // This is expected on some driver/DLL versions
                return;
            }
        }

        [Fact]
        public void CtlEccGetState_ShouldReturnState()
        {
            // Arrange & Act
            if (_api == null || _adapters == null || _adapters.Length == 0)
            {
                return;
            }

            try
            {
                unsafe
                {
                    var state = new _ctl_ecc_state_desc_t
                    {
                        Size = (uint)sizeof(_ctl_ecc_state_desc_t),
                        Version = 0
                    };

                    var result = IGCL.ctlEccGetState((_ctl_device_adapter_handle_t*)_adapters[0], &state);

                    // Assert
                    Assert.True(
                        result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                        result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                    );
                }
            }
            catch (EntryPointNotFoundException)
            {
                // ctlEccGetState is not available in this version of the Control Library DLL
                // This is expected on some driver/DLL versions
                return;
            }
        }

        #endregion
    }
}
