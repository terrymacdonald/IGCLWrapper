using Xunit;
using System;
using IGCLWrapper;

namespace IGCLWrapper.Tests.ClangSharp
{
    /// <summary>
    /// Tests for ClangSharp-generated IGCL bindings
    /// These tests validate that the new bindings work correctly
    /// </summary>
    public class BasicApiTests : IDisposable
    {
        private readonly IGCLApi? _api;
        private readonly bool _hasHardware;

        public BasicApiTests()
        {
            try
            {
                _api = IGCLApi.Initialize();
                _hasHardware = true;
            }
            catch (IGCLException)
            {
                // No Intel hardware available or driver not installed
                _hasHardware = false;
            }
        }

        [Fact]
        public void Initialize_ShouldSucceed()
        {
            // Test that we can initialize the API
            // If no hardware, this is still considered a pass (skipped)
            if (!_hasHardware)
            {
                Assert.True(true, "No Intel hardware available - test skipped");
                return;
            }

            Assert.NotNull(_api);
        }

        [Fact]
        public unsafe void EnumerateAdapters_ShouldReturnAdapters()
        {
            if (!_hasHardware || _api == null)
            {
                Assert.True(true, "No Intel hardware available - test skipped");
                return;
            }

            var adapters = _api.EnumerateAdapters();
            
            Assert.NotNull(adapters);
            Assert.NotEmpty(adapters);
            
            // Verify adapters are valid pointers
            foreach (var adapter in adapters)
            {
                Assert.True(adapter != null, "Adapter handle should not be null");
            }
        }

        [Fact]
        public unsafe void GetAdapterProperties_ShouldSucceed()
        {
            if (!_hasHardware || _api == null)
            {
                Assert.True(true, "No Intel hardware available - test skipped");
                return;
            }

            var adapters = _api.EnumerateAdapters();
            Assert.NotEmpty(adapters);

            var firstAdapter = adapters[0];
            var props = IGCLHelpers.GetProperties(firstAdapter);

            // Verify structure was filled correctly
            Assert.Equal((uint)sizeof(_ctl_device_adapter_properties_t), props.Size);
            Assert.Equal((byte)1, props.Version);
            
            // Test passed - adapter properties retrieved successfully
        }

        [Fact]
        public unsafe void EnumerateDisplays_ShouldWork()
        {
            if (!_hasHardware || _api == null)
            {
                Assert.True(true, "No Intel hardware available - test skipped");
                return;
            }

            var adapters = _api.EnumerateAdapters();
            Assert.NotEmpty(adapters);

            var firstAdapter = adapters[0];
            var displays = _api.EnumerateDisplays(firstAdapter);

            // It's okay if no displays are connected
            Assert.NotNull(displays);
            
            if (displays.Length > 0)
            {
                // If displays exist, verify they're valid
                foreach (var display in displays)
                {
                    Assert.True(display != null, "Display handle should not be null");
                }
            }
        }

        [Fact]
        public unsafe void GetDisplayProperties_ShouldSucceed_WhenDisplayConnected()
        {
            if (!_hasHardware || _api == null)
            {
                Assert.True(true, "No Intel hardware available - test skipped");
                return;
            }

            var adapters = _api.EnumerateAdapters();
            Assert.NotEmpty(adapters);

            var firstAdapter = adapters[0];
            var displays = _api.EnumerateDisplays(firstAdapter);

            if (displays.Length == 0)
            {
                Assert.True(true, "No displays connected - test skipped");
                return;
            }

            var firstDisplay = displays[0];
            var props = IGCLHelpers.GetDisplayProperties(firstDisplay);

            // Verify structure was filled correctly
            Assert.Equal((uint)sizeof(_ctl_display_properties_t), props.Size);
            Assert.Equal((byte)0, props.Version);
            
            // Display should have some valid type
            Assert.True(props.Type != 0, "Display should have valid output type");
        }

        [Fact]
        public unsafe void GetDisplayTiming_ShouldReturnValidData()
        {
            if (!_hasHardware || _api == null)
            {
                Assert.True(true, "No Intel hardware available - test skipped");
                return;
            }

            var adapters = _api.EnumerateAdapters();
            var firstAdapter = adapters[0];
            var displays = _api.EnumerateDisplays(firstAdapter);

            if (displays.Length == 0)
            {
                Assert.True(true, "No displays connected - test skipped");
                return;
            }

            var firstDisplay = displays[0];
            
            // Use static helper methods
            var (width, height) = IGCLHelpers.GetResolution(firstDisplay);
            var refreshRate = IGCLHelpers.GetRefreshRate(firstDisplay);

            if (IGCLHelpers.IsActive(firstDisplay))
            {
                Assert.True(width > 0, "Active display should have valid width");
                Assert.True(height > 0, "Active display should have valid height");
                Assert.True(refreshRate > 0, "Active display should have valid refresh rate");
            }
        }

        [Fact]
        public void VersionHelpers_ShouldWorkCorrectly()
        {
            // Test version manipulation helpers
            uint version = IGCLApi.MakeVersion(1, 2);
            Assert.Equal(1u, IGCLApi.GetMajorVersion(version));
            Assert.Equal(2u, IGCLApi.GetMinorVersion(version));

            version = IGCLApi.MakeVersion(5, 12);
            Assert.Equal(5u, IGCLApi.GetMajorVersion(version));
            Assert.Equal(12u, IGCLApi.GetMinorVersion(version));
        }

        [Fact]
        public unsafe void StructHelper_ShouldCreateValidStructures()
        {
            // Test structure creation helpers
            var initArgs = IGCLStructHelper.CreateInitArgs();
            Assert.Equal((uint)sizeof(_ctl_init_args_t), initArgs.Size);
            Assert.Equal((byte)0, initArgs.Version);

            var adapterProps = IGCLStructHelper.CreateAdapterProperties();
            Assert.Equal((uint)sizeof(_ctl_device_adapter_properties_t), adapterProps.Size);
            Assert.Equal((byte)1, adapterProps.Version);

            var displayProps = IGCLStructHelper.CreateDisplayProperties();
            Assert.Equal((uint)sizeof(_ctl_display_properties_t), displayProps.Size);
            Assert.Equal((byte)0, displayProps.Version);
        }

        public void Dispose()
        {
            _api?.Dispose();
        }
    }
}
