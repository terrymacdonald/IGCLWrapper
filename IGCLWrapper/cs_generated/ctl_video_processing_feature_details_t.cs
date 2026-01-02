using System.Runtime.CompilerServices;

namespace IGCLWrapper
{
    /// <include file='ctl_video_processing_feature_details_t.xml' path='doc/member[@name="ctl_video_processing_feature_details_t"]/*' />
    public unsafe partial struct ctl_video_processing_feature_details_t
    {
        /// <include file='ctl_video_processing_feature_details_t.xml' path='doc/member[@name="ctl_video_processing_feature_details_t.Size"]/*' />
        [NativeTypeName("uint32_t")]
        public uint Size;

        /// <include file='ctl_video_processing_feature_details_t.xml' path='doc/member[@name="ctl_video_processing_feature_details_t.Version"]/*' />
        [NativeTypeName("uint8_t")]
        public byte Version;

        /// <include file='ctl_video_processing_feature_details_t.xml' path='doc/member[@name="ctl_video_processing_feature_details_t.FeatureType"]/*' />
        public ctl_video_processing_feature_t FeatureType;

        /// <include file='ctl_video_processing_feature_details_t.xml' path='doc/member[@name="ctl_video_processing_feature_details_t.ValueType"]/*' />
        public ctl_property_value_type_t ValueType;

        /// <include file='ctl_video_processing_feature_details_t.xml' path='doc/member[@name="ctl_video_processing_feature_details_t.Value"]/*' />
        public ctl_property_info_t Value;

        /// <include file='ctl_video_processing_feature_details_t.xml' path='doc/member[@name="ctl_video_processing_feature_details_t.CustomValueSize"]/*' />
        [NativeTypeName("int32_t")]
        public int CustomValueSize;

        /// <include file='ctl_video_processing_feature_details_t.xml' path='doc/member[@name="ctl_video_processing_feature_details_t.pCustomValue"]/*' />
        public void* pCustomValue;

        /// <include file='ctl_video_processing_feature_details_t.xml' path='doc/member[@name="ctl_video_processing_feature_details_t.ReservedFields"]/*' />
        [NativeTypeName("uint32_t[16]")]
        public _ReservedFields_e__FixedBuffer ReservedFields;

        /// <include file='_ReservedFields_e__FixedBuffer.xml' path='doc/member[@name="_ReservedFields_e__FixedBuffer"]/*' />
        [InlineArray(16)]
        public partial struct _ReservedFields_e__FixedBuffer
        {
            public uint e0;
        }
    }
}
