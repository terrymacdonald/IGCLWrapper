namespace IGCLWrapper
{
    /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t"]/*' />
    public enum ctl_result_t
    {
        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_SUCCESS"]/*' />
        CTL_RESULT_SUCCESS = 0x00000000,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_SUCCESS_STILL_OPEN_BY_ANOTHER_CALLER"]/*' />
        CTL_RESULT_SUCCESS_STILL_OPEN_BY_ANOTHER_CALLER = 0x00000001,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_SUCCESS_END"]/*' />
        CTL_RESULT_ERROR_SUCCESS_END = 0x0000FFFF,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_GENERIC_START"]/*' />
        CTL_RESULT_ERROR_GENERIC_START = 0x40000000,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_NOT_INITIALIZED"]/*' />
        CTL_RESULT_ERROR_NOT_INITIALIZED = 0x40000001,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_ALREADY_INITIALIZED"]/*' />
        CTL_RESULT_ERROR_ALREADY_INITIALIZED = 0x40000002,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_DEVICE_LOST"]/*' />
        CTL_RESULT_ERROR_DEVICE_LOST = 0x40000003,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_OUT_OF_HOST_MEMORY"]/*' />
        CTL_RESULT_ERROR_OUT_OF_HOST_MEMORY = 0x40000004,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_OUT_OF_DEVICE_MEMORY"]/*' />
        CTL_RESULT_ERROR_OUT_OF_DEVICE_MEMORY = 0x40000005,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INSUFFICIENT_PERMISSIONS"]/*' />
        CTL_RESULT_ERROR_INSUFFICIENT_PERMISSIONS = 0x40000006,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_NOT_AVAILABLE"]/*' />
        CTL_RESULT_ERROR_NOT_AVAILABLE = 0x40000007,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_UNINITIALIZED"]/*' />
        CTL_RESULT_ERROR_UNINITIALIZED = 0x40000008,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_VERSION"]/*' />
        CTL_RESULT_ERROR_UNSUPPORTED_VERSION = 0x40000009,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_FEATURE"]/*' />
        CTL_RESULT_ERROR_UNSUPPORTED_FEATURE = 0x4000000a,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_ARGUMENT"]/*' />
        CTL_RESULT_ERROR_INVALID_ARGUMENT = 0x4000000b,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_API_HANDLE"]/*' />
        CTL_RESULT_ERROR_INVALID_API_HANDLE = 0x4000000c,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_HANDLE"]/*' />
        CTL_RESULT_ERROR_INVALID_NULL_HANDLE = 0x4000000d,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_NULL_POINTER"]/*' />
        CTL_RESULT_ERROR_INVALID_NULL_POINTER = 0x4000000e,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_SIZE"]/*' />
        CTL_RESULT_ERROR_INVALID_SIZE = 0x4000000f,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_SIZE"]/*' />
        CTL_RESULT_ERROR_UNSUPPORTED_SIZE = 0x40000010,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_UNSUPPORTED_IMAGE_FORMAT"]/*' />
        CTL_RESULT_ERROR_UNSUPPORTED_IMAGE_FORMAT = 0x40000011,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_DATA_READ"]/*' />
        CTL_RESULT_ERROR_DATA_READ = 0x40000012,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_DATA_WRITE"]/*' />
        CTL_RESULT_ERROR_DATA_WRITE = 0x40000013,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_DATA_NOT_FOUND"]/*' />
        CTL_RESULT_ERROR_DATA_NOT_FOUND = 0x40000014,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_NOT_IMPLEMENTED"]/*' />
        CTL_RESULT_ERROR_NOT_IMPLEMENTED = 0x40000015,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_OS_CALL"]/*' />
        CTL_RESULT_ERROR_OS_CALL = 0x40000016,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_KMD_CALL"]/*' />
        CTL_RESULT_ERROR_KMD_CALL = 0x40000017,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_UNLOAD"]/*' />
        CTL_RESULT_ERROR_UNLOAD = 0x40000018,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_ZE_LOADER"]/*' />
        CTL_RESULT_ERROR_ZE_LOADER = 0x40000019,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_OPERATION_TYPE"]/*' />
        CTL_RESULT_ERROR_INVALID_OPERATION_TYPE = 0x4000001a,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_NULL_OS_INTERFACE"]/*' />
        CTL_RESULT_ERROR_NULL_OS_INTERFACE = 0x4000001b,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_NULL_OS_ADAPATER_HANDLE"]/*' />
        CTL_RESULT_ERROR_NULL_OS_ADAPATER_HANDLE = 0x4000001c,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_NULL_OS_DISPLAY_OUTPUT_HANDLE"]/*' />
        CTL_RESULT_ERROR_NULL_OS_DISPLAY_OUTPUT_HANDLE = 0x4000001d,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_WAIT_TIMEOUT"]/*' />
        CTL_RESULT_ERROR_WAIT_TIMEOUT = 0x4000001e,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_PERSISTANCE_NOT_SUPPORTED"]/*' />
        CTL_RESULT_ERROR_PERSISTANCE_NOT_SUPPORTED = 0x4000001f,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_PLATFORM_NOT_SUPPORTED"]/*' />
        CTL_RESULT_ERROR_PLATFORM_NOT_SUPPORTED = 0x40000020,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_UNKNOWN_APPLICATION_UID"]/*' />
        CTL_RESULT_ERROR_UNKNOWN_APPLICATION_UID = 0x40000021,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_ENUMERATION"]/*' />
        CTL_RESULT_ERROR_INVALID_ENUMERATION = 0x40000022,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_FILE_DELETE"]/*' />
        CTL_RESULT_ERROR_FILE_DELETE = 0x40000023,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_RESET_DEVICE_REQUIRED"]/*' />
        CTL_RESULT_ERROR_RESET_DEVICE_REQUIRED = 0x40000024,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_FULL_REBOOT_REQUIRED"]/*' />
        CTL_RESULT_ERROR_FULL_REBOOT_REQUIRED = 0x40000025,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_LOAD"]/*' />
        CTL_RESULT_ERROR_LOAD = 0x40000026,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_UNKNOWN"]/*' />
        CTL_RESULT_ERROR_UNKNOWN = 0x4000FFFF,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_RETRY_OPERATION"]/*' />
        CTL_RESULT_ERROR_RETRY_OPERATION = 0x40010000,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_IGSC_LOADER"]/*' />
        CTL_RESULT_ERROR_IGSC_LOADER = 0x40010001,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_GENERIC_END"]/*' />
        CTL_RESULT_ERROR_GENERIC_END = 0x4000FFFF,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_START"]/*' />
        CTL_RESULT_ERROR_CORE_START = 0x44000000,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED"]/*' />
        CTL_RESULT_ERROR_CORE_OVERCLOCK_NOT_SUPPORTED = 0x44000001,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_VOLTAGE_OUTSIDE_RANGE"]/*' />
        CTL_RESULT_ERROR_CORE_OVERCLOCK_VOLTAGE_OUTSIDE_RANGE = 0x44000002,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_FREQUENCY_OUTSIDE_RANGE"]/*' />
        CTL_RESULT_ERROR_CORE_OVERCLOCK_FREQUENCY_OUTSIDE_RANGE = 0x44000003,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_POWER_OUTSIDE_RANGE"]/*' />
        CTL_RESULT_ERROR_CORE_OVERCLOCK_POWER_OUTSIDE_RANGE = 0x44000004,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_TEMPERATURE_OUTSIDE_RANGE"]/*' />
        CTL_RESULT_ERROR_CORE_OVERCLOCK_TEMPERATURE_OUTSIDE_RANGE = 0x44000005,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_IN_VOLTAGE_LOCKED_MODE"]/*' />
        CTL_RESULT_ERROR_CORE_OVERCLOCK_IN_VOLTAGE_LOCKED_MODE = 0x44000006,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_RESET_REQUIRED"]/*' />
        CTL_RESULT_ERROR_CORE_OVERCLOCK_RESET_REQUIRED = 0x44000007,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_WAIVER_NOT_SET"]/*' />
        CTL_RESULT_ERROR_CORE_OVERCLOCK_WAIVER_NOT_SET = 0x44000008,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_DEPRECATED_API"]/*' />
        CTL_RESULT_ERROR_CORE_OVERCLOCK_DEPRECATED_API = 0x44000009,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_LED_GET_STATE_NOT_SUPPORTED_FOR_I2C_LED"]/*' />
        CTL_RESULT_ERROR_CORE_LED_GET_STATE_NOT_SUPPORTED_FOR_I2C_LED = 0x4400000a,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_LED_SET_STATE_NOT_SUPPORTED_FOR_I2C_LED"]/*' />
        CTL_RESULT_ERROR_CORE_LED_SET_STATE_NOT_SUPPORTED_FOR_I2C_LED = 0x4400000b,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_LED_TOO_FREQUENT_SET_REQUESTS"]/*' />
        CTL_RESULT_ERROR_CORE_LED_TOO_FREQUENT_SET_REQUESTS = 0x4400000c,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_VRAM_MEMORY_SPEED_OUTSIDE_RANGE"]/*' />
        CTL_RESULT_ERROR_CORE_OVERCLOCK_VRAM_MEMORY_SPEED_OUTSIDE_RANGE = 0x4400000d,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_OVERCLOCK_INVALID_CUSTOM_VF_CURVE"]/*' />
        CTL_RESULT_ERROR_CORE_OVERCLOCK_INVALID_CUSTOM_VF_CURVE = 0x4400000e,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CORE_END"]/*' />
        CTL_RESULT_ERROR_CORE_END = 0x0440FFFF,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_3D_START"]/*' />
        CTL_RESULT_ERROR_3D_START = 0x60000000,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_3D_END"]/*' />
        CTL_RESULT_ERROR_3D_END = 0x6000FFFF,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_MEDIA_START"]/*' />
        CTL_RESULT_ERROR_MEDIA_START = 0x50000000,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_MEDIA_END"]/*' />
        CTL_RESULT_ERROR_MEDIA_END = 0x5000FFFF,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_DISPLAY_START"]/*' />
        CTL_RESULT_ERROR_DISPLAY_START = 0x48000000,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_AUX_ACCESS_FLAG"]/*' />
        CTL_RESULT_ERROR_INVALID_AUX_ACCESS_FLAG = 0x48000001,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_SHARPNESS_FILTER_FLAG"]/*' />
        CTL_RESULT_ERROR_INVALID_SHARPNESS_FILTER_FLAG = 0x48000002,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_DISPLAY_NOT_ATTACHED"]/*' />
        CTL_RESULT_ERROR_DISPLAY_NOT_ATTACHED = 0x48000003,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_DISPLAY_NOT_ACTIVE"]/*' />
        CTL_RESULT_ERROR_DISPLAY_NOT_ACTIVE = 0x48000004,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_POWERFEATURE_OPTIMIZATION_FLAG"]/*' />
        CTL_RESULT_ERROR_INVALID_POWERFEATURE_OPTIMIZATION_FLAG = 0x48000005,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_POWERSOURCE_TYPE_FOR_DPST"]/*' />
        CTL_RESULT_ERROR_INVALID_POWERSOURCE_TYPE_FOR_DPST = 0x48000006,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_PIXTX_GET_CONFIG_QUERY_TYPE"]/*' />
        CTL_RESULT_ERROR_INVALID_PIXTX_GET_CONFIG_QUERY_TYPE = 0x48000007,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_PIXTX_SET_CONFIG_OPERATION_TYPE"]/*' />
        CTL_RESULT_ERROR_INVALID_PIXTX_SET_CONFIG_OPERATION_TYPE = 0x48000008,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_SET_CONFIG_NUMBER_OF_SAMPLES"]/*' />
        CTL_RESULT_ERROR_INVALID_SET_CONFIG_NUMBER_OF_SAMPLES = 0x48000009,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_PIXTX_BLOCK_ID"]/*' />
        CTL_RESULT_ERROR_INVALID_PIXTX_BLOCK_ID = 0x4800000a,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_PIXTX_BLOCK_TYPE"]/*' />
        CTL_RESULT_ERROR_INVALID_PIXTX_BLOCK_TYPE = 0x4800000b,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INVALID_PIXTX_BLOCK_NUMBER"]/*' />
        CTL_RESULT_ERROR_INVALID_PIXTX_BLOCK_NUMBER = 0x4800000c,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_INSUFFICIENT_PIXTX_BLOCK_CONFIG_MEMORY"]/*' />
        CTL_RESULT_ERROR_INSUFFICIENT_PIXTX_BLOCK_CONFIG_MEMORY = 0x4800000d,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_3DLUT_INVALID_PIPE"]/*' />
        CTL_RESULT_ERROR_3DLUT_INVALID_PIPE = 0x4800000e,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_3DLUT_INVALID_DATA"]/*' />
        CTL_RESULT_ERROR_3DLUT_INVALID_DATA = 0x4800000f,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_3DLUT_NOT_SUPPORTED_IN_HDR"]/*' />
        CTL_RESULT_ERROR_3DLUT_NOT_SUPPORTED_IN_HDR = 0x48000010,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_3DLUT_INVALID_OPERATION"]/*' />
        CTL_RESULT_ERROR_3DLUT_INVALID_OPERATION = 0x48000011,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_3DLUT_UNSUCCESSFUL"]/*' />
        CTL_RESULT_ERROR_3DLUT_UNSUCCESSFUL = 0x48000012,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_AUX_DEFER"]/*' />
        CTL_RESULT_ERROR_AUX_DEFER = 0x48000013,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_AUX_TIMEOUT"]/*' />
        CTL_RESULT_ERROR_AUX_TIMEOUT = 0x48000014,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_AUX_INCOMPLETE_WRITE"]/*' />
        CTL_RESULT_ERROR_AUX_INCOMPLETE_WRITE = 0x48000015,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_I2C_AUX_STATUS_UNKNOWN"]/*' />
        CTL_RESULT_ERROR_I2C_AUX_STATUS_UNKNOWN = 0x48000016,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_I2C_AUX_UNSUCCESSFUL"]/*' />
        CTL_RESULT_ERROR_I2C_AUX_UNSUCCESSFUL = 0x48000017,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_LACE_INVALID_DATA_ARGUMENT_PASSED"]/*' />
        CTL_RESULT_ERROR_LACE_INVALID_DATA_ARGUMENT_PASSED = 0x48000018,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_EXTERNAL_DISPLAY_ATTACHED"]/*' />
        CTL_RESULT_ERROR_EXTERNAL_DISPLAY_ATTACHED = 0x48000019,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CUSTOM_MODE_STANDARD_CUSTOM_MODE_EXISTS"]/*' />
        CTL_RESULT_ERROR_CUSTOM_MODE_STANDARD_CUSTOM_MODE_EXISTS = 0x4800001a,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CUSTOM_MODE_NON_CUSTOM_MATCHING_MODE_EXISTS"]/*' />
        CTL_RESULT_ERROR_CUSTOM_MODE_NON_CUSTOM_MATCHING_MODE_EXISTS = 0x4800001b,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_CUSTOM_MODE_INSUFFICIENT_MEMORY"]/*' />
        CTL_RESULT_ERROR_CUSTOM_MODE_INSUFFICIENT_MEMORY = 0x4800001c,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_ADAPTER_ALREADY_LINKED"]/*' />
        CTL_RESULT_ERROR_ADAPTER_ALREADY_LINKED = 0x4800001d,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_ADAPTER_NOT_IDENTICAL"]/*' />
        CTL_RESULT_ERROR_ADAPTER_NOT_IDENTICAL = 0x4800001e,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_ADAPTER_NOT_SUPPORTED_ON_LDA_SECONDARY"]/*' />
        CTL_RESULT_ERROR_ADAPTER_NOT_SUPPORTED_ON_LDA_SECONDARY = 0x4800001f,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_SET_FBC_FEATURE_NOT_SUPPORTED"]/*' />
        CTL_RESULT_ERROR_SET_FBC_FEATURE_NOT_SUPPORTED = 0x48000020,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_ERROR_DISPLAY_END"]/*' />
        CTL_RESULT_ERROR_DISPLAY_END = 0x4800FFFF,

        /// <include file='ctl_result_t.xml' path='doc/member[@name="ctl_result_t.CTL_RESULT_MAX"]/*' />
        CTL_RESULT_MAX,
    }
}
