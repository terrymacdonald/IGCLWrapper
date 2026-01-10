using System;
using System.Collections.Generic;

namespace IGCLWrapper
{
    /// <summary>
    /// LED helper: enumerate LEDs and get/set state.
    /// </summary>
    public sealed class IGCLLedHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLLedHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        /// <summary>
        /// Enumerate LED handles for the adapter.
        /// </summary>
        /// <returns>Read-only list of LED handles.</returns>
        public unsafe IReadOnlyList<IntPtr> EnumLeds()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        /// <summary>
        /// Get LED properties using the native struct.
        /// </summary>
        /// <param name="ledHandle">LED handle.</param>
        /// <returns>LED properties struct.</returns>
        public unsafe ctl_led_properties_t LedGetPropertiesNative(IntPtr ledHandle)
        {
            ThrowIfDisposed();
            var props = CreateLedProperties();
            var result = IGCL.ctlLedGetProperties((_ctl_led_handle_t*)ledHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get LED properties");
            return props;
        }

        /// <summary>
        /// Get LED properties as a DTO.
        /// </summary>
        /// <param name="ledHandle">LED handle.</param>
        /// <returns>LED properties DTO.</returns>
        public LedPropertiesDto LedGetProperties(IntPtr ledHandle)
        {
            var native = LedGetPropertiesNative(ledHandle);
            return LedPropertiesDto.FromNative(native);
        }

        /// <summary>
        /// Get LED state using the native struct.
        /// </summary>
        /// <param name="ledHandle">LED handle.</param>
        /// <returns>LED state struct.</returns>
        public unsafe ctl_led_state_t LedGetStateNative(IntPtr ledHandle)
        {
            ThrowIfDisposed();
            var state = CreateLedState();
            var result = IGCL.ctlLedGetState((_ctl_led_handle_t*)ledHandle, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get LED state");
            return state;
        }

        /// <summary>
        /// Get LED state as a DTO.
        /// </summary>
        /// <param name="ledHandle">LED handle.</param>
        /// <returns>LED state DTO.</returns>
        public LedStateDto LedGetState(IntPtr ledHandle)
        {
            var native = LedGetStateNative(ledHandle);
            return LedStateDto.FromNative(native);
        }

        /// <summary>
        /// Set LED state using the native struct.
        /// </summary>
        /// <param name="ledHandle">LED handle.</param>
        /// <param name="state">LED state struct.</param>
        public unsafe void LedSetStateNative(IntPtr ledHandle, ctl_led_state_t state)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlLedSetState((_ctl_led_handle_t*)ledHandle, &state, (uint)sizeof(ctl_led_state_t));
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set LED state");
        }

        /// <summary>
        /// Set LED state using a DTO.
        /// </summary>
        /// <param name="ledHandle">LED handle.</param>
        /// <param name="state">LED state DTO.</param>
        public void LedSetState(IntPtr ledHandle, LedStateDto state)
        {
            LedSetStateNative(ledHandle, state.ToNative());
        }

        private static unsafe IReadOnlyList<IntPtr> EnumerateHandles(_ctl_device_adapter_handle_t* adapter)
        {
            uint count = 0;
            var result = IGCL.ctlEnumLeds(adapter, &count, null);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS && count == 0)
                throw new IGCLException(result, "Failed to get LED count");
            if (count == 0)
                return Array.Empty<IntPtr>();
            var handles = new IntPtr[count];
            fixed (IntPtr* pHandles = handles)
            {
                result = IGCL.ctlEnumLeds(adapter, &count, (_ctl_led_handle_t**)pHandles);
                if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                    throw new IGCLException(result, "Failed to enumerate LEDs");
            }
            return handles;
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLLedHelper));
        }

        private static unsafe ctl_led_properties_t CreateLedProperties() => new ctl_led_properties_t { Size = (uint)sizeof(ctl_led_properties_t), Version = 0 };
        private static unsafe ctl_led_state_t CreateLedState() => new ctl_led_state_t { Size = (uint)sizeof(ctl_led_state_t), Version = 0, color = new ctl_led_color_t { Size = (uint)sizeof(ctl_led_color_t), Version = 0 } };
        /// <summary>
        /// Create an LED state struct with Size and Version initialized.
        /// </summary>
        /// <returns>Initialized LED state struct.</returns>
        public static unsafe ctl_led_state_t CreateLedStateStruct() => CreateLedState();

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCLLedDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    /// <summary>
    /// DTO for LED properties.
    /// </summary>
    public struct LedPropertiesDto
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Indicates whether LED control is supported.
        /// </summary>
        public bool CanControl;
        /// <summary>
        /// Indicates whether LED uses I2C.
        /// </summary>
        public bool IsI2C;
        /// <summary>
        /// Indicates whether LED uses PWM.
        /// </summary>
        public bool IsPwm;
        /// <summary>
        /// Indicates whether RGB is supported.
        /// </summary>
        public bool HaveRgb;

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>LED properties DTO.</returns>
        public static LedPropertiesDto FromNative(ctl_led_properties_t native)
        {
            return new LedPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                CanControl = IGCLLedDtoBool.ToBool(native.canControl),
                IsI2C = IGCLLedDtoBool.ToBool(native.isI2C),
                IsPwm = IGCLLedDtoBool.ToBool(native.isPWM),
                HaveRgb = IGCLLedDtoBool.ToBool(native.haveRGB)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>LED properties struct.</returns>
        public ctl_led_properties_t ToNative()
        {
            return new ctl_led_properties_t
            {
                Size = Size,
                Version = Version,
                canControl = IGCLLedDtoBool.ToByte(CanControl),
                isI2C = IGCLLedDtoBool.ToByte(IsI2C),
                isPWM = IGCLLedDtoBool.ToByte(IsPwm),
                haveRGB = IGCLLedDtoBool.ToByte(HaveRgb)
            };
        }
    }

    /// <summary>
    /// DTO for LED state.
    /// </summary>
    public struct LedStateDto
    {
        /// <summary>
        /// Size of the native struct.
        /// </summary>
        public uint Size;
        /// <summary>
        /// Version of the native struct.
        /// </summary>
        public byte Version;
        /// <summary>
        /// Indicates whether the LED is on.
        /// </summary>
        public bool IsOn;
        /// <summary>
        /// PWM value.
        /// </summary>
        public double Pwm;
        /// <summary>
        /// LED color struct.
        /// </summary>
        public ctl_led_color_t Color;

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>LED state DTO.</returns>
        public static LedStateDto FromNative(ctl_led_state_t native)
        {
            return new LedStateDto
            {
                Size = native.Size,
                Version = native.Version,
                IsOn = IGCLLedDtoBool.ToBool(native.isOn),
                Pwm = native.pwm,
                Color = native.color
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>LED state struct.</returns>
        public unsafe ctl_led_state_t ToNative()
        {
            var color = Color;
            if (color.Size == 0)
                color.Size = (uint)sizeof(ctl_led_color_t);

            var size = Size;
            if (size == 0)
                size = (uint)sizeof(ctl_led_state_t);

            return new ctl_led_state_t
            {
                Size = size,
                Version = Version,
                isOn = IGCLLedDtoBool.ToByte(IsOn),
                pwm = Pwm,
                color = color
            };
        }
    }
}
