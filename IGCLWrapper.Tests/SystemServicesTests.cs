using Xunit;
using System;
using System.Threading;
using IGCLWrapper; // Use the new bindings project

namespace IGCLWrapper.Tests
{
    public class SystemServicesTests : IDisposable
    {
        private readonly IGCLHelper _IGCLHelper;

        public SystemServicesTests()
        {
            _IGCLHelper = new IGCLHelper();
            _IGCLHelper.Initialize();
        }

        public void Dispose()
        {
            _IGCLHelper.Terminate();
            _IGCLHelper.Dispose();
        }

        [Fact]
        public void InitializeAndTerminateIGCL_ShouldReturnSuccess()
        {
            // The constructor and Dispose methods already test this.
            // If they fail, the test will fail.
        }

        [Fact]
        public void QuerySystemServices_ShouldReturnValidInterface()
        {
            IIGCLSystem system = _IGCLHelper.GetSystemServices();
            Assert.NotNull(system);
            system.Dispose();
        }

        [Fact]
        public void QuerySystem1Interface_ShouldReturnValidInterface()
        {
            IIGCLSystem system = _IGCLHelper.GetSystemServices();
            Assert.NotNull(system);            

            bool supportsSystem1 = IGCL.SupportsSystem1Interface(system);
            if (supportsSystem1)
            {
                IIGCLSystem1 system1 = IGCL.QuerySystem1Interface(system);
                Assert.NotNull(system1);
                system1.Dispose();

            }
            system.Dispose();
        }

        [Fact]
        public void QuerySystem2Interface_ShouldReturnValidInterface()
        {
            IIGCLSystem system = _IGCLHelper.GetSystemServices();
            Assert.NotNull(system);

            bool supportsSystem2 = IGCL.SupportsSystem2Interface(system);
            if (supportsSystem2)
            {
                IIGCLSystem2 system2 = IGCL.QuerySystem2Interface(system);
                Assert.NotNull(system2);
                system2.Dispose();

            }
            system.Dispose();
        }
    }
}
