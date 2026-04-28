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
    }
}
