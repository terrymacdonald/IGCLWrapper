using Xunit;
using System;
using IGCLWrapper; // Use the new bindings project

namespace IGCLWrapper.Tests
{
    public class SystemServicesTests : IDisposable
    {
        private ctl_api_handle_t _apiHandle;
        private ctl_device_adapter_handle_t[] _adapters;

        public SystemServicesTests()
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
        public void InitializeIGCL_ShouldSucceed()
        {
            // Test already performed in constructor
            Assert.NotEqual(IntPtr.Zero, _apiHandle);
        }

        [Fact]
        public void InitializeIGCLWithDifferentFlags_ShouldWork()
        {
            ctl_api_handle_t testHandle = IntPtr.Zero;

            // Test initialization without Level Zero
            ctl_init_args_t initArgs = new ctl_init_args_t();
            initArgs.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(initArgs);
            initArgs.Version = 0;
            initArgs.flags = 0; // No flags
            initArgs.AppVersion = 0;
            initArgs.SupportedVersion = 0;

            ctl_result_t result = IGCL.ctlInit(ref initArgs, ref testHandle);
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_ZE_LOADER);

            if (testHandle != IntPtr.Zero)
            {
                IGCL.ctlClose(testHandle);
            }
        }

        [Fact]
        public void EnumerateDevices_ShouldReturnValidDeviceCount()
        {
            uint adapterCount = 0;
            ctl_result_t result = IGCL.ctlEnumerateDevices(_apiHandle, ref adapterCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            Assert.True(adapterCount > 0);
        }

        [Fact]
        public void GetDeviceProperties_ShouldReturnValidProperties()
        {
            ctl_device_adapter_properties_t properties = new ctl_device_adapter_properties_t();
            properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
            properties.Version = 1;

            ctl_result_t result = IGCL.ctlGetDeviceProperties(_adapters[0], ref properties);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            // Verify basic properties
            Assert.True(properties.device_id_size > 0);
            Assert.NotEqual(ctl_device_type_t.CTL_DEVICE_TYPE_MAX, properties.device_type);
        }

        [Fact]
        public void CheckDriverVersion_ShouldReturnValidVersion()
        {
            ctl_version_info_t version = 0;
            ctl_result_t result = IGCL.ctlCheckDriverVersion(_adapters[0], version);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }

        [Fact]
        public void GetFirmwareProperties_ShouldReturnValidProperties()
        {
            ctl_firmware_properties_t properties = new ctl_firmware_properties_t();
            properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
            properties.Version = 1;

            ctl_result_t result = IGCL.ctlGetFirmwareProperties(_adapters[0], ref properties);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }

        [Fact]
        public void EnumerateFirmwareComponents_ShouldWork()
        {
            uint componentCount = 0;
            ctl_result_t result = IGCL.ctlEnumerateFirmwareComponents(_adapters[0], ref componentCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (componentCount > 0)
            {
                ctl_firmware_component_handle_t[] components = new ctl_firmware_component_handle_t[componentCount];
                result = IGCL.ctlEnumerateFirmwareComponents(_adapters[0], ref componentCount, components);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting properties of first component
                ctl_firmware_component_properties_t properties = new ctl_firmware_component_properties_t();
                properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
                properties.Version = 1;

                result = IGCL.ctlGetFirmwareComponentProperties(components[0], ref properties);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void GetPCIProperties_ShouldReturnValidProperties()
        {
            ctl_pci_properties_t properties = new ctl_pci_properties_t();
            properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
            properties.Version = 1;

            ctl_result_t result = IGCL.ctlPciGetProperties(_adapters[0], ref properties);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            // Verify PCI properties
            Assert.True(properties.address.bus >= 0);
            Assert.True(properties.address.device >= 0);
            Assert.True(properties.address.function >= 0);
        }

        [Fact]
        public void GetPCIState_ShouldReturnValidState()
        {
            ctl_pci_state_t state = new ctl_pci_state_t();
            state.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(state);
            state.Version = 1;

            ctl_result_t result = IGCL.ctlPciGetState(_adapters[0], ref state);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }

        [Fact]
        public void EnumerateEngineGroups_ShouldReturnValidEngines()
        {
            uint engineCount = 0;
            ctl_result_t result = IGCL.ctlEnumEngineGroups(_adapters[0], ref engineCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (engineCount > 0)
            {
                ctl_engine_handle_t[] engines = new ctl_engine_handle_t[engineCount];
                result = IGCL.ctlEnumEngineGroups(_adapters[0], ref engineCount, engines);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting properties of first engine
                ctl_engine_properties_t properties = new ctl_engine_properties_t();
                properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
                properties.Version = 1;

                result = IGCL.ctlEngineGetProperties(engines[0], ref properties);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                Assert.NotEqual(ctl_engine_group_t.CTL_ENGINE_GROUP_MAX, properties.type);
            }
        }

        [Fact]
        public void EnumerateFans_ShouldWork()
        {
            uint fanCount = 0;
            ctl_result_t result = IGCL.ctlEnumFans(_adapters[0], ref fanCount, null);
            // Fans may not be present on all systems
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
        }

        [Fact]
        public void EnumerateFrequencyDomains_ShouldReturnValidDomains()
        {
            uint freqCount = 0;
            ctl_result_t result = IGCL.ctlEnumFrequencyDomains(_adapters[0], ref freqCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (freqCount > 0)
            {
                ctl_freq_handle_t[] freqDomains = new ctl_freq_handle_t[freqCount];
                result = IGCL.ctlEnumFrequencyDomains(_adapters[0], ref freqCount, freqDomains);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting properties of first frequency domain
                ctl_freq_properties_t properties = new ctl_freq_properties_t();
                properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
                properties.Version = 1;

                result = IGCL.ctlFrequencyGetProperties(freqDomains[0], ref properties);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                Assert.NotEqual(ctl_freq_domain_t.CTL_FREQ_DOMAIN_MAX, properties.type);
            }
        }

        [Fact]
        public void EnumerateMemoryModules_ShouldWork()
        {
            uint memCount = 0;
            ctl_result_t result = IGCL.ctlEnumMemoryModules(_adapters[0], ref memCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (memCount > 0)
            {
                ctl_mem_handle_t[] memModules = new ctl_mem_handle_t[memCount];
                result = IGCL.ctlEnumMemoryModules(_adapters[0], ref memCount, memModules);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting properties of first memory module
                ctl_mem_properties_t properties = new ctl_mem_properties_t();
                properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
                properties.Version = 1;

                result = IGCL.ctlMemoryGetProperties(memModules[0], ref properties);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                Assert.NotEqual(ctl_mem_type_t.CTL_MEM_TYPE_UNKNOWN, properties.type);
            }
        }

        [Fact]
        public void EnumeratePowerDomains_ShouldReturnValidDomains()
        {
            uint powerCount = 0;
            ctl_result_t result = IGCL.ctlEnumPowerDomains(_adapters[0], ref powerCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (powerCount > 0)
            {
                ctl_pwr_handle_t[] powerDomains = new ctl_pwr_handle_t[powerCount];
                result = IGCL.ctlEnumPowerDomains(_adapters[0], ref powerCount, powerDomains);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting properties of first power domain
                ctl_power_properties_t properties = new ctl_power_properties_t();
                properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
                properties.Version = 1;

                result = IGCL.ctlPowerGetProperties(powerDomains[0], ref properties);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            }
        }

        [Fact]
        public void EnumerateTemperatureSensors_ShouldReturnValidSensors()
        {
            uint tempCount = 0;
            ctl_result_t result = IGCL.ctlEnumTemperatureSensors(_adapters[0], ref tempCount, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            if (tempCount > 0)
            {
                ctl_temp_handle_t[] tempSensors = new ctl_temp_handle_t[tempCount];
                result = IGCL.ctlEnumTemperatureSensors(_adapters[0], ref tempCount, tempSensors);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                // Test getting properties of first temperature sensor
                ctl_temp_properties_t properties = new ctl_temp_properties_t();
                properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
                properties.Version = 1;

                result = IGCL.ctlTemperatureGetProperties(tempSensors[0], ref properties);
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

                Assert.NotEqual(ctl_temp_sensors_t.CTL_TEMP_SENSORS_MAX, properties.type);
            }
        }

        [Fact]
        public void EnumerateLeds_ShouldWork()
        {
            uint ledCount = 0;
            ctl_result_t result = IGCL.ctlEnumLeds(_adapters[0], ref ledCount, null);
            // LEDs may not be present on all systems
            Assert.True(result == ctl_result_t.CTL_RESULT_SUCCESS ||
                       result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE);
        }

        [Fact]
        public void GetOverclockProperties_ShouldReturnValidProperties()
        {
            ctl_oc_properties_t properties = new ctl_oc_properties_t();
            properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
            properties.Version = 1;

            ctl_result_t result = IGCL.ctlOverclockGetProperties(_adapters[0], ref properties);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }

        [Fact]
        public void GetEccProperties_ShouldReturnValidProperties()
        {
            ctl_ecc_properties_t properties = new ctl_ecc_properties_t();
            properties.Size = (uint)System.Runtime.InteropServices.Marshal.SizeOf(properties);
            properties.Version = 1;

            ctl_result_t result = IGCL.ctlEccGetProperties(_adapters[0], ref properties);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
        }
    }
}
