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
                Assert.NotEqual(0u, props.device_id_size);
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
                    var result = IGCL.ctlEnumerateDevices(IntPtr.Zero, &count, null);
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
                var versionInfo = new _ctl_version_info_t
                {
                    major_version = 1,
                    minor_version = 0,
                    build_number = 0
                };

                var result = IGCL.ctlCheckDriverVersion(adapters[0], versionInfo);

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
                Assert.NotEqual(0, props.pci_device_id);
                Assert.NotEqual(0, props.rev_id);
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
    }
}
