using System;

namespace IGCLWrapper
{
    /// <summary>
    /// ECC helper: properties and state management.
    /// </summary>
    public sealed class IGCLEccHelper : IDisposable
    {
        private readonly IGCLApiHelper _api;
        private readonly IntPtr _adapter;
        private bool _disposed;

        internal IGCLEccHelper(IGCLApiHelper api, IntPtr adapter)
        {
            _api = api;
            _adapter = adapter;
        }

        /// <summary>
        /// Get ECC properties using the native struct.
        /// </summary>
        /// <returns>ECC properties struct.</returns>
        public unsafe ctl_ecc_properties_t EccGetPropertiesNative()
        {
            ThrowIfDisposed();
            var props = CreateEccProperties();
            var result = IGCL.ctlEccGetProperties((_ctl_device_adapter_handle_t*)_adapter, &props);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get ECC properties");
            return props;
        }

        /// <summary>
        /// Get ECC properties as a DTO.
        /// </summary>
        /// <returns>ECC properties DTO.</returns>
        public EccPropertiesDto EccGetProperties()
        {
            var native = EccGetPropertiesNative();
            return EccPropertiesDto.FromNative(native);
        }

        /// <summary>
        /// Get ECC state description.
        /// </summary>
        /// <returns>ECC state description struct.</returns>
        public unsafe ctl_ecc_state_desc_t EccGetState()
        {
            ThrowIfDisposed();
            var state = CreateEccState();
            var result = IGCL.ctlEccGetState((_ctl_device_adapter_handle_t*)_adapter, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, "Failed to get ECC state");
            return state;
        }

        /// <summary>
        /// Set ECC state.
        /// </summary>
        /// <param name="desiredState">Desired ECC state.</param>
        public unsafe void EccSetState(ctl_ecc_state_t desiredState)
        {
            ThrowIfDisposed();
            var state = CreateEccState();
            state.currentEccState = desiredState;
            var result = IGCL.ctlEccSetState((_ctl_device_adapter_handle_t*)_adapter, &state);
            if (result != ctl_result_t.CTL_RESULT_SUCCESS)
                throw new IGCLException(result, $"Failed to set ECC state to {desiredState}");
        }

        private void ThrowIfDisposed()
        {
            if (_disposed)
                throw new ObjectDisposedException(nameof(IGCLEccHelper));
        }

        /// <summary>
        /// Compare ECC properties while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left properties struct.</param>
        /// <param name="right">Right properties struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreEccPropertiesEqual(ctl_ecc_properties_t left, ctl_ecc_properties_t right)
        {
            return EccPropertiesDto.FromNative(left).Equals(EccPropertiesDto.FromNative(right));
        }

        /// <summary>
        /// Compare ECC state descriptions while ignoring native-only fields.
        /// </summary>
        /// <param name="left">Left state description struct.</param>
        /// <param name="right">Right state description struct.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public static bool AreEccStateDescriptionsEqual(ctl_ecc_state_desc_t left, ctl_ecc_state_desc_t right)
        {
            return left.Size == right.Size &&
                   left.Version == right.Version &&
                   left.currentEccState == right.currentEccState &&
                   left.pendingEccState == right.pendingEccState;
        }

        private static unsafe ctl_ecc_properties_t CreateEccProperties() => new ctl_ecc_properties_t { Size = (uint)sizeof(ctl_ecc_properties_t), Version = 0 };
        private static unsafe ctl_ecc_state_desc_t CreateEccState() => new ctl_ecc_state_desc_t { Size = (uint)sizeof(ctl_ecc_state_desc_t), Version = 0 };

        /// <summary>
        /// Mark the helper as disposed.
        /// </summary>
        public void Dispose()
        {
            _disposed = true;
        }
    }

    internal static class IGCLEccDtoBool
    {
        public static bool ToBool(byte value) => value != 0;
        public static byte ToByte(bool value) => value ? (byte)1 : (byte)0;
    }

    /// <summary>
    /// DTO for ECC properties.
    /// </summary>
    public struct EccPropertiesDto : IEquatable<EccPropertiesDto>
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
        /// Indicates whether ECC is supported.
        /// </summary>
        public bool IsSupported;
        /// <summary>
        /// Indicates whether ECC can be controlled.
        /// </summary>
        public bool CanControl;

        /// <summary>
        /// Compare ECC properties.
        /// </summary>
        /// <param name="other">Other properties instance.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public bool Equals(EccPropertiesDto other)
        {
            return Size == other.Size &&
                   Version == other.Version &&
                   IsSupported == other.IsSupported &&
                   CanControl == other.CanControl;
        }

        /// <summary>
        /// Compare to another object.
        /// </summary>
        /// <param name="obj">Object to compare.</param>
        /// <returns>True when equal; otherwise, false.</returns>
        public override bool Equals(object? obj) => obj is EccPropertiesDto other && Equals(other);

        /// <summary>
        /// Get a hash code for this instance.
        /// </summary>
        /// <returns>Hash code value.</returns>
        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(Size);
            hash.Add(Version);
            hash.Add(IsSupported);
            hash.Add(CanControl);
            return hash.ToHashCode();
        }

        /// <summary>
        /// Create a DTO from a native struct.
        /// </summary>
        /// <param name="native">Native struct.</param>
        /// <returns>ECC properties DTO.</returns>
        public static EccPropertiesDto FromNative(ctl_ecc_properties_t native)
        {
            return new EccPropertiesDto
            {
                Size = native.Size,
                Version = native.Version,
                IsSupported = IGCLEccDtoBool.ToBool(native.isSupported),
                CanControl = IGCLEccDtoBool.ToBool(native.canControl)
            };
        }

        /// <summary>
        /// Convert this DTO to a native struct.
        /// </summary>
        /// <returns>ECC properties struct.</returns>
        public ctl_ecc_properties_t ToNative()
        {
            return new ctl_ecc_properties_t
            {
                Size = Size,
                Version = Version,
                isSupported = IGCLEccDtoBool.ToByte(IsSupported),
                canControl = IGCLEccDtoBool.ToByte(CanControl)
            };
        }
    }
}

