using Xunit;
using System;
using IGCLWrapper; // Use the new bindings project

namespace IGCLWrapper.Tests
{
    public class DisplayServicesTests : IDisposable
    {
        private readonly IGCLHelper _IGCLHelper;
        private readonly IIGCLSystem _system;
        private readonly IIGCLDisplayServices _displayServices;

        public DisplayServicesTests()
        {
            _IGCLHelper = new IGCLHelper();
            _IGCLHelper.Initialize();
            _system = _IGCLHelper.GetSystemServices();

            SWIGTYPE_p_p_IGCL__IIGCLDisplayServices s = IGCL.new_displaySerP_Ptr();
            _system.GetDisplaysServices(s);
            _displayServices = IGCL.displaySerP_Ptr_value(s);
        }

        public void Dispose()
        {
            _displayServices.Dispose();
            _system.Dispose();
            _IGCLHelper.Terminate();
            _IGCLHelper.Dispose();
        }

        [Fact]
        public void QueryDisplayServices_ShouldReturnValidInterface()
        {
            Assert.NotNull(_displayServices);
        }

        [Fact]
        public void EnumerateDisplays_ShouldReturnAtLeastOneDisplay()
        {
            SWIGTYPE_p_p_IGCL__IIGCLDisplayList displays_ptr = IGCL.new_displayListP_Ptr();
            IGCL_RESULT res = _displayServices.GetDisplays(displays_ptr);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);

            IIGCLDisplayList displays = IGCL.displayListP_Ptr_value(displays_ptr);
            Assert.NotNull(displays);
            Assert.True(displays.Size() > 0);
            displays.Release();            
        }

        [Fact]
        public void GetDisplayResolution_ShouldReturnValidResolution()
        {
            SWIGTYPE_p_p_IGCL__IIGCLDisplayList displays_ptr = IGCL.new_display_list_ptr();
            IGCL_RESULT res = _displayServices.GetDisplays(displays_ptr);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);

            IIGCLDisplayList displays = new IIGCLDisplayList(IGCL.display_list_ptr_value(displays_ptr), true);
            Assert.NotNull(displays);
            Assert.True(displays.Size() > 0);

            SWIGTYPE_p_p_IGCL__IIGCLDisplay display_ptr = IGCL.new_display_ptr();
            res = displays.At(0, display_ptr);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);

            IIGCLDisplay display = new IIGCLDisplay(IGCL.display_ptr_value(display_ptr), true);
            Assert.NotNull(display);

            SWIGTYPE_p_p_IGCL__IIGCLDisplayResolution displayResolution_ptr = IGCL.new_display_resolution_ptr();
            res = display.GetResolution(displayResolution_ptr);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);

            IIGCLDisplayResolution displayResolution = new IIGCLDisplayResolution(IGCL.display_resolution_ptr_value(displayResolution_ptr), true);
            Assert.NotNull(displayResolution);

            uint width = 0, height = 0;
            res = displayResolution.GetDesktopResolution(ref width, ref height);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);
            Assert.True(width > 0);
            Assert.True(height > 0);

            displayResolution.Dispose();
            display.Dispose();
            displays.Dispose();
            IGCL.delete_display_list_ptr(displays_ptr);
            IGCL.delete_display_ptr(display_ptr);
            IGCL.delete_display_resolution_ptr(displayResolution_ptr);
        }

        [Fact]
        public void GetDisplayRefreshRate_ShouldReturnValidRefreshRate()
        {
            SWIGTYPE_p_p_IGCL__IIGCLDisplayList displays_ptr = IGCL.new_display_list_ptr();
            IGCL_RESULT res = _displayServices.GetDisplays(displays_ptr);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);

            IIGCLDisplayList displays = new IIGCLDisplayList(IGCL.display_list_ptr_value(displays_ptr), true);
            Assert.NotNull(displays);
            Assert.True(displays.Size() > 0);

            SWIGTYPE_p_p_IGCL__IIGCLDisplay display_ptr = IGCL.new_display_ptr();
            res = displays.At(0, display_ptr);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);

            IIGCLDisplay display = new IIGCLDisplay(IGCL.display_ptr_value(display_ptr), true);
            Assert.NotNull(display);

            uint refreshRate = 0;
            res = display.RefreshRate(ref refreshRate);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);
            Assert.True(refreshRate > 0);

            display.Dispose();
            displays.Dispose();
            IGCL.delete_display_list_ptr(displays_ptr);
            IGCL.delete_display_ptr(display_ptr);
        }
    }
}
