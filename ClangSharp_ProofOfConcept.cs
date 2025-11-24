// ============================================================================
// PROOF OF CONCEPT: ClangSharp-Generated Bindings
// ============================================================================
// This shows what ClangSharpPInvokeGenerator would automatically create
// Compare this to the SWIG-generated version to see the difference
// ============================================================================

using System;
using System.Runtime.InteropServices;

namespace IGCLWrapper.Generated
{
    // ========================================================================
    // Enums - Same as SWIG, but cleaner
    // ========================================================================
    
    [Flags]
    public enum ctl_init_flag_t : uint
    {
        CTL_INIT_FLAG_USE_LEVEL_ZERO = 1 << 0,
    }
    
    public enum ctl_result_t : int
    {
        CTL_RESULT_SUCCESS = 0,
        CTL_RESULT_ERROR_UNSUPPORTED_VERSION = 0x70000001,
        CTL_RESULT_ERROR_INVALID_NULL_POINTER = 0x70000002,
        // ... rest of enum values
    }
    
    public enum ctl_display_output_types_t : int
    {
        CTL_DISPLAY_OUTPUT_TYPES_INVALID = 0,
        CTL_DISPLAY_OUTPUT_TYPES_DISPLAYPORT = 1,
        CTL_DISPLAY_OUTPUT_TYPES_HDMI = 2,
        // ... rest
    }
    
    // ========================================================================
    // Structures - VALUE TYPES with direct field access (not classes!)
    // ========================================================================
    
    /// <summary>
    /// Initialization arguments for IGCL API
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct ctl_init_args_t
    {
        /// <summary>Size of this structure</summary>
        public uint Size;
        
        /// <summary>Version of this structure</summary>
        public byte Version;
        
        /// <summary>Application version</summary>
        public uint AppVersion;
        
        /// <summary>Initialization flags</summary>
        public ctl_init_flag_t flags;
        
        /// <summary>Supported API version</summary>
        public uint SupportedVersion;
        
        /// <summary>Application UID (optional)</summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public byte[] ApplicationUID;
        
        /// <summary>
        /// Helper method to create properly initialized structure
        /// </summary>
        public static ctl_init_args_t Create()
        {
            return new ctl_init_args_t
            {
                Size = (uint)Marshal.SizeOf<ctl_init_args_t>(),
                Version = 0,
                AppVersion = IGCL.CTL_MakeVersion(1, 0),
                flags = ctl_init_flag_t.CTL_INIT_FLAG_USE_LEVEL_ZERO,
                SupportedVersion = IGCL.CTL_IMPL_VERSION,
                ApplicationUID = new byte[16]
            };
        }
    }
    
    /// <summary>
    /// Display timing information
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct ctl_display_timing_t
    {
        public uint HActive;
        public uint VActive;
        public uint HTotal;
        public uint VTotal;
        public uint HBlank;
        public uint VBlank;
        public uint RefreshRate;
        public uint PixelClock;
    }
    
    /// <summary>
    /// Display properties structure
    /// Compare this to SWIG version - this is a STRUCT, not a CLASS!
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public partial struct ctl_display_properties_t
    {
        // ====================================================================
        // Direct field access - NO P/Invoke overhead!
        // ====================================================================
        
        /// <summary>Size of this structure</summary>
        public uint Size;
        
        /// <summary>Version of this structure</summary>
        public byte Version;
        
        /// <summary>OS-specific display/encoder ID</summary>
        public IntPtr Os_display_encoder_handle;  // Simplified for POC
        
        /// <summary>Display output type</summary>
        public ctl_display_output_types_t Type;
        
        /// <summary>Attached display mux type</summary>
        public int AttachedDisplayMuxType;
        
        /// <summary>Protocol converter output type</summary>
        public ctl_display_output_types_t ProtocolConverterOutput;
        
        /// <summary>Supported spec version</summary>
        public uint SupportedSpec;
        
        /// <summary>Supported output bits per color</summary>
        public uint SupportedOutputBPCFlags;
        
        /// <summary>Protocol converter type flags</summary>
        public uint ProtocolConverterType;
        
        /// <summary>Display configuration flags</summary>
        public uint DisplayConfigFlags;
        
        /// <summary>Enabled display features</summary>
        public uint FeatureEnabledFlags;
        
        /// <summary>Supported display features</summary>
        public uint FeatureSupportedFlags;
        
        /// <summary>Advanced features enabled</summary>
        public uint AdvancedFeatureEnabledFlags;
        
        /// <summary>Advanced features supported</summary>
        public uint AdvancedFeatureSupportedFlags;
        
        /// <summary>Applied timing information</summary>
        public ctl_display_timing_t Display_Timing_Info;
        
        /// <summary>Reserved for future use</summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
        public uint[] ReservedFields;
        
        // ====================================================================
        // Helper method (you can add these yourself)
        // ====================================================================
        
        /// <summary>
        /// Create properly initialized display properties structure
        /// </summary>
        public static ctl_display_properties_t Create()
        {
            return new ctl_display_properties_t
            {
                Size = (uint)Marshal.SizeOf<ctl_display_properties_t>(),
                Version = 0,
                ReservedFields = new uint[16]
            };
        }
    }
    
    // ========================================================================
    // P/Invoke Declarations - Direct, clean, efficient
    // ========================================================================
    
    public static unsafe partial class IGCL
    {
        private const string LibraryName = "ControlLib";
        
        // ====================================================================
        // Constants (from macros in header)
        // ====================================================================
        
