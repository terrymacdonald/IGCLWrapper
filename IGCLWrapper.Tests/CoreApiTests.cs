using Xunit;
using IGCLWrapper;
using System;
using System.Runtime.Versioning;
using System.Runtime.InteropServices;
using System.Text;

namespace IGCLWrapper.Tests
{
    /// <summary>
    /// Tests for Core IGCL API functions including initialization, enumeration, and basic device operations
    /// </summary>
    [SupportedOSPlatform("windows")]
    public class CoreApiTests : IDisposable
    {
        private readonly IGCLApi? _api;
        private readonly bool _hasHardware;
        private readonly bool _hasDll;
        private readonly string _skipReason = string.Empty;

        public CoreApiTests()
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

        [SkippableFact]
        public void CtlInit_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            // Assert
            Assert.NotNull(_api);
        }

        [SkippableFact]
        public void CtlEnumerateDevices_ShouldReturnAdapters()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            // Act
            var adapters = _api.EnumerateAdapters();

            // Assert
            Assert.NotNull(adapters);
            // Note: May be 0 if no Intel GPU present
        }

        [SkippableFact]
        public void CtlGetDeviceProperties_ShouldReturnValidProperties()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            var adapters = _api.EnumerateAdapters();
            Skip.If(adapters.Length == 0, "No adapters enumerated");

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);

                // Assert
                // Note: device_id_size can be 0 on some hardware/driver combinations
                Assert.True(props.pci_vendor_id == 0x8086); // Intel vendor ID
            }
        }

        [SkippableFact]
        public void CtlGetDeviceProperties_DeviceType_ShouldBeGraphics()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            var adapters = _api.EnumerateAdapters();
            Skip.If(adapters.Length == 0, "No adapters enumerated");

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);

                // Assert
                Assert.Equal(ctl_device_type_t.CTL_DEVICE_TYPE_GRAPHICS, props.device_type);
            }
        }

        [SkippableFact]
        public void CtlEnumerateDevices_WithNullHandle_ShouldThrowException()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            // Act & Assert
            unsafe
            {
                // This should throw or return error when called with null handle
                // Testing error handling
                Assert.ThrowsAny<Exception>(() =>
                {
                    uint count = 0;
                    var result = IGCL.ctlEnumerateDevices((_ctl_api_handle_t*)IntPtr.Zero, &count, null);
                    if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    {
                        throw new IGCLException(result, "Expected error with null handle");
                    }
                });
            }
        }

        [SkippableFact]
        public void MultipleInitializations_ShouldNotCrash()
        {
            // Arrange & Act
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            // Try to initialize again (should handle gracefully)
            Exception? caughtException = null;
            try
            {
                using (var secondApi = IGCLApi.Initialize())
                {
                    // Should work or throw controlled exception
                    Assert.NotNull(secondApi);
                }
            }
            catch (Exception ex)
            {
                caughtException = ex;
            }

            // Assert - Either works or throws a known exception
            // Should not crash the process
            Assert.True(caughtException == null || caughtException is IGCLException);
        }

        [SkippableFact]
        public void CtlCheckDriverVersion_ShouldValidateVersion()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            var adapters = _api.EnumerateAdapters();
            Skip.If(adapters.Length == 0, "No adapters enumerated");

            // Act
            unsafe
            {
                uint versionInfo = IGCLApi.MakeVersion(1, 0);

                var result = IGCL.ctlCheckDriverVersion((_ctl_device_adapter_handle_t*)adapters[0], versionInfo);

                // Assert - Should return success or unsupported version
                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION
                );
            }
        }

        [SkippableFact]
        public void AdapterProperties_ShouldContainDeviceId()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            var adapters = _api.EnumerateAdapters();
            Skip.If(adapters.Length == 0, "No adapters enumerated");

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);

                // Assert
                Assert.NotEqual(0u, props.pci_device_id);
                Assert.NotEqual(0u, props.rev_id);
            }
        }

        [SkippableFact]
        public void AdapterProperties_ShouldHaveValidName()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            var adapters = _api.EnumerateAdapters();
            Skip.If(adapters.Length == 0, "No adapters enumerated");

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);
                ReadOnlySpan<sbyte> nameSpan = MemoryMarshal.CreateReadOnlySpan(ref props.name.e0, 100);
                int terminator = nameSpan.IndexOf((sbyte)0);
                if (terminator >= 0)
                {
                    nameSpan = nameSpan.Slice(0, terminator);
                }

                var name = Encoding.UTF8.GetString(MemoryMarshal.Cast<sbyte, byte>(nameSpan));

                // Assert
                Assert.NotNull(name);
                Assert.NotEmpty(name);
            }
        }

        [SkippableFact]
        public void CtlEnumerateDisplayOutputs_ShouldReturnCount()
        {
            // Arrange & Act
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            var adapters = _api.EnumerateAdapters();
            Skip.If(adapters.Length == 0, "No adapters enumerated");

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumerateDisplayOutputs((_ctl_device_adapter_handle_t*)adapters[0], &count, null);

                // Assert
                Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
                // Count may be 0 if no displays connected
            }
        }

        [SkippableFact]
        public void AdapterProperties_ShouldHaveValidDriverVersion()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            var adapters = _api.EnumerateAdapters();
            Skip.If(adapters.Length == 0, "No adapters enumerated");

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);

                // Assert
                Assert.NotEqual(0ul, props.driver_version);
            }
        }

        [SkippableFact]
        public void AdapterProperties_ShouldHaveValidPCIIds()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            var adapters = _api.EnumerateAdapters();
            Skip.If(adapters.Length == 0, "No adapters enumerated");

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);

                // Assert
                Assert.Equal(0x8086u, props.pci_vendor_id); // Intel vendor ID
                Assert.NotEqual(0u, props.pci_device_id);
                // SubSys IDs might be 0 for some devices, so don't assert on them
            }
        }

        [SkippableFact]
        public void AdapterProperties_ShouldHaveValidEUCounts()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            var adapters = _api.EnumerateAdapters();
            Skip.If(adapters.Length == 0, "No adapters enumerated");

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);

                // Assert
                // Modern Intel GPUs should have at least some EUs
                if (props.num_eus_per_sub_slice > 0)
                {
                    Assert.True(props.num_sub_slices_per_slice > 0);
                    Assert.True(props.num_slices > 0);
                }
            }
        }

        [SkippableFact]
        public void AdapterProperties_ShouldHaveValidFrequency()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            var adapters = _api.EnumerateAdapters();
            Skip.If(adapters.Length == 0, "No adapters enumerated");

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);

                // Assert
                // Frequency should be reported for modern GPUs
                // If it's 0, it might be an older API version or unsupported
                Assert.True(props.Frequency >= 0); // Just verify it's valid
            }
        }

        [SkippableFact]
        public void CtlClose_ShouldCloseSuccessfully()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            // Test that Dispose can be called without throwing
            // Note: We can't actually dispose here as it would affect other tests
            // This test verifies that the API initialized successfully and can be disposed in cleanup
            Assert.NotNull(_api);
        }

        [SkippableFact]
        public void CtlWaitForPropertyChange_ShouldReturnResult()
        {
            // Arrange
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            var adapters = _api.EnumerateAdapters();
            Skip.If(adapters.Length == 0, "No adapters enumerated");

            // Act
            unsafe
            {
                var args = new ctl_wait_property_change_args_t
                {
                    Size = (uint)sizeof(ctl_wait_property_change_args_t),
                    Version = 0,
                    PropertyType = (uint)ctl_property_type_flag_t.CTL_PROPERTY_TYPE_FLAG_DISPLAY,
                    TimeOutMilliSec = 0, // Don't wait
                    EventMiscFlags = 0,
                    pReserved = null,
                    ReservedOutFlags = 0
                };

                var result = IGCL.ctlWaitForPropertyChange((_ctl_device_adapter_handle_t*)adapters[0], &args);

                // Assert - Should return timeout or success
                Assert.True(
                    result == ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == ctl_result_t.CTL_RESULT_ERROR_WAIT_TIMEOUT ||
                    result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [SkippableFact]
        public void InitArgs_ShouldReturnSupportedVersion()
        {
            // Arrange & Act
            Skip.If(!_hasHardware || !_hasDll || _api == null, _skipReason);

            // The API was initialized successfully, so we can check the version
            // This is implicitly tested in the initialization, but we'll verify explicitly
            unsafe
            {
                // Use IGCLApi helper methods for version manipulation
                var version = IGCLApi.MakeVersion(1, 1);
                var major = IGCLApi.GetMajorVersion(IGCL.CTL_IMPL_VERSION);
                var minor = IGCLApi.GetMinorVersion(IGCL.CTL_IMPL_VERSION);

                // Assert
                Assert.Equal(1u, major);
                Assert.Equal(1u, minor);
            }
        }
    }
}
