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

        public unsafe IReadOnlyList<IntPtr> EnumLeds()
        {
            ThrowIfDisposed();
            return EnumerateHandles((_ctl_device_adapter_handle_t*)_adapter);
        }

        public unsafe ctl_led_properties_t LedGetPropertiesNative(IntPtr ledHandle)
        {
            ThrowIfDisposed();
            var props = CreateLedProperties();
            var result = IGCL.ctlLedGetProperties((_ctl_led_handle_t*)ledHandle, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get LED properties");
            return props;
        }

        public LedPropertiesDto LedGetProperties(IntPtr ledHandle)
        {
            var native = LedGetPropertiesNative(ledHandle);
            return LedPropertiesDto.FromNative(native);
        }

        public unsafe ctl_led_state_t LedGetStateNative(IntPtr ledHandle)
        {
            ThrowIfDisposed();
            var state = CreateLedState();
            var result = IGCL.ctlLedGetState((_ctl_led_handle_t*)ledHandle, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get LED state");
            return state;
        }

        public LedStateDto LedGetState(IntPtr ledHandle)
        {
            var native = LedGetStateNative(ledHandle);
            return LedStateDto.FromNative(native);
        }

        public unsafe void LedSetStateNative(IntPtr ledHandle, ctl_led_state_t state)
        {
            ThrowIfDisposed();
            var result = IGCL.ctlLedSetState((_ctl_led_handle_t*)ledHandle, &state, (uint)sizeof(ctl_led_state_t));
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to set LED state");
        }

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
        public static unsafe ctl_led_state_t CreateLedStateStruct() => CreateLedState();

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

    public struct LedPropertiesDto
    {
        public uint Size;
        public byte Version;
        public bool CanControl;
        public bool IsI2C;
        public bool IsPwm;
        public bool HaveRgb;

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

    public struct LedStateDto
    {
        public uint Size;
        public byte Version;
        public bool IsOn;
        public double Pwm;
        public ctl_led_color_t Color;

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
