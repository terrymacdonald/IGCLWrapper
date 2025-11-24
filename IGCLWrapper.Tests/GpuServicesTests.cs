using Xunit;
using System;
using IGCLWrapper;

namespace IGCLWrapper.Tests
{
    public class GpuServicesTests : IDisposable
    {
        private SWIGTYPE_p__ctl_api_handle_t? _apiHandle;
        private uint _adapterCount;

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
        public void EnumerateDevices_ShouldReturnValidDeviceCount()
        {
            var countPtr = IGCL.new_igcl_uint32P();
            ctl_result_t result = IGCL.ctlEnumerateDevices(_apiHandle, countPtr, null);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);
            
            uint count = IGCL.igcl_uint32P_value(countPtr);
            Assert.True(count > 0);
            
            IGCL.delete_igcl_uint32P(countPtr);
        }

        [Fact]
        public void GetDeviceProperties_ShouldReturnValidProperties()
        {
            // First get the adapter handle
            var countPtr = IGCL.new_igcl_uint32P();
            IGCL.igcl_uint32P_assign(countPtr, _adapterCount);
            
            // Allocate pointer for adapter
            var adapterPtr = IGCL.new_deviceAdapterHandleP();
            
            ctl_result_t result = IGCL.ctlEnumerateDevices(_apiHandle, countPtr, adapterPtr);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            // Get first adapter (IntPtr is a value type, check for IntPtr.Zero instead)
            var firstAdapter = IGCL.deviceAdapterHandleP_value(adapterPtr);
            Assert.NotEqual(IntPtr.Zero, firstAdapter);

            // Get properties
            var propsPtr = IGCL.new_adapterPropertiesP();
            result = IGCL.IGCL_GetAdapterProperties(firstAdapter, propsPtr);
            Assert.Equal(ctl_result_t.CTL_RESULT_SUCCESS, result);

            // Cleanup
            IGCL.delete_adapterPropertiesP(propsPtr);
            IGCL.delete_deviceAdapterHandleP(adapterPtr);
            IGCL.delete_igcl_uint32P(countPtr);
        }

        [Fact]
        public void TestAdapterEnumeration()
        {
            // This test verifies we can enumerate adapters
            Assert.NotNull(_apiHandle);
            Assert.True(_adapterCount > 0, $"Expected at least 1 adapter, found {_adapterCount}");
        }
    }
}
