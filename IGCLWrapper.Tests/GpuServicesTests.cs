using Xunit;
using System;
using IGCLWrapper; // Use the new bindings project

namespace IGCLWrapper.Tests
{
    public class GpuServicesTests : IDisposable
    {
        private readonly IGCLHelper _IGCLHelper;
        private readonly IIGCLSystem _sys;
        private readonly IIGCLGPUList _gpus;

        public GpuServicesTests()
        {
            _IGCLHelper = new IGCLHelper();
            _IGCLHelper.Initialize();
            _sys = _IGCLHelper.GetSystemServices();
            _gpus = _sys.GetGPUs();
        }

        public void Dispose()
        {
            _gpus.Dispose();
            _sys.Dispose();
            _IGCLHelper.Terminate();
            _IGCLHelper.Dispose();
        }

        [Fact]
        public void EnumerateGPUs_ShouldReturnAtLeastOneGPU()
        {
            Assert.NotNull(_gpus);
            Assert.True(_gpus.Size() > 0);

            SWIGTYPE_p_p_IGCL__IIGCLGPU gpu_ptr = IGCL.new_gpu_ptr();
            IGCL_RESULT res = _gpus.At(0, gpu_ptr);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);

            IIGCLGPU gpu = new IIGCLGPU(IGCL.gpu_ptr_value(gpu_ptr), true);
            Assert.NotNull(gpu);

            string gpuName = "";
            res = gpu.Name(ref gpuName);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);
            Assert.False(string.IsNullOrEmpty(gpuName));

            gpu.Dispose();
            IGCL.delete_gpu_ptr(gpu_ptr);
        }

        [Fact]
        public void QueryGPU1Interface_ShouldReturnValidInterface()
        {
            SWIGTYPE_p_p_IGCL__IIGCLGPU gpu_ptr = IGCL.new_gpu_ptr();
            IGCL_RESULT res = _gpus.At(0, gpu_ptr);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);

            IIGCLGPU gpu = new IIGCLGPU(IGCL.gpu_ptr_value(gpu_ptr), true);
            Assert.NotNull(gpu);

            SWIGTYPE_p_p_void gpu1_ptr = IGCL.new_void_ptr();
            res = gpu.QueryInterface(IIGCLGPU1.IID(), gpu1_ptr);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);

            IIGCLGPU1 gpu1 = new IIGCLGPU1(IGCL.void_ptr_value(gpu1_ptr), true);
            Assert.NotNull(gpu1);

            gpu1.Dispose();
            gpu.Dispose();
            IGCL.delete_gpu_ptr(gpu_ptr);
            IGCL.delete_void_ptr(gpu1_ptr);
        }

        [Fact]
        public void QueryGPU2Interface_ShouldReturnValidInterface()
        {
            SWIGTYPE_p_p_IGCL__IIGCLGPU gpu_ptr = IGCL.new_gpu_ptr();
            IGCL_RESULT res = _gpus.At(0, gpu_ptr);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);

            IIGCLGPU gpu = new IIGCLGPU(IGCL.gpu_ptr_value(gpu_ptr), true);
            Assert.NotNull(gpu);

            SWIGTYPE_p_p_void gpu2_ptr = IGCL.new_void_ptr();
            res = gpu.QueryInterface(IIGCLGPU2.IID(), gpu2_ptr);
            Assert.Equal(IGCL_RESULT.IGCL_OK, res);

            IIGCLGPU2 gpu2 = new IIGCLGPU2(IGCL.void_ptr_value(gpu2_ptr), true);
            Assert.NotNull(gpu2);

            gpu2.Dispose();
            gpu.Dispose();
            IGCL.delete_gpu_ptr(gpu_ptr);
            IGCL.delete_void_ptr(gpu2_ptr);
        }
    }
}