        public const uint CTL_IMPL_MAJOR_VERSION = 1;
        public const uint CTL_IMPL_MINOR_VERSION = 0;
        public const uint CTL_IMPL_VERSION = (CTL_IMPL_MAJOR_VERSION << 16) | CTL_IMPL_MINOR_VERSION;
        
        // ====================================================================
        // Helper functions for version manipulation
        // ====================================================================
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CTL_MakeVersion(uint major, uint minor)
        {
            return (major << 16) | (minor & 0x0000ffff);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CTL_GetMajorVersion(uint version)
        {
            return version >> 16;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint CTL_GetMinorVersion(uint version)
        {
            return version & 0x0000ffff;
        }
        
        // ====================================================================
        // API Functions - Direct P/Invoke, no wrappers
        // ====================================================================
        
        /// <summary>
        /// Initialize the IGCL API
        /// </summary>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ctl_result_t ctlInit(
            ref ctl_init_args_t pInitDesc,
            out IntPtr phAPIHandle);
        
        /// <summary>
        /// Close the IGCL API handle
        /// </summary>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ctl_result_t ctlClose(
            IntPtr hAPIHandle);
        
        /// <summary>
        /// Enumerate device adapters
        /// </summary>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ctl_result_t ctlEnumerateDevices(
            IntPtr hAPIHandle,
            ref uint pCount,
            IntPtr phDevices);  // Can be null for count query
        
        /// <summary>
        /// Enumerate display outputs for an adapter
        /// </summary>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ctl_result_t ctlEnumerateDisplayOutputs(
            IntPtr hDeviceAdapter,
            ref uint pCount,
            IntPtr phDisplayOutputs);  // Can be null for count query
        
        /// <summary>
        /// Get display properties
        /// </summary>
        [DllImport(LibraryName, CallingConvention = CallingConvention.Cdecl)]
        public static extern ctl_result_t ctlGetDisplayProperties(
            IntPtr hDisplayOutput,
            ref ctl_display_properties_t pProperties);
    }
    
    // ========================================================================
    // Example Usage - Compare to SWIG version
    // ========================================================================
    
    public static class UsageExample
    {
        public static void BasicUsage()
        {
            // ================================================================
            // SWIG Version (BROKEN):
            // ================================================================
            // var apiHandlePtr = IGCL.new_apiHandleP();           // Allocate pointer wrapper
            // var initArgs = new ctl_init_args_t();               // Allocate class object
            // initArgs.Size = /* ... */;                          // P/Invoke call
            // initArgs.Version = 0;                               // P/Invoke call
            // var result = IGCL.ctlInit(..., apiHandlePtr);       // CRASH!
            
            // ================================================================
            // ClangSharp Version (WORKS):
            // ================================================================
            
            // Initialize API
            ctl_init_args_t initArgs = ctl_init_args_t.Create();
            // OR manually:
            // ctl_init_args_t initArgs = new()
            // {
            //     Size = (uint)Marshal.SizeOf<ctl_init_args_t>(),
            //     Version = 0,
            //     AppVersion = IGCL.CTL_MakeVersion(1, 0),
            //     flags = ctl_init_flag_t.CTL_INIT_FLAG_USE_LEVEL_ZERO,
            //     SupportedVersion = IGCL.CTL_IMPL_VERSION,
            //     ApplicationUID = new byte[16]
            // };
            
            IntPtr hAPI;
            ctl_result_t result = IGCL.ctlInit(ref initArgs, out hAPI);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
            {
                throw new Exception($"Failed to initialize IGCL: {result}");
            }
            
            try
            {
                // Get adapter count
                uint adapterCount = 0;
                result = IGCL.ctlEnumerateDevices(hAPI, ref adapterCount, IntPtr.Zero);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new Exception($"Failed to get adapter count: {result}");
                
                // Get adapters
                IntPtr[] adapters = new IntPtr[adapterCount];
                unsafe
                {
                    fixed (IntPtr* pAdapters = adapters)
                    {
                        result = IGCL.ctlEnumerateDevices(hAPI, ref adapterCount, (IntPtr)pAdapters);
                        if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                            throw new Exception($"Failed to enumerate adapters: {result}");
                    }
                }
                
                // For each adapter, enumerate displays
                foreach (var adapter in adapters)
                {
                    uint displayCount = 0;
                    result = IGCL.ctlEnumerateDisplayOutputs(adapter, ref displayCount, IntPtr.Zero);
                    if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                        continue;
                    
                    IntPtr[] displays = new IntPtr[displayCount];
                    unsafe
                    {
                        fixed (IntPtr* pDisplays = displays)
                        {
                            result = IGCL.ctlEnumerateDisplayOutputs(adapter, ref displayCount, (IntPtr)pDisplays);
                            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                                continue;
                        }
                    }
                    
                    // Get properties for each display
                    foreach (var display in displays)
                    {
                        ctl_display_properties_t props = ctl_display_properties_t.Create();
                        result = IGCL.ctlGetDisplayProperties(display, ref props);
                        if (result == ctl_result_t.CTL_RESULT_SUCCESS)
                        {
                            Console.WriteLine($"Display Type: {props.Type}");
                            Console.WriteLine($"Refresh Rate: {props.Display_Timing_Info.RefreshRate}");
                            Console.WriteLine($"Resolution: {props.Display_Timing_Info.HActive}x{props.Display_Timing_Info.VActive}");
                        }
                    }
                }
            }
            finally
            {
                // Cleanup
                IGCL.ctlClose(hAPI);
            }
        }
    }
}
