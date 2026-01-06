using System;
using System.Linq;
using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    internal static class FacadeTestUtils
    {
        internal static (IGCLApiHelper api, IGCLAdapterHelper adapter) RequireAdapter()
        {
            if (!IGCLApiHelper.IsIGCLDllAvailable(out var dllError))
                throw new SkipException($"IGCL DLL unavailable: {dllError}");

            if (!IGCLHardwareDetection.HasIntelGPU(out var hwError))
                throw new SkipException($"Intel GPU not detected: {hwError}");

            var api = IGCLApiHelper.Initialize();
            var adapter = api.EnumerateAdapters().FirstOrDefault();
            if (adapter == null)
            {
                api.Dispose();
                throw new SkipException("No adapters returned from IGCL.");
            }

            return (api, adapter);
        }

        internal static T InvokeOrSkip<T>(Func<T> func, string reason)
        {
            try
            {
                return func();
            }
            catch (IGCLException ex)
            {
                if (ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                {
                    throw new SkipException($"{reason}: {ex.Result}");
                }
                throw;
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new SkipException($"{reason}: {ex.Message}");
            }
        }

        internal static void InvokeOrSkip(Action action, string reason)
        {
            try
            {
                action();
            }
            catch (IGCLException ex)
            {
                if (ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE ||
                    ex.Result == ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION)
                {
                    throw new SkipException($"{reason}: {ex.Result}");
                }
                throw;
            }
            catch (EntryPointNotFoundException ex)
            {
                throw new SkipException($"{reason}: {ex.Message}");
            }
        }
    }
}
