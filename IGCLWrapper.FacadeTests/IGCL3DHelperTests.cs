using System;
using System.Runtime.Versioning;
using Xunit;

namespace IGCLWrapper.FacadeTests
{
    [SupportedOSPlatform("windows")]
    [Collection("Passive")]
    [Trait("Category", "Passive")]
    public class IGCL3DHelperTests
    {
        [SkippableFact]
        public void GetSupported3DCapabilities_ShouldSucceedOrSkip()
        {
            var (api, adapter) = FacadeTestUtils.RequireAdapter();
            using (api)
            {
                var helper = api.Get3DHelper(adapter);
                var caps = FacadeTestUtils.InvokeOrSkip(() => helper.GetSupported3DCapabilities(), "3D capabilities unsupported");
                Assert.True(caps.Size > 0);
            }
        }

        [Fact]
        public unsafe void ThreeDFeatureGetSetDto_ToNative_ShouldUseManagedFieldsAndNullPointers()
        {
            var dto = new ThreeDFeatureGetSetDto
            {
                FeatureType = ctl_3d_feature_t.CTL_3D_FEATURE_FRAME_LIMIT,
                ApplicationName = "FacadeTests",
                Set = true,
                ValueType = ctl_property_value_type_t.CTL_PROPERTY_VALUE_TYPE_BOOL,
                Value = new PropertyDto
                {
                    BoolType = new PropertyBooleanDto { Enable = true }
                },
                CustomValue = new System.Collections.Generic.List<byte> { 1, 2, 3, 4 }
            };

            var native = dto.ToNative();

            Assert.Equal((sbyte)Math.Min(dto.ApplicationName!.Length, sbyte.MaxValue), native.ApplicationNameLength);
            Assert.Equal(dto.CustomValue!.Count, native.CustomValueSize);
            Assert.True(native.ApplicationName == null);
            Assert.True(native.pCustomValue == null);
            Assert.Equal((byte)1, native.bSet);

            var fromNative = ThreeDFeatureGetSetDto.FromNative(native);
            Assert.Equal(dto.FeatureType, fromNative.FeatureType);
            Assert.Equal(dto.ValueType, fromNative.ValueType);
            Assert.True(fromNative.CustomValue == null);
        }

        [Fact]
        public void Create3DFeatureSetRequest_ShouldInitializeUsefulDefaults()
        {
            var value = new PropertyDto
            {
                UIntType = new PropertyUIntDto { Enable = true, Value = 120 }
            };

            var dto = IGCL3DHelper.Create3DFeatureSetRequest(
                ctl_3d_feature_t.CTL_3D_FEATURE_FRAME_LIMIT,
                ctl_property_value_type_t.CTL_PROPERTY_VALUE_TYPE_UINT32,
                value,
                "Game.exe");

            Assert.True(dto.Set);
            Assert.Equal(ctl_3d_feature_t.CTL_3D_FEATURE_FRAME_LIMIT, dto.FeatureType);
            Assert.Equal(ctl_property_value_type_t.CTL_PROPERTY_VALUE_TYPE_UINT32, dto.ValueType);
            Assert.Equal("Game.exe", dto.ApplicationName);
            Assert.True(dto.Value.UIntType.Enable);
            Assert.Equal((uint)120, dto.Value.UIntType.Value);
        }

        [Fact]
        public void ValidateSet3DFeatureRequest_DefaultDto_ShouldThrow()
        {
            var ex = Assert.Throws<ArgumentException>(() => IGCL3DHelper.ValidateSet3DFeatureRequest(default));
            Assert.Contains("default", ex.Message, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void ThreeDFeatureCapsDto_ShouldRoundTripMetadata()
        {
            var native = new ctl_3d_feature_caps_t
            {
                Size = 32u,
                Version = 1,
                NumSupportedFeatures = 5u
            };

            var dto = ThreeDFeatureCapsDto.FromNative(native);
            Assert.Equal(native.Size, dto.Size);
            Assert.Equal(native.Version, dto.Version);
            Assert.Equal(native.NumSupportedFeatures, dto.NumSupportedFeatures);

            var roundtrip = dto.ToNative();
            Assert.Equal(native.NumSupportedFeatures, roundtrip.NumSupportedFeatures);
        }
    }
}
