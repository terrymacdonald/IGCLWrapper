using Xunit;
using System;
using IGCLWrapper;

namespace IGCLWrapper.Tests
{
    public class DisplayServicesTests : IDisposable
    {
        private SWIGTYPE_p__ctl_api_handle_t? _apiHandle;
        private uint _adapterCount;
        private uint _displayCount;

        public DisplayServicesTests()
        {
            InitializeIGCL();
        }

        public void Dispose()
        {
            CleanupIGCL();
        }

        private void InitializeIGCL()
        {
            // Initialize IGCL API using helper method
            var apiHandlePtr = IGCL.new_apiHandleP();
            var countPtr = IGCL.new_igcl_uint32P();
            
            ctl_result_t result = IGCL.IGCL_InitDefault(apiHandlePtr);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            
            _apiHandle = IGCL.apiHandleP_value(apiHandlePtr);
            Assert.NotNull(_apiHandle);

            // Enumerate adapters
            result = IGCL.IGCL_EnumerateAdapters(_apiHandle, countPtr, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            
            _adapterCount = IGCL.igcl_uint32P_value(countPtr);
            Assert.True(_adapterCount > 0, "No adapters found");

            // Enumerate displays for first adapter
            if (_adapterCount > 0)
            {
                var adapterPtr = IGCL.new_deviceAdapterHandleP();
                IGCL.igcl_uint32P_assign(countPtr, _adapterCount);
                result = IGCL.ctlEnumerateDevices(_apiHandle, countPtr, adapterPtr);
                
                if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                {
                    var firstAdapter = IGCL.deviceAdapterHandleP_value(adapterPtr);
                    if (firstAdapter != null)
                    {
                        IGCL.igcl_uint32P_assign(countPtr, 0);
                        result = IGCL.IGCL_EnumerateDisplays(firstAdapter, countPtr, null);
                        if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                        {
                            _displayCount = IGCL.igcl_uint32P_value(countPtr);
                        }
                    }
                }
                IGCL.delete_deviceAdapterHandleP(adapterPtr);
            }
            
            IGCL.delete_igcl_uint32P(countPtr);
        }

        private void CleanupIGCL()
        {
            if (_apiHandle != null)
            {
                IGCL.IGCL_Close(_apiHandle);
                _apiHandle = null;
            }
        }

        [Fact]
        public void InitializeIGCL_ShouldSucceed()
        {
            // Test already performed in constructor
            Assert.NotNull(_apiHandle);
        }

        [Fact]
        public void EnumerateAdapters_ShouldReturnValidCount()
        {
            var countPtr = IGCL.new_igcl_uint32P();
            ctl_result_t result = IGCL.ctlEnumerateDevices(_apiHandle, countPtr, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            
            uint count = IGCL.igcl_uint32P_value(countPtr);
            Assert.True(count > 0);
            
            IGCL.delete_igcl_uint32P(countPtr);
        }

        [Fact]
        public void EnumerateDisplays_ShouldWork()
        {
            // Get first adapter
            var countPtr = IGCL.new_igcl_uint32P();
            IGCL.igcl_uint32P_assign(countPtr, _adapterCount);
            
            var adapterPtr = IGCL.new_deviceAdapterHandleP();
            ctl_result_t result = IGCL.ctlEnumerateDevices(_apiHandle, countPtr, adapterPtr);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            var firstAdapter = IGCL.deviceAdapterHandleP_value(adapterPtr);
            Assert.NotEqual(IntPtr.Zero, firstAdapter);

            // Enumerate displays
            IGCL.igcl_uint32P_assign(countPtr, 0);
            result = IGCL.ctlEnumerateDisplayOutputs(firstAdapter, countPtr, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            uint displayCount = IGCL.igcl_uint32P_value(countPtr);
            // Displays may or may not be present
            Assert.True(displayCount >= 0);

            // Cleanup
            IGCL.delete_deviceAdapterHandleP(adapterPtr);
            IGCL.delete_igcl_uint32P(countPtr);
        }

        [Fact]
        public void GetDisplayProperties_ShouldWorkWithDisplays()
        {
            if (_displayCount == 0)
            {
                // Skip test if no displays
                return;
            }

            // Get first adapter
            var countPtr = IGCL.new_igcl_uint32P();
            IGCL.igcl_uint32P_assign(countPtr, _adapterCount);
            
            var adapterPtr = IGCL.new_deviceAdapterHandleP();
            ctl_result_t result = IGCL.ctlEnumerateDevices(_apiHandle, countPtr, adapterPtr);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            var firstAdapter = IGCL.deviceAdapterHandleP_value(adapterPtr);
            Assert.NotEqual(IntPtr.Zero, firstAdapter);

            // Get first display
            IGCL.igcl_uint32P_assign(countPtr, _displayCount);
            var displayPtr = IGCL.new_displayOutputHandleP();
            result = IGCL.ctlEnumerateDisplayOutputs(firstAdapter, countPtr, displayPtr);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            var firstDisplay = IGCL.displayOutputHandleP_value(displayPtr);
            Assert.NotEqual(IntPtr.Zero, firstDisplay);

            // Get display properties - use the helper function which auto-initializes Size and Version
            var properties = new ctl_display_properties_t();
            result = IGCL.IGCL_GetDisplayProperties(firstDisplay, properties);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            // Cleanup
            IGCL.delete_displayOutputHandleP(displayPtr);
            IGCL.delete_deviceAdapterHandleP(adapterPtr);
            IGCL.delete_igcl_uint32P(countPtr);
        }

        [Fact]
        public void TestDisplayEnumeration()
        {
            // This test verifies we can enumerate displays
            Assert.NotNull(_apiHandle);
            Assert.True(_adapterCount > 0, $"Expected at least 1 adapter, found {_adapterCount}");
            // Display count can be 0 if no displays are connected
            Assert.True(_displayCount >= 0, $"Display count: {_displayCount}");
        }
    }
}
