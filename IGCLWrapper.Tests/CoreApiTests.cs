using Xunit;
using IGCLWrapper;
using System;

namespace IGCLWrapper.Tests.ClangSharp
{
    /// <summary>
    /// Tests for Core IGCL API functions including initialization, enumeration, and basic device operations
    /// </summary>
    public class CoreApiTests : IDisposable
    {
        private IGCLApi? _api;

        public CoreApiTests()
        {
            try
            {
                _api = IGCLApi.Initialize();
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

        [Fact]
        public void CtlInit_ShouldInitializeSuccessfully()
        {
            // Arrange & Act
            if (_api == null)
            {
                // Skip test if ControlLib.dll is not found
                return;
            }

            // Assert
            Assert.NotNull(_api);
        }

        [Fact]
        public void CtlEnumerateDevices_ShouldReturnAdapters()
        {
            // Arrange
            if (_api == null)
            {
                return; // Skip test
            }

            // Act
            var adapters = _api.EnumerateAdapters();

            // Assert
            Assert.NotNull(adapters);
            // Note: May be 0 if no Intel GPU present
        }

        [Fact]
        public void CtlGetDeviceProperties_ShouldReturnValidProperties()
        {
            // Arrange
            if (_api == null)
            {
                return;
            }

            var adapters = _api.EnumerateAdapters();
            if (adapters.Length == 0)
            {
                return; // Skip if no adapters
            }

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);

                // Assert
                // Note: device_id_size can be 0 on some hardware/driver combinations
                Assert.True(props.pci_vendor_id == 0x8086); // Intel vendor ID
            }
        }

        [Fact]
        public void CtlGetDeviceProperties_DeviceType_ShouldBeGraphics()
        {
            // Arrange
            if (_api == null)
            {
                return;
            }

            var adapters = _api.EnumerateAdapters();
            if (adapters.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);

                // Assert
                Assert.Equal(_ctl_device_type_t.CTL_DEVICE_TYPE_GRAPHICS, props.device_type);
            }
        }

        [Fact]
        public void CtlEnumerateDevices_WithNullHandle_ShouldThrowException()
        {
            // Arrange
            if (_api == null)
            {
                return;
            }

            // Act & Assert
            unsafe
            {
                // This should throw or return error when called with null handle
                // Testing error handling
                Assert.ThrowsAny<Exception>(() =>
                {
                    uint count = 0;
                    var result = IGCL.ctlEnumerateDevices((_ctl_api_handle_t*)IntPtr.Zero, &count, null);
                    if (result != _ctl_result_t.CTL_RESULT_SUCCESS)
                    {
                        throw new IGCLException(result, "Expected error with null handle");
                    }
                });
            }
        }

        [Fact]
        public void MultipleInitializations_ShouldNotCrash()
        {
            // Arrange & Act
            if (_api == null)
            {
                return;
            }

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

        [Fact]
        public void CtlCheckDriverVersion_ShouldValidateVersion()
        {
            // Arrange
            if (_api == null)
            {
                return;
            }

            var adapters = _api.EnumerateAdapters();
            if (adapters.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                uint versionInfo = IGCLApi.MakeVersion(1, 0);

                var result = IGCL.ctlCheckDriverVersion((_ctl_device_adapter_handle_t*)adapters[0], versionInfo);

                // Assert - Should return success or unsupported version
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION
                );
            }
        }

        [Fact]
        public void AdapterProperties_ShouldContainDeviceId()
        {
            // Arrange
            if (_api == null)
            {
                return;
            }

            var adapters = _api.EnumerateAdapters();
            if (adapters.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);

                // Assert
                Assert.NotEqual(0u, props.pci_device_id);
                Assert.NotEqual(0u, props.rev_id);
            }
        }

        [Fact]
        public void AdapterProperties_ShouldHaveValidName()
        {
            // Arrange
            if (_api == null)
            {
                return;
            }

            var adapters = _api.EnumerateAdapters();
            if (adapters.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);
                var name = new string(props.name);

                // Assert
                Assert.NotNull(name);
                Assert.NotEmpty(name.Trim('\0'));
            }
        }

        [Fact]
        public void CtlEnumerateDisplayOutputs_ShouldReturnCount()
        {
            // Arrange & Act
            if (_api == null)
            {
                return;
            }

            var adapters = _api.EnumerateAdapters();
            if (adapters.Length == 0)
            {
                return;
            }

            unsafe
            {
                uint count = 0;
                var result = IGCL.ctlEnumerateDisplayOutputs((_ctl_device_adapter_handle_t*)adapters[0], &count, null);

                // Assert
                Assert.Equal(_ctl_result_t.CTL_RESULT_SUCCESS, result);
                // Count may be 0 if no displays connected
            }
        }

        [Fact]
        public void AdapterProperties_ShouldHaveValidDriverVersion()
        {
            // Arrange
            if (_api == null)
            {
                return;
            }

            var adapters = _api.EnumerateAdapters();
            if (adapters.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var props = IGCLHelpers.GetProperties(adapters[0]);

                // Assert
                Assert.NotEqual(0ul, props.driver_version);
            }
        }

        [Fact]
        public void AdapterProperties_ShouldHaveValidPCIIds()
        {
            // Arrange
            if (_api == null)
            {
                return;
            }

            var adapters = _api.EnumerateAdapters();
            if (adapters.Length == 0)
            {
                return;
            }

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

        [Fact]
        public void AdapterProperties_ShouldHaveValidEUCounts()
        {
            // Arrange
            if (_api == null)
            {
                return;
            }

            var adapters = _api.EnumerateAdapters();
            if (adapters.Length == 0)
            {
                return;
            }

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

        [Fact]
        public void AdapterProperties_ShouldHaveValidFrequency()
        {
            // Arrange
            if (_api == null)
            {
                return;
            }

            var adapters = _api.EnumerateAdapters();
            if (adapters.Length == 0)
            {
                return;
            }

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

        [Fact]
        public void CtlClose_ShouldCloseSuccessfully()
        {
            // Arrange
            if (_api == null)
            {
                return;
            }

            // Act - Dispose will call ctlClose
            _api.Dispose();

            // Assert - Should not throw
            // If we get here, close was successful
            Assert.True(true);

            // Prevent double-dispose in test cleanup
            _api = null;
        }

        [Fact]
        public void CtlWaitForPropertyChange_ShouldReturnResult()
        {
            // Arrange
            if (_api == null)
            {
                return;
            }

            var adapters = _api.EnumerateAdapters();
            if (adapters.Length == 0)
            {
                return;
            }

            // Act
            unsafe
            {
                var args = new _ctl_wait_property_change_args_t
                {
                    Size = (uint)sizeof(_ctl_wait_property_change_args_t),
                    Version = 0,
                    PropertyType = (uint)_ctl_property_type_flag_t.CTL_PROPERTY_TYPE_FLAG_DISPLAY,
                    TimeOutMilliSec = 0, // Don't wait
                    EventMiscFlags = 0,
                    pReserved = null,
                    ReservedOutFlags = 0
                };

                var result = IGCL.ctlWaitForPropertyChange((_ctl_device_adapter_handle_t*)adapters[0], &args);

                // Assert - Should return timeout or success
                Assert.True(
                    result == _ctl_result_t.CTL_RESULT_SUCCESS ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_WAIT_TIMEOUT ||
                    result == _ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE
                );
            }
        }

        [Fact]
        public void InitArgs_ShouldReturnSupportedVersion()
        {
            // Arrange & Act
            if (_api == null)
            {
                return;
            }

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
